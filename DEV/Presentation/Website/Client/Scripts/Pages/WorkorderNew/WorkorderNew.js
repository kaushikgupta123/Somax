var workOrdersSearchdt;
var searchType;
var searchStatus;
var searchShift;
var searchPriority;
var searchFailcode;
var woStatus;
var run = false;
var ZoomConfig = { zoomType: "window", lensShape: "round", lensSize: 1000, zoomWindowFadeIn: 500, zoomWindowFadeOut: 500, lensFadeIn: 100, lensFadeOut: 100, easing: true, scrollZoom: true, zoomWindowWidth: 450, zoomWindowHeight: 450 };
var equipmentid = -1;
var layoutType = 2;
var cardviewstartvalue = 0;
var cardviewlwngth = 10;
var grdcardcurrentpage = 1;
var currentorderedcolumn = 1;
var currentorder = 'asc';
//V2-347
var CompleteStartDateVw = '';
var CompleteEndDateVw = '';
var StartCreateteDate = '';
var EndCreateteDate = '';
var StartScheduledDate = '';
var EndScheduledDate = '';
var StartActualFinishDate = '';
var EndActualFinishDate = '';
var CreateStartDateVw = '';
var CreateEndDateVw = '';
var today = $.datepicker.formatDate('mm/dd/yy', new Date());
var pageno = 0;
var WOAllowedPrintNumber = 50;
function filterinfo(id, value) {
    this.key = id;
    this.value = value;
}

