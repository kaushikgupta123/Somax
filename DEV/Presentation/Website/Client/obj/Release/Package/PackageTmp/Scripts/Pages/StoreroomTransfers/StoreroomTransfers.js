//#region Global variables
var gridname = "StoreroomTransfers_IncomingTransfer_Search";
var CustomQueryDisplayId = "1";
var run = false;
var selectCount = 0;
var dtTable;
function filterinfo(id, value) {
    this.key = id;
    this.value = value;
}
var ProcessSelectedItemArray = [];
function funcIncomingStoreroomProcess(StoreroomTransferId, ReceiveQty) {
    this.StoreroomTransferId = StoreroomTransferId;
    this.ReceiveQty = ReceiveQty;
}
//#endregion
$(function () {
    $(".actionBar").fadeIn();
    $(document).find('.dtpickerNew').datepicker({
        changeMonth: true,
        changeYear: true,
        beforeShow: function (i) { if ($(i).attr('readonly')) { return false; } },
        "dateFormat": "mm/dd/yy",
        autoclose: true,
        minDate: new Date()
    }).inputmask('mm/dd/yyyy');
    ShowbtnLoaderclass("LoaderDrop");
    ShowbtnLoader("btnsortmenu");
    $(document).find('.select2picker').select2({});
    $(document).on('click', "#action", function () {
        $(".actionDrop").slideToggle();
    });
    $(document).on('focusout', "#action", function () {
        $(".actionDrop").fadeOut();
    });
    $("#sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $('#dismiss, .overlay').on('click', function () {

        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();

    });
    $('.overlay2').on('click', function () {
        $('#ApproveWOadvsearchcontainer').find('.sidebar').removeClass('active');
        $(document).find('.overlay2').fadeOut();
    });
    $('#sidebarCollapse').on('click', function () {
        $('#sidebar').addClass('active');
        $('.overlay').fadeIn();
        $('.collapse.in').toggleClass('in');
        $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    });
    
    $(document).find(".scheduledBlock").css("display", "inline-block");
    $(document).find("#SrchBttnNew").css("display", "inline-block");
    //#region Load Grid With Status
    var partcurrentstatus = localStorage.getItem("CURRENTTABSTATUS");
    if (partcurrentstatus != 'undefined' && partcurrentstatus != null && partcurrentstatus != "") {
        CustomQueryDisplayId = partcurrentstatus;
        $(document).find("#Status").val(CustomQueryDisplayId).trigger('change');
    }
    else {
        CustomQueryDisplayId = "1";
    }
    //#endregion
    $("#lsGridAction :input").attr("disabled", "disabled");
    generateIncomingStoreroomTransferDataTable();
    $("#StoreroomId").prop("selectedIndex", 0).val();
});

var order = '1';
var orderDir = 'asc';
var IsMaintain = false;
var IsReceive = false;
function generateIncomingStoreroomTransferDataTable() {
    var printCounter = 0;
    if ($(document).find('#StoreroomTransferIncomingSearchTable').hasClass('dataTable')) {
        dtTable.destroy();
    }
    dtTable = $("#StoreroomTransferIncomingSearchTable").DataTable({
        colReorder: {
            fixedColumnsLeft: 2
        },
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[1, "asc"]],
        stateSave: true,
        "stateSaveCallback": function (settings, data) {
            if (run == true) {
                if (data.order) {
                    data.order[0][0] = order;
                    data.order[0][1] = orderDir;
                }
                var filterinfoarray = getfilterinfoarray($("#txtColumnSearch"), $('#advsearchsidebar'));
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
                        order = LayoutInfo.order[0][0];
                        orderDir = LayoutInfo.order[0][1];
                        callback(JSON.parse(json.LayoutInfo));
                        if (json.FilterInfo) {
                            setsearchui(JSON.parse(json.FilterInfo), $("#txtColumnSearch"), $(".filteritemcount"), $("#advsearchfilteritems"));

                        }
                    }
                    else {
                        callback(json);
                    }

                }
            });
            //return o;
        },
        scrollX: true,
        //fixedColumns: {
        //    leftColumns: 3,
        //},
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'Storeroom Transfer List'
            },
            {
                extend: 'print',
                title: 'Storeroom Transfer List'
            },
            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Storeroom Transfer List',
                extension: '.csv',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                orientation: 'landscape',
                pageSize: 'A3',
                title: 'Storeroom Transfer List'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/StoreroomTransfers/GetStoreroomTransferGridData",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.CustomQueryDisplayId = CustomQueryDisplayId;
                d.StoreroomId = LRTrim($(document).find('#StoreroomId').val());
                d.SearchText = LRTrim($(document).find('#txtColumnSearch').val());
                d.Order = order;
                //d.orderDir = orderDir;
            },
            "dataSrc": function (result) {
                let colOrder = dtTable.order();
                orderDir = colOrder[0][1];

                totalcount = result.recordsTotal;
                IsMaintain = result.IsMaintain;
                IsReceive = result.IsReceive;
                //HidebtnLoader("btnsortmenu");
                HidebtnLoaderclass("LoaderDrop");
                if (result.data.length == "0") {
                    $(document).find('.import-export').attr('disabled', 'disabled');
                }
                else {
                    $(document).find('.import-export').removeAttr('disabled');
                }

                return result.data;
            },
            global: true
        },
        "columns":
            [
                {
                    "data": "PartClientLookupId",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": true,
                    "className": "text-left",
                    "name": "1"

                },
                {
                    "data": "PartDescription", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2",
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-300'>" + data + "</div>";
                    }
                },
                { "data": "QuantityIssued", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right", "name": "3", },
                {
                    "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": false,
                    'render': function (data, type, row) {
                        if (row.Status == "InTransit") {
                            var qty = "0";
                            var exist = $.grep(ProcessSelectedItemArray, function (obj) {
                                return obj.StoreroomTransferId === row.StoreroomTransferId;
                            });
                            if (exist.length > 0) {
                                qty = exist[0].ReceiveQty;
                            }
                            else {
                                qty = "0";
                            }
                            if (parseFloat(qty) > 0) {
                                return "<div style='width:110px !important; position:relative;'><input type='text' style='width:90px !important;text-align:right;' class='duration  dt-inline-text decimalinputupto2places grd-hours' autocomplete='off' value='" + qty + "' maskedFormat='9,6'><i class='fa fa-check-circle is-saved-check' style='float: right; position: absolute; top: 8px; right:-3px; color:green;' title='success'></i><i class='fa fa-times-circle is-saved-times' style='float: right; position: absolute; top: 8px; right:-3px; color:red;display:none;'></i><img src='../Content/Images/grid-cell-loader.gif' class='is-saved-loader' style='float:right;position:absolute;top: 8px; right:-3px;display:none;' /></div>";
                            }
                            else {
                                return "<div style='width:110px !important; position:relative;'><input type='text' style='width:90px !important;text-align:right;' class='duration  dt-inline-text decimalinputupto2places grd-hours' autocomplete='off' value='" + qty + "' maskedFormat='9,6'><i class='fa fa-check-circle is-saved-check' style='float: right; position: absolute; top: 8px; right:-3px; color:green;display:none;' title='success'></i><i class='fa fa-times-circle is-saved-times' style='float: right; position: absolute; top: 8px; right:-3px; color:red;display:none;'></i><img src='../Content/Images/grid-cell-loader.gif' class='is-saved-loader' style='float:right;position:absolute;top: 8px; right:-3px;display:none;' /></div>";
                            }
                        }
                    }
                },
                {
                    "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "5"
                },
                {
                    "data": "QuantityReceived", "autoWidth": true, "bSearchable": true, "className": "text-right", "bSortable": true, "name": "6"
                },
                {
                    "data": "StoreroomName", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "7"
                },
                {
                    "data": "StoreroomTransferId", "autoWidth": true, "bSearchable": true, "className": "text-center", "bSortable": false,
                    "mRender": function (data, type, row) {

                        if (IsMaintain === true) {
                            var UpdateTransferRequest = "";
                            var DeleteTransferRequestBtn = "";
                            var ForceCompleteBtn = "";
                            if (row.Status === 'Open') {
                                UpdateTransferRequest = '<a class="btn btn-blue UpdateTransferRequest gridinnerbutton" style="margin-left: 0;" title= "Update Transfer Request">Update Transfer Request</a>';
                                DeleteTransferRequestBtn = '<a class="btn btn-blue DeleteTransferRequestBtn gridinnerbutton" style="margin-left: 0;" title= "Delete Transfer Request">Delete Transfer Request</a>';
                            }
                            else if (row.Status === 'InTransit') {
                                ForceCompleteBtn = '<a class="btn btn-blue ForceCompleteBtn gridinnerbutton" style="margin-left: 0;" title= "Force Complete">Force Complete</a>';
                            }
                            return '<button type="button" class="btn btn-blue actionbtns" style="border:0;outline:0" data-id="' + row.StoreroomTransferId + '"><strong>...</strong></button>' +
                                '<div id="' + row.StoreroomTransferId + '" class="actionbtndiv" style="display:none;">' +
                                '<a class="btn btn-blue newTransferRequestBtn gridinnerbutton" style="margin-left: 0;" title="New Transfer Request">New Transfer Request</a>'
                                + UpdateTransferRequest
                                + DeleteTransferRequestBtn 
                                + ForceCompleteBtn +
                                '</div >' +
                                '<div class="maskaction ' + row.StoreroomTransferId + '" data-id="' + row.StoreroomTransferId + '"  onclick="funcCloseActionbtn()"></div>';
                        } else {
                            return '';
                        }
                    }
                }

            ],
        //columnDefs: [
        //    {
        //        targets: [0, 1],
        //        className: 'noVis'
        //    }
        //],
        initComplete: function (settings, json) {
            SetPageLengthMenu();
            //----------conditional column hiding-------------//
            //var api = new $.fn.dataTable.Api(settings);
            //var columns = dtTable.settings().init().columns;
            //var arr = [];
            //var j = 0;
            //while (j < json.hiddenColumnList.length) {
            //    var clsname = '.' + json.hiddenColumnList[j];
            //    var title = dtTable.columns(clsname).header();
            //    titleArray.push(title[0].innerHTML);
            //    classNameArray.push(clsname);
            //    dtTable.columns(clsname).visible(false);
            //    var sortMenuItem = '.dropdown-menu' + ' ' + clsname;
            //    $(sortMenuItem).remove();

            //    //---hide adv search items---
            //    var advclsname = '.' + "prc-" + json.hiddenColumnList[j];
            //    $(document).find(advclsname).hide();
            //    j++;
            //}
            //----------------------------------------------//

            if (json.IsMaintain == true && json.IsReceive == true) {
                $(document).find('#btnProcess').show();
            }
            else {
                $(document).find('#btnProcess').hide();
            }
            $("#lsGridAction :input").removeAttr("disabled");
            $("#lsGridAction :button").removeClass("disabled");
            DisableExportButton($("#StoreroomTransferIncomingSearchTable"), $(document).find('.import-export'));
            CloseLoader();
        }
    });
}
function intVal(i) {
    return typeof i === 'string' ?
        i.replace(/[\$,]/g, '') * 1 :
        typeof i === 'number' ?
            i : 0;
};
$(document).on('click', '#StoreroomTransferIncomingSearchTable_paginate .paginate_button', function () {
    run = true;
});
$(document).on('change', '#StoreroomTransferIncomingSearchTable_length .searchdt-menu', function () {
    run = true;
});
function getfilterinfoarray(txtsearchelement, advsearchcontainer) {
    var filterinfoarray = [];
    var f = new filterinfo('searchstring', LRTrim(txtsearchelement.val()));
    filterinfoarray.push(f);
    advsearchcontainer.find('.adv-item').each(function (index, item) {
        if ($(this).parent('div').is(":visible")) {
            f = new filterinfo($(this).attr('id'), $(this).val());
            filterinfoarray.push(f);
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
            if ($('#' + item.key).parent('div').is(":visible")) {
                $('#' + item.key).val(item.value);
                if (item.value) {
                    selectCount++;
                    searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + item.key + '">' + $("label[for='" + item.key + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
                }
            }
            else if (item.key == 'advrequiredDatedaterange' && item.value !== '') {
                $('#' + item.key).val(item.value);
                if (item.value) {
                    searchitemhtml = searchitemhtml + '<span style="display:none;" class="label label-primary tagTo" id="_' + item.key + '">' + $("label[for='" + item.key + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
                }
            }
            if (item.key == 'RequiredDate') {
                $("#RequiredDate").trigger('change.select2');
            }
            advcountercontainer.text(selectCount);
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    searchstringcontainer.html(searchitemhtml);
}

$(document).on('change', '#Status', function () {
    var thisval = $(this).val()
    CustomQueryDisplayId = thisval;
    localStorage.setItem("CURRENTTABSTATUS", thisval);
    if ($(document).find('#StoreroomTransferIncomingSearchTable').hasClass('dataTable')) {
        ChangeListLSHeaderInfo();
    }
});
$(document).on('click', '.btnCross', function () {
    run = true;
    var btnCrossed = $(this).parent().attr('id');
    var searchtxtId = btnCrossed.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    selectCount--;
    dtTable.page('first').draw('page');
});
$(document).on('change', '#StoreroomId', function () {
    generateIncomingStoreroomTransferDataTable();
});

function ChangeListLSHeaderInfo() {
    dtTable.page('first').draw('page');
}
function clearAdvanceSearch() {
    selectCount = 0;
    $(".filteritemcount").text(selectCount);
}
////#endregion
$('#StoreroomTransferIncomingSearchTable').find('th').click(function () {
    run = true;
    order = $(this).data('col');
});

//#region ColumnVisibility

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
    clearAdvanceSearch();
    var partcurrentstatus = localStorage.getItem("CURRENTTABSTATUS");
    if (partcurrentstatus) {
        CustomQueryDisplayId = partcurrentstatus;
        $('#mspsearchListul li').each(function (index, value) {
            if ($(this).attr('id') == CustomQueryDisplayId && $(this).attr('id') != '0') {
                $('#partsearchtitle').text($(this).text());
                $(".searchList li").removeClass("activeState");
                $(this).addClass('activeState');
            }
        });
    }
    else { $('#partsearchtitle').text(getResourceValue("AlertActive")); }
    var txtSearchval = LRTrim($(document).find('#txtColumnSearch').val());
    if (txtSearchval) {
        GenerateSearchList(txtSearchval, false);
        var searchitemhtml = "";
        searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $('#txtColumnSearch').attr('id') + '">Search: ' + txtSearchval + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
        searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
        $("#advsearchfilteritems").html(searchitemhtml);
    }
    else {
        dtTable.page('first').draw('page');
    }
    var container = $(document).find('#searchBttnNewDrop');
    container.hide("slideToggle");
}

//#endregion


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

//#region New Search button


$(document).on('click', "#SrchBttnNew", function () {
    $.ajax({
        url: '/Base/PopulateNewSearchList',
        type: 'GET',
        data: { tableName: 'IncomingStoreroomTransfer' },
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
    run = true;
    $.ajax({
        url: '/Base/ModifyNewSearchList',
        type: 'POST',
        data: { tableName: 'IncomingStoreroomTransfer', searchText: txtSearchval, isClear: isClear },
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
                dtTable.page('first').draw('page');
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
    clearAdvanceSearch();
    //activeStatus = 0;
    var txtSearchval = LRTrim($(document).find('#txtColumnSearch').val());
    if (txtSearchval) {
        GenerateSearchList(txtSearchval, false);
        var searchitemhtml = "";
        searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $('#txtColumnSearch').attr('id') + '">Search: ' + txtSearchval + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
        searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
        $("#advsearchfilteritems").html(searchitemhtml);
    }
    else {
        run = true;
        generateIncomingStoreroomTransferDataTable();
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

//#endregion

//#region StoreroomStatus Dropdown Change
$(document).on('change', '#Storeroom', function () {
    run = true;
    generateIncomingStoreroomTransferDataTable();

});
//#endregion

//#region Estimated hour edit

var enterhit = 0;
var oldHour = 0;
var oldScheduledDate = '';
$(document).on('change', '.grd-hours', function () {
    var row = $(this).parents('tr');
    var data = dtTable.row(row).data();
    var keycode = (event.keyCode ? event.keyCode : event.which);
    var thstextbox = $(this);
    var StoreroomTransferId = data.StoreroomTransferId;
    var ReceiveQty = thstextbox.val();
    thstextbox.siblings('.is-saved-check').show();
        enterhit = 1;
        if (thstextbox.val() == "" || thstextbox.val() == "0") {
            thstextbox.siblings('.is-saved-check').hide();
        }
        var exist = $.grep(ProcessSelectedItemArray, function (obj) {
            return obj.StoreroomTransferId === StoreroomTransferId;
        });
        if (ProcessSelectedItemArray.length > 0 && exist.length > 0) {
            var index = -1;
            for (var i = 0; i < ProcessSelectedItemArray.length; ++i) {
                if (ProcessSelectedItemArray[i].StoreroomTransferId === StoreroomTransferId) {
                    index = i;
                    break;
                }
            }
            if (ReceiveQty == "" || ReceiveQty == "0") {
                ProcessSelectedItemArray.splice(index, 1);
            }
            else {
                ProcessSelectedItemArray[index].ReceiveQty = ReceiveQty;
            }
        }
        else {
            var thisApproval = new funcIncomingStoreroomProcess(data.StoreroomTransferId, ReceiveQty);
            ProcessSelectedItemArray.push(thisApproval);
        }
    event.stopPropagation();
});
//#endregion
//#region Process
$(document).on('click', "#btnProcess", function () {
    var STData = ProcessSelectedItemArray;
    if (ProcessSelectedItemArray.length == 0)
        return;
    var STData = ProcessSelectedItemArray;

    var validqty = true;
    $.each(STData, function (index, item) {
        if (item.ReceiveQty != "" && isNaN(item.ReceiveQty) == true) {
            validqty = false;
        }
    });
    if (validqty == false) {
        ShowErrorAlert("Please enter valid Recieve Qty.");
        return false;
    }
    STData = JSON.stringify({ 'STData': STData });
    $.ajax({
        url: '/StoreroomTransfers/ProcessIncomingStoreroomTransfers',
        data: STData,
        contentType: 'application/json; charset=utf-8',
        type: "POST",
        beforeSend: function () {
            ShowLoader();
        },
        datatype: "json",
        success: function (data) {
            if (data.Result == "success") {
                ProcessSelectedItemArray = [];
                $('.grd-hours').val('');
                SuccessAlertSetting.text = getResourceValue("AlertReceiptSuccess");
                swal(SuccessAlertSetting, function () {
                    generateIncomingStoreroomTransferDataTable();
                });
            }
            else {
                ShowErrorAlert(data);
            }
        },
        complete: function () {
            CloseLoader();
        }
    });
});
//#endregion

$(document).on('click', '#idIncomingTransfer', function () {
    if (!$(this).hasClass('active')) {
        window.location = '/StoreroomTransfers/IncomingTransfers';
    }
    
});
$(document).on('click', '#idOutgoingTransfer', function () {
    if (!$(this).hasClass('active')) {
        window.location = '/StoreroomTransfers/OutgoingTransfers';
    }    
});

//region Action Function
function funcCloseActionbtn() {
    var btnid = $(this).attr("data-id");
    $(document).find(".actionbtndiv").hide();
    $(document).find(".maskaction").hide();
}

$(document).on('click', '.actionbtns', function (e) {
    var row = $(this).parents('tr');
    var data = dtTable.row(row).data();
    $(document).find("#" + data.StoreroomTransferId + "").show();
    $(document).find("." + data.StoreroomTransferId).show();
});
//#region Add  Transfer Request
function SetSTControls() {
    CloseLoader();
    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse(document);
    $('input, form').blur(function () {
        $(this).valid();
    });
    $('.select2picker, form').change(function () {
        var validstate = "";
        try {
            validstate = $(this).valid();
        } catch (e) {
            validstate = undefined;
        }
        var areaddescribedby = $(this).attr('aria-describedby');
        if (areaddescribedby) {
            if (validstate) {
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
    $(document).find('.select2picker').select2({});
    $(document).find('.dtpicker').datepicker({
        //minDate: 0,
        changeMonth: true,
        changeYear: true,
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy');
    SetFixedHeadStyle();

}
$(document).on('click', '.newTransferRequestBtn', function (e) {
    var row = $(this).parents('tr');
    var data = dtTable.row(row).data();
    $.ajax({
        url: "/StoreroomTransfers/AddTransferRequest",
        type: "GET",
        dataType: 'html',
        data: {
            'RequestStoreroomId': $('#StoreroomId').find('option:selected').val(),
            'RequestStoreroomName': $('#StoreroomId').find('option:selected').text()
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#TransferRequestPopUp').html(data);
            $('#AddTransferRequestModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            SetSTControls();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});

function partTransferRequestAddOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        $(document).find('#AddTransferRequestModalpopup').modal("hide");

        SuccessAlertSetting.text = getResourceValue("AlertAddTransferRequest");

        swal(SuccessAlertSetting, function () {
            dtTable.page('first').draw('page');
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
//#endregion
$(document).on('click', '.UpdateTransferRequest', function (e) {
    var row = $(this).parents('tr');
    var data = dtTable.row(row).data();
    $.ajax({
        url: "/StoreroomTransfers/UpdateTransferRequest",
        type: "GET",
        dataType: 'html',
        data: {
            'StoreroomTransferId': data.StoreroomTransferId,
            'RequestStoreroomId': $('#StoreroomId').find('option:selected').val(),
            'RequestStoreroomName': $('#StoreroomId').find('option:selected').text(),
            'PartClientLookupId': data.PartClientLookupId
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#TransferRequestPopUp').html(data);
            $('#AddTransferRequestModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            SetSTControls();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});
function partTransferRequestUpdateOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        $(document).find('#AddTransferRequestModalpopup').modal("hide");
        SuccessAlertSetting.text = getResourceValue("AlertUpdateTransferRequest");
        swal(SuccessAlertSetting, function () {
            dtTable.page('first').draw('page');
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', '.DeleteTransferRequestBtn', function (e) {
    var row = $(this).parents('tr');
    var data = dtTable.row(row).data();
    CancelAlertSetting.text = getResourceValue("ConfirmDeletedTransferRequest");
    swal(CancelAlertSetting, function () {
    $.ajax({
        url: "/StoreroomTransfers/DeleteTransferRequest",
        data: {
            'StoreroomTransferId': data.StoreroomTransferId,
        },
        type: "POST",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.Result == "success") {
                SuccessAlertSetting.text = getResourceValue("AlertDeletedTransferRequest");

                swal(SuccessAlertSetting, function () {
                    dtTable.page('first').draw('page');
                });
            }
        },
        complete: function () {
            CloseLoader();
        }
    });
});
});

$(document).on('click', '.ForceCompleteBtn', function (e) {
    var row = $(this).parents('tr');
    var data = dtTable.row(row).data();
    CancelAlertSetting.text = getResourceValue("ConfirmCompleteTransferRequest");
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: "/StoreroomTransfers/ForceCompleteTransferRequest",
            data: {
                'StoreroomTransferId': data.StoreroomTransferId,
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    SuccessAlertSetting.text = getResourceValue("AlertCompleteTransferRequest");

                    swal(SuccessAlertSetting, function () {
                        dtTable.page('first').draw('page');
                    });
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
});

$(document).on('click', '#btnStoreroomcancel,.clearstate', function () {
    var areaChargeToId = "";
    $(document).find('#AddTransferRequestModalpopup select').each(function (i, item) {
        areaChargeToId = $(document).find("#" + item.getAttribute('id')).attr('aria-describedby');
        $('#' + areaChargeToId).hide();
    });

});
//endregion