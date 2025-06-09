var WOCMainorder = '0';
var WOCMainorderDir = 'asc';
var gridname = "MaintenanceCompletionWorkbench_Search";
var dtMaintenanceCompletionWorkbench;
var ZoomConfig = { zoomType: "window", lensShape: "round", lensSize: 1000, zoomWindowFadeIn: 500, zoomWindowFadeOut: 500, lensFadeIn: 100, lensFadeOut: 100, easing: true, scrollZoom: true, zoomWindowWidth: 450, zoomWindowHeight: 450 };
var run = false;
$(document).ready(function () {
    var MaintCompstatus = localStorage.getItem("MaintCompstatus");
    if (MaintCompstatus) {
        var text = "";
        CustomQueryDisplayId = MaintCompstatus;
        $("#MaintenanceCompDropdown").val(CustomQueryDisplayId).trigger('change');
        $('#complitionView li').removeClass('selected')
        $('#complitionView li[data-original-index="' + CustomQueryDisplayId + '"]').addClass('selected')
    }
    else {
        MaintCompstatus = "0";
        CustomQueryDisplayId = MaintCompstatus;
        localStorage.setItem("MaintCompstatus", "0");
        generateMaintenanceCompletionWorkbench();
    }

});

function generateMaintenanceCompletionWorkbench() {
    var EquipmentId = 0;
    var rCount = 0;
    var visibility;
    var duration = parseInt($(document).find('#MaintenanceDropdown').val());
    if ($(document).find('#tblMaintenanceCompletionWorkbench').hasClass('dataTable')) {
        dtMaintenanceCompletionWorkbench.destroy();
    }
    dtMaintenanceCompletionWorkbench = $("#tblMaintenanceCompletionWorkbench").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        stateSave: true,
        "stateSaveCallback": function (settings, data) {
            if (run == true) {
                if (data.order) {
                    data.order[0][0] = WOCMainorder;
                    data.order[0][1] = WOCMainorderDir;
                }
                var filterinfoarray = getfilterinfoarrayMainComp($("#txtColumnCompSearch"));
                $.ajax({
                    "url": "/Base/CreateUpdateState",
                    "data": {
                        GridName: gridname,
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
            var o;
            $.ajax({
                "url": "/Base/GetLayout",
                "data": {
                    GridName: gridname
                },
                "async": false,
                "dataType": "json",
                "success": function (json) {
                    if (json.LayoutInfo) {
                        var LayoutInfo = JSON.parse(json.LayoutInfo);
                        WOCMainorder = LayoutInfo.order[0][0];
                        WOCMainorderDir = LayoutInfo.order[0][1];
                        callback(JSON.parse(json.LayoutInfo));
                        if (json.FilterInfo) {
                            setsearchuiMainComp(JSON.parse(json.FilterInfo), $("#txtColumnCompSearch"), $("#searchfilteritems"));
                        }
                    }
                    else {
                        callback(json);
                    }

                }
            });
        },
        scrollX: false,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/DashBoard/GetWorkOrderCompletionWorkbenchGrid",
            data: function (d) {
                d.CustomQueryDisplayId = localStorage.getItem("MaintCompstatus");
                d.txtSearchval = LRTrim($(document).find('#txtColumnCompSearch').val());
                d.Order = WOCMainorder;
            },
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (json) {
                let colOrder = dtMaintenanceCompletionWorkbench.order();
                WOCMainorderDir = colOrder[0][1];
                visibility = json.partSecurity;
                rCount = json.data.length;
                return json.data;
            }
        },
        columnDefs: [
            {
                targets: [0],
                className: 'noVis'
            }
        ],
        "columns":
            [
                {
                    "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1",
                    "mRender": function (data, type, row) {
                        return '<a class=lnk_Wosearch data-WoId="' + row.WorkOrderId + '" href="javascript:void(0)">' + data + '</a>';
                    }
                },
                { "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2" },
                { "data": "EquipmentClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3" },
                { "data": "AssetName", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "4" },
                {
                    "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "5",
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
                        else if (data == statusCode.Planning) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--wide' style='width:95px;' >" + getStatusValue(data) + "</span >";
                        }

                        else {
                            return getStatusValue(data);
                        }
                    }
                },
                { "data": "Type", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "6" },
                { "data": "CreateDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "7" },
                { "data": "Priority", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "8" },
                { "data": "ScheduledStartDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "9" },
                { "data": "RequiredDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "10" },
                { "data": "CompleteDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "11" },
            ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', '#tblMaintenanceCompletionWorkbench_paginate .paginate_button', function () {
    run = true;
});
$(document).on('change', '#tblMaintenanceCompletionWorkbench_length .searchdt-menu', function () {
    run = true;
});
$('#tblMaintenanceCompletionWorkbench').find('th').click(function () {
    run = true;
    WOCMainorder = $(this).data('col');
});
function filterinfoMain(id, value) {
    this.key = id;
    this.value = value;
}
function getfilterinfoarrayMainComp(txtsearchelement) {
    var filterinfoarray = [];
    var f = new filterinfoMain('searchstringMaintComp', LRTrim(txtsearchelement.val()));
    filterinfoarray.push(f);
    return filterinfoarray;
}
function setsearchuiMainComp(data, txtsearchelement, searchstringcontainer) {
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
        data: { tableName: 'MaintenanceCompletionWorkbench_Search' },
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
        data: { tableName: 'MaintenanceCompletionWorkbench_Search', searchText: txtSearchval, isClear: isClear },
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
                dtMaintenanceCompletionWorkbench.page('first').draw('page');
                CloseLoader();
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
    var txtSearchval = LRTrim($(document).find('#txtColumnCompSearch').val());
    if (txtSearchval) {
        GenerateSearchListMain(txtSearchval, false);
        var searchitemhtml = "";
        searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $('#txtColumnCompSearch').attr('id') + '">Search: ' + txtSearchval + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossMain" aria-hidden="true"></a></span>';
        searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
        $("#searchfilteritems").html(searchitemhtml);
    }
    else {
        dtMaintenanceCompletionWorkbench.page('first').draw('page');
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
    dtMaintenanceCompletionWorkbench.page('first').draw('page');
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
//#endregion

//#region View dropdown change
$(document).on('change', '#MaintenanceCompDropdown', function (e) {
    $(".updateArea").hide();
    $(".actionBar").fadeIn();
    run = true;

    $(".searchList li").removeClass("activeState");
    $(this).addClass('activeState');
    $(document).find('#searcharea').hide("slide");
    var optionval = $(this).val();
    localStorage.setItem("MaintCompstatus", optionval);
    CustomQueryDisplayId = optionval;


    if ($(document).find('#txtColumnCompSearch').val() !== '') {
        $('#searchfilteritems').find('span').html('');
        $('#searchfilteritems').find('span').removeClass('tagTo');
    }
    $(document).find('#txtColumnCompSearch').val('');


    if (optionval.length !== 0) {
        ShowbtnLoaderclass("LoaderDrop");
        if (typeof (dtMaintenanceCompletionWorkbench) === "undefined") {
            generateMaintenanceCompletionWorkbench();
        }
        else {
            dtMaintenanceCompletionWorkbench.page('first').draw('page');
        }


    }

});

//#endregion

//#region Redirect to details
$(document).on('click', '.lnk_Wosearch', function (e) {
    e.preventDefault();
    var index_row = $('#tblMaintenanceCompletionWorkbench tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = dtMaintenanceCompletionWorkbench.row(row).data();
    var WorkOrderId = data.WorkOrderId;
    var ClientLookupId = data.ClientLookupId;
    
    $.ajax({
        url: "/Dashboard/CompletionWorkbench_Details",
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
            if ($('#Labor').length > 0) {
                LoadLaborTab();
            }
            else if ($('#Parts').length > 0) {
                $('[data-tab="Parts"]').trigger('click');
            }
           
        }
    });

});

var WoId;
$(document).on('mouseover', '.assignedItem', function (e) {
    var thise = $(this);
    if (LRTrim(thise.find('.tooltipcards').text()).length > 0) {
        thise.find('.tooltipcards').attr('style', 'display :block !important;');
        return;
    }
    WoId = $(this).attr('id');
    var innerText = this.innerText.trim();
    var waPersonnelId = $(this).attr('waPersonnelId');
    if (waPersonnelId == -1) {
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
                    $('#spn' + WoId).html(data.personnelList);
                }
            },
            "complete": function () {
                thise.find('.loadingImg').hide();
                thise.find('.tooltipcards').attr('style', 'display :block !important;');
            }
        });
    }
});

function ZoomImage(element) {
    element.elevateZoom(ZoomConfig);
}
function SetWOCompletionDetailEnvironment() {
    CloseLoader();
    ZoomImage($(document).find('#EquipZoom'));

    SetFixedHeadStyle();
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
    $(document).find('#wocdetaildesmodal').modal('show');
});
//#endregion
//#region V2-853 Reset Grid
$('#btnResetGrid').click(function (e) {
    CancelAlertSetting.text = getResourceValue("ResetGridAlertMessage");
    swal(CancelAlertSetting, function () {
        var localstorageKeys = [];
        localstorageKeys.push("MaintCompstatus");
        DeleteGridLayout('MaintenanceCompletionWorkbench_Search', dtMaintenanceCompletionWorkbench, localstorageKeys);
        GenerateSearchListMain('', true);
        window.location.href = "../Dashboard/Dashboard";
    });
});
//#endregion