//V2-347
$(document).ready(function () {
    ProgressbarDynamicStatus();
    $(document).find('.dtpicker_3').datepicker({
        minDate: new Date(),
        changeMonth: true,
        changeYear: true,
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy');

});
//#region Dropdown toggle
$(document).on('click', "#spnDropToggle", function () {
    $(document).find('#searcharea').show("slide");
});
$(document).mouseup(function (e) {
    var container = $(document).find('#searcharea');
    if (!container.is(e.target) && container.has(e.target).length === 0) {
        container.hide("slide");
    }
});
$(document).mouseup(function (e) {
    var container = $(document).find('#searchBttnNewDrop');
    if (!container.is(e.target) && container.has(e.target).length === 0) {
        container.hide("slideToggle");
    }
});
//#endregion

//#region Change View
var layoutVal;
$(document).on('click', "#cardviewliLayout", function () {
    if (layoutType == 1) {
        return;
    }
    ShowbtnLoader("layoutsortmenu");
    layoutType = 1;
    localStorage.setItem("layoutTypeval", layoutType);
    layoutVal = $(document).find('#cardviewliLayout').text();
    $('#layoutsortmenu').text("Layout" + " : " + layoutVal);
    $(document).find('#Active').hide();
    $(document).find('#ActiveCard').show();
    HidebtnLoader("layoutsortmenu");
    $(document).find('#liCustomize').prop("disabled", true);
    var info = workOrdersSearchdt.page.info();
    var pageclicked = info.page;
    cardviewlwngth = info.length;
    cardviewstartvalue = cardviewlwngth * pageclicked;
    grdcardcurrentpage = pageclicked + 1;
    currentorderedcolumn = order; 
    currentorder = orderDir; 
    //In browser when Card view selected then loading page to stay Card view (End)
    GetDatatableLayout();
    ShowCardView();
});
var workOrderIdCard;
$(document).on('mouseover', '.assignedItem', function (e) {
    var thise = $(this);
    if (LRTrim(thise.find('.tooltipcards').text()).length > 0) {
        thise.find('.tooltipcards').attr('style', 'display :block !important;');
        return;
    }
    workOrderIdCard = $(this).attr('id');
    var innerText = this.innerText.trim();
    var waPersonnelId = $(this).attr('waPersonnelId');
    if (waPersonnelId == -1) {
        $.ajax({
            "url": "/WorkOrder/PopulateHover",
            "data": {
                workOrderId: workOrderIdCard
            },
            "dataType": "json",
            "type": "POST",
            "beforeSend": function (data) {
                thise.find('.loadingImg').show();
            },
            "success": function (data) {
                if (data.personnelList != null) {
                    $('#spn' + workOrderIdCard).html(data.personnelList);
                }
            },
            "complete": function () {
                thise.find('.loadingImg').hide();
                thise.find('.tooltipcards').attr('style', 'display :block !important;');
            }
        });
    }
});
$(document).on('mouseout', '.assignedItem', function (e) {
    var thise = $(this);
    if (LRTrim(thise.find('.tooltipcards').text()).length > 0) {
        thise.find('.tooltipcards').attr('style', 'display :none !important;');
        return;
    }
});

function ShowCardView() {
    personnelList = localStorage.getItem('ASSIGNEDUSERSLIST');
    $.ajax({
        url: '/WorkOrder/GetCardViewData',
        type: 'POST',
        data: {
            currentpage: grdcardcurrentpage,
            start: cardviewstartvalue,
            length: cardviewlwngth,
            currentorderedcolumn: currentorderedcolumn,
            currentorder: currentorder,
            CustomQueryDisplayId: woStatus,
            CompleteStartDateVw: ValidateDate(CompleteStartDateVw),
            CompleteEndDateVw: ValidateDate(CompleteEndDateVw),
            CreateStartDateVw: ValidateDate(CreateStartDateVw),
            CreateEndDateVw: ValidateDate(CreateEndDateVw),
            workorder: LRTrim($("#gridadvsearchworkorder").val()),
            description: LRTrim($("#gridadvsearchdescription").val()),
            Chargeto: LRTrim($("#gridadvsearchChargeto").val()),
            Chargetoname: LRTrim($("#gridadvsearchChargetoname").val()),
            AssetLocation: LRTrim($("#gridadvsearchAssetLocation").val()),
            types: $("#gridadvsearchtype").val(),
            status: $("#gridadvsearchstatus").val(),
            shifts: $("#gridadvsearchshift").val(),
            AssetGroup1Ids: $("#AssetGroup1Id").val(),
            AssetGroup2Ids: $("#AssetGroup2Id").val(),
            AssetGroup3Ids: $("#AssetGroup3Id").val(),
            AssetGroup1ClientLookUpId: LRTrim($('#AssetGroup1ClientLookUpId').val()),
            AssetGroup2ClientLookUpId: LRTrim($("#AssetGroup2ClientLookUpId").val()),
            AssetGroup3ClientLookUpId: LRTrim($("#AssetGroup3ClientLookUpId").val()),
            priorities: $("#gridadvsearchpriority").val(),
            failcodes: $("#gridadvsearchfailcode").val(),
            StartCreateDate: ValidateDate(StartCreateteDate),
            EndCreateDate: ValidateDate(EndCreateteDate),
            creator: LRTrim($("#gridadvsearchcreator").val()),
            assigned: LRTrim($("#gridadvsearchassigned").val()),
            StartScheduledDate: ValidateDate(StartScheduledDate),
            EndScheduledDate: ValidateDate(EndScheduledDate),
            StartActualFinishDate: ValidateDate(StartActualFinishDate),
            EndActualFinishDate: ValidateDate(EndActualFinishDate),
            ActualDuration: LRTrim($("#gridadvsearchactualduration").val()),
            txtSearchval: $(document).find('#txtColumnSearch').val(),
            personnelList: personnelList,
            sourcetypes: $("#gridadvsearchsource").val(),
            downRequired:$("#txtDownRequired").val(),//V2-892
            //V2-984
            assignedwo:$("#gridadvsearchassign").val(),
            requiredDate: LRTrim($("#gridadvsearchrequiredate").val()),
            planner: $("#gridadvsearchplanner").val(), //V2-1078,
            projectIds: $("#gridadvsearchproject").val() //V2-1135
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#ActiveCard').show();
            $(document).find('#ActiveCard').html(data).show();
            $(document).find('#tblworkorders_paginate li').each(function (index, value) {
                $(this).removeClass('active');
                if ($(this).data('currentpage') == grdcardcurrentpage) {
                    $(this).addClass('active');
                }
            });
        },
        complete: function () {
            if ($(document).find('#spnNoData').length > 0) {
                $(document).find('.import-export').prop('disabled', true);
            }
            else {
                $(document).find('.import-export').prop('disabled', false);
            }
            $('#ActiveCard').find('.chksearch').each(function (i) {
                var value = $(this).val();
                var found = SelectedWoIdToCancel.some(function (el) {
                    return el.WorkOrderId == value;
                });
                if (found) {
                    $(this).prop('checked', true);
                }
                else {
                    $(this).prop('checked', false);
                }
            });
            if (SelectedWoIdToCancel.length > 0) {
                $(document).find('#cardScheduleDrop,#cardApproveDrop,.wobtngrdcomplete,.wobtngrdcancel').hide();
            }
            else {
                $(document).find('#cardScheduleDrop,#cardApproveDrop,.wobtngrdcomplete,.wobtngrdcancel').show();
            }
            //#region V2-1024 Page Navigation Show Hide
            if ($(document).find('#spntotalpages').text() <= 1) {
                $(document).find('.pagenavdiv').hide();
            }
            else {
                $(document).find('.pagenavdiv').show();
            }
            //#endregion

            $(document).find('#cardviewpagelengthdrp').select2({ minimumResultsForSearch: -1 }).val(cardviewlwngth).trigger('change.select2');
            HidebtnLoader("btnsortmenu");
            HidebtnLoader("layoutsortmenu");
            HidebtnLoader("SrchBttnNew");
            HidebtnLoader("sidebarCollapse");
            HidebtnLoader("txtColumnSearch");
            HidebtnLoader("btnWoAdd");
            //changes for  V2-576
            //here 2 means workrequest
            //here 4 means completed
            var status = $("#wosearchListul li.activeState").attr('id')
            if (status == "2" || status == "4") {
                $('.wobtngrdcancel').hide();
            } else {
                $('.wobtngrdcancel').show();
            }

            //if (status == "1" || status == "4") {
            //    $('#PlanningCheckList').hide();
            //}
            //else {
            //    $('#PlanningCheckList').show();
            //}

            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', "#tableviewliLayout", function () {
    if (layoutType == 2) {
        return;
    }
    $(document).find('#tblworkorders').show();
    layoutType = 2;
    localStorage.setItem("layoutTypeval", layoutType);
    ShowbtnLoader("layoutsortmenu");
    layoutVal = $(document).find('#tableviewliLayout').text();
    $('#layoutsortmenu').text("Layout" + " : " + layoutVal);
    $(document).find('#ActiveCard').hide();
    $(document).find('#Active').show();
    $('#liCustomize').prop("disabled", false);
    HidebtnLoader("layoutsortmenu");
    localStorage.removeItem("WODETAILFROM");
    localStorage.removeItem("WOCURRENTCARDVIEWSTATE");
    if (workOrdersSearchdt) {
        workOrdersSearchdt.page.len(cardviewlwngth).order([[currentorderedcolumn, currentorder]]).page(grdcardcurrentpage - 1).draw('page');
        $(document).find('#tblworkorders_length .searchdt-menu').val(cardviewlwngth).trigger('change.select2');
    }
    else {
        generateWorkordersDataTable();
    }

});
//#endregion

//#region New Search button
$(document).on('click', "#SrchBttnNew", function () {
    $.ajax({
        url: '/Base/PopulateNewSearchList',
        type: 'GET',
        data: { tableName: 'WorkOrder' },
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
        data: { tableName: 'WorkOrder', searchText: txtSearchval, isClear: isClear },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            var i; var str = '';
            for (i = 0; i < data.searchOptionList.length; i++) {
                str += '<li><a href="javascript:void(0)"><i class="fa fa-search" style="font-size: 1rem;position: relative;top: -1px;left: 0px;"></i> &nbsp;' + data.searchOptionList[i] + '</a></li>';
            }
            UlSearchList.innerHTML = str;
        },
        complete: function () {
            if (isClear == false) {
                if (layoutType == 2) {
                    workOrdersSearchdt.page('first').draw('page');
                    CloseLoader();
                }
                else {
                    cardviewstartvalue = 0;
                    grdcardcurrentpage = 1;

                    LayoutFilterinfoUpdate();
                    ShowCardView();
                }
            }
            else {
                CloseLoader();
            }
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
$(document).on('click', '.txtSearchClick', function () {
    TextSearch();
});
function TextSearch() {
    run = true;
    hGridclearAdvanceSearch();
    $("#gridadvsearchstatus").val('');
    var txtSearchval = LRTrim($(document).find('#txtColumnSearch').val());
    if (txtSearchval) {
        GenerateSearchList(txtSearchval, false);
        var searchitemhtml = "";
        searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $('#txtColumnSearch').attr('id') + '">Search: ' + txtSearchval + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossWorkorder" aria-hidden="true"></a></span>';
        searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
        $("#advsearchfilteritems").html(searchitemhtml);
    }
    else {
        if (layoutType == 2) {
            //Add on 30/06/2020
            //var workorderstatus = localStorage.getItem("workorderstatus");
            //woStatus = workorderstatus;
            //Add on 30/06/2020
            workOrdersSearchdt.page('first').draw('page');
        }
        else {
            cardviewstartvalue = 0;
            grdcardcurrentpage = 1;
            GetDatatableLayout();
            LayoutFilterinfoUpdate();
            ShowCardView();
        }
    }
    var container = $(document).find('#searchBttnNewDrop');
    container.hide("slideToggle");
}
$(document).on('click', '#UlSearchList li', function () {
    var v = LRTrim($(this).text());
    $(document).find('#txtColumnSearch').val(v);
    TextSearch();
});
$(document).on('click', '#cancelText', function () {
    $(document).find('#txtColumnSearch').val('');
});
$(document).on('click', '#clearText', function () {
    GenerateSearchList('', true);
});
//#endregion
var IsAddWorkOrderFromEquipmentstatus = false;
$(function () {
    if ($(document).find("#detmaintab").length === 0) {
        ShowbtnLoaderclass("LoaderDrop");
        ShowbtnLoader("btnsortmenu");
    }
    $(".updateArea").hide();
    $(".actionBar").fadeIn();
    $(document).find('.select2picker').select2({});
    $(".dtpicker").keypress(function (event) { event.preventDefault(); });
    $('#searchTable').DataTable();
    $('#searchTable2').DataTable();

    $("#action").click(function () {
        $(".actionDrop").slideToggle();
    });
    $(".actionDrop ul li a").click(function () {
        $(".actionDrop").fadeOut();
    });
    $("#action").focusout(function () {
        $(".actionDrop").fadeOut();
    });
    $(document).on('click', '#liPdf', function () {
        $(".buttons-pdf")[0].click();
        funcCloseExportbtn();
    });
    $(document).on('click', '#liCsv', function () {
        $(".buttons-csv")[0].click();
        funcCloseExportbtn();
    });
    $(document).on('click', "#liExcel", function () {
        $(".buttons-excel")[0].click();
        funcCloseExportbtn();
    });
    $(document).on('click', '#liPrint', function () {
        $(".buttons-print")[0].click();
        funcCloseExportbtn();
    });
    $(document).find(".sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $(document).on('click', '#dismiss, .overlay', function () {
        $('.sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });

    IsAddWorkOrderFromEquipmentstatus = $('#IsAddWorkOrderFromEquipment').val();
    if (IsAddWorkOrderFromEquipmentstatus == 'True') {
        //#region V2-808
        fxAddPhotoforMobileInActionMenu();
        //#endregion
        var workOrderID = $(document).find('#workOrderModel_WorkOrderId').val();
        LoadCost(workOrderID, 'Actual');
        Activity(workOrderID);
        generateRequestOrderGrid(workOrderID);
        SetWorkworderDetailEnvironment();
        var stext = $(document).find('#workOrderModel_Status').val();
        if (stext == "WorkRequest" || stext == "Approved" || stext == "AwaitApproval" || stext == "Scheduled") {
            $(document).find('#spnlinkToSearch').text(getResourceValue("spnWorkOrderOpen"));
            localStorage.setItem("workorderstatus", "3");
        }
        else {
            if (stext == "Complete") {
                var startandEnddate = $("#spnwocompletedate").text();
                localStorage.setItem("WOCompleteStartDateVw", startandEnddate);
                localStorage.setItem("WOCompleteEndDateVw", startandEnddate);
                localStorage.setItem("workorderstatus", "17");
                $(document).find('#spnlinkToSearch').text(getResourceValue("spnWorkOrderCompleted") + " - " + startandEnddate + " - " + startandEnddate);
            }
            else {
                if (stext == "Canceled" || stext == "Denied") {
                    var crestestartandenddate = $("#hdncreatedate").val();
                    localStorage.setItem('WOCreateStartDateVw', crestestartandenddate);
                    localStorage.setItem('WOCreateEndDateVw', crestestartandenddate);
                    localStorage.setItem("workorderstatus", "23");
                    $(document).find('#spnlinkToSearch').text(getResourceValue("spnAllStatus") + " - " + crestestartandenddate + " - " + crestestartandenddate);
                }

            }

        }

    }

    //#region V2-735
    var IsDetailWorkOrderFromDashBoard = $('#IsDetailWorkOrderFromDashBoard').val();
    if (IsDetailWorkOrderFromDashBoard == 'True') {
        //#region V2-808
        fxAddPhotoforMobileInActionMenu();
        //#endregion
        var workOrderID = $(document).find('#workOrderModel_WorkOrderId').val();
        LoadCost(workOrderID, 'Actual');
        Activity(workOrderID);
        LoadImages(workOrderID);
        generateRequestOrderGrid(workOrderID);
        SetWorkworderDetailEnvironment();
        var stext = $(document).find('#workOrderModel_Status').val();
        if (stext == "WorkRequest" || stext == "Approved" || stext == "AwaitApproval" || stext == "Scheduled") {
            $(document).find('#spnlinkToSearch').text(getResourceValue("spnWorkOrderOpen"));
            localStorage.setItem("workorderstatus", "3");
        }

    }
    //#endregion
    //#region V2-1136
    var IsDetailFromNotification = $('#IsDetailFromNotification').val();
    if (IsDetailFromNotification == 'True') {
        fxAddPhotoforMobileInActionMenu();
        var workOrderID = $(document).find('#workOrderModel_WorkOrderId').val();
        LoadCost(workOrderID, 'Actual');
        Activity(workOrderID);
        LoadImages(workOrderID);
        generateRequestOrderGrid(workOrderID);
        SetWorkworderDetailEnvironment();
        var WorkOrderAlertName = $('#WorkOrderAlertName').val();
        if (WorkOrderAlertName == "WorkRequestApprovalNeeded" || WorkOrderAlertName == "WorkRequestApproved" || WorkOrderAlertName == "WorkRequestDenied" || WorkOrderAlertName == "WRApprovalRouting") {
            $(document).find('#spnlinkToSearch').text(getResourceValue("spnWorkRequest"));
            localStorage.setItem("workorderstatus", "2");
        }
        if (WorkOrderAlertName == "WorkOrderApprovalNeeded" || WorkOrderAlertName == "WorkOrderAssign") {
            $(document).find('#spnlinkToSearch').text(getResourceValue("spnWorkOrderOpen"));
            localStorage.setItem("workorderstatus", "3");
        }
        if (WorkOrderAlertName == "WorkOrderComplete") {
            $(document).find('#spnlinkToSearch').text(getResourceValue("spnCompletedWorkOrdersLast90Days"));
            localStorage.setItem("workorderstatus", "8");
        }
        if (WorkOrderAlertName == "WorkOrderCommentMention") {
            $(document).find('#spnlinkToSearch').text(getResourceValue("spnAllStatusesLast90Days"));
            localStorage.setItem("workorderstatus", "22");
        }
       
    }
    //#endregion
    var workorderstatus = localStorage.getItem("workorderstatus");
    woStatus = workorderstatus;
    if (workorderstatus && workorderstatus !== "0") {
        var text = "";
        if (workorderstatus == '4' || workorderstatus == '6' || workorderstatus == '7' || workorderstatus == '8' || workorderstatus == '9' || workorderstatus == '17') {
            $('#cmbcompleteview').val(workorderstatus).trigger('change');
            text = $('#wosearchListul').find('li').eq(8).text();
            $('#wosearchtitle').text(text);
            $("#wosearchListul li").removeClass("activeState");
            $("#wosearchListul li").eq(8).addClass('activeState');
        }
        //364
        if (workorderstatus == '18' || workorderstatus == '19' || workorderstatus == '20' || workorderstatus == '21' || workorderstatus == '22' || workorderstatus == '23') {
            $('#cmbcreateview').val(workorderstatus).trigger('change');
            text = $('#wosearchListul').find('li').eq(0).text();
            $('#wosearchtitle').text(text);
            $("#wosearchListul li").removeClass("activeState");
            $("#wosearchListul li").eq(0).addClass('activeState');
        }
        //364
        else {
            $('#wosearchListul li').each(function (index, value) {
                if ($(this).attr('id') == woStatus && $(this).attr('id') != '0') {
                    $('#wosearchtitle').text($(this).text());
                    $(this).addClass('activeState');
                }
            });
        }

        //364
        if ($('#wosearchListul').find('.activeState').attr('id') == '0') {
            if ($(document).find('#cmbcreateview').val() != '23')
                text = text + " - " + $(document).find('#cmbcreateview option[value=' + workorderstatus + ']').text();
            else
                text = text + " - " + $('#createdaterange').val();
            $('#wosearchtitle').text(text);
        }
        //364

        //v2 371 Point 3
        //-------------------------------------------------------
        if ($('#wosearchListul').find('.activeState').attr('id') == '4') {
            if ($(document).find('#cmbcompleteview').val() != '17')
                text = text + " - " + $(document).find('#cmbcompleteview option[value=' + workorderstatus + ']').text();
            else
                text = text + " - " + $('#completedaterange').val();
            $('#wosearchtitle').text(text);
        }
        //-------------------------------------------------------
    }
    //364
    else {
        $('#wosearchListul li').each(function (index, value) {
            if ($(this).attr('id') == 3) {
                woStatus = $(this).attr('id');
                $('#wosearchtitle').text($(this).text());
                $("#wosearchListul li").removeClass("activeState");
                $(this).addClass('activeState');
            }
        });
    }
    //364
    var displaymode = localStorage.getItem("WODETAILFROM");
    if (displaymode == 'CV') {
        $(document).find('#Active').hide();
        //$(document).find('#tblworkorders').hide();
        var thisstate = JSON.parse(localStorage.getItem("WOCURRENTCARDVIEWSTATE"));
        grdcardcurrentpage = thisstate.currentpage;
        cardviewstartvalue = thisstate.start;
        cardviewlwngth = thisstate.length;
        currentorderedcolumn = thisstate.currentorderedcolumn;
        currentorder = thisstate.order;
        $('#layoutsortmenu').text("Layout : " + "Card View");
        layoutType = 1;
        $('#btnsortmenu').text(thisstate.sorttext);
        generateWorkordersDataTable();
        ShowCardView();
    }
    else {
        $(document).find('#tblworkorders').show();
        generateWorkordersDataTable();
    }

    $(document).on('click', '#drpDwnLink', function (e) {
        e.preventDefault();
        $(document).find("#drpDwn").slideToggle();
    });
    $(document).on('click', '#drpDwnLink2', function (e) {
        e.preventDefault();
        $(document).find("#drpDwn2").slideToggle();
    });
    $(document).on('change', '#colorselector', function (evt) {
        $(document).find('.tabsArea').hide();
        var a = $(this).val();
        if ($(this).val() === 'WorkorderOverview') {
            opendiv(evt, 'ChargeTo');
        }
        else {
            openCity(evt, $(this).val());
        }
        $('#' + $(this).val()).show();
    });

    $('.select2picker, form').change(function () {
        var areaddescribedby = $(this).attr('aria-describedby');
        if ($(this).closest('form').length > 0) {
            if ($(this).valid()) {
                if (typeof areaddescribedby != 'undefined') {
                    $('#' + areaddescribedby).hide();
                }
            }
            else {
                if (typeof areaddescribedby != 'undefined') {
                    $('#' + areaddescribedby).show();
                }
            }
        }
    });
});
$(document).on('keyup', '#wrsearctxtbox', function (e) {
    var tagElems = $(document).find('#wosearchListul').children();
    $(tagElems).hide();
    for (var i = 0; i < tagElems.length; i++) {
        var tag = $(tagElems).eq(i);
        if ($(tag).text().toLowerCase().includes($(this).val().toLowerCase()) == true || $(this).val().toLowerCase().includes($(tag).text().toLowerCase()) == true) {
            $(tag).show();
        }
    }
});
$(document).on('click', '.wrsearchdrpbox', function (e) {
    $(".updateArea").hide();
    $(".actionBar").fadeIn();
    $(document).find('.chksearch').prop('checked', false);
    $('.itemcount').text(0);
    SelectedWoIdToCancel = [];
    run = true;
    if ($(this).attr('id') == '16') {
        personnelList = localStorage.getItem('ASSIGNEDUSERSLIST');
        if (personnelList != null && personnelList.length > 0) {
            assignedUsers = localStorage.getItem('ASSIGNEDUSERSLIST').split(',');
        }
        if (assignedUsers != null && assignedUsers.length > 0) {
            var selectedValues = new Array();
            for (var i = 0; i < assignedUsers.length; i++) {
                selectedValues[i] = assignedUsers[i];
            }
            $("#ddlUserSelect").val(selectedValues).trigger('change.select2');
        }
        $(document).find('#searcharea').hide("slide");
        $(document).find('#AssignedUsersModal').modal('show');
        return;
    }
    else {
        assignedUsers = [];
        localStorage.removeItem('ASSIGNEDUSERSLIST');
        personnelList = "";
        $("#ddlUserSelect").val('').trigger("change.select2");
    }

    //V2-364
    if ($(this).attr('id') == '0') {
        $(document).find('#searcharea').hide("slide");
        var val = localStorage.getItem("workorderstatus");
        if (val == '18' || val == '19' || val == '20' || val == '21' || val == '22' || val == '23') {
            $('#cmbcreateview').val(val).trigger('change');
        }
        $(document).find('#WODateRangeModalForCreateDate').modal('show');
        return;
    }

    else {
        CreateStartDateVw = '';
        CreateEndDateVw = '';
        localStorage.removeItem('WOCreateStartDateVw');
        localStorage.removeItem('WOCreateEndDateVw');
        $(document).find('#cmbcreateview').val('').trigger('change');
    }
    //V2-364

    //V2-347
    if ($(this).attr('id') == '4') {
        $(document).find('#searcharea').hide("slide");
        var val = localStorage.getItem("workorderstatustext");
        if (val == '6' || val == '7' || val == '8' || val == '9' || val == '17') {
            $('#cmbcompleteview').val(val).trigger('change');
        }
        $(document).find('#WODateRangeModal').modal('show');
        return;
    }
    else {
        CompleteStartDateVw = '';
        CompleteEndDateVw = '';
        localStorage.removeItem('WOCompleteStartDateVw');
        localStorage.removeItem('WOCompleteEndDateVw');
        $(document).find('#cmbcompleteview').val('').trigger('change');
    }

    if ($(this).attr('id') != '0') {
        $('#wosearchtitle').text($(this).text());
        localStorage.setItem("workorderstatustext", $(this).text());
    }
    else {
        $('#wosearchtitle').text(getResourceValue("spnWorkOrder"));
        localStorage.setItem("workorderstatustext", getResourceValue("spnWorkOrder"));
    }
    $(".searchList li").removeClass("activeState");
    $(this).addClass('activeState');
    $(document).find('#searcharea').hide("slide");
    var optionval = $(this).attr('id');
    localStorage.setItem("workorderstatus", optionval);
    woStatus = optionval;
    if ($(document).find('#txtColumnSearch').val() !== '') {
        $('#advsearchfilteritems').find('span').html('');
        $('#advsearchfilteritems').find('span').removeClass('tagTo');
    }
    $(document).find('#txtColumnSearch').val('');
    if (layoutType == 1) {
        cardviewstartvalue = 0;
        grdcardcurrentpage = 1;

        LayoutFilterinfoUpdate();
        ShowCardView();
    }
    else {
        if (optionval.length !== 0) {
            ShowbtnLoaderclass("LoaderDrop");
            workOrdersSearchdt.page('first').draw('page');
        }
    }

});

//V2-364
$(document).on('change', '#cmbcreateview', function (e) {
    var thielement = $(this);
    CustomQueryDisplayId = thielement.val();
    if (thielement.val() == '23') {
        var strtlocal = localStorage.getItem('WOCreateStartDateVw');
        if (strtlocal) {
            CreateStartDateVw = strtlocal;
        }
        else {
            CreateStartDateVw = today;
        }
        var endlocal = localStorage.getItem('WOCreateEndDateVw');
        if (endlocal) {
            CreateEndDateVw = endlocal;
        }
        else {
            CreateEndDateVw = today;
        }
        $(document).find('#timeperiodcontainerForCreateDate').show();
        $(document).find('#createdaterange').daterangepicker({
            format: 'MM/DD/YYYY',
            startDate: CreateStartDateVw,
            endDate: CreateEndDateVw,
            "locale": {
                "applyLabel": getResourceValue("JsApply"),
                "cancelLabel": getResourceValue("CancelAlert")
            }
        }, function (start, end, label) {
            CreateStartDateVw = start.format('MM/DD/YYYY');
            CreateEndDateVw = end.format('MM/DD/YYYY');
        });
    }
    else {
        CreateStartDateVw = '';
        CreateEndDateVw = '';
        localStorage.removeItem('WOCreateStartDateVw');
        localStorage.removeItem('WOCreateEndDateVw');
        $(document).find('#timeperiodcontainerForCreateDate').hide();
    }
})

$(document).on('click', '#btntimeperiodForCreateDate', function (e) {
    run = true;
    var daterangeval = $(document).find('#cmbcreateview').val();
    if (daterangeval == '') {
        return;
    }
    CustomQueryDisplayId = daterangeval;
    if (daterangeval != '23') {
        CreateStartDateVw = '';
        CreateEndDateVw = '';
        localStorage.removeItem('WOCreateStartDateVw');
        localStorage.removeItem('WOCreateEndDateVw');
    }
    else {
        localStorage.setItem('WOCreateStartDateVw', CreateStartDateVw);
        localStorage.setItem('WOCreateEndDateVw', CreateEndDateVw);
    }
    $(document).find('#WODateRangeModalForCreateDate').modal('hide');
    var text = $('#wosearchListul').find('li').eq(0).text();

    if (daterangeval != '23')

        text = text + " - " + $(document).find('#cmbcreateview option[value=' + daterangeval + ']').text();
    else
        text = text + " - " + $('#createdaterange').val();

    $('#wosearchtitle').text(text);
    $("#wosearchListul li").removeClass("activeState");
    $("#wosearchListul li").eq(0).addClass('activeState');
    localStorage.setItem("workorderstatus", daterangeval);
    if ($(document).find('#txtColumnSearch').val() !== '') {
        $('#advsearchfilteritems').find('span').html('');
        $('#advsearchfilteritems').find('span').removeClass('tagTo');
    }
    $(document).find('#txtColumnSearch').val('');

    woStatus = daterangeval;
    if (layoutType == 1) {
        cardviewstartvalue = 0;
        grdcardcurrentpage = 1;

        LayoutFilterinfoUpdate();
        ShowCardView();
    }
    if (daterangeval.length !== 0) {
        ShowbtnLoaderclass("LoaderDrop");
        workOrdersSearchdt.page('first').draw('page');
    }

});
//V2-364

//V2-347
$(document).on('change', '#cmbcompleteview', function (e) {
    var thielement = $(this);
    woStatus = thielement.val();

    if (thielement.val() == '17') {
        CompleteStartDateVw = today;
        CompleteEndDateVw = today;
        var strtlocal = localStorage.getItem('WOCompleteStartDateVw');
        if (strtlocal) {
            CompleteStartDateVw = strtlocal;
        }
        else {
            CompleteStartDateVw = today;
        }
        var endlocal = localStorage.getItem('WOCompleteEndDateVw');
        if (endlocal) {
            CompleteEndDateVw = endlocal;
        }
        else {
            CompleteEndDateVw = today;
        }
        $(document).find('#timeperiodcontainer').show();
        $(document).find('#completedaterange').daterangepicker({
            format: 'MM/DD/YYYY',
            startDate: CompleteStartDateVw,
            endDate: CompleteEndDateVw,
            "locale": {
                "applyLabel": getResourceValue("JsApply"),
                "cancelLabel": getResourceValue("CancelAlert")
            }
        }, function (start, end, label) {
            CompleteStartDateVw = start.format('MM/DD/YYYY');
            CompleteEndDateVw = end.format('MM/DD/YYYY');
        });
    }
    else {
        $(document).find('#timeperiodcontainer').hide();
    }
});
$(document).on('click', '#btntimeperiod', function (e) {
    run = true;
    var daterangeval = $(document).find('#cmbcompleteview').val();
    if (daterangeval == '') {
        return;
    }
    CustomQueryDisplayId = daterangeval;
    if (daterangeval != '17') {
        CompleteStartDateVw = '';
        CompleteEndDateVw = '';
        localStorage.removeItem('WOCompleteStartDateVw');
        localStorage.removeItem('WOCompleteEndDateVw');
    }
    else {
        localStorage.setItem('WOCompleteStartDateVw', CompleteStartDateVw);
        localStorage.setItem('WOCompleteEndDateVw', CompleteEndDateVw);
    }
    $(document).find('#WODateRangeModal').modal('hide');
    var text = $('#wosearchListul').find('li').eq(8).text();

    //v2 371 Point 3
    //-------------------------------------------------------
    if (daterangeval != '17')
        text = text + " - " + $(document).find('#cmbcompleteview option[value=' + daterangeval + ']').text();
    else
        text = text + " - " + $('#completedaterange').val();
    //-------------------------------------------------------

    $('#wosearchtitle').text(text);
    $("#wosearchListul li").removeClass("activeState");
    $("#wosearchListul li").eq(8).addClass('activeState');
    localStorage.setItem("workorderstatus", daterangeval);
    if ($(document).find('#txtColumnSearch').val() !== '') {
        $('#advsearchfilteritems').find('span').html('');
        $('#advsearchfilteritems').find('span').removeClass('tagTo');
    }
    $(document).find('#txtColumnSearch').val('');
    woStatus = daterangeval;
    if (layoutType == 1) {
        cardviewstartvalue = 0;
        grdcardcurrentpage = 1;

        LayoutFilterinfoUpdate();
        ShowCardView();
    }
    if (daterangeval.length !== 0) {
        ShowbtnLoaderclass("LoaderDrop");
        workOrdersSearchdt.page('first').draw('page');
    }
});
//V2-347
var assignedUsers = [];
var personnelList = "";
$(function () {
    personnelList = localStorage.getItem('ASSIGNEDUSERSLIST');
    if (personnelList != null && personnelList.length > 0) {
        assignedUsers = localStorage.getItem('ASSIGNEDUSERSLIST').split(',');
    }
    if (assignedUsers != null && assignedUsers.length > 0) {
        var selectedValues = new Array();
        for (var i = 0; i < assignedUsers.length; i++) {
            selectedValues[i] = assignedUsers[i];
        }
        $("#ddlUserSelect").val(selectedValues).trigger('change');
    }
});
$(document).on('change', '#ddlUserSelect', function () {
    var val = $(this).val();
    if (val && val.length > 0) {
        $('#btnAssignedUsers').removeAttr('disabled');
    }
    else {
        $('#btnAssignedUsers').attr('disabled', 'disabled');
    }
});
$(document).on('click', '#btnAssignedUsers', function () {
    run = true;
    personnelList = ''; var i = 0; assignedUsers = [];
    assignedUsers = $(document).find('#ddlUserSelect').val();
    if (assignedUsers.length == 0) {
        return false;
    }
    localStorage.setItem('ASSIGNEDUSERSLIST', assignedUsers);
    for (var i = 0; i < assignedUsers.length; i++) {
        personnelList = personnelList + assignedUsers[i] + ',';
    }
    personnelList = personnelList.slice(0, -1);
    $(document).find('#AssignedUsersModal').modal('hide');
    var text = $('#wosearchListul').find('li').eq(3).text();
    $('#wosearchtitle').text(text);
    $("#wosearchListul li").removeClass("activeState");
    $("#wosearchListul li").eq(3).addClass('activeState');
    var optionval = $("#wosearchListul li").eq(3).attr('id');
    localStorage.setItem("workorderstatus", optionval);
    woStatus = optionval;
    if (layoutType == 1) {
        cardviewstartvalue = 0;
        grdcardcurrentpage = 1;

        LayoutFilterinfoUpdate();
        ShowCardView();
    }
    else {
        if (assignedUsers.length !== 0) {
            ShowbtnLoaderclass("LoaderDrop");
            workOrdersSearchdt.page('first').draw('page');
        }
    }
    if ($(document).find('#txtColumnSearch').val() !== '') {
        $('#advsearchfilteritems').find('span').html('');
        $('#advsearchfilteritems').find('span').removeClass('tagTo');
    }
    $(document).find('#txtColumnSearch').val('');
});

$(document).on('change', '#cardviewpagelengthdrp', function () {
    cardviewlwngth = $(this).val();
    grdcardcurrentpage = parseInt(cardviewstartvalue / cardviewlwngth) + 1;
    cardviewstartvalue = parseInt((grdcardcurrentpage - 1) * cardviewlwngth) + 1;

    LayoutUpdate('pagelength');
    ShowCardView();
});

//#region dropzone
function ZoomImage(element) {
    element.elevateZoom(ZoomConfig);
}
function clearDropzone() {
    deleteServer = false;
    if ($(document).find('#dropzoneForm').length > 0) {
        Dropzone.forElement("div#dropzoneForm").destroy();
    }
}
$(document).on('click', '.setImage', function () {
    var workorderId = $(document).find('#workOrderModel_WorkOrderId').val();
    var imageName = $(this).data('image');
    $.ajax({
        url: '../Base/SaveUploadedFileToServer',
        type: 'POST',
        data: { 'fileName': imageName, objectId: workorderId, TableName: "WorkOrder", AttachObjectName: "WorkOrder" },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.result == "0") {
                $('#EquipZoom').attr('src', data.imageurl);
                $('.equipImg').attr('src', data.imageurl);
                $(document).find('#AzureImage').append('<a href="javascript:void(0)" id="deleteImg" class="trashIcon" title="Delete"><i class="fa fa-trash"></i></a>');
                $('#EquipZoom').data('zoomImage', data.imageurl).elevateZoom(ZoomConfig);
                $("#EquipZoom").on('load', function () {
                    CloseLoader();
                    ShowImageSaveSuccessAlert();
                });
            }
            else {
                CloseLoader();
                var errorMessage = getResourceValue("NotAuthorisedUploadFileAlert");
                ShowErrorAlert(errorMessage);

            }

        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', '#linkToSearch', function () {
    window.location.href = "../WorkOrder/Index?page=Maintenance_Work_Order_Search";
});
//#endregion
//#region Photos
$(document).on('click', '#deleteImg', function () {
    var WorkOrderId = $('#workOrderModel_WorkOrderId').val();
    var ClientOnPremise = $('#workOrderModel_ClientOnPremise').val();
    if (ClientOnPremise == 'True') {
        DeleteOnPremiseImage(WorkOrderId);
    }
    else {
        DeleteAzureImage(WorkOrderId);
    }

});
function DeleteAzureImage(WorkOrderId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/WorkOrder/DeleteImageFromAzure',
            type: 'POST',
            data: { _WorkOrderId: WorkOrderId, TableName: "WorkOrder", Profile: true, Image: true },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data === "success" || data === "not found") {
                    RedirectToPmDetail(WorkOrderId, "AzureImageReload");
                    ShowImageDeleteSuccessAlert();
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}

function DeleteOnPremiseImage(WorkOrderId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/WorkOrder/DeleteImageFromOnPremise',
            type: 'POST',
            data: { _WorkOrderId: WorkOrderId, TableName: "WorkOrder", Profile: true, Image: true },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data === "success" || data === "not found") {
                    RedirectToPmDetail(WorkOrderId, "OnPremiseImageReload");
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

$(document).on('click', "#sidebarCollapse", function () {
    $('#renderworkorder').find('.sidebar').addClass('active');
    $('.overlay').fadeIn();
    $('.collapse.in').toggleClass('in');
    $('a[aria-expanded=true]').attr('aria-expanded', 'false');

});
function openCity(evt, cityName) {
    evt.preventDefault();
    switch (cityName) {
        case "WOTasks":
            generateWoTaskGrid();
            break;
        case "WODowntime":
            generateWODowntimeGrid();
            break;
        case "WOAssignment":
            generateWoAssignmentGrid();
            break;
        case "WONotes":
            generateWoNotesGrid();
            break;
        case "WOAttachments":
            generateWoAttachmentsGrid();
            break;
        case "WOEventLog":
            GenerateSJEventLogGrid();
            break;
        case "WOEstimatesPart":
            generateWoEstimatesPartGrid(0);
            break;
        case "WOEstimatesPurchased":
            generateWoEstimatePurchaseGrid();
            $(document).find(".sidebar").mCustomScrollbar({
                theme: "minimal"
            });
            break;
        case "WOEstimatesLabor":
            generateWoEstimatesLaborGrid();
            break;
        case "WOEstimatesOther":
            generateWoEstimatesOtherGrid();
            break;
        case "WOEstimatesSummery":
            generateWoEstimatesSummeryGrid();
            break;
        case "WOActualParts":
            generateWoActualPartsGrid();
            break;
        case "WOActualLabor":
            generateWoActualLaborGrid();
            break;
        case "WOActualOther":
            generateWoActualOtherGrid();
            break;
        case "WOActualSummery":
            generateWoActualSummeryGrid();
            break;
        case "Photos":
            LoadImages($(document).find('#workOrderModel_WorkOrderId').val());
            break;
    }
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(cityName).style.display = "block";

    if (typeof evt.currentTarget !== "undefined") {
        evt.currentTarget.className += " active";
    }
    else {
        evt.target.className += " active";
    }
}

function opendiv(evt, cityName) {
    $("#WorkorderOverview").find("#btnrequestChargeto").addClass('active');
    document.getElementById(cityName).style.display = "block";
}
$(document).on('click', ".addWorkorder", function (e) {
    $.ajax({
        url: "/WorkOrder/AddWorkOrders",
        type: "GET",
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderworkorder').html(data);
            localStorage.removeItem("workorderstatus");
        },
        complete: function () {
            $(document).find('.dtpicker_3').datepicker({
                minDate: new Date(),
                changeMonth: true,
                changeYear: true,
                "dateFormat": "mm/dd/yy",
                autoclose: true
            }).inputmask('mm/dd/yyyy');
            SetControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});

var SelectedWoIdToCancel = [];
var SelectedLookupIdToSchedule = [];
var SelectedStatusSchedule = []
var SelectedPersonnelIdSchedule = [];
var WoIdListToComplete = [];
var SelectedLookupIdToApprove = [];
var SelectedLookupIdToDeny = [];
var SelectedWoIdToSchedule = [];
var SelectedWoIdToApprove = [];
var SelectedWoIdToDeny = [];

//#region new code for grid

$('#tblworkorders').on('mouseenter', '.ghover', function (e) {
    var rowData = workOrdersSearchdt.row(this).data();
    var workOrderId = rowData.WorkOrderId;
    var thise = $(this);
    if (rowData.WorkAssigned_PersonnelId == -1) {
        if (LRTrim(thise.find('.tooltipgrid').text()).length > 0) {
            thise.find('.tooltipgrid').attr('style', 'display :block !important;');
            return;
        }
    }

    if (rowData.WorkAssigned_PersonnelId == -1) {
        $.ajax({
            "url": "/WorkOrder/PopulateHover",
            "data": {
                workOrderId: workOrderId
            },
            "dataType": "json",
            "type": "POST",
            "beforeSend": function (data) {
                thise.find('.loadingImg').show();
            },
            "success": function (data) {
                if (data.personnelList != null) {
                    $('#' + workOrderId).text(data.personnelList);
                }
            },
            "complete": function () {
                thise.find('.loadingImg').hide();
                thise.find('.tooltipgrid').attr('style', 'display :block !important;');
            }
        });
    }
});
$('#tblworkorders').on('mouseleave', '.ghover', function (e) {
    $(this).find('.tooltipgrid').attr('style', 'display :none !important;');
});
var order = '1';//WO Sorting
var orderDir = 'asc';//WO Sorting
function generateWorkordersDataTable() {
    if ($(document).find('#tblworkorders').hasClass('dataTable')) {
        workOrdersSearchdt.destroy();
    }
    workOrdersSearchdt = $("#tblworkorders").DataTable({
        colReorder: {
            fixedColumnsLeft: 2
        },
        rowGrouping: true,
        searching: true,
        serverSide: true,
        //"pagingType": "full_numbers",
        "pagingType": "input",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[1, "asc"]],
        stateSave: true,
        "stateSaveCallback": function (settings, data) {
            if (run == true) {
                if (data.order) {
                    data.order[0][0] = order;
                    data.order[0][1] = orderDir;
                }//WO Sorting
                var filterinfoarray = getfilterinfoarraywo($("#txtColumnSearch"), $('#advsearchsidebarWorkorder'));
                $.ajax({
                    "url": gridStateSaveUrl,
                    "data": {
                        GridName: "WorkOrder_Search",
                        LayOutInfo: JSON.stringify(data),
                        FilterInfo: JSON.stringify(filterinfoarray)
                    },
                    "dataType": "json",
                    "type": "POST",
                    "success": function () { return; }
                });
            }
            run = false;
        },
        "stateLoadCallback": function (settings, callback) {
            $.ajax({
                "url": "/Base/GetLayout",
                "data": {
                    GridName: "WorkOrder_Search"
                },
                "async": false,
                "dataType": "json",
                "success": function (json) {
                    hGridfilteritemcount = 0;
                    if (json.LayoutInfo !== '') {
                        var LayoutInfo = JSON.parse(json.LayoutInfo);//WO Sorting
                        order = LayoutInfo.order[0][0];//WO Sorting
                        orderDir = LayoutInfo.order[0][1]; //WO Sorting
                        callback(JSON.parse(json.LayoutInfo));
                        if (json.FilterInfo !== '') {
                            setsearchuiwo(JSON.parse(json.FilterInfo), $("#txtColumnSearch"), $(".filteritemcount"), $("#advsearchfilteritems"));
                        }
                    }
                    else {
                        $(document).find("#txtDownRequired").val("").trigger('change.select2');
                        callback(json.LayoutInfo);
                    }
                }
            });
        },
        scrollX: true,
        fixedColumns: {
            leftColumns: 2
        },
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true
        },
        sDom: 'Btlipr',
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'WorkOrder List',
                orientation: 'landscape',
                pageSize: 'A4'
            },
            {
                extend: 'print',
                title: 'WorkOrder List'
            },
            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'WorkOrder List',
                extension: '.csv'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                exportOptions: {
                    columns: ':visible'
                },
                orientation: 'landscape',
                pageSize: 'A4',
                title: 'WorkOrder List'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/WorkOrder/GetWorkOrderMaintGrid",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.CustomQueryDisplayId = woStatus;
                d.CompleteStartDateVw = ValidateDate(CompleteStartDateVw);
                d.CompleteEndDateVw = ValidateDate(CompleteEndDateVw);
                d.CreateStartDateVw = ValidateDate(CreateStartDateVw);
                d.CreateEndDateVw = ValidateDate(CreateEndDateVw);
                d.workorder = LRTrim($("#gridadvsearchworkorder").val());
                d.description = LRTrim($("#gridadvsearchdescription").val());
                d.Chargeto = LRTrim($("#gridadvsearchChargeto").val());
                d.Chargetoname = LRTrim($("#gridadvsearchChargetoname").val());
                d.AssetLocation = LRTrim($("#gridadvsearchAssetLocation").val());
                d.types = $("#gridadvsearchtype").val();
                d.status = $("#gridadvsearchstatus").val();
                d.shifts = $("#gridadvsearchshift").val();
                d.AssetGroup1ClientLookUpId = LRTrim($('#AssetGroup1ClientLookUpId').val());
                d.AssetGroup2ClientLookUpId = LRTrim($("#AssetGroup2ClientLookUpId").val());
                d.AssetGroup3ClientLookUpId = LRTrim($("#AssetGroup3ClientLookUpId").val());
                d.AssetGroup1Ids = $("#AssetGroup1Id").val();
                d.AssetGroup2Ids = $("#AssetGroup2Id").val();
                d.AssetGroup3Ids = $("#AssetGroup3Id").val();
                d.priorities = $("#gridadvsearchpriority").val();
                d.failcodes = $("#gridadvsearchfailcode").val();
                d.StartCreateDate = ValidateDate(StartCreateteDate);
                d.EndCreateDate = ValidateDate(EndCreateteDate);
                d.creator = LRTrim($("#gridadvsearchcreator").val());
                d.assigned = LRTrim($("#gridadvsearchassigned").val());
                d.StartScheduledDate = ValidateDate(StartScheduledDate);
                d.EndScheduledDate = ValidateDate(EndScheduledDate);
                d.StartActualFinishDate = ValidateDate(StartActualFinishDate);
                d.EndActualFinishDate = ValidateDate(EndActualFinishDate);
                d.ActualDuration = LRTrim($("#gridadvsearchactualduration").val());
                d.txtSearchval = LRTrim($(document).find('#txtColumnSearch').val());
                d.personnelList = personnelList;
                d.sourcetypes = $("#gridadvsearchsource").val();
                d.Order = order;//WO Sorting
                d.downRequired = $("#txtDownRequired").val();//V2-892
                //V2-984
                d.assignedwo = $("#gridadvsearchassign").val();
                d.requiredDate = LRTrim($("#gridadvsearchrequiredate").val());
                d.planner = $("#gridadvsearchplanner").val(); //V2-1078
                d.projectIds = $("#gridadvsearchproject").val(); //V2-1135
            },
            "dataSrc": function (result) {
                let colOrder = workOrdersSearchdt.order();
                orderDir = colOrder[0][1];

                if (result.data.length < 1) {
                    $(document).find('.import-export').prop('disabled', true);
                }
                else {
                    $(document).find('.import-export').prop('disabled', false);
                }

                //changes for V2-576
                //here 2 means workrequest
                //here 4 means completed
                var status = $("#wosearchListul li.activeState").attr('id')
                if (status == "2" || status == "4") {
                    $('#btnCancelWolist').next().css('margin-right', '0px');
                    $('#btnCancelWolist').hide();
                } else {
                    $('#btnCancelWolist').next().css('margin-right', '7px');
                    $('#btnCancelWolist').show();
                }
                //here 1 means Scheduled Work Orders
                //here 4 means Completed Work Orders
                //if (status == "1" || status == "4") {
                //    $('#PlanningCheckList').hide();
                //}
                //else {
                //    $('#PlanningCheckList').show();
                //}


                HidebtnLoader("btnsortmenu");
                HidebtnLoaderclass("LoaderDrop");
                return result.data;
            },
            global: true
        },
        "columns":
            [
                {
                    "data": "WorkOrderId",
                    orderable: false,
                    "bSortable": false,
                    className: 'select-checkbox dt-body-center',
                    targets: 0,
                    'render': function (data, type, full, meta) {
                        return '<input type="checkbox" name="id[]" data-eqid="' + data + '" class="chksearch"  value="'
                            + $('<div/>').text(data).html() + '">';
                    }
                },
                {
                    "data": "ClientLookupId",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": true,
                    "className": "text-left",
                    "name": "0",
                    "mRender": function (data, type, full, row) {
                        if (full['PartsOnOrder'] > 0) {
                            return '<div  class="width-100"><a class="lnk_workorder" href="javascript:void(0)">' + data + '<span style="margin-left:8px"  class="m-badge m-badge--purple">' + full['PartsOnOrder'] + '</a></div>';
                        } else {
                            return '<div  class="width-100"><a class="lnk_workorder" href="javascript:void(0)">' + data + '</a></div>'
                        }
                    }
                },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1",
                    mRender: function (data, type, full, meta) {
                        return "<div class='text-wrap width-300'>" + data + "</div>";
                    }
                },
                { "data": "ChargeToClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2" },
                { "data": "ChargeTo_Name", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "AssetLocation", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    mRender: function (data, type, full, meta) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                {
                    "data": "Type", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "4",
                    mRender: function (data, type, full, meta) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                {
                    "data": "Status", "autoWidth": false, "bSearchable": true, "bSortable": true, "name": "5",
                    render: function (data, type, row, meta) {
                        if (data == statusCode.Approved) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--yellow m-badge--wide'>" + (data) + "</span >";
                        }
                        else if (data == statusCode.Canceled) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--orange m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Complete) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--green m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Denied) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--purple m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Scheduled) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--blue m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.WorkRequest) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--red m-badge--wide' style='width:95px;' >" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.AwaitApproval) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--pink m-badge--wide' style='width:100px;' >" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Planning) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--grey m-badge--wide' style='width:95px;' >" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.AwaitApproval) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--teal' style='width:95px;' >" + getStatusValue(data) + "</span >";
                        }
                        else {
                            return getStatusValue(data);
                        }
                    }

                },
                {
                    "data": "Shift", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    mRender: function (data, type, full, meta) {
                        if (data == null) {
                            data = "";
                        } else {
                            data = data;
                        }
                        return "<div class='text-wrap width-150'>" + data + "</div>";
                    }
                },
                {
                    "data": "AssetGroup1ClientlookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "7"
                },
                {
                    "data": "AssetGroup2ClientlookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "8"
                },
                {
                    "data": "AssetGroup3ClientlookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "9"
                },
                { "data": "Priority", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "CreateDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "type": "date",
                    render: function (data, type, row, meta) {
                        if (data == null) {
                            return '';
                        } else {
                            return data;
                        }
                    }
                },
                { "data": "Creator", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "Assigned", "autoWidth": true, "bSearchable": true, "bSortable": true, "sClass": "ghover",
                    mRender: function (data, type, full, meta) {
                        if (full.WorkAssigned_PersonnelId == -1) {
                            return "<span>" + data + "</span><span class='tooltipgrid' id=" + full.WorkOrderId + "></span><span class='loadingImg' style='display:none !important;'><img src='/Images/lineLoader.gif' style='width:55px;height:auto;position:absolute;left:6px;top:30px;'></span>";

                        }
                        else {
                            return "<span>" + data + "</span>";
                        }
                    }
                },
                {
                    "data": "ScheduledStartDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "type": "date",
                    render: function (data, type, row, meta) {
                        if (data == null) {
                            return '';
                        } else {
                            return data;
                        }
                    }
                },
                { "data": "FailureCode", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "ActualFinishDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "type": "date",
                    render: function (data, type, row, meta) {
                        if (data == null) {
                            return '';
                        } else {
                            return data;
                        }
                    }
                },
                { "data": "ActualDuration", "autoWidth": true, "bSearchable": true, "bSortable": true, "type": "decimal", "className": "text-right" },
                { "data": "SourceType", "autoWidth": true, "bSearchable": true, "bSortable": true, "type": "decimal" },
                {
                    "data": "RequiredDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "type": "date",
                    render: function (data, type, row, meta) {
                        if (data == null) {
                            return '';
                        } else {
                            return data;
                        }
                    }
                },
                /***** v2 - 850 ****/
                { "data": "ProjectClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                /******V2 - 892 ****/
                {
                    "data": "DownRequired", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-center",
                    "mRender": function (data, type, row) {
                        if (data == false) {
                            return '<label class="m-checkbox m-checkbox--air m-checkbox--state-brand somax-checkbox lbl-innergrid-checkbox">' +
                                '<input type="checkbox" class="status" onclick="return false"><span></span></label>';
                        }
                        else {
                            return '<label class="m-checkbox m-checkbox--air m-checkbox--solid m-checkbox--state-brand somax-checkbox lbl-innergrid-checkbox">' +
                                '<input type="checkbox" checked="checked" class="status" onclick="return false"><span></span></label>';
                        }
                    }
                },
                { "data": "PlannerFullName", "autoWidth": true, "bSearchable": true, "bSortable": true },//V2-1078


            ],
        "columnDefs": [
            {
                render: function (data, type, full, meta) {
                    return "<div class='text-wrap width-400'>" + data + "</div>";
                },
                targets: [1, 14]
            },
            {
                targets: [0, 1],
                className: 'noVis'
            }
        ],
        'rowCallback': function (row, data, dataIndex) {
            var found = SelectedWoIdToCancel.some(function (el) {
                return el.WorkOrderId == data.WorkOrderId;
            });
            if (found) {
                $(row).find('input[type="checkbox"]').prop('checked', true);
            }
        },
        initComplete: function () {
            SetPageLengthMenu();
            $("#woGridAction :input").removeAttr("disabled");
            $("#woGridAction :button").removeClass("disabled");
            DisableExportButton($('#tblworkorders'), $(document).find('.import-export'));
        }
    });
}
//#endregion
$(document).on('click', '#woreaddescription', function () {
    $(document).find('#wodetaildesmodaltext').text($(this).data("des"));
    $(document).find('#wodetaildesmodal').modal('show');
});
$(document).on('click', '#tblworkorders_paginate .paginate_button', function () {
    if (layoutType == 1) {
        var currentselectedpage = parseInt($(document).find('#txtcurrentpage').val());
        cardviewlwngth = $(document).find('#cardviewpagelengthdrp').val();
        cardviewstartvalue = cardviewlwngth * currentselectedpage;
        var lastpage = parseInt($(document).find('#spntotalpages').text());

        if ($(this).attr('id') == 'tbl_previous') {
            if (currentselectedpage == 1) {
                return false;
            }
            cardviewstartvalue = cardviewlwngth * (currentselectedpage - 2);
            grdcardcurrentpage = grdcardcurrentpage - 1;
        }
        else if ($(this).attr('id') == 'tbl_next') {
            if (currentselectedpage == lastpage) {
                return false;
            }
            cardviewstartvalue = cardviewlwngth * (currentselectedpage);
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
            grdcardcurrentpage = parseInt($(document).find('#spntotalpages').text());
            cardviewstartvalue = cardviewlwngth * (grdcardcurrentpage - 1);
        }

        LayoutUpdate('pagination');
        ShowCardView();
    }
    run = true;
});
$(document).on('change', '#tblworkorders_length .searchdt-menu', function () {
    run = true;
});
$('#tblworkorders').find('th').click(function () {
    run = true;
    order = $(this).data('col');
    currentorderedcolumn = order;
});

$(function () {
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            var colname = order;//WO Sorting
            var coldir = orderDir;//WO Sorting
            var jsonResult = $.ajax({
                "url": "/WorkOrder/GetWorkOrderPrintData",
                "type": "get",
                "datatype": "json",
                data: {
                    _CustomQueryDisplayId: woStatus,
                    _CompleteStartDateVw: ValidateDate(CompleteStartDateVw),
                    _CompleteEndDateVw: ValidateDate(CompleteEndDateVw),
                    _CreateStartDateVw: ValidateDate(CreateStartDateVw),
                    _CreateEndDateVw: ValidateDate(CreateEndDateVw),
                    _ClientLookupId: LRTrim($("#gridadvsearchworkorder").val()),
                    _Description: LRTrim($("#gridadvsearchdescription").val()),
                    _ChargeToClientLookupId: LRTrim($("#gridadvsearchChargeto").val()),
                    _ChargeToName: LRTrim($("#gridadvsearchChargetoname").val()),
                    _Type: $("#gridadvsearchtype").val().join(),
                    _Status: $("#gridadvsearchstatus").val().join(),
                    _Shift: $("#gridadvsearchshift").val().join(),
                    _AssetGroup1ClientLookUpId: LRTrim($('#AssetGroup1ClientLookUpId').val()),
                    _AssetGroup2ClientLookUpId: LRTrim($("#AssetGroup2ClientLookUpId").val()),
                    _AssetGroup3ClientLookUpId: LRTrim($("#AssetGroup3ClientLookUpId").val()),
                    _AssetGroup1Id: $('#AssetGroup1Id').val().join(),
                    _AssetGroup2Id: $("#AssetGroup2Id").val().join(),
                    _AssetGroup3Id: $("#AssetGroup3Id").val().join(),
                    _Priority: $("#gridadvsearchpriority").val().join(),
                    _StartCreateDate: ValidateDate(StartCreateteDate),
                    _EndCreateDate: ValidateDate(EndCreateteDate),
                    _Creator: LRTrim($("#gridadvsearchcreator").val()),
                    _Assigned: LRTrim($("#gridadvsearchassigned").val()),
                    _StartScheduledDate: ValidateDate(StartScheduledDate),
                    _EndScheduledDate: ValidateDate(EndScheduledDate),
                    _Failcode: $("#gridadvsearchfailcode").val().join(),
                    _StartActualFinishDate: ValidateDate(StartActualFinishDate),
                    _EndActualFinishDate: ValidateDate(EndActualFinishDate),
                    _ActualDuration: LRTrim($("#gridadvsearchactualduration").val()),
                    _colname: colname,//WO Sorting
                    _coldir: coldir,//WO Sorting
                    _SourceType: $("#gridadvsearchsource").val().join(),
                    _txtSearchval: LRTrim($("#txtColumnSearch").val()),
                    _personnelList: personnelList,
                    _downRequired: $("#txtDownRequired").val(),                    
                    _assignedwo: $("#gridadvsearchassign").val().join(),
                    _requiredDate: LRTrim($("#gridadvsearchrequiredate").val()),
                    _planner: $("#gridadvsearchplanner").val().join(), //V2-1078
                    _projectIds: $("#gridadvsearchproject").val().join() //V2-1135
                },
                success: function (result) {
                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data;
            var visiblecolumnsIndex = $("#tblworkorders thead tr th").not(":eq(0)").map(function (key) {
                return this.getAttribute('data-th-index');
            }).get();
            var d = [];
            $.each(thisdata, function (index, item) {
                if (item.ClientLookupId != null) {
                    item.ClientLookupId = item.ClientLookupId;
                }
                else {
                    item.ClientLookupId = "";
                }
                if (item.Description != null) {
                    item.Description = item.Description;
                }
                else {
                    item.Description = "";
                }

                if (item.ChargeToClientLookupId != null) {
                    item.ChargeToClientLookupId = item.ChargeToClientLookupId;
                }
                else {
                    item.ChargeToClientLookupId = "";
                }
                if (item.ChargeTo_Name != null) {
                    item.ChargeTo_Name = item.ChargeTo_Name;
                }
                else {
                    item.ChargeTo_Name = "";
                }
                if (item.Type != null) {
                    item.Type = item.Type;
                }
                else {
                    item.Type = "";
                }
                if (item.Status != null) {
                    item.Status = item.Status;
                }
                else {
                    item.Status = "";
                }

                if (item.Shift != null) {
                    item.Shift = item.Shift;
                }
                else {
                    item.Shift = "";
                }
                if (item.AssetGroup1ClientLookUpId != null) {
                    item.AssetGroup1ClientLookUpId = item.AssetGroup1ClientLookUpId;
                }
                else {
                    item.AssetGroup1ClientLookUpId = "";
                }
                if (item.AssetGroup2ClientLookUpId != null) {
                    item.AssetGroup2ClientLookUpId = item.AssetGroup2ClientLookUpId;
                }
                else {
                    item.AssetGroup2ClientLookUpId = "";
                }
                if (item.AssetGroup3ClientLookUpId != null) {
                    item.AssetGroup3ClientLookUpId = item.AssetGroup3ClientLookUpId;
                }
                else {
                    item.AssetGroup3ClientLookUpId = "";
                }
                if (item.Priority != null) {
                    item.Priority = item.Priority;
                }
                else {
                    item.Priority = "";
                }
                if (item.CreateDate != null) {
                    item.CreateDate = item.CreateDate;
                }
                else {
                    item.CreateDate = "";
                }
                if (item.Creator != null) {
                    item.Creator = item.Creator;
                }
                else {
                    item.Creator = "";
                }
                if (item.AssignedFullName != null) {
                    item.Assigned = item.AssignedFullName;
                }
                else {
                    item.Assigned = "";
                }
                if (item.ScheduledStartDate != null) {
                    item.ScheduledStartDate = item.ScheduledStartDate;
                }
                else {
                    item.ScheduledStartDate = "";
                }
                if (item.FailureCode != null) {
                    item.FailureCode = item.FailureCode;
                }
                else {
                    item.FailureCode = "";
                }
                if (item.ActualFinishDate != null) {
                    item.ActualFinishDate = item.ActualFinishDate;
                }
                else {
                    item.ActualFinishDate = "";
                }
                if (item.ActualDuration != null) {
                    item.ActualDuration = item.ActualDuration;
                }
                else {
                    item.ActualDuration = "";
                }
                if (item.SourceType != null) {
                    item.SourceType = item.SourceType;
                }
                else {
                    item.SourceType = "";
                }
                if (item.RequiredDate != null) {
                    item.RequiredDate = item.RequiredDate;
                }
                else {
                    item.RequiredDate = "";
                }
                if (item.AssetGroup1Id != null) {
                    item.AssetGroup1Id = item.AssetGroup1Id;
                }
                else {
                    item.AssetGroup1Id = "";
                }
                if (item.AssetGroup2Id != null) {
                    item.AssetGroup2Id = item.AssetGroup2Id;
                }
                else {
                    item.AssetGroup2Id = "";
                }
                if (item.AssetGroup3Id != null) {
                    item.AssetGroup3Id = item.AssetGroup3Id;
                }
                else {
                    item.AssetGroup3Id = "";
                }
                //V2-850***
                if (item.ProjectClientLookupId != null) {
                    item.ProjectClientLookupId = item.ProjectClientLookupId;
                }
                else {
                    item.ProjectClientLookupId = "";
                }
                //****V2-892***
                if (item.DownRequired != null) {
                    item.DownRequired = item.DownRequired;
                }
                //****
                //V2-1078
                if (item.PlannerFullName != null) {
                    item.PlannerFullName = item.PlannerFullName;
                }
                else {
                    item.PlannerFullName = "";
                }
                var fData = [];
                $.each(visiblecolumnsIndex, function (index, inneritem) {
                    var key = Object.keys(item)[inneritem];
                    var value = item[key];
                    fData.push(value);
                });
                d.push(fData);
            });
            return {
                body: d,
                header: $("#tblworkorders thead tr th").not(":eq(0)").find('div').map(function (key) {
                    return this.innerHTML;
                }).get()
            };
        }
    });
});

//#region Advance Search
$(document).on('click', "#btnDataAdvSrchworkorder", function (e) {
    run = true;
    $(document).find('#txtColumnSearch').val('');
    var searchitemhtml = "";
    hGridfilteritemcount = 0;
    $('#advsearchsidebarWorkorder').find('.adv-item').each(function (index, item) {
        if ($(this).hasClass('dtpicker')) {
            $(this).val(ValidateDate($(this).val()));
        }
        if ([].concat($(this).val()).filter(function (valueOfFilter) { return valueOfFilter != ''; }).length) {
            hGridfilteritemcount++;
            var s = $(this).attr('id');
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossWorkorder" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $('#advsearchfilteritems').html(searchitemhtml);
    $('#renderworkorder').find('.sidebar').removeClass('active');
    $('.overlay').fadeOut();

    hGridAdvanceSearch();
    if (layoutType == 1) {
        cardviewstartvalue = 0;
        grdcardcurrentpage = 1;

        LayoutFilterinfoUpdate();
        ShowCardView();
    }
    else {
        workOrdersSearchdt.page('first').draw('page');
    }

});
function hGridAdvanceSearch() {
    $('.filteritemcount').text(hGridfilteritemcount);
}

function hGridclearAdvanceSearch() {
    $('#advsearchsidebarWorkorder').find('input:text').val('');
    $('#advsearchsidebarWorkorder').find("select").val("").trigger('change.select2');
    hGridfilteritemcount = 0;
    $(".filteritemcount").text(hGridfilteritemcount);
    $('#advsearchfilteritems').find('span').html('');
    $('#advsearchfilteritems').find('span').removeClass('tagTo');

    $("#CreateDate").val("").trigger('change');
    StartCreateteDate = '';
    EndCreateteDate = '';
    $("#ScheduledDate").val("").trigger('change');
    StartScheduledDate = '';
    EndScheduledDate = '';
    $("#ActualFinishDate").val("").trigger('change');
    StartActualFinishDate = '';
    EndActualFinishDate = '';
}

$(document).on('change', '#dtgridadvsearchCreated', function () {
    var thisval = $(this).val();
    switch (thisval) {
        case '2':
            $('#Createtimeperiodcontainer').hide();
            StartCreateteDate = today;
            EndCreateteDate = today;
            break;
        case '3':
            $('#Createtimeperiodcontainer').hide();
            StartCreateteDate = PreviousDateByDay(7);
            EndCreateteDate = today;
            break;
        case '4':
            $('#Createtimeperiodcontainer').hide();
            StartCreateteDate = PreviousDateByDay(30);
            EndCreateteDate = today;
            break;
        case '5':
            $('#Createtimeperiodcontainer').hide();
            StartCreateteDate = PreviousDateByDay(60);
            EndCreateteDate = today;
            break;
        case '6':
            $('#Createtimeperiodcontainer').hide();
            StartCreateteDate = PreviousDateByDay(90);
            EndCreateteDate = today;
            break;
        case '10':
            $('#Createtimeperiodcontainer').show();
            StartCreateteDate = today;
            EndCreateteDate = today;
            $('#advcreatedaterange').val(StartCreateteDate + ' - ' + EndCreateteDate);
            $(document).find('#advcreatedaterange').daterangepicker(daterangepickersetting, function (start, end, label) {
                StartCreateteDate = start.format('MM/DD/YYYY');
                EndCreateteDate = end.format('MM/DD/YYYY');
            });
            break;
        default:
            $('#Createtimeperiodcontainer').hide();
            $(document).find('#advcreatedaterange').daterangepicker({
                format: 'MM/DD/YYYY'
            });
            StartCreateteDate = '';
            EndCreateteDate = '';
            break;
    }
});
$(document).on('change', '#dtgridadvsearchScheduled', function () {
    var thisval = $(this).val();
    switch (thisval) {
        case '2':
            $('#Scheduledtimeperiodcontainer').hide();
            StartScheduledDate = today;
            EndScheduledDate = today;
            break;
        case '3':
            $('#Scheduledtimeperiodcontainer').hide();
            StartScheduledDate = PreviousDateByDay(7);
            EndScheduledDate = today;
            break;
        case '4':
            $('#Scheduledtimeperiodcontainer').hide();
            StartScheduledDate = PreviousDateByDay(30);
            EndScheduledDate = today;
            break;
        case '5':
            $('#Scheduledtimeperiodcontainer').hide();
            StartScheduledDate = PreviousDateByDay(60);
            EndScheduledDate = today;
            break;
        case '6':
            $('#Scheduledtimeperiodcontainer').hide();
            StartScheduledDate = PreviousDateByDay(90);
            EndScheduledDate = today;
            break;
        case '10':
            $('#Scheduledtimeperiodcontainer').show();
            StartScheduledDate = today;
            EndScheduledDate = today;
            $('#advScheduleddaterange').val(StartScheduledDate + ' - ' + EndScheduledDate);
            $(document).find('#advScheduleddaterange').daterangepicker(daterangepickersetting, function (start, end, label) {
                StartScheduledDate = start.format('MM/DD/YYYY');
                EndScheduledDate = end.format('MM/DD/YYYY');
            });
            break;
        default:
            $('#Scheduledtimeperiodcontainer').hide();
            $(document).find('#advScheduleddaterange').daterangepicker({
                format: 'MM/DD/YYYY'
            });
            StartScheduledDate = '';
            EndScheduledDate = '';
            break;
    }
});
$(document).on('change', '#dtgridadvsearchActualFinish', function () {
    var thisval = $(this).val();
    switch (thisval) {
        case '2':
            $('#ActualFinishtimeperiodcontainer').hide();
            StartActualFinishDate = today;
            EndActualFinishDate = today;
            break;
        case '3':
            $('#ActualFinishtimeperiodcontainer').hide();
            StartActualFinishDate = PreviousDateByDay(7);
            EndActualFinishDate = today;
            break;
        case '4':
            $('#ActualFinishtimeperiodcontainer').hide();
            StartActualFinishDate = PreviousDateByDay(30);
            EndActualFinishDate = today;
            break;
        case '5':
            $('#ActualFinishtimeperiodcontainer').hide();
            StartActualFinishDate = PreviousDateByDay(60);
            EndActualFinishDate = today;
            break;
        case '6':
            $('#ActualFinishtimeperiodcontainer').hide();
            StartActualFinishDate = PreviousDateByDay(90);
            EndActualFinishDate = today;
            break;
        case '10':
            $('#ActualFinishtimeperiodcontainer').show();
            StartActualFinishDate = today;
            EndActualFinishDate = today;
            $('#advActualFinishdaterange').val(StartActualFinishDate + ' - ' + EndActualFinishDate);
            $(document).find('#advActualFinishdaterange').daterangepicker(daterangepickersetting, function (start, end, label) {
                StartActualFinishDate = start.format('MM/DD/YYYY');
                EndActualFinishDate = end.format('MM/DD/YYYY');
            });
            break;
        default:
            $('#ActualFinishtimeperiodcontainer').hide();
            $(document).find('#advActualFinishdaterange').daterangepicker({
                format: 'MM/DD/YYYY'
            });
            StartActualFinishDate = '';
            EndActualFinishDate = '';
            break;

    }
});
//#endregion
$(document).on('click', '.btnCrossWorkorder', function () {
    run = true;
    var btnCrossedId = $(this).parent().attr('id');
    var searchtxtId = btnCrossedId.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    if (hGridfilteritemcount > 0) hGridfilteritemcount--;
    if (searchtxtId == "gridadvsearchstatus") {
        $(document).find("#gridadvsearchstatus").val("").trigger('change.select2');
    }
    if (searchtxtId == "gridadvsearchtype") {
        $(document).find("#gridadvsearchtype").val("").trigger('change.select2');
    }
    if (searchtxtId == "gridadvsearchshift") {
        $(document).find("#gridadvsearchshift").val("").trigger('change.select2');
    }
    if (searchtxtId == "gridadvsearchpriority") {
        $(document).find("#gridadvsearchpriority").val("").trigger('change.select2');
    }
    if (searchtxtId == "gridadvsearchfailcode") {
        $(document).find("#gridadvsearchfailcode").val("").trigger('change.select2');
    }
    if (searchtxtId == "gridadvsearchsource") {
        $(document).find("#gridadvsearchsource").val("").trigger('change.select2');
    }

    if (searchtxtId == "AssetGroup1Id") {
        $(document).find("#AssetGroup1Id").val("").trigger('change.select2');
    }
    if (searchtxtId == "AssetGroup2Id") {
        $(document).find("#AssetGroup2Id").val("").trigger('change.select2');
    }
    if (searchtxtId == "AssetGroup3Id") {
        $(document).find("#AssetGroup3Id").val("").trigger('change.select2');
    }
   

    hGridAdvanceSearch();
    if (layoutType == 1) {
        cardviewstartvalue = 0;
        grdcardcurrentpage = 1;

        LayoutFilterinfoUpdate();
        ShowCardView();
    }
    else {
        workOrdersSearchdt.page('first').draw('page');
    }
});


function cardviewstate(currentpage, start, length, currentorderedcolumn, sorttext, order) {
    this.currentpage = currentpage;
    this.start = start;
    this.length = length;
    this.currentorderedcolumn = currentorderedcolumn;
    this.sorttext = sorttext;
    this.order = order;
}

$(document).on('click', '.lnk_workorder', function (e) {
    var workOrderId;
    var workassigned_personnelid;
    var assignedfullname;
    var titletext = $('#wosearchtitle').text();
    localStorage.setItem("workorderstatustext", titletext);
    if (layoutType == 1) {
        workOrderId = $(this).attr('id');
        workassigned_personnelid = $(this).attr('wapersonnelid');
        assignedfullname = $(this).attr('assignedfullname');
        var currentcardviewstate = new cardviewstate(grdcardcurrentpage, cardviewstartvalue, cardviewlwngth, currentorderedcolumn, $('#btnsortmenu').text(), currentorder);
        localStorage.setItem("WOCURRENTCARDVIEWSTATE", JSON.stringify(currentcardviewstate));
        localStorage.setItem("WODETAILFROM", "CV");
    }
    else {
        var row = $(this).parents('tr');
        var data = workOrdersSearchdt.row(row).data();
        workOrderId = data.WorkOrderId;
        workassigned_personnelid = data.workassigned_personnelid;
        assignedfullname = data.assignedfullname;
    }
    $.ajax({
        url: "/WorkOrder/WorkOrderDetails",
        type: "POST",
        data: { workOrderId: workOrderId },
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderworkorder').html(data);

            if ($('#IsAddWorkOrderFromEquipment').val() == 'True') {
                var stext = $(document).find('#workOrderModel_Status').val();
                $(document).find('#spnlinkToSearch').text(stext);
            }
            else if ($('#IsDetailWorkOrderFromDashBoard').val() == 'True') {
                var stext = $(document).find('#workOrderModel_Status').val();
                $(document).find('#spnlinkToSearch').text(stext);
            }
            else if ($('#IsDetailFromNotification').val() == 'True') {
                var stext = $(document).find('#workOrderModel_Status').val();
                $(document).find('#spnlinkToSearch').text(stext);
            }
            else {
                $(document).find('#spnlinkToSearch').text(titletext);
            }
            generateRequestOrderGrid(workOrderId);
        },
        complete: function () {
            SetWorkworderDetailEnvironment();
            var workOrderID = $(document).find('#workOrderModel_WorkOrderId').val();
            LoadCost(workOrderID, 'Actual');
            Activity(workOrderID);
            LoadImages(workOrderID);
            SetControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});

$(document).on('click', "#workorderSidebar", function () {
    $(document).find('#btnrequestChargeto').addClass('active');
    $(document).find('#RequestChargeTo').show();
});
/*V2-780*/
function textWorkOrderFromEquipmentstatus(IsAddWorkOrderFromEquipmentstatus, stext, startandEnddate, crestestartandenddate) {
    if (IsAddWorkOrderFromEquipmentstatus == 'True') {
        if (stext == "WorkRequest" || stext == "Approved" || stext == "AwaitApproval" || stext == "Scheduled") {
            $(document).find('#spnlinkToSearch').text(getResourceValue("spnWorkOrderOpen"));
            localStorage.setItem("workorderstatus", "3");
        }
        else {
            if (stext == "Complete") {
                localStorage.setItem("WOCompleteStartDateVw", startandEnddate);
                localStorage.setItem("WOCompleteEndDateVw", startandEnddate);
                localStorage.setItem("workorderstatus", "17");
                $(document).find('#spnlinkToSearch').text(getResourceValue("spnWorkOrderCompleted") + " - " + startandEnddate + " - " + startandEnddate);
            }
            else {
                if (stext == "Canceled" || stext == "Denied") {
                    localStorage.setItem('WOCreateStartDateVw', crestestartandenddate);
                    localStorage.setItem('WOCreateEndDateVw', crestestartandenddate);
                    localStorage.setItem("workorderstatus", "23");
                    $(document).find('#spnlinkToSearch').text(getResourceValue("spnAllStatus") + " - " + crestestartandenddate + " - " + crestestartandenddate);
                }

            }

        }

    }
    else {
        $(document).find('#spnlinkToSearch').text(localStorage.getItem("workorderstatustext"));
    }
}
$(document).on('click', "#editworkorder", function (e) {
    e.preventDefault();
    var workOrderID = $(document).find('#workOrderModel_WorkOrderId').val();
    var CreateDate = $(document).find('#workOrderModel_CreateDate').val();
    var clientLookupId = $(document).find('#workOrderModel_ClientLookupId').val();
    var status = $('#workOrderModel_Status').val();
    var description = $('#workOrderModel_Description').val();
    var type = $(document).find('#spnwotype').text();
    var priority = $(document).find("#spnwopriority").text();
    var chargeto = $(document).find("#spnwoChargeToClientLookupId").text();
    var chargetoname = $(document).find("#spnwoChargeTo_Name").text();
    var scheduleddate = $(document).find("#spnwoscheduledstartdate").text();
    var completedate = $(document).find("#spnwocompletedate").text();
    var ScheduledDuration = $('#workOrderModel_ScheduledDuration').val();
    var assignedFullName = $(document).find('#workOrderSummaryModel_AssignedFullName').val();
    var assigned = $(document).find('#workOrderSummaryModel_Assigned').val();
    var workAssigned_PersonnelId = $(document).find('#workOrderSummaryModel_WorkAssigned_PersonnelId').val();
    //v2-463
    var Downdate = $('#workOrderSummaryModel_EquipDownDate').val();
    var DownMinutes = $('#workOrderSummaryModel_EquipDownHours').val();
    var projectClientLookupId = $(document).find('#workOrderModel_ProjectClientLookupId').val();
    var assetLocation = $(document).find('#workOrderModel_AssetLocation').val();
    IsAddWorkOrderFromEquipmentstatus = $('#IsAddWorkOrderFromEquipment').val();
    var startandEnddate = $("#spnwocompletedate").text();
    var crestestartandenddate = $("#hdncreatedate").val();
    var stext = $(document).find('#workOrderModel_Status').val();
    //v2-847
    var AssetGroup1Name = $(document).find('#workOrderSummaryModel_AssetGroup1Name').val();
    var AssetGroup2Name = $(document).find('#workOrderSummaryModel_AssetGroup2Name').val();
    var AssetGroup1ClientlookupId = $(document).find('#workOrderSummaryModel_AssetGroup1ClientlookupId').val();
    var AssetGroup2ClientlookupId = $(document).find('#workOrderSummaryModel_AssetGroup2ClientlookupId').val();
    //
    $.ajax({
        url: "/WorkOrder/EditWorkOrderDynamic",
        type: "GET",
        dataType: 'html',
        data: { workOrderID: workOrderID, ClientLookupId: clientLookupId, Status: status, Description: description, Type: type, Priority: priority, ChargeTo: chargeto, ChargeToName: chargetoname, ScheduledDate: scheduleddate, Assigned: assigned, CompleteDate: completedate, ScheduledDuration: ScheduledDuration, AssignedFullName: assignedFullName, WorkAssigned_PersonnelId: workAssigned_PersonnelId, Downdate: Downdate, DownMinutes: DownMinutes, ProjectClientLookupId: projectClientLookupId, AssetLocation: assetLocation, AssetGroup1Name: AssetGroup1Name, AssetGroup2Name: AssetGroup2Name, AssetGroup1ClientlookupId: AssetGroup1ClientlookupId, AssetGroup2ClientlookupId: AssetGroup2ClientlookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderworkorder').html(data);
            textWorkOrderFromEquipmentstatus(IsAddWorkOrderFromEquipmentstatus, stext, startandEnddate, crestestartandenddate);
            if (!$('#imgChargeToTreeLineItemDynamic').hasClass('TreeAdjustNew2')) {
                $("#imgChargeToTreeLineItemDynamic").addClass("TreeAdjustNew2");
            }

        },
        complete: function () {
            $(document).find('.dtpicker_2').datepicker({
                minDate: new Date(Date.parse(CreateDate)),
                changeMonth: true,
                changeYear: true,
                "dateFormat": "mm/dd/yy",
                autoclose: true
            }).inputmask('mm/dd/yyyy');
            SetControls();


        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', "ul.vtabs li", function () {
    if ($(this).find('#drpDwnLink').length > 0 || $(this).find('#drpDwnLink2').length > 0) {
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

$(document).on('click', "#workorderSidebar", function () {
    $(document).find('#btnrequestChargeto').addClass('active');
    $(document).find('#ChargeTo').show();
});

$(document).on('click', "#btnSaveAnotherOpenWO,#btnSaveWO", function () {
    if ($(document).find("form").valid()) {
        return;
    }
    else {
        var errorTab = $(".input-validation-error").parents('div:eq(0)').attr('id');
        if (errorTab === 'RequestCharge') {
            $('#requesttab').trigger('click');
        }
        else {
            $('#statustab').trigger('click');
        }
    }
});

function WorksOrderAddOnSuccess(data) {
    CloseLoader();
    if (data.data === "success") {
        if (data.Command === "save" || data.Command === "complete") {

            if (data.mode === "add") {
                localStorage.setItem("workorderstatus", '3');
                localStorage.setItem("workorderstatustext", getResourceValue("spnOpenWorkOrder"));
                SuccessAlertSetting.text = getResourceValue("WorkOrderAddAlert");
            }
            else if (data.mode === "complete") {
                localStorage.setItem("workorderstatus", '4');
                localStorage.setItem("workorderstatustext", getResourceValue("spnCompleteWorkOrder"));
                SuccessAlertSetting.text = getResourceValue("WorkOrderCompleteAlert");
            }
            else {
                SuccessAlertSetting.text = getResourceValue("WorkOrderUpdateAlert");
            }
            swal(SuccessAlertSetting, function () {
                RedirectToPmDetail(data.WorkOrderMasterId, "overview");
            });
        }
        else {
            SuccessAlertSetting.text = getResourceValue("WorkOrderAddAlert");
            ResetErrorDiv();
            swal(SuccessAlertSetting, function () {
                $(document).find('#requesttab').addClass('active').trigger('click');
                $(document).find('form').trigger("reset");
                $(document).find('form').find("select").not("#colorselector").val("").trigger('change.select2');
                $(document).find('form').find("select").removeClass("input-validation-error");
                $(document).find('form').find("input").removeClass("input-validation-error");
                $(document).find('form').find("textarea").removeClass("input-validation-error");
            });
        }
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).ready(function () {
    $('#contactTable').DataTable();
    $(".actionBar").fadeIn();
    $("#woGridAction :input").attr("disabled", "disabled");
});


function RedirectToPmDetail(workorderid, mode) {
    $.ajax({
        url: "/WorkOrder/WorkOrderDetails",
        type: "POST",
        dataType: 'html',
        data: { workOrderId: workorderid, IsWorkOrderFromEquipment: IsAddWorkOrderFromEquipmentstatus },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderworkorder').html(data);
            var stext = $(document).find('#workOrderModel_Status').val();
            var startandEnddate = $("#spnwocompletedate").text();
            var crestestartandenddate = $("#hdncreatedate").val();
            textWorkOrderFromEquipmentstatus(IsAddWorkOrderFromEquipmentstatus, stext, startandEnddate, crestestartandenddate);
        },
        complete: function () {
            colorarray = [];
            generateRequestOrderGrid(workorderid);
            SetWorkworderDetailEnvironment();
            LoadCost(workorderid, 'Actual');
            LoadImages(workorderid);
            Activity(workorderid);
            if (mode === "overview") {
                $('#lioverview').trigger('click');
                $('#colorselector').val('WorkorderOverview');
            }
            if (mode === "tasks") {
                $('#wotask').trigger('click');
                $('#colorselector').val('WOTasks');
            }
            if (mode === "assignments") {
                $('#woassignment').trigger('click');
                $('#colorselector').val('WOAssignment');
            }
            if (mode === "notes") {
                $('#wonotes').trigger('click');
                $('#colorselector').val('WONotes');
            }
            if (mode === "attachments") {
                $('#woattachments').trigger('click');
                $('#colorselector').val('WOAttachments');
            }
            if (mode === "estimatespart") {
                $(document).find("#drpDwn").slideToggle();
                $('#woestimateparts').trigger('click');
                $('#colorselector').val('WOEstimatesPart');
            }
            if (mode === "estimatespurchase") {
                $(document).find("#drpDwn").slideToggle();
                $('#woestimatepurchased').trigger('click');
                $('#colorselector').val('WOEstimatesPurchased');
            }
            if (mode === "estimateslabor") {
                $(document).find("#drpDwn").slideToggle();
                $('#woestimatelabor').trigger('click');
                $('#colorselector').val('WOEstimatesLabor');
            }
            if (mode === "estimatesother") {
                $(document).find("#drpDwn").slideToggle();
                $('#woestimateother').trigger('click');
                $('#colorselector').val('WOEstimatesOther');
            }
            if (mode === "estimatesummary") {
                $(document).find("#drpDwn").slideToggle();
                $('#woestimatesummery').trigger('click');
                $('#colorselector').val('WOEstimatesSummery');
            }
            if (mode === "actualpart") {
                $(document).find("#drpDwn2").slideToggle();
                $('#woactualparts').trigger('click');
                $('#colorselector').val('WOActualParts');
            }
            if (mode === "actuallabor") {
                $(document).find("#drpDwn2").slideToggle();
                $('#woactuallabor').trigger('click');
                $('#colorselector').val('WOActualLabor');
            }
            if (mode === "actualother") {
                $(document).find("#drpDwn2").slideToggle();
                $('#woactualother').trigger('click');
                $('#colorselector').val('WOActualOther');
            }
            if (mode === "actualsummery") {
                $(document).find("#drpDwn2").slideToggle();
                $('#woactualsummery').trigger('click');
                $('#colorselector').val('WOActualSummery');
            }
            if (mode === "AzureImageReload" || mode === "OnPremiseImageReload") {
                $(document).find("#drpDwn2").slideToggle();
                $('#overviewcontainer').hide();
                $('#WorkorderOverview').hide();
                $('.tabcontent2').hide();
                $('#auditlogcontainer').hide();
                $('.imageDropZone').show();
                $(document).find('#btnnblock').removeClass("col-xl-6");
                $(document).find('#btnnblock').addClass("col-xl-12");
                $(document).find('#workorderSidebar').removeClass("active");
                $(document).find('#photot').addClass("active");
            }
            if (mode === "downtime") {
                $('#woDowntime').trigger('click');
                $('#colorselector').val('WODowntime');
            }
            //if (mode === "OnPremiseImageReload") {
            //    $(document).find("#drpDwn2").slideToggle();
            //    $('#overviewcontainer').hide();
            //    $('#WorkorderOverview').hide();
            //    $('.tabcontent2').hide();
            //    $('#auditlogcontainer').hide();
            //    $('.imageDropZone').show();
            //    $(document).find('#btnnblock').removeClass("col-xl-6");
            //    $(document).find('#btnnblock').addClass("col-xl-12");
            //    $(document).find('#workorderSidebar').removeClass("active");
            //    $(document).find('#photot').addClass("active");
            //}

        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', "#btnCancelAddWO", function () {
    var workorderId = $(document).find('#workOrderModel_WorkOrderId').val();
    if (workorderId == 0) {
        swal(CancelAlertSetting, function () {
            window.location.href = "../WorkOrder/Index?page=Maintenance_Work_Order_Search";
        });
    } else {
        RedirectToDetailOncancel(workorderId, "overview");
    }
});
$(document).on('click', "#brdworkorder", function () {
    var workorderId = $(this).attr('data-val');
    RedirectToPmDetail(workorderId);
});
function SetControls() {
    CloseLoader();
    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse(document);
    $('input, form').blur(function () {
        $(this).valid();
    });
    $('.select2picker, form').change(function () {
        var areaddescribedby = $(this).attr('aria-describedby');
        if ($(this).closest('form').length > 0) {
            if ($(this).valid()) {
                if (typeof areaddescribedby !== 'undefined') {
                    $('#' + areaddescribedby).hide();
                }
            }
            else {
                if (typeof areaddescribedby !== 'undefined') {
                    $('#' + areaddescribedby).show();
                }
            }
        }
    });
    $(document).find('.select2picker').select2({});
    ZoomImage($(document).find('#EquipZoom'));
    $(document).find('.dtpicker').datepicker({
        changeMonth: true,
        changeYear: true,
        beforeShow: function (i) { if ($(i).attr('readonly')) { return false; } },
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy');
    SetFixedHeadStyle();
};
function SetWorkworderDetailEnvironment() {
    CloseLoader();
    ZoomImage($(document).find('#EquipZoom'));
    Dropzone.autoDiscover = false;
    if ($(document).find('#dropzoneForm').length > 0) {
        var myDropzone = new Dropzone("div#dropzoneForm", {
            url: "../Base/SaveUploadedFile",
            addRemoveLinks: true,
            paramName: 'file',
            maxFilesize: 10, // MB
            maxFiles: 4,
            dictDefaultMessage: 'Drag files here to upload, or click to select one',
            acceptedFiles: ".jpeg,.jpg,.png",
            init: function () {
                this.on("removedfile", function (file) {
                    if (file.type != 'image/jpeg' && file.type != 'image/jpg' && file.type != 'image/png') {
                        ShowErrorAlert(getResourceValue("spnValidImage"));
                    }
                });
            },
            autoProcessQueue: true,
            dictRemoveFile: "X",
            uploadMultiple: true,
            dictRemoveFileConfirmation: "Do you want to remove this file ?",
            dictCancelUpload: "X",
            parallelUploads: 1,
            dictMaxFilesExceeded: "You can not upload any more files.",
            success: function (file, response) {
                var imgName = response;
                file.previewElement.classList.add("dz-success");
                var radImage = '<a class="lnkContainer setImage" data-toggle="tooltip" title="Upload a selective Image!" data-image="' + file.name + '" style="cursor:pointer;"><i class="fa fa-upload" style="cursor:pointer;"></i></a>';
                $(file.previewElement).append(radImage);
            },
            error: function (file, response) {
                if (file.size > (1024 * 1024 * 10)) // not more than 10mb
                {
                    ShowImageSizeExceedAlert();
                }
                file.previewElement.classList.add("dz-error");
                var _this = this;
                _this.removeFile(file);
            }
        });
    }
    SetFixedHeadStyle();
}
$(document).on('click', "#btnCancelOnDemandWO", function () {
    CancelDdWo();
});
$(document).on('click', "#btnCancelWoDesc", function () {
    CancelDdWo();
});
function CancelDdWo() {
    swal(CancelAlertSetting, function () {
        window.location.href = "../WorkOrder/Index?page=Maintenance_Work_Order_Search";
    });
}

//#region WorkRequestAdd
$(function () {
    $(document).on('change', "#woRequestModel_ChargeTo", function () {
        $(document).find('#woRequestModel_ChargeToClientLookupId').val($("#woRequestModel_ChargeTo option:selected").text());
    });
    // Commented for V2-608
    //$(document).on('change', "#woRequestModel_ChargeType", function () {
    //    $(document).find('#txtChargeTo').val('');
    //    var type = $(this).val();
    //    if (type === "Equipment") {
    //        $("#imgChargeToTree").show();
    //    }
    //    else {
    //        $("#imgChargeToTree").hide();
    //    }
    //});
    $(document).on('change', ".radSelectWo", function () {
        if ($("#sanitationOnDemandWOModel_PlantLocationDescription").is(":visible")) {
            var PlantLocationId = $(this).data('plantlocationid');
            var Description = $(this).data('description');
            var ChargeType = $(this).data('chargetype');
            $(document).find('#sanitationOnDemandWOModel_PlantLocationDescription').val(Description);
            $(document).find('#sanitationOnDemandWOModel_PlantLocationId').val(PlantLocationId);
            $(document).find('#sanitationOnDemandWOModel_ChargeType').val(ChargeType);
            $(document).find('#txtChargeTo').val(clientlookupid).removeClass('input-validation-error');
        }
        else {
            var s = $(this).data;
            $(document).find('#hdnChargeTo').val('0');
            equipmentid = $(this).data('equipmentid');
            var clientlookupid = $(this).data('clientlookupid');
            var chargetoname = $(this).data('itemname');
            chargetoname = chargetoname.substring(0, chargetoname.length - 1);
            $.ajax({
                url: '/PlantLocationTree/MapEquipmentHierarchyTree',
                datatype: "json",
                type: "post",
                contenttype: 'application/json; charset=utf-8',
                async: false,
                cache: false,
                data: { _EquipmentId: equipmentid },
                success: function (data) {
                    $('#commonWOTreeModal').modal('hide');
                    if ($("#woRequestModel_ChargeTo").is(":visible")) {
                        $(document).find('#woRequestModel_ChargeTo').val(data).trigger('change');
                        $(document).find('#woRequestModel_ChargeToClientLookupId').val($("#woRequestModel_ChargeTo option:selected").text());
                    }
                    else {
                        $(document).find('#txtChargeTo').val(clientlookupid).removeClass('input-validation-error');
                        $(document).find('#workOrderModel_ChargeTo_Name').val(chargetoname);
                        $(document).find('#woOnDemandModel_ChargeToClientLookupId').val(clientlookupid).trigger('change');
                        $(document).find('#woDescriptionModel_ChargeToClientLookupId').val(clientlookupid).trigger('change');
                    }
                    $('#woEquipTreeModal').modal('hide');
                }
            });
        }
    });
    $(document).on('click', "#btnCancelAddWorkRequest", function () {
        RedirectToDetailOncancelForDashboard();
    });
    $(document).on('click', '#imgChargeToTree', function (e) {
        $(this).blur();
        generateEquipmentTree(-1);
    });
    function RedirectToDetailOncancelForDashboard() {
        swal(CancelAlertSetting, function () {
            window.location.href = '/Dashboard/Dashboard';
        });
    }
    function generateEquipmentTree(paramVal) {
        $.ajax({
            url: '/PlantLocationTree/EquipmentHierarchyTree',
            datatype: "json",
            type: "post",
            contenttype: 'application/json; charset=utf-8',
            cache: false,
            beforeSend: function () {
                ShowLoader();
                $(document).find(".cntTree").html("<b>Processing...</b>");
            },
            success: function (data) {
                $(document).find(".cntTree").html(data);
            },
            complete: function () {
                CloseLoader();
                treeTable($(document).find('#tblTree'));
                $(document).find('.radSelect').each(function () {
                    if ($(document).find('#hdnChargeTo').val() == '0' || $(document).find('#hdnChargeTo').val() == '') {

                        if ($(this).data('equipmentid') === equipmentid) {
                            $(this).attr('checked', true);
                        }

                    }
                    else {

                        if ($(this).data('equipmentid') == $(document).find('#hdnChargeTo').val()) {
                            $(this).attr('checked', true);
                        }

                    }
                });
                //-- V2-518 collapse all element
                // looking for the collapse icon and triggered click to collapse
                $.each($(document).find('#tblTree > tbody > tr').find('img[src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAACXBIWXMAAAsTAAALEwEAmpwYAAAKT2lDQ1BQaG90b3Nob3AgSUNDIHByb2ZpbGUAAHjanVNnVFPpFj333vRCS4iAlEtvUhUIIFJCi4AUkSYqIQkQSoghodkVUcERRUUEG8igiAOOjoCMFVEsDIoK2AfkIaKOg6OIisr74Xuja9a89+bN/rXXPues852zzwfACAyWSDNRNYAMqUIeEeCDx8TG4eQuQIEKJHAAEAizZCFz/SMBAPh+PDwrIsAHvgABeNMLCADATZvAMByH/w/qQplcAYCEAcB0kThLCIAUAEB6jkKmAEBGAYCdmCZTAKAEAGDLY2LjAFAtAGAnf+bTAICd+Jl7AQBblCEVAaCRACATZYhEAGg7AKzPVopFAFgwABRmS8Q5ANgtADBJV2ZIALC3AMDOEAuyAAgMADBRiIUpAAR7AGDIIyN4AISZABRG8lc88SuuEOcqAAB4mbI8uSQ5RYFbCC1xB1dXLh4ozkkXKxQ2YQJhmkAuwnmZGTKBNA/g88wAAKCRFRHgg/P9eM4Ors7ONo62Dl8t6r8G/yJiYuP+5c+rcEAAAOF0ftH+LC+zGoA7BoBt/qIl7gRoXgugdfeLZrIPQLUAoOnaV/Nw+H48PEWhkLnZ2eXk5NhKxEJbYcpXff5nwl/AV/1s+X48/Pf14L7iJIEyXYFHBPjgwsz0TKUcz5IJhGLc5o9H/LcL//wd0yLESWK5WCoU41EScY5EmozzMqUiiUKSKcUl0v9k4t8s+wM+3zUAsGo+AXuRLahdYwP2SycQWHTA4vcAAPK7b8HUKAgDgGiD4c93/+8//UegJQCAZkmScQAAXkQkLlTKsz/HCAAARKCBKrBBG/TBGCzABhzBBdzBC/xgNoRCJMTCQhBCCmSAHHJgKayCQiiGzbAdKmAv1EAdNMBRaIaTcA4uwlW4Dj1wD/phCJ7BKLyBCQRByAgTYSHaiAFiilgjjggXmYX4IcFIBBKLJCDJiBRRIkuRNUgxUopUIFVIHfI9cgI5h1xGupE7yAAygvyGvEcxlIGyUT3UDLVDuag3GoRGogvQZHQxmo8WoJvQcrQaPYw2oefQq2gP2o8+Q8cwwOgYBzPEbDAuxsNCsTgsCZNjy7EirAyrxhqwVqwDu4n1Y8+xdwQSgUXACTYEd0IgYR5BSFhMWE7YSKggHCQ0EdoJNwkDhFHCJyKTqEu0JroR+cQYYjIxh1hILCPWEo8TLxB7iEPENyQSiUMyJ7mQAkmxpFTSEtJG0m5SI+ksqZs0SBojk8naZGuyBzmULCAryIXkneTD5DPkG+Qh8lsKnWJAcaT4U+IoUspqShnlEOU05QZlmDJBVaOaUt2ooVQRNY9aQq2htlKvUYeoEzR1mjnNgxZJS6WtopXTGmgXaPdpr+h0uhHdlR5Ol9BX0svpR+iX6AP0dwwNhhWDx4hnKBmbGAcYZxl3GK+YTKYZ04sZx1QwNzHrmOeZD5lvVVgqtip8FZHKCpVKlSaVGyovVKmqpqreqgtV81XLVI+pXlN9rkZVM1PjqQnUlqtVqp1Q61MbU2epO6iHqmeob1Q/pH5Z/YkGWcNMw09DpFGgsV/jvMYgC2MZs3gsIWsNq4Z1gTXEJrHN2Xx2KruY/R27iz2qqaE5QzNKM1ezUvOUZj8H45hx+Jx0TgnnKKeX836K3hTvKeIpG6Y0TLkxZVxrqpaXllirSKtRq0frvTau7aedpr1Fu1n7gQ5Bx0onXCdHZ4/OBZ3nU9lT3acKpxZNPTr1ri6qa6UbobtEd79up+6Ynr5egJ5Mb6feeb3n+hx9L/1U/W36p/VHDFgGswwkBtsMzhg8xTVxbzwdL8fb8VFDXcNAQ6VhlWGX4YSRudE8o9VGjUYPjGnGXOMk423GbcajJgYmISZLTepN7ppSTbmmKaY7TDtMx83MzaLN1pk1mz0x1zLnm+eb15vft2BaeFostqi2uGVJsuRaplnutrxuhVo5WaVYVVpds0atna0l1rutu6cRp7lOk06rntZnw7Dxtsm2qbcZsOXYBtuutm22fWFnYhdnt8Wuw+6TvZN9un2N/T0HDYfZDqsdWh1+c7RyFDpWOt6azpzuP33F9JbpL2dYzxDP2DPjthPLKcRpnVOb00dnF2e5c4PziIuJS4LLLpc+Lpsbxt3IveRKdPVxXeF60vWdm7Obwu2o26/uNu5p7ofcn8w0nymeWTNz0MPIQ+BR5dE/C5+VMGvfrH5PQ0+BZ7XnIy9jL5FXrdewt6V3qvdh7xc+9j5yn+M+4zw33jLeWV/MN8C3yLfLT8Nvnl+F30N/I/9k/3r/0QCngCUBZwOJgUGBWwL7+Hp8Ib+OPzrbZfay2e1BjKC5QRVBj4KtguXBrSFoyOyQrSH355jOkc5pDoVQfujW0Adh5mGLw34MJ4WHhVeGP45wiFga0TGXNXfR3ENz30T6RJZE3ptnMU85ry1KNSo+qi5qPNo3ujS6P8YuZlnM1VidWElsSxw5LiquNm5svt/87fOH4p3iC+N7F5gvyF1weaHOwvSFpxapLhIsOpZATIhOOJTwQRAqqBaMJfITdyWOCnnCHcJnIi/RNtGI2ENcKh5O8kgqTXqS7JG8NXkkxTOlLOW5hCepkLxMDUzdmzqeFpp2IG0yPTq9MYOSkZBxQqohTZO2Z+pn5mZ2y6xlhbL+xW6Lty8elQfJa7OQrAVZLQq2QqboVFoo1yoHsmdlV2a/zYnKOZarnivN7cyzytuQN5zvn//tEsIS4ZK2pYZLVy0dWOa9rGo5sjxxedsK4xUFK4ZWBqw8uIq2Km3VT6vtV5eufr0mek1rgV7ByoLBtQFr6wtVCuWFfevc1+1dT1gvWd+1YfqGnRs+FYmKrhTbF5cVf9go3HjlG4dvyr+Z3JS0qavEuWTPZtJm6ebeLZ5bDpaql+aXDm4N2dq0Dd9WtO319kXbL5fNKNu7g7ZDuaO/PLi8ZafJzs07P1SkVPRU+lQ27tLdtWHX+G7R7ht7vPY07NXbW7z3/T7JvttVAVVN1WbVZftJ+7P3P66Jqun4lvttXa1ObXHtxwPSA/0HIw6217nU1R3SPVRSj9Yr60cOxx++/p3vdy0NNg1VjZzG4iNwRHnk6fcJ3/ceDTradox7rOEH0x92HWcdL2pCmvKaRptTmvtbYlu6T8w+0dbq3nr8R9sfD5w0PFl5SvNUyWna6YLTk2fyz4ydlZ19fi753GDborZ752PO32oPb++6EHTh0kX/i+c7vDvOXPK4dPKy2+UTV7hXmq86X23qdOo8/pPTT8e7nLuarrlca7nuer21e2b36RueN87d9L158Rb/1tWeOT3dvfN6b/fF9/XfFt1+cif9zsu72Xcn7q28T7xf9EDtQdlD3YfVP1v+3Njv3H9qwHeg89HcR/cGhYPP/pH1jw9DBY+Zj8uGDYbrnjg+OTniP3L96fynQ89kzyaeF/6i/suuFxYvfvjV69fO0ZjRoZfyl5O/bXyl/erA6xmv28bCxh6+yXgzMV70VvvtwXfcdx3vo98PT+R8IH8o/2j5sfVT0Kf7kxmTk/8EA5jz/GMzLdsAAAAgY0hSTQAAeiUAAICDAAD5/wAAgOkAAHUwAADqYAAAOpgAABdvkl/FRgAAAHFJREFUeNpi/P//PwMlgImBQsA44C6gvhfa29v3MzAwOODRc6CystIRbxi0t7fjDJjKykpGYrwwi1hxnLHQ3t7+jIGBQRJJ6HllZaUUKYEYRYBPOB0gBShKwKGA////48VtbW3/8clTnBIH3gCKkzJgAGvBX0dDm0sCAAAAAElFTkSuQmCC"]'), function (i, elem) {
                    var parentId = elem.parentNode.parentNode.getAttribute('data-tt-id');
                    $(document).find('#tblTree > tbody > tr[data-tt-id=' + parentId + ']').trigger('click');
                });
                //-- collapse all element
                $('#commonWOTreeModal').modal('show');
            },
            error: function (xhr) {
                alert('error');
            }
        });
    }
});
function WorksRequestAddOnSuccess(data) {
    if (data.data === "success") {
        localStorage.setItem("workorderstatus", '2');
        localStorage.setItem("workorderstatustext", getResourceValue("spnWorkRequest"));
        if (data.Command === "save") {
            SuccessAlertSetting.text = getResourceValue("spnWoRequestAddSuccessfully");
            swal(SuccessAlertSetting, function () {
                RedirectToPmDetail(data.workOrderID, "overview");
            });
        }
        //V2-928
        //else {
        //    SuccessAlertSetting.text = getResourceValue("spnWoRequestAddSuccessfully");
        //    ResetErrorDiv();
        //    swal(SuccessAlertSetting, function () {
        //        $(document).find('form').trigger("reset");
        //        $(document).find('form').find("select").val("").trigger('change.select2');
        //        $(document).find("#imgChargeToTree").hide();
        //        $(document).find('form').find("select").removeClass("input-validation-error");
        //        $(document).find('form').find("textarea").removeClass("input-validation-error");
        //    });
        //}
    }
    else {
        CloseLoader();
        ShowGenericErrorOnAddUpdate(data);
    }
}
//#endregion

//#region Equipment Hierarchy ModalTree 
$(document).on('click', '#imgChargeToTreeLineItem', function (e) {
    $(this).blur();
    generateWoEquipmentTree(-1);
});
function generateWoEquipmentTree(paramVal) {
    $.ajax({
        url: '/PlantLocationTree/WorkOrderEquipmentHierarchyTree',
        datatype: "json",
        type: "post",
        contenttype: 'application/json; charset=utf-8',
        async: true,
        cache: false,
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find(".cntTree").html(data);
        },
        complete: function () {
            CloseLoader();
            $('#woEquipTreeModal').modal('show');
            treeTable($(document).find('#tblTree'));
            $(document).find('.radSelectWo').each(function () {
                if ($(document).find('#hdnChargeTo').val() == '0' || $(document).find('#hdnChargeTo').val() == '') {

                    if ($(this).data('equipmentid') === equipmentid) {
                        $(this).attr('checked', true);
                    }

                }
                else {

                    if ($(this).data('equipmentid') == $(document).find('#hdnChargeTo').val()) {
                        $(this).attr('checked', true);
                    }

                }

            });
            //-- V2-518 collapse all element
            // looking for the collapse icon and triggered click to collapse
            $.each($(document).find('#tblTree > tbody > tr').find('img[src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAACXBIWXMAAAsTAAALEwEAmpwYAAAKT2lDQ1BQaG90b3Nob3AgSUNDIHByb2ZpbGUAAHjanVNnVFPpFj333vRCS4iAlEtvUhUIIFJCi4AUkSYqIQkQSoghodkVUcERRUUEG8igiAOOjoCMFVEsDIoK2AfkIaKOg6OIisr74Xuja9a89+bN/rXXPues852zzwfACAyWSDNRNYAMqUIeEeCDx8TG4eQuQIEKJHAAEAizZCFz/SMBAPh+PDwrIsAHvgABeNMLCADATZvAMByH/w/qQplcAYCEAcB0kThLCIAUAEB6jkKmAEBGAYCdmCZTAKAEAGDLY2LjAFAtAGAnf+bTAICd+Jl7AQBblCEVAaCRACATZYhEAGg7AKzPVopFAFgwABRmS8Q5ANgtADBJV2ZIALC3AMDOEAuyAAgMADBRiIUpAAR7AGDIIyN4AISZABRG8lc88SuuEOcqAAB4mbI8uSQ5RYFbCC1xB1dXLh4ozkkXKxQ2YQJhmkAuwnmZGTKBNA/g88wAAKCRFRHgg/P9eM4Ors7ONo62Dl8t6r8G/yJiYuP+5c+rcEAAAOF0ftH+LC+zGoA7BoBt/qIl7gRoXgugdfeLZrIPQLUAoOnaV/Nw+H48PEWhkLnZ2eXk5NhKxEJbYcpXff5nwl/AV/1s+X48/Pf14L7iJIEyXYFHBPjgwsz0TKUcz5IJhGLc5o9H/LcL//wd0yLESWK5WCoU41EScY5EmozzMqUiiUKSKcUl0v9k4t8s+wM+3zUAsGo+AXuRLahdYwP2SycQWHTA4vcAAPK7b8HUKAgDgGiD4c93/+8//UegJQCAZkmScQAAXkQkLlTKsz/HCAAARKCBKrBBG/TBGCzABhzBBdzBC/xgNoRCJMTCQhBCCmSAHHJgKayCQiiGzbAdKmAv1EAdNMBRaIaTcA4uwlW4Dj1wD/phCJ7BKLyBCQRByAgTYSHaiAFiilgjjggXmYX4IcFIBBKLJCDJiBRRIkuRNUgxUopUIFVIHfI9cgI5h1xGupE7yAAygvyGvEcxlIGyUT3UDLVDuag3GoRGogvQZHQxmo8WoJvQcrQaPYw2oefQq2gP2o8+Q8cwwOgYBzPEbDAuxsNCsTgsCZNjy7EirAyrxhqwVqwDu4n1Y8+xdwQSgUXACTYEd0IgYR5BSFhMWE7YSKggHCQ0EdoJNwkDhFHCJyKTqEu0JroR+cQYYjIxh1hILCPWEo8TLxB7iEPENyQSiUMyJ7mQAkmxpFTSEtJG0m5SI+ksqZs0SBojk8naZGuyBzmULCAryIXkneTD5DPkG+Qh8lsKnWJAcaT4U+IoUspqShnlEOU05QZlmDJBVaOaUt2ooVQRNY9aQq2htlKvUYeoEzR1mjnNgxZJS6WtopXTGmgXaPdpr+h0uhHdlR5Ol9BX0svpR+iX6AP0dwwNhhWDx4hnKBmbGAcYZxl3GK+YTKYZ04sZx1QwNzHrmOeZD5lvVVgqtip8FZHKCpVKlSaVGyovVKmqpqreqgtV81XLVI+pXlN9rkZVM1PjqQnUlqtVqp1Q61MbU2epO6iHqmeob1Q/pH5Z/YkGWcNMw09DpFGgsV/jvMYgC2MZs3gsIWsNq4Z1gTXEJrHN2Xx2KruY/R27iz2qqaE5QzNKM1ezUvOUZj8H45hx+Jx0TgnnKKeX836K3hTvKeIpG6Y0TLkxZVxrqpaXllirSKtRq0frvTau7aedpr1Fu1n7gQ5Bx0onXCdHZ4/OBZ3nU9lT3acKpxZNPTr1ri6qa6UbobtEd79up+6Ynr5egJ5Mb6feeb3n+hx9L/1U/W36p/VHDFgGswwkBtsMzhg8xTVxbzwdL8fb8VFDXcNAQ6VhlWGX4YSRudE8o9VGjUYPjGnGXOMk423GbcajJgYmISZLTepN7ppSTbmmKaY7TDtMx83MzaLN1pk1mz0x1zLnm+eb15vft2BaeFostqi2uGVJsuRaplnutrxuhVo5WaVYVVpds0atna0l1rutu6cRp7lOk06rntZnw7Dxtsm2qbcZsOXYBtuutm22fWFnYhdnt8Wuw+6TvZN9un2N/T0HDYfZDqsdWh1+c7RyFDpWOt6azpzuP33F9JbpL2dYzxDP2DPjthPLKcRpnVOb00dnF2e5c4PziIuJS4LLLpc+Lpsbxt3IveRKdPVxXeF60vWdm7Obwu2o26/uNu5p7ofcn8w0nymeWTNz0MPIQ+BR5dE/C5+VMGvfrH5PQ0+BZ7XnIy9jL5FXrdewt6V3qvdh7xc+9j5yn+M+4zw33jLeWV/MN8C3yLfLT8Nvnl+F30N/I/9k/3r/0QCngCUBZwOJgUGBWwL7+Hp8Ib+OPzrbZfay2e1BjKC5QRVBj4KtguXBrSFoyOyQrSH355jOkc5pDoVQfujW0Adh5mGLw34MJ4WHhVeGP45wiFga0TGXNXfR3ENz30T6RJZE3ptnMU85ry1KNSo+qi5qPNo3ujS6P8YuZlnM1VidWElsSxw5LiquNm5svt/87fOH4p3iC+N7F5gvyF1weaHOwvSFpxapLhIsOpZATIhOOJTwQRAqqBaMJfITdyWOCnnCHcJnIi/RNtGI2ENcKh5O8kgqTXqS7JG8NXkkxTOlLOW5hCepkLxMDUzdmzqeFpp2IG0yPTq9MYOSkZBxQqohTZO2Z+pn5mZ2y6xlhbL+xW6Lty8elQfJa7OQrAVZLQq2QqboVFoo1yoHsmdlV2a/zYnKOZarnivN7cyzytuQN5zvn//tEsIS4ZK2pYZLVy0dWOa9rGo5sjxxedsK4xUFK4ZWBqw8uIq2Km3VT6vtV5eufr0mek1rgV7ByoLBtQFr6wtVCuWFfevc1+1dT1gvWd+1YfqGnRs+FYmKrhTbF5cVf9go3HjlG4dvyr+Z3JS0qavEuWTPZtJm6ebeLZ5bDpaql+aXDm4N2dq0Dd9WtO319kXbL5fNKNu7g7ZDuaO/PLi8ZafJzs07P1SkVPRU+lQ27tLdtWHX+G7R7ht7vPY07NXbW7z3/T7JvttVAVVN1WbVZftJ+7P3P66Jqun4lvttXa1ObXHtxwPSA/0HIw6217nU1R3SPVRSj9Yr60cOxx++/p3vdy0NNg1VjZzG4iNwRHnk6fcJ3/ceDTradox7rOEH0x92HWcdL2pCmvKaRptTmvtbYlu6T8w+0dbq3nr8R9sfD5w0PFl5SvNUyWna6YLTk2fyz4ydlZ19fi753GDborZ752PO32oPb++6EHTh0kX/i+c7vDvOXPK4dPKy2+UTV7hXmq86X23qdOo8/pPTT8e7nLuarrlca7nuer21e2b36RueN87d9L158Rb/1tWeOT3dvfN6b/fF9/XfFt1+cif9zsu72Xcn7q28T7xf9EDtQdlD3YfVP1v+3Njv3H9qwHeg89HcR/cGhYPP/pH1jw9DBY+Zj8uGDYbrnjg+OTniP3L96fynQ89kzyaeF/6i/suuFxYvfvjV69fO0ZjRoZfyl5O/bXyl/erA6xmv28bCxh6+yXgzMV70VvvtwXfcdx3vo98PT+R8IH8o/2j5sfVT0Kf7kxmTk/8EA5jz/GMzLdsAAAAgY0hSTQAAeiUAAICDAAD5/wAAgOkAAHUwAADqYAAAOpgAABdvkl/FRgAAAHFJREFUeNpi/P//PwMlgImBQsA44C6gvhfa29v3MzAwOODRc6CystIRbxi0t7fjDJjKykpGYrwwi1hxnLHQ3t7+jIGBQRJJ6HllZaUUKYEYRYBPOB0gBShKwKGA////48VtbW3/8clTnBIH3gCKkzJgAGvBX0dDm0sCAAAAAElFTkSuQmCC"]'), function (i, elem) {
                var parentId = elem.parentNode.parentNode.getAttribute('data-tt-id');
                $(document).find('#tblTree > tbody > tr[data-tt-id=' + parentId + ']').trigger('click');
            });
            //-- collapse all element
        },
        error: function (xhr) {
            alert('error');
        }
    });
}
function generatePlantLocationTree(paramVal) {
    $.ajax({
        url: '/PlantLocationTree/PlantLocationTreeLookup',
        datatype: "json",
        type: "post",
        contenttype: 'application/json; charset=utf-8',
        async: true,
        cache: false,
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find(".cntTree").html(data);
        },
        complete: function () {
            CloseLoader();
            $('#woEquipTreeModal').modal('show');
            treeTable($(document).find('#tblTree'));
            $(document).find('.radSelectPl').each(function () {
                if ($(this).data('equipmentid') === equipmentid)
                    $(this).attr('checked', true);
            });
        },
        error: function (xhr) {
            alert('error');
        }
    });
}


function generateWRPlantLocationTree(paramVal) {
    $.ajax({
        url: '/PlantLocationTree/WRPlantLocationTreeLookup',
        datatype: "json",
        type: "post",
        contenttype: 'application/json; charset=utf-8',
        async: true,
        cache: false,
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find(".cntTree").html(data);
        },
        complete: function () {
            CloseLoader();
            $('#commonWOTreeModal').modal('show');
            treeTable($(document).find('#tblTree'));
            $(document).find('.radSelectWRPl').each(function () {
                if ($(this).data('equipmentid') === equipmentid)
                    $(this).attr('checked', true);
            });
        },
        error: function (xhr) {
            alert('error');
        }
    });
}
$(document).on('change', '.radSelect', function () {
    $(document).find('#hdnChargeTo').val('0');
    equipmentid = $(this).data('equipmentid');
    var clientlookupid = $(this).data('clientlookupid').split("(")[0];
    $('#commonWOTreeModal').modal('hide');
    $(document).find('#workOrderModel_ChargeToClientLookupId,#woOnDemandModel_ChargeToClientLookupId,#woDescriptionModel_ChargeToClientLookupId').val(equipmentid).trigger('change');
    $(document).find('#txtChargeTo').val(clientlookupid);
    $(document).find('#txtChargeTo').val(clientlookupid).removeClass('input-validation-error');
});
$(document).on('change', '#workOrderModel_ChargeToClientLookupId', function () {
    var text = $(this).find('option:selected').text();
    var chargeToName = text.split('-')[1].trim();
    if ($('#workOrderModel_ChargeTo_Name').length > 0) {
        $('#workOrderModel_ChargeTo_Name').val(chargeToName);
    }
});
$(document).on('change', '.radSelectPl', function () {
    equipmentid = $(this).data('equipmentid');
    var clientlookupid = $(this).data('clientlookupid').split("(")[0];
    $('#woEquipTreeModal').modal('hide');
    $(document).find('#woOnDemandModel_ChargeToClientLookupId').val(equipmentid).trigger('change');
    $(document).find('#txtChargeTo').val(clientlookupid);

});

$(document).on('change', '.radSelectWRPl', function () {
    equipmentid = $(this).data('equipmentid');
    var clientlookupid = $(this).data('clientlookupid').split("(")[0];
    $('#commonWOTreeModal').modal('hide');
    $(document).find('#woRequestModel_ChargeToClientLookupId').val(equipmentid).trigger('change');
    $(document).find('#txtChargeTo').val(clientlookupid);

});
function ChargeTypeCheck() {
    var chargeTyp = $(document).find('#workOrderModel_ChargeType').val();
    if (typeof chargeTyp === 'undefined') {
        $(document).find('#workOrderModel_ChargeToId').hide();
    }
    if (chargeTyp == "") {
        $("#imgChargeToTreeLineItem").hide();
    }
    else {
        if (chargeTyp == "Equipment") {
            $("#imgChargeToTreeLineItem").show();
        }
        else {
            $("#imgChargeToTreeLineItem").hide();
        }
    }
}
//#endregion

//#region Cancel, Print and Complete


var workOrderIdCard;
$(document).on('click', '#actionCompleteWO,.wobtngrdcomplete', function () {
    if ($(document).find('#actionCompleteWO').length > 0) {
        workOrderIdCard = $(document).find('#workOrderModel_WorkOrderId').val();
    }
    else {
        if (layoutType == 1) {
            workOrderIdCard = $(this).find('span').attr('id');
        }
        else {
            workOrderIdCard = $(document).find('#workOrderModel_WorkOrderId').val();
        }
    }
    var hdnIspopupShow = $(document).find('#hdnFoodSafetyPopup').val();
    var htmlMsg = '';
    if (hdnIspopupShow == 'FOOD SERVICES') {
        $.ajax({
            url: '/WorkOrder/GetFoodServicesMessage',
            type: "GET",
            datatype: "json",
            success: function (data) {
                htmlMsg = data.data;
                swal({
                    title: "Food Safety Conditions",
                    text: htmlMsg,
                    html: true,
                    type: "warning",
                    showCancelButton: true,
                    closeOnConfirm: false,
                    confirmButtonClass: "btn-sm btn-primary",
                    cancelButtonClass: "btn-sm",
                    confirmButtonText: getResourceValue("CancelAlertYes"),
                    cancelButtonText: getResourceValue("CancelAlertNo")
                }, function () {
                    CompleteWorkorderJob();
                });
            },
            complete: function () {
            },
            error: function () {
            }
        });
    }
    else {
        CompleteWorkorderJob();
    }
});

function CompleteWorkorderJob() {
    $.ajax({
        url: '/WorkOrder/CompleteWorkOrder',
        data: {
            workorderId: workOrderIdCard,
            CompleteComments: ""
        },
        type: "GET",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data == "success") {
                $(document).find('#completeModalDetailsPageSingle').modal('hide');
                SuccessAlertSetting.text = getResourceValue("WorkOrderCompleteAlert");
                if ($(document).find('.crdVu').length == 0) {
                    assignedUsers = "";
                    localStorage.setItem('ASSIGNEDUSERSLIST', assignedUsers);
                    personnelList = "";
                }
                swal(SuccessAlertSetting, function () {
                    if (layoutType == 2) {
                        RedirectToPmDetail(workOrderIdCard, "overview");
                    }
                    else {
                        if ($(document).find('.summaryview').length > 0) {
                            RedirectToPmDetail(workOrderIdCard, "overview");
                        }
                        else {
                            ShowCardView();
                        }
                    }
                });
            }
            else {
                $(document).find('#completeModalDetailsPageSingle').modal('hide');
                GenericSweetAlertMethod(data);
            }
        },
        error: function () {
            CloseLoader();
        },
        complete: function () {
            RemoveUpdateAreaInfo();
            CloseLoader();
        }
    });
}
$(document).on('click', '#CompleteWoCheckList', function () {

    if (SelectedWoIdToCancel.length < 1) {
        ShowGridItemSelectionAlert();
        return false;
    }
    else {
        var found = false;
        for (var i = 0; i < SelectedWoIdToCancel.length; i++) {
            if (SelectedWoIdToCancel[i].Status === 'Approved' || SelectedWoIdToCancel[i].Status === 'Scheduled') /*SelectedWoIdToCancel[i].Status === 'WorkRequest' v2-313*/ {
                found = true;
                break;
            }

        }
        if (found === false) {
            var errorMessage = getResourceValue("WorkOrderBatchCompletionAlert");
            ShowErrorAlert(errorMessage);
            return false;
        }
        else {
            var hdnIspopupShow = $(document).find('#hdnFoodSafetyPopup').val();
            var htmlMsg = '';
            if (hdnIspopupShow == 'FOOD SERVICES') {
                $.ajax({
                    url: '/WorkOrder/GetFoodServicesMessage',
                    type: "GET",
                    datatype: "json",
                    success: function (data) {
                        htmlMsg = data.data;
                        swal({
                            title: getResourceValue("FoodSafetyAlert"),
                            text: htmlMsg,
                            html: true,
                            type: "warning",
                            showCancelButton: true,
                            closeOnConfirm: false,
                            confirmButtonClass: "btn-sm btn-primary",
                            cancelButtonClass: "btn-sm",
                            confirmButtonText: getResourceValue("CancelAlertYes"),
                            cancelButtonText: getResourceValue("CancelAlertNo")
                        }, function () {
                            CompleteWorkorderBatchJob();
                        });
                    },
                    complete: function () { },
                    error: function () { }
                });
            }
            else {
                CompleteWorkorderBatchJob();
            }
        }
    }
});

function CompleteWorkorderBatchJob() {
    for (var i = 0; i < SelectedWoIdToCancel.length; i++) {
        WoIdListToComplete.push(SelectedWoIdToCancel[i].WorkOrderId);
    }
    var jsonResult = {
        "list": SelectedWoIdToCancel,
        "cancelreason": '',
        "comments": ""
    };
    $.ajax({
        url: '/WorkOrder/CompleteWorkOrderBatch',
        type: "POST",
        datatype: "json",
        data: JSON.stringify(jsonResult),
        contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.data == "success") {
                SuccessAlertSetting.text = getResourceValue("WorkOrderCompleteAlert");
                swal(SuccessAlertSetting, function () {
                    if (layoutType !== 2) {
                        ShowCardView();
                    }
                });
            }
            else {
                GenericSweetAlertMethod(data.data);
            }
            SelectedWoIdToCancel = [];
            WoIdListToComplete = [];
            $(".updateArea").hide();
            $(".actionBar").fadeIn();
            $(document).find('.chksearch').prop('checked', false);
            $(document).find('.itemcount').text(0);
            pageno = workOrdersSearchdt.page.info().page;
            workOrdersSearchdt.page(pageno).draw('page');
        },
        complete: function () {
            SelectedWoIdToCancel = [];
            WoIdListToComplete = [];
            $(document).find('#txtcompletecommentsbatch').val("").trigger('change');
            $(document).find('#completeModalDetailsPage').modal('hide');
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', '#actionCancelWO,.cancel_workorder,.wobtngrdcancel', function () {
    $(document).find('#txtCancelReasonSelect').val("").trigger("change.select2");
    $(document).find('#txtcancelcomments').val("").trigger('change');
    $(document).find('.select2picker').select2({});
    if ($(document).find('#actionCancelWO').length > 0) {
        workOrderIdCard = $(document).find('#workOrderModel_WorkOrderId').val();
        $(document).find('#cancelModalDetailsPage').modal('show');
    }
    else {
        if (layoutType == 1) {
            workOrderIdCard = $(this).find('span').attr('id');
            var woStatus = $(this).find('span').attr('status');
            if (woStatus != "Canceled" && woStatus != "Complete") {
                $(document).find('#cancelModalDetailsPage').modal('show');
            }
        }
        else {
            workOrderIdCard = $(document).find('#workOrderModel_WorkOrderId').val();
            $(document).find('#cancelModalDetailsPage').modal('show');
        }
    }
});
$(document).on('click', '#cancelWorkOrderJob', function () {
    var cancelreason = $(document).find('#txtCancelReasonSelect').val();
    var comments = $(document).find('#txtcancelcomments').val();
    $.ajax({
        url: '/WorkOrder/CancelJob',
        data: { WorkorderId: workOrderIdCard, CancelReason: cancelreason, Comments: comments },
        type: "GET",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#cancelModalDetailsPage').modal('hide');
            if (data.data == "success") {
                SuccessAlertSetting.text = getResourceValue("WorkOrderCancelsuccessAlert");
                if ($(document).find('.crdVu').length == 0) {
                    assignedUsers = "";
                    localStorage.setItem('ASSIGNEDUSERSLIST', assignedUsers);
                    personnelList = "";
                }
                swal(SuccessAlertSetting, function () {
                    CloseLoader();
                    if (layoutType == 2) {
                        RedirectToPmDetail(workOrderIdCard, "overview");
                    }
                    else {
                        if ($(document).find('.summaryview').length > 0) {
                            RedirectToPmDetail(workOrderIdCard, "overview");
                        }
                        else {
                            ShowCardView();
                        }
                    }
                });
            }
            else {
                GenericSweetAlertMethod(data.data);
                pageno = workOrdersSearchdt.page.info().page;
                workOrdersSearchdt.page(pageno).draw('page');
            }
        },
        complete: function () {
            RemoveUpdateAreaInfo();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', '#btnCancelWolist', function () {
    if (SelectedWoIdToCancel.length < 1) {
        ShowGridItemSelectionAlert();
        return false;
    }
    else {
        var found = false;
        for (var i = 0; i < SelectedWoIdToCancel.length; i++) {
            if (SelectedWoIdToCancel[i].Status == 'Approved' || SelectedWoIdToCancel[i].Status == 'Scheduled') {
                found = true;
                break;
            }
        }
        if (found === false) {
            var errorMessage = getResourceValue("CancelCompleteWorkOrderAlert");
            ShowErrorAlert(errorMessage);
            return false;
        }
        $.ajax({
            url: '/WorkOrder/PopulateCancelReasonDropdown',
            type: "GET",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                $("#txtCancelReasonBatchSelect").empty();
                $("#txtCancelReasonBatchSelect").append("<option value=''>" + "--Select--" + "</option>");
                for (var i = 0; i < data.cancelReasonList.length; i++) {
                    var id = data.cancelReasonList[i].Value;
                    var name = data.cancelReasonList[i].Text;
                    $("#txtCancelReasonBatchSelect").append("<option value='" + id + "'>" + name + "</option>");
                }
            },
            complete: function () {
                $(document).find('.select2picker').select2({});
                $(document).find('#txtcancelBatchcomments').val("").trigger('change');
                $(document).find('#cancelModalIndexPage').modal('show');
                CloseLoader();
            }
        });
    }
});
$(document).on('click', '#cancelWorkOrderListJob', function () {
    var v = $(document).find('#txtCancelReasonBatchSelect').val();
    var jsonResult = {
        "list": SelectedWoIdToCancel,
        "cancelreason": $('#txtCancelReasonBatchSelect').val(),
        "comments": $('#txtcancelBatchcomments').val()
    };
    $.ajax({
        url: '/WorkOrder/CancelWoList',
        type: "POST",
        datatype: "json",
        data: JSON.stringify(jsonResult),
        contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#cancelModalIndexPage').modal('hide');
            if (data.data == "success") {
                SuccessAlertSetting.text = getResourceValue("WorkOrderCancelsuccessAlert");
                swal(SuccessAlertSetting, function () {
                    if (layoutType !== 2) {
                        ShowCardView();
                    }
                });
            }
            else {
                GenericSweetAlertMethod(data.data);
            }
            $(".updateArea").hide();
            $(".actionBar").fadeIn();
            $(document).find('.chksearch').prop('checked', false);
            $(document).find('.itemcount').text(0);
            SelectedWoIdToCancel = [];
            pageno = workOrdersSearchdt.page.info().page;
            workOrdersSearchdt.page(pageno).draw('page');
        },
        complete: function () {
            CloseLoader();
            $(document).find('#txtcancelBatchcomments').val("").trigger('change');
            $('#cancelModalIndexPage').modal('hide');
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', '.wobtngrdschedule', function () {
    var workorderid = $(this).attr('id');
    var scheduledStartDate = $(this).attr('scheduledStartDate');
    var scheduledDuration = $(this).attr('ScheduledDuration');
    $(document).find('#woScheduleModel_ScheduledDuration').val(scheduledDuration);
    $(document).find('#woScheduleModel_WorkOrderId').val(parseInt(workorderid));
    $(document).find('#Schedulestartdate').val(scheduledStartDate);
    $(document).find('.select2picker').select2({});
    $.ajax({
        "url": '/WorkOrder/PopulatePopUpJs',
        "data": { workOrderId: workorderid },
        "type": 'GET',
        "dataType": 'json',
        "beforeSend": function () { ShowLoader() },
        "success": function (data) {
            var assgList = new Array();
            for (var i = 0; i < data.AssignedList.length; i++) {
                assgList[i] = data.AssignedList[i].Value;
            }
            $('#ddlUser').val(assgList).trigger("change.select2");
        },
        "complete": function () {
            $(window).scrollTop(0);
            $(document).find('.dtpicker').datepicker({
                changeMonth: true,
                changeYear: true,
                "dateFormat": "mm/dd/yy",
                autoclose: true
            }).inputmask('mm/dd/yyyy');
            $(document).find('#ScheduleModal').modal({ backdrop: 'static', keyboard: false });
            if ($('#ui-datepicker-div').hasClass('ui-downtime')) {
                $('#ui-datepicker-div').removeClass('ui-downtime');
            }
            CloseLoader();
            SetControls();
        }
    });
});

$(document).on('click', '.wobtngrdremoveschedule', function () {
    var workOrderID = $(this).attr('id');
    swal({
        title: getResourceValue("spnAreyousure"),
        text: "Please confirm to remove schedule.",
        type: "warning",
        showCancelButton: true,
        closeOnConfirm: false,
        confirmButtonClass: "btn-sm btn-primary",
        cancelButtonClass: "btn-sm",
        confirmButtonText: getResourceValue("CancelAlertYes"),
        cancelButtonText: getResourceValue("CancelAlertNo")
    }, function () {
        ShowLoader();
        $.ajax({
            url: '/WorkOrder/RemoveScheduleWO',
            data: {
                workorderId: workOrderID
            },
            type: "GET",
            datatype: "json",
            success: function (data) {
                if (data.data == "success") {
                    SuccessAlertSetting.text = getResourceValue("spnWorkorderSuccessfullyRemovedScheduled");
                    swal(SuccessAlertSetting, function () {
                        if (layoutType == 2) {
                            RedirectToPmDetail(workOrderID, "overview");
                        }
                        else {
                            ShowCardView();
                        }
                    }
                    );
                }
                else {
                    GenericSweetAlertMethod(data.error);
                }
            }
            , complete: function () {
                CloseLoader();
            },
            error: function () {
                CloseLoader();
            }
        });
    });
});

$(document).on('click', '#ScheduleWoCheckList', function () {
    if (SelectedWoIdToCancel.length < 1) {
        ShowGridItemSelectionAlert();
        return false;
    }
    else {
        var found = false;
        for (var i = 0; i < SelectedWoIdToCancel.length; i++) {
            if (SelectedWoIdToCancel[i].Status === 'Approved' || SelectedWoIdToCancel[i].Status === 'Scheduled') {
                found = true;
                break;
            }

        }
        if (found == false) {
            var errorMessage = getResourceValue("WorkOrderApproveScheduleAlert");
            ShowErrorAlert(errorMessage);
            return false;
        }
        else {
            for (var i = 0; i < SelectedWoIdToCancel.length; i++) {
                SelectedWoIdToSchedule.push(SelectedWoIdToCancel[i].WorkOrderId);
                SelectedLookupIdToSchedule.push(SelectedWoIdToCancel[i].ClientLookupId);
                SelectedStatusSchedule.push(SelectedWoIdToCancel[i].Status);
            }
            ShowbtnLoader("schedulesortmenu");
            HidebtnLoader("schedulesortmenu");
            $(document).find('#Schedulestartdate').val("").trigger('change');
            $('#ddlUser').val("").trigger("change.select2");
            $(document).find('#woScheduleModel_WorkOrderIds').val(SelectedWoIdToSchedule);
            $(document).find('#woScheduleModel_ClientLookupIds').val(SelectedLookupIdToSchedule);
            $(document).find('#woScheduleModel_Status').val(SelectedStatusSchedule);
            $(document).find('.select2picker').select2({});
            $(document).find('#ScheduleModal').modal('show');
        }
    }
});

$(document).on('click', '#RemoveScheduleWoCheckList', function () {
    if (SelectedWoIdToCancel.length < 1) {
        ShowGridItemSelectionAlert();
        return false;
    }
    else {
        var found = false;
        for (var i = 0; i < SelectedWoIdToCancel.length; i++) {
            if (SelectedWoIdToCancel[i].Status === 'Scheduled') {
                found = true;
                break;
            }

        }
        if (found === false) {
            var errorMessage = getResourceValue("WorkOrderRemovescheduleScheduledAlert");
            ShowErrorAlert(errorMessage);
            return false;
        }
        else {
            for (var i = 0; i < SelectedWoIdToCancel.length; i++) {
                SelectedWoIdToSchedule.push(SelectedWoIdToCancel[i].WorkOrderId);
                SelectedLookupIdToSchedule.push(SelectedWoIdToCancel[i].WorkOrderId);
            }
            ShowbtnLoader("schedulesortmenu");
            HidebtnLoader("schedulesortmenu");
            $(document).find('#woScheduleModel_WorkOrderIds').val(SelectedWoIdToSchedule);
            $(document).find('#woScheduleModel_ClientLookupIds').val(SelectedLookupIdToSchedule);


            swal({
                title: getResourceValue("spnAreyousure"),
                text: "Please confirm to remove scheduled.",
                type: "warning",
                showCancelButton: true,
                closeOnConfirm: false,
                confirmButtonClass: "btn-sm btn-primary",
                cancelButtonClass: "btn-sm",
                confirmButtonText: getResourceValue("CancelAlertYes"),
                cancelButtonText: getResourceValue("CancelAlertNo")
            }, function () {
                ShowLoader();
                var jsonResult = {
                    "list": SelectedWoIdToCancel
                }
                $.ajax({
                    url: '/WorkOrder/RemoveScheduleWoList',
                    type: "POST",
                    datatype: "json",
                    data: JSON.stringify(jsonResult),
                    contentType: 'application/json; charset=utf-8',
                    beforeSend: function () {
                        ShowLoader();
                    },
                    success: function (data) {
                        if (data.data == "success") {
                            SuccessAlertSetting.text = getResourceValue("spnWorkorderSuccessfullyRemovedScheduled");
                            swal(SuccessAlertSetting, function () {
                                if (layoutType !== 2) {
                                    ShowCardView();

                                }
                            });
                        }
                        else {
                            GenericSweetAlertMethod(data.data);

                        }
                        $(".updateArea").hide();
                        $(".actionBar").fadeIn();
                        $(document).find('.chksearch').prop('checked', false);
                        $(document).find('.itemcount').text(0);
                        SelectedWoIdToCancel = [];
                        SelectedLookupIdToSchedule = [];
                        SelectedWoIdToSchedule = [];
                        pageno = workOrdersSearchdt.page.info().page;
                        workOrdersSearchdt.page(pageno).draw('page');
                    },
                    complete: function () {
                        CloseLoader();
                    },
                    error: function () {
                        CloseLoader();
                    }
                });
            });
        }
    }
});

$(document).on('click', '#ApproveWoCheckList', function () {

    if (SelectedWoIdToCancel.length < 1) {
        ShowGridItemSelectionAlert();
        return false;
    }
    else {
        var found = false;
        for (var i = 0; i < SelectedWoIdToCancel.length; i++) {
            var stext = SelectedWoIdToCancel[i].Status;

            if (stext === 'WorkRequest' || stext === 'AwaitApproval') {
                found = true;
                break;
            }
        }
        if (found === false) {
            var errorMessage = getResourceValue("WorkOrderApproveWorkRequestAlert");
            ShowErrorAlert(errorMessage);
            return false;
        }
        else {
            for (var i = 0; i < SelectedWoIdToCancel.length; i++) {
                SelectedWoIdToApprove.push(SelectedWoIdToCancel[i].WorkOrderId);
                SelectedLookupIdToApprove.push(SelectedWoIdToCancel[i].ClientLookupId);
            }
            ShowbtnLoader("approvesortmenu");
            HidebtnLoader("approvesortmenu");
            $(document).find('#approveWorkOrderModel_WorkOrderIds').val(SelectedWoIdToApprove);
            $(document).find('#approveWorkOrderModel_ClientLookupIds').val(SelectedLookupIdToApprove);
            swal({
                title: getResourceValue("spnAreyousure"),
                text: getResourceValue("spnPleaseConfirmToApprove"),
                type: "warning",
                showCancelButton: true,
                closeOnConfirm: false,
                confirmButtonClass: "btn-sm btn-primary",
                cancelButtonClass: "btn-sm",
                confirmButtonText: getResourceValue("CancelAlertYes"),
                cancelButtonText: getResourceValue("CancelAlertNo")
            }, function () {
                ShowLoader();
                var jsonResult = {
                    "list": SelectedWoIdToCancel
                }
                $.ajax({
                    url: '/WorkOrder/ApproveWoList',
                    type: "POST",
                    datatype: "json",
                    data: JSON.stringify(jsonResult),
                    contentType: 'application/json; charset=utf-8',
                    beforeSend: function () {
                        ShowLoader();
                    },
                    success: function (data) {
                        if (data.data == "success") {
                            SuccessAlertSetting.text = getResourceValue("spnWorkorderSuccessfullyApprovedAlert");
                            swal(SuccessAlertSetting, function () {
                                if (layoutType !== 2) {
                                    ShowCardView();

                                }
                            });
                        }
                        else {
                            GenericSweetAlertMethod(data.data);

                        }
                        $(".updateArea").hide();
                        $(".actionBar").fadeIn();
                        $(document).find('.chksearch').prop('checked', false);
                        $(document).find('.itemcount').text(0);
                        SelectedWoIdToCancel = [];
                        SelectedLookupIdToApprove = [];
                        SelectedWoIdToApprove = [];
                        pageno = workOrdersSearchdt.page.info().page;
                        workOrdersSearchdt.page(pageno).draw('page');
                    },
                    complete: function () {
                        CloseLoader();
                    },
                    error: function () {
                        CloseLoader();
                    }
                });
            });
        }
    }
});

$(document).on('click', '.approveDropMenu  #PlanningCheckList', function () {

    if (SelectedWoIdToCancel.length < 1) {
        ShowGridItemSelectionAlert();
        return false;
    }
    else {
        var found = false;
        for (var i = 0; i < SelectedWoIdToCancel.length; i++) {
            if (SelectedWoIdToCancel[i].Status === 'WorkRequest') {
                found = true;
                break;
            }
        }
        if (found === false) {
            var errorMessage = getResourceValue("WorkOrderPlannerWorkRequestAlert");
            ShowErrorAlert(errorMessage);
            return false;
        }


        $.ajax({
            url: '/WorkOrder/PopulatePersonDropdown',
            type: "GET",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                $("#personnelselect").empty();
                $("#personnelselect").append("<option value=''>" + "--Select--" + "</option>");
                for (var i = 0; i < data.plannerList.length; i++) {
                    var id = data.plannerList[i].Value;
                    var name = data.plannerList[i].Text;
                    $("#personnelselect").append("<option value='" + id + "'>" + name + "</option>");
                }
            },
            complete: function () {
                $(document).find('.select2picker').select2({});
                $(document).find('#planningModalIndexPage').modal({ backdrop: 'static', keyboard: false });
                $(document).find('#planningModalIndexPage').modal('show');
                CloseLoader();
            }
        });
    }
});

$(document).on('click', '.cancelJobBtn', function () {

    var areaddescribedby = $(document).find("#personnelselect").attr('aria-describedby');
    if (typeof areaddescribedby !== 'undefined') {
        $('#' + areaddescribedby).hide();
    }
    $(document).find("#personnelselect").removeClass("input-validation-error");

});

$(document).on('change', '#personnelselect', function () {
    var planner = $('#personnelselect').val();

    if (planner == undefined || planner == "") {
        var areaddescribedby = $(document).find("#personnelselect").attr('aria-describedby');
        if (typeof areaddescribedby !== 'undefined') {
            $(document).find("#personnelselect").addClass("input-validation-error");
            $('#' + areaddescribedby).show();
        }
    }
    else {
        var areaddescribedby = $(document).find("#personnelselect").attr('aria-describedby');
        if (typeof areaddescribedby !== 'undefined') {
            $(document).find("#personnelselect").removeClass("input-validation-error");
            $('#' + areaddescribedby).hide();
        }
    }
});

$(document).on('click', '#planningWorkOrderListJob', function () {

    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse('#planningForm');
    $('#planningForm').validate();
    var isvalid = $('#planningForm').valid();
    if (!isvalid) {
        $('#planningForm').submit();
    }


    if (isvalid) {
        var jsonResult = {
            "list": SelectedWoIdToCancel,
            "Planner_PersonnelId": parseInt($('#personnelselect').val())
        }

        $.ajax({
            url: '/WorkOrder/PlanningWoList',
            type: "POST",
            datatype: "json",
            data: JSON.stringify(jsonResult),
            contentType: 'application/json; charset=utf-8',
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.data == "success") {
                    //needs to update success message
                    SuccessAlertSetting.text = getResourceValue("spnWorkorderPlannerUpdatedAlert");
                    swal(SuccessAlertSetting, function () {
                        $(document).find('#planningModalIndexPage').modal('hide');
                        if (layoutType !== 2) {
                            ShowCardView();
                        }
                    });
                }
                else {
                    GenericSweetAlertMethod(data.data);
                }
                $(".updateArea").hide();
                $(".actionBar").fadeIn();
                $(document).find('.chksearch').prop('checked', false);
                $(document).find('.itemcount').text(0);
                SelectedWoIdToCancel = [];
                SelectedLookupIdToApprove = [];
                SelectedWoIdToApprove = [];
                pageno = workOrdersSearchdt.page.info().page;
                workOrdersSearchdt.page(pageno).draw('page');
            },
            complete: function () {
                CloseLoader();
            },
            error: function () {
                CloseLoader();
            }
        });
    }
});


$(document).on('click', '.plannerDetailCardViewWo', function () {

    var workorderid = $(this).attr('id');
    $('#hdnworkorderId').val(workorderid)

    $.ajax({
        url: '/WorkOrder/PopulatePersonDropdown',
        type: "GET",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $("#personnelselect").empty();
            $("#personnelselect").append("<option value=''>" + "--Select--" + "</option>");
            for (var i = 0; i < data.plannerList.length; i++) {
                var id = data.plannerList[i].Value;
                var name = data.plannerList[i].Text;
                $("#personnelselect").append("<option value='" + id + "'>" + name + "</option>");
            }
        },
        complete: function () {
            $(document).find('.select2picker').select2({});
            $(document).find('#planningWorkOrderListJob').attr('id', 'btnworkorderPlannerCardView');
            $(document).find('#planningModalIndexPage').modal({ backdrop: 'static', keyboard: false });
            $(document).find('#planningModalIndexPage').modal('show');
            CloseLoader();
        }
    });

});

$(document).on('click', "#btnworkorderPlannerCardView", function (e) {
    var workOrderID = $('#hdnworkorderId').val();

    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse('#planningForm');
    $('#planningForm').validate();
    var isvalid = $('#planningForm').valid();
    if (!isvalid) {
        $('#planningForm').submit();
    }

    if (isvalid) {

        var jsonResult =
        {
            WorkorderId: workOrderID,
            Planner_PersonnelId: $('#personnelselect').val()
        }

        $.ajax({
            url: '/WorkOrder/PlanningWo',
            type: "POST",
            datatype: "json",
            data: JSON.stringify(jsonResult),
            contentType: 'application/json; charset=utf-8',
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.data == "success") {
                    SuccessAlertSetting.text = getResourceValue("spnWorkorderPlannerUpdatedAlert");
                    swal(SuccessAlertSetting, function () {
                        $(document).find('#planningModalIndexPage').modal('hide');
                        ShowCardView();
                    });
                }
                else {
                    GenericSweetAlertMethod(data.data);
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
});


$(document).on('click', '.ApproveDetailsCardViewWO', function () {
    var workOrderID = $(this).attr('id');
    swal({
        title: getResourceValue("spnAreyousure"),
        text: getResourceValue("spnPleaseConfirmToApprove"),
        type: "warning",
        showCancelButton: true,
        closeOnConfirm: false,
        confirmButtonClass: "btn-sm btn-primary",
        cancelButtonClass: "btn-sm",
        confirmButtonText: getResourceValue("CancelAlertYes"),
        cancelButtonText: getResourceValue("CancelAlertNo")
    }, function () {
        ShowLoader();
        $.ajax({
            url: '/WorkOrder/ApproveWO',
            data: {
                workorderId: workOrderID
            },
            type: "GET",
            datatype: "json",
            success: function (data) {
                if (data.data == "success") {
                    SuccessAlertSetting.text = getResourceValue("spnWorkorderSuccessfullyApprovedAlert");
                    swal(SuccessAlertSetting, function () {
                        ShowCardView();
                    }
                    );
                }
                else {
                    GenericSweetAlertMethod(data.error);
                }
            }
            , complete: function () {
                CloseLoader();
            },
            error: function () {
                CloseLoader();
            }
        });
    });
});
$(document).on('click', '#DenyWoCheckList', function () {
    if (SelectedWoIdToCancel.length < 1) {
        ShowGridItemSelectionAlert();
        return false;
    }
    else {
        var found = false;
        for (var i = 0; i < SelectedWoIdToCancel.length; i++) {
            if (SelectedWoIdToCancel[i].Status === 'WorkRequest') {
                found = true;
                break;
            }

        }
        if (found === false) {
            var errorMessage = getResourceValue("WorkOrderDenyWorkRequestAlert");
            ShowErrorAlert(errorMessage);
            return false;
        }
        else {
            for (var i = 0; i < SelectedWoIdToCancel.length; i++) {
                SelectedWoIdToDeny.push(SelectedWoIdToCancel[i].WorkOrderId);
                SelectedLookupIdToDeny.push(SelectedWoIdToCancel[i].WorkOrderId);
            }
            ShowbtnLoader("approvesortmenu");
            HidebtnLoader("approvesortmenu");
            $(document).find('#woDenymodel_WorkOrderIds').val(SelectedWoIdToDeny);
            $(document).find('#woDenymodel_ClientLookupIds').val(SelectedLookupIdToDeny);
            $(document).find('#txtdenysearchcomments').val("").trigger('change');
            $(document).find('#denyModalSearchPage').modal('show');
        }
    }
});

$(document).on('click', '.DenyDetailsCardViewWO', function () {
    workOrderIdCard = $(this).attr('id');
    $(document).find('#denyModalDetailsPage').modal('show');
});
$(document).on('click', '#denyWorkOrderBatchJob', function () {
    var workOrderID = $(document).find('#workOrderModel_WorkOrderId').val();
    var comments = $('#txtdenysearchcomments').val();
    if (comments === "") {
        errormsgtitle = getResourceValue("spnNoDenyComments");
        errormsgtext = getResourceValue("spnPleaseEnterDenyComments");
    }
    if (comments !== "") {
        var jsonResult = {
            "list": SelectedWoIdToCancel,
            "comments": comments
        }
        $.ajax({
            url: '/WorkOrder/DenyWoList',
            type: "POST",
            datatype: "json",
            data: JSON.stringify(jsonResult),
            contentType: 'application/json; charset=utf-8',
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                $('#denyModalDetailsPage').modal('hide');
                if (data.data == "success") {
                    SuccessAlertSetting.text = getResourceValue("spnWorkorderSuccessfullyDeniedAlert");
                    swal(SuccessAlertSetting, function () {
                        if (layoutType !== 2) {
                            ShowCardView();
                        }
                    });
                }
                else {
                    GenericSweetAlertMethod(data.data);
                }
                $(".updateArea").hide();
                $(".actionBar").fadeIn();
                $(document).find('.chksearch').prop('checked', false);
                $(document).find('.itemcount').text(0);
                SelectedWoIdToCancel = [];
                SelectedLookupIdToDeny = [];
                SelectedWoIdToDeny = [];
                pageno = workOrdersSearchdt.page.info().page;
                workOrdersSearchdt.page(pageno).draw('page');
            },
            complete: function () {
                CloseLoader();
                $(document).find('#txtdenysearchcomments').val("").trigger('change');
                $('#denyModalSearchPage').modal('hide');
            },
            error: function () {
                CloseLoader();
            }
        });
    }
    else {
        swal({
            title: errormsgtitle,
            text: errormsgtext,
            type: "warning",
            showCancelButton: false,
            confirmButtonClass: "btn-sm btn-danger",
            cancelButtonClass: "btn-sm",
            confirmButtonText: getResourceValue("SaveAlertOk"),
            cancelButtonText: getResourceValue("CancelAlertNo")
        });
    }
});
function RemoveUpdateAreaInfo() {
    $(".updateArea").hide();
    $(".actionBar").fadeIn();
    $(document).find('.chksearch').prop('checked', false);
    $(document).find('.itemcount').text(0);
    SelectedWoIdToCancel = [];
    WoIdListToComplete = [];
    SelectedLookupIdToSchedule = [];
    SelectedStatusSchedule = [];
    assgList = [];
    SelectedPersonnelIdSchedule = [];
    SelectedLookupIdToApprove = [];
    SelectedLookupIdToDeny = [];
    SelectedWoIdToSchedule = [];
    SelectedWoIdToApprove = [];
    SelectedWoIdToDeny = [];
}
//#region Clear Validation Error
$(document).on('click', '.btncancelschedule', function () {
    var areaChargeToId = $(document).find("#ddlUser").attr('aria-describedby');
    $('#' + areaChargeToId).hide();
    $(document).find("#Schedulestartdate").removeClass("input-validation-error");
});

//#endregion
function WoScheduleAddOnSuccess(data) {
    $('#ScheduleModal').modal('hide');
    if (data.data === "success") {
        SuccessAlertSetting.text = getResourceValue("ScheduleUpdateAlert");
        swal(SuccessAlertSetting, function () {
            CloseLoader();
            if ($(document).find('#ActiveCard').length > 0) {
                assgList = [];
                $(document).find('#Schedulestartdate').val("").trigger('change');
                $('#ddlUser').val("").trigger("change.select2");
                if (layoutType == 2) {
                    pageno = workOrdersSearchdt.page.info().page;
                    workOrdersSearchdt.page(pageno).draw('page');
                }
                else {
                    ShowCardView();
                }
            }
            else {
                assgList = [];
                $(document).find('#Schedulestartdate').val("").trigger('change');
                $('#ddlUser').val("").trigger("change.select2");
                RedirectToPmDetail(data.workorderid, "overview");
            }
        });
    }
    else {
        CloseLoader();
        GenericSweetAlertMethod(data.data);
        pageno = workOrdersSearchdt.page.info().page;
        workOrdersSearchdt.page(pageno).draw('page');
    }
    RemoveUpdateAreaInfo();
    $(document).find('#Schedulestartdate').val("").trigger('change');
    $('#ddlUser').val(null).trigger("change.select2");
    $(document).find('#ScheduleModal').modal('hide');
}

$(document).on('click', '.cleardesctxt', function () {
    $(document).find('#txtcancelcomments').val("").trigger('change');
});
$(document).on('click', '#printWO,#printNoEditWO', function () {
    var WOArray = [];
    var workorderId = $(document).find('#workOrderModel_WorkOrderId').val();
    var clientLookupId = $(document).find('#workOrderModel_ClientLookupId').val();
    var status = $(document).find('#workOrderModel_Status').val();
    var item = new PartNotInInventorySelectedItem(workorderId, clientLookupId, status);
    WOArray.push(item);
    var jsonResult = {
        "list": WOArray,
        "cancelreason": "",
        "comments": "",
        "PrintingCountConnectionID": 0
    };
    $.ajax({
        url: '/WorkOrder/SetPrintWoListFromIndex',
        data: JSON.stringify(jsonResult),
        type: "POST",
        datatype: "json",
        contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (result) {
            if (result.success) {
                window.open("/WorkOrder/GenerateWorkOrderPrint", "_blank");
            }
        },
        complete: function () {
            CloseLoader();
            WOArray = [];
        }
    });
});
$(document).on('click', '#PrintWoCheckListDevExpress', function () {
    if (SelectedWoIdToCancel.length < 1) {
        ShowGridItemSelectionAlert();
        return false;
    }
    else if (SelectedWoIdToCancel.length > WOAllowedPrintNumber) {
        var errorMessage = "You can select maximum " + WOAllowedPrintNumber + " records to proceed.";
        ShowErrorAlert(errorMessage);
        return false;
    }
    else {
        var jsonResult = {
            "list": SelectedWoIdToCancel,
            "cancelreason": "",
            "comments": "",
            "PrintingCountConnectionID": PrintingCountConnectionID
        }
        {
            $.ajax({
                url: '/WorkOrder/SetPrintWoListFromIndex',
                data: JSON.stringify(jsonResult),
                type: "POST",
                datatype: "json",
                contentType: 'application/json; charset=utf-8',
                beforeSend: function () {
                    ShowLoader();
                },
                success: function (result) {
                    if (result.success) {
                        window.open("/WorkOrder/GenerateWorkOrderPrint", "_blank");
                    }
                },
                complete: function () {
                    CloseLoader();
                    $(".updateArea").hide();
                    $(".actionBar").fadeIn();
                    $(document).find('.chksearch').prop('checked', false);
                    $('.itemcount').text(0);
                    SelectedWoIdToCancel = [];
                }
            });
        }
    }
});
$(document).on('click', '#PrintWoCheckList', function () {
    if (SelectedWoIdToCancel.length < 1) {
        ShowGridItemSelectionAlert();
        return false;
    }
    else if (SelectedWoIdToCancel.length > WOAllowedPrintNumber) {
        var errorMessage = "You can select maximum " + WOAllowedPrintNumber + " records to proceed.";
        ShowErrorAlert(errorMessage);
        return false;
    }
    else {
        var jsonResult = {
            "list": SelectedWoIdToCancel,
            "cancelreason": "",
            "comments": "",
            "PrintingCountConnectionID": PrintingCountConnectionID
        }
        {
            $.ajax({
                url: '/WorkOrder/PrintWoListFromIndex',
                data: JSON.stringify(jsonResult),
                type: "POST",
                datatype: "json",
                contentType: 'application/json; charset=utf-8',
                responseType: 'arraybuffer',
                beforeSend: function () {
                    ShowProgressBar();
                },
                success: function (result) {
                    if (result.success && result.jsonStringExceed == false) {
                        PdfPrintAllWoList(result.pdf);
                    }
                    else {
                        var errorMessage = getResourceValue("PdfFileSizeExceedAlert");
                        ShowErrorAlert(errorMessage);
                        return false;
                    }

                },
                complete: function () {
                    $(".updateArea").hide();
                    $(".actionBar").fadeIn();
                    $(document).find('.chksearch').prop('checked', false);
                    $('.itemcount').text(0);
                    SelectedWoIdToCancel = [];
                    CloseProgressBar();
                }
            });
        }
    }
});
$(document).on('change', '.chksearch', function () {

    if (layoutType == 2) {
        var data = workOrdersSearchdt.row($(this).parents('tr')).data();
        if (!this.checked) {
            SelectedWoIdToCancel = SelectedWoIdToCancel.filter(function (el) {
                return el.WorkOrderId !== data.WorkOrderId;
            });
        }
        else {
            var item = new PartNotInInventorySelectedItem(data.WorkOrderId, data.ClientLookupId, data.Status);
            var found = SelectedWoIdToCancel.some(function (el) {
                return el.WorkOrderId === data.WorkOrderId;
            });
            if (!found) { SelectedWoIdToCancel.push(item); }
        }
    }
    else {
        var workOrderId = $(this).attr("id");
        if (!this.checked) {
            SelectedWoIdToCancel = SelectedWoIdToCancel.filter(function (el) {
                return el.WorkOrderId !== workOrderId;
            });
        }
        else {
            var clientLookupId = $(this).parents('.cardRow').find('.lnk_workorder').attr('clientlookupid');
            var status = $(this).parents('.cardRow').find('.lnk_workorder').attr('status');
            var item = new PartNotInInventorySelectedItem(workOrderId, clientLookupId, status);
            var found = SelectedWoIdToCancel.some(function (el) {
                return el.WorkOrderId === workOrderId;
            });
            if (!found) { SelectedWoIdToCancel.push(item); }
        }
    }
    if (SelectedWoIdToCancel.length > 0) {
        $(".actionBar").hide();
        $(".updateArea").fadeIn();
        $('#btnupdateequip').removeAttr("disabled");
        $('#PrintWoCheckList,#PrintWoCheckListDevExpress').removeAttr("disabled");
        if ($('#CompleteWoCheckList').length > 0)
            $('#CompleteWoCheckList').removeAttr("disabled");
        if ($('#CompleteWoCheckListWizard').length > 0)
            $('#CompleteWoCheckListWizard').removeAttr("disabled");
        if (layoutType == 1) {
            $(document).find('#cardScheduleDrop,#cardApproveDrop,.wobtngrdcomplete,.wobtngrdcompleteWizard,.wobtngrdcancel').hide();
        }
    }
    else {
        if (layoutType == 2) {
            $(".updateArea").hide();
            $(".actionBar").fadeIn();
            $('#btnupdateequip').prop("disabled", "disabled");
            $('#PrintWoCheckList,#PrintWoCheckListDevExpress').prop("disabled", "disabled");
            if ($('#CompleteWoCheckList').length > 0)
                $('#CompleteWoCheckList').prop("disabled", "disabled");
            if ($('#CompleteWoCheckListWizard').length > 0)
                $('#CompleteWoCheckListWizard').prop("disabled", "disabled");
        }
        else {
            CloseLoader();
            $(".updateArea").hide();
            $(".actionBar").fadeIn();
            $(document).find('.chksearch').prop('checked', false);
            $('.itemcount').text(0);
            SelectedWoIdToCancel = [];
            $(document).find('#cardScheduleDrop,#cardApproveDrop,.wobtngrdcomplete,.wobtngrdcompleteWizard,.wobtngrdcancel').show();
        }
    }
    $('.itemcount').text(SelectedWoIdToCancel.length);
});
function PartNotInInventorySelectedItem(WorkOrderId, ClientLookupId, Status) {
    this.WorkOrderId = WorkOrderId;
    this.ClientLookupId = ClientLookupId;
    this.Status = Status;
};
function PdfPrintAllWoList(pdf) {
    var blob = b64StrtoBlob(pdf, 'application/pdf');
    var blobUrl = URL.createObjectURL(blob);
    var htmlbody = '<iframe id="iframeid" src=' + blobUrl + ' style="position:fixed; top:10; left:0; bottom:0; right:0; width:100%; height:100%; border:none; margin:0; padding:0; overflow:hidden; z-index:999999;" style="display:none" > </iframe>';
    var winhtml = window.open("", "PdfWndow", "width=800,height=800");
    if (winhtml) {
        winhtml.document.write(htmlbody);
        winhtml.document.getElementById('iframeid').contentWindow.print();
    }
    else {
        var errorMessage = "Please check if pop-up is enabled for Somax.";
        ShowErrorAlert(errorMessage);
        return false;
    }
}
function b64StrtoBlob(b64Data, contentType, sliceSize) {
    contentType = contentType || '';
    sliceSize = sliceSize || 512;
    var byteCharacters = atob(b64Data);
    var byteArrays = [];
    for (var offset = 0; offset < byteCharacters.length; offset += sliceSize) {
        var slice = byteCharacters.slice(offset, offset + sliceSize);
        var byteNumbers = new Array(slice.length);
        for (var i = 0; i < slice.length; i++) {
            byteNumbers[i] = slice.charCodeAt(i);
        }
        var byteArray = new Uint8Array(byteNumbers);
        byteArrays.push(byteArray);
    }
    var blob = new Blob(byteArrays, { type: contentType, ContentDisposition: "inline" });
    return blob;
}

//#endregion

//#region ColumnVisibility
$(document).on('click', '#liCustomize', function () {
    funCustomizeBtnClick(workOrdersSearchdt, true);
});
$(document).on('click', '.saveConfig', function () {
    var colOrder = [0, 1];
    funCustozeSaveBtn(workOrdersSearchdt, colOrder);
    run = true;
    workOrdersSearchdt.state.save(run);
});
//#endregion

//#region V2-276
function LoadCost(workOrderID, costtype) {
    $.ajax({
        "url": "/WorkOrder/GetAllCosts",
        data: { WorkOrderId: workOrderID, costtype: costtype },
        type: "POST",
        beforeSend: function () {
            $(document).find('#costdataloader').show();
        },
        datatype: "json",
        success: function (data) {
            $(document).find('#totalval').text(data.TotalCost);
            $(document).find('#partsvalue').text(data.PartCost);
            $(document).find('#partprogressbar').width(data.PartCostPercentage + '%');

            $(document).find('#laborvalue').text(data.LaborCost);
            $(document).find('#laborprogressbar').width(data.LaborCostPercentage + '%');

            $(document).find('#othervalue').text(data.OtherCost);
            $(document).find('#oherprogressbar').width(data.OtherCostPercentage + '%');
        },
        complete: function () {
            $(document).find('#costbody').show();
            $(document).find('.selectpicker').selectpicker();
            $(document).find('#costdataloader').hide();
        }
    });
}
$(document).on('change', '#drpcost', function () {
    var workOrderID = $(document).find('#workOrderModel_WorkOrderId').val();
    var costtype = $(this).val();
    $(document).find('#costbody').hide();
    LoadCost(workOrderID, costtype);
});
function Activity(workOrderID) {
    $.ajax({
        "url": "/WorkOrder/LoadActivity",
        data: { WorkOrderId: workOrderID },
        type: "POST",
        datatype: "json",
        success: function (data) {
            $(document).find('#activityitems').html(data);
            $(document).find("#activityList").mCustomScrollbar({
                theme: "minimal"
            });
        },
        complete: function () {
            $(document).find('#activitydataloader').hide();
            var ftext = '';
            var bgcolor = '';
            $(document).find('#activityitems').find('.activity-header-item').each(function () {
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
                $(this).attr('style', 'color:' + bgcolor + ' !important;border:1px solid ' + bgcolor + ' !important;');
                ftext = thistext;
            });
            LoadComments(workOrderID);
        }
    });
}
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
function LoadComments(workOrderID) {
    $.ajax({
        "url": "/WorkOrder/LoadComments",
        data: { WorkOrderId: workOrderID },
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

//#region CKEditor
$(document).on("focus", "#wotxtcommentsnew", function () {
    $(document).find('.ckeditorarea').show();
    var noteitem = $(document).find('.editcomments').parents('.kt-notes__item');
    noteitem.find('.ckeditorarea').hide();
    noteitem.find('.kt-notes__body').show();
    noteitem.find('.commenteditdelearea').show();
    LoadCkEditor('wotxtcomments');
    $("#wotxtcommentsnew").hide();
    $(".ckeditorfield").show();
});
$(document).on('click', '#btnsavecommands', function () {
    var selectedUsers = [];
    const data = getDataFromTheEditor();
    if (!data) {
        return false;
    }
    var workorderId = $(document).find('#workOrderModel_WorkOrderId').val();
    var woClientLookupId = $(document).find('#workOrderModel_ClientLookupId').val();
    var noteId = 0;
    if (LRTrim(data) == "") {
        return false;
    }
    else {
        $(document).find('.ck-content').find('.mention').each(function (item, index) {
            selectedUsers.push($(index).data('user-id'));
        });
        $.ajax({
            url: '/WorkOrder/AddComments',
            type: 'POST',
            beforeSend: function () {
                ShowLoader();
            },
            data: {
                workOrderId: workorderId,
                content: data,
                woClientLookupId: woClientLookupId,
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
                        RedirectToPmDetail(workorderId);
                    });
                }
                else {
                    ShowGenericErrorOnAddUpdate(data);
                    CloseLoader();
                }
            },
            complete: function () {
                ClearEditor();
                $("#wotxtcommentsnew").show();
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
    $("#wotxtcommentsnew").show();
    $(".ckeditorfield").hide();
});
$(document).on('click', '.editcomments', function () {
    $(document).find(".ckeditorarea").each(function () {
        $(this).html('');
    });
    $("#wotxtcommentsnew").show();
    $(".ckeditorfield").hide();
    var notesitem = $(this).parents('.kt-notes__item').eq(0);
    notesitem.find('.ckeditorarea').html(CreateEditorHTML('wotxtcommentsEdit'));
    var noteitem = $(document).find('.editcomments').parents('.kt-notes__item');
    noteitem.find('.kt-notes__body').show();
    noteitem.find('.commenteditdelearea').show();
    var rawHTML = $.parseHTML($(this).parents('.kt-notes__item').find('.kt-notes__body').find('.originalContent').html());
    LoadCkEditorEdit('wotxtcommentsEdit', rawHTML);
    $(document).find('.ckeditorarea').hide();
    notesitem.find('.ckeditorarea').show();
    notesitem.find('.kt-notes__body').hide();
    notesitem.find('.commenteditdelearea').hide();
});

$(document).on('click', '.deletecomments', function () {
    DeleteWoNote($(this).attr('id'));
});
$(document).on('click', '.btneditcomments', function () {
    var data = getDataFromTheEditor();
    var workorderId = $(document).find('#workOrderModel_WorkOrderId').val();
    var noteId = $(this).parents('.kt-notes__item').find('.editcomments').attr('id');
    var woClientLookupId = $(document).find('#workOrderModel_ClientLookupId').val();
    var updatedindex = $(this).parents('.kt-notes__item').find('.hdnupdatedindex').val();
    $.ajax({
        url: '/WorkOrder/AddComments',
        type: 'POST',
        beforeSend: function () {
            ShowLoader();
        },
        data: { workOrderId: workorderId, content: LRTrim(data), noteId: noteId, updatedindex: updatedindex },
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
                    RedirectToPmDetail(workorderId);
                });
            }
            else {
                ShowGenericErrorOnAddUpdate(data);
                CloseLoader();
            }
        },
        complete: function () {
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
function DeleteWoNote(notesId) {
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
                        RedirectToPmDetail($(document).find('#workOrderModel_WorkOrderId').val());
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

//#region V2-389
function getfilterinfoarraywo(txtsearchelement, advsearchcontainer) {
    var filterinfoarray = [];
    var f = new filterinfo('searchstring', LRTrim(txtsearchelement.val()));
    filterinfoarray.push(f);
    advsearchcontainer.find('.adv-item').each(function (index, item) {
        if ($(this).parent('div').is(":visible")) {
            f = new filterinfo($(this).attr('id'), JoinStringStringArray($(this).val()));
            filterinfoarray.push(f);

            if ($(this).parent('div').find('div').hasClass('range-timeperiod')) {
                if ($(this).parent('div').find('input').val() !== '' && $(this).val() == '10') {
                    f = new filterinfo('this-' + $(this).attr('id'), JoinStringStringArray($(this).parent('div').find('input').val()));
                    filterinfoarray.push(f);
                }
            }
        }
    });
    return filterinfoarray;
}
function setsearchuiwo(data, txtsearchelement, advcountercontainer, searchstringcontainer) {
    var searchitemhtml = '';
    $.each(data, function (index, item) {
        if (item.key == 'searchstring' && item.value) {
            var txtSearchval = item.value;
            if (item.value) {
                txtsearchelement.val(txtSearchval);
                searchitemhtml = "";
                searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + txtsearchelement.attr('id') + '">Search: ' + txtSearchval + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossWorkorder" aria-hidden="true"></a></span>';
            }
            return false;
        }
        else {
            if ($('#' + item.key).parent('div').is(":visible")) {
                if (item.value) {
                    hGridfilteritemcount++;
                    searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + item.key + '">' + $("label[for='" + item.key + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossWorkorder" aria-hidden="true"></a></span>';
                }               
                if ($('#' + item.key).hasClass('has-dtrangepicker') && item.value !== '') {
                    $('#' + item.key).val(item.value).trigger('change');
                    var datarangeval = data.filter(function (val) { return val.key === 'this-' + item.key; });
                    if (datarangeval.length > 0) {
                        if (datarangeval[0].value) {
                            var rangeid = $('#' + item.key).parent('div').find('input').attr('id');
                            $('#' + rangeid).css('display', 'block');
                            $('#' + rangeid).val(datarangeval[0].value);

                            if (item.key === 'dtgridadvsearchCreated') {
                                StartCreateteDate = datarangeval[0].value.split(' - ')[0];
                                EndCreateteDate = datarangeval[0].value.split(' - ')[1];
                                $(document).find('#advcreatedaterange').daterangepicker(daterangepickersetting, function (start, end, label) {
                                    StartCreateteDate = start.format('MM/DD/YYYY');
                                    EndCreateteDate = end.format('MM/DD/YYYY');
                                });
                            }

                            if (item.key === 'dtgridadvsearchScheduled') {
                                StartScheduledDate = datarangeval[0].value.split(' - ')[0];
                                EndScheduledDate = datarangeval[0].value.split(' - ')[1];
                                $(document).find('#advScheduleddaterange').daterangepicker(daterangepickersetting, function (start, end, label) {
                                    StartScheduledDate = start.format('MM/DD/YYYY');
                                    EndScheduledDate = end.format('MM/DD/YYYY');
                                });
                            }

                            if (item.key === 'dtgridadvsearchActualFinish') {
                                StartActualFinishDate = datarangeval[0].value.split(' - ')[0];
                                EndActualFinishDate = datarangeval[0].value.split(' - ')[1];
                                $(document).find('#advActualFinishdaterange').daterangepicker(daterangepickersetting, function (start, end, label) {
                                    StartActualFinishDate = start.format('MM/DD/YYYY');
                                    EndActualFinishDate = end.format('MM/DD/YYYY');
                                });
                            }
                        }
                    }
                }
                else {
                    if ($('#' + item.key).attr('multiple')) {
                        var stringToArray = item.value.split(',');
                        $('#' + item.key).val(stringToArray).trigger('change.select2');
                    } else {
                        $('#' + item.key).val(item.value);
                    }
                }
            }           
            advcountercontainer.text(hGridfilteritemcount);
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    searchstringcontainer.html(searchitemhtml);

}
//#endregion

//#region Card view griddatalayout update
//For column , order , page and page length change
function LayoutUpdate(area) {
    $.ajax({
        "url": "/Base/GetLayout",
        "data": {
            GridName: "WorkOrder_Search"
        },
        "async": false,
        "dataType": "json",
        "success": function (json) {

            hGridfilteritemcount = 0;
            if (json.LayoutInfo !== '') {
                var gridstate = JSON.parse(json.LayoutInfo);
                gridstate.start = cardviewstartvalue;
                if (area === 'column' || area === 'order') {
                    gridstate.order[0] = [currentorderedcolumn, currentorder];
                }
                else if (area === 'pagination') {//
                }
                else if (area === 'pagelength') {
                    gridstate.length = cardviewlwngth;
                }

                if (json.FilterInfo !== '') {
                    setsearchuiwo(JSON.parse(json.FilterInfo), $("#txtColumnSearch"), $(".filteritemcount"), $("#advsearchfilteritems"));
                }
                $.ajax({
                    "url": gridStateSaveUrl,
                    "data": {
                        GridName: "WorkOrder_Search",
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
            GridName: "WorkOrder_Search"
        },
        "async": false,
        "dataType": "json",
        "success": function (json) {
            if (json.LayoutInfo !== '') {
                var gridstate = JSON.parse(json.LayoutInfo);
                gridstate.start = cardviewstartvalue;
                var filterinfoarray = getfilterinfoarraywo($("#txtColumnSearch"), $('#advsearchsidebarWorkorder'));
                $.ajax({
                    "url": gridStateSaveUrl,
                    "data": {
                        GridName: "WorkOrder_Search",
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
//This function with return comma seperated string from array like ['om','acx'] to 'om,acx';
function JoinStringStringArray(value) {
    return $.type(value) == "array" ? value.filter(function (val) { return val != ""; }).join() : value;
}
//#endregion

//#region V2-611 Add workorder
function WorksOrderDynamicAddOnSuccess(data) {
    CloseLoader();
    if (data.data === "success") {
        if (data.Command === "save") {

            if (data.mode === "add") {
                localStorage.setItem("workorderstatus", '3');
                localStorage.setItem("workorderstatustext", getResourceValue("spnOpenWorkOrder"));
                SuccessAlertSetting.text = getResourceValue("WorkOrderAddAlert");
            }

            swal(SuccessAlertSetting, function () {
                RedirectToPmDetail(data.WorkOrderMasterId, "overview");
            });
        }
        else {
            SuccessAlertSetting.text = getResourceValue("WorkOrderAddAlert");
            ResetErrorDiv();
            swal(SuccessAlertSetting, function () {
                $(document).find('#requesttab').addClass('active').trigger('click');
                $(document).find('form').trigger("reset");
                $(document).find('form').find("select").not("#colorselector").val("").trigger('change.select2');
                $(document).find('form').find("select").removeClass("input-validation-error");
                $(document).find('form').find("input").removeClass("input-validation-error");
                $(document).find('form').find("textarea").removeClass("input-validation-error");
                //hide cross button of pop up look up after clicking save and another button 
                $(document).find('form').find('i.fa.fa-close').closest('button.btntxtInputGroup').filter(function () {
                    return $(this).css('display') === 'block';
                }).hide();
                //
            });
        }

    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}

$(document).on('click', "#btnCancelAddWODynamic", function () {
    swal(CancelAlertSetting, function () {
        window.location.href = "../WorkOrder/Index?page=Maintenance_Work_Order_Search";
    });
});
//#endregion

//#region  Edit workorder dynamic
function WorksOrderDynamicEditOnSuccess(data) {
    CloseLoader();
    if (data.data === "success") {
        if (data.Command === "save") {
            SuccessAlertSetting.text = getResourceValue("WorkOrderUpdateAlert");
            swal(SuccessAlertSetting, function () {
                RedirectToPmDetail(data.WorkOrderMasterId, "overview");
            });
        }
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', "#btnCancelEditWODynamic", function () {
    var WorkOrderId = $(document).find('#EditWorkOrder_WorkOrderId').val();
    swal(CancelAlertSetting, function () {
        RedirectToPmDetail(WorkOrderId, "overview");
    });
});
//#endregion
function fxAddPhotoforMobileInActionMenu() {
    //#region V2-808
    var CheckLoggedInFromMobile = CheckLoggedInFromMob();
    if (CheckLoggedInFromMobile == true) {
        $(document).find('.ImageGridAddPhoto').remove();
    }
    else {
        $(document).find('#liaddphotofrommobile').remove();
    }
    //#endregion
}

//#region V2-853 Reset Grid
$('#liResetGridClearBtn').click(function (e) {
    CancelAlertSetting.text = getResourceValue("ResetGridAlertMessage");
    swal(CancelAlertSetting, function () {
        var localstorageKeys = [];
        localstorageKeys.push("layoutTypeval");
        localstorageKeys.push("workorderstatus");
        localstorageKeys.push("WOCompleteStartDateVw");
        localstorageKeys.push("WOCompleteEndDateVw");
        localstorageKeys.push("WOCreateStartDateVw");
        localstorageKeys.push("WOCreateEndDateVw");
        localstorageKeys.push("workorderstatustext");
        localstorageKeys.push("ASSIGNEDUSERSLIST");
        localstorageKeys.push("WOCURRENTCARDVIEWSTATE");
        localstorageKeys.push("WODETAILFROM");
        DeleteGridLayout('WorkOrder_Search', workOrdersSearchdt, localstorageKeys);
        GenerateSearchList('', true);
        window.location.href = "../WorkOrder/Index?page=Maintenance_Work_Order_Search";
    });
});
//#endregion
//#region V2-1024
$(document).on('keyup', '#tblworkorders_paginate .paginate_input', function () {

    if (layoutType == 1) {
        var currentselectedpage = parseInt($(document).find('#txtcurrentpage').val());
        var lastpage = parseInt($(document).find('#spntotalpages').text());
        if (currentselectedpage > lastpage) {
            currentselectedpage = lastpage;
        }
        if (currentselectedpage < 1) {
            currentselectedpage = 1;
        }
        cardviewlwngth = $(document).find('#cardviewpagelengthdrp').val();
        cardviewstartvalue = cardviewlwngth * (currentselectedpage - 1);
        grdcardcurrentpage = currentselectedpage;

        LayoutUpdate('pagination');
        ShowCardView();
    }
    run = true;
});
//#region load CardView with previous state
var DefaultLayoutInfo = '{"time":currentTime,"start":0,"length":10,"order":[[1,"asc"]],"search":{"search":"","smart":true,"regex":false,"caseInsensitive":true},"columns":[],"ColReorder":[]}';
function GetDatatableLayout() {
    $.ajax({
        "url": "/Base/GetLayout",
        "data": {
            GridName: "WorkOrder_Search"
        },
        "async": false,
        "dataType": "json",
        "success": function (json) {
            if (json.LayoutInfo !== '') {
                var LayoutInfo = JSON.parse(json.LayoutInfo);
                var pageclicked = (LayoutInfo.start / LayoutInfo.length);
                cardviewlwngth = LayoutInfo.length;
                cardviewstartvalue = cardviewlwngth * pageclicked;
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
                        GridName:  "WorkOrder_Search",
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