//#region Common 
var PurchaseOrderReceiptLineItemArray = [];
var QuantitySelectedItemArray = [];
var ClearSelectedItemArray = [];
var selectedcount = 0;
var totalcount = 0;
var AcceptClearStatus = false;
var AcceptAllStatus = false;
var AcceptSelectedStatus = false;
var ClearAllStatus = false;
var ClearSelectedStatus = false;
var selectCount = 0;
var gridname = "PurchaseOrderReceipt_Search";
var run = false;
var CustomQueryDisplayId = 0;
function filterinfo(id, value) {
    this.key = id;
    this.value = value;
}
//#endregion

$(document).ready(function () {
    ShowbtnLoaderclass("LoaderDrop");
    ShowbtnLoader("btnsortmenu");
    $(document).find('.select2picker').select2({});
    $(document).on('click', "#action", function () {
        $(".actionDrop").slideToggle();
    });
    $(".actionDrop ul li a").click(function () {
        $(".actionDrop").fadeOut();
    });
    $(document).on('focusout', "#action", function () {
        $(".actionDrop").fadeOut();
    });
    $(".sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $(".actionBar").fadeIn();
    $("#POReceiptGridAction :input").attr("disabled", "disabled");
    $(document).on('click', '.dismiss, .overlay', function () {
        $(document).find('.sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });
    $(document).on('click', '#drpDwnLink', function (e) {
        e.preventDefault();
        $(document).find("#drpDwn").slideToggle();
    });
    $(document).on('change', '#colorselector', function (evt) {
        $(document).find('.tabsArea').hide();
        openCity(evt, $(this).val());
        $('#' + $(this).val()).show();
    });
});
var PartToClientLookupIdQRcode = [];
var PurchaseorderReceiveLineItemSelectedArray = [];
function openCity(evt, cityName) {
    var PurchaseOrderId = $(document).find('#PurchaseOrderModel_PurchaseOrderId').val();
    evt.preventDefault();
    switch (cityName) {
        case "PurchasingOverview":
            RedirectDetail(PurchaseOrderId);
            break;
        case "PurchasingOverviewlineitem":
            generateLineiItemdataTable();
            break;
        case "POReceiptHistory":
            GenerateReceiptHistoryGrid(PurchaseOrderId);
            $("#LineItems").hide();
            $("#HistoryLineItems").show();
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
    if (cityName != "PurchasingOverviewlineitem") {
        document.getElementById(cityName).style.display = "block";
    }
   
    if (typeof evt.currentTarget !== "undefined") {
        evt.currentTarget.className += " active";
    }
    else {
        evt.target.className += " active";
    }
}
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
$(document).ready(function () {
    CustomQueryDisplayId = localStorage.getItem("PURCHASEORDERRECEIPTSTATUS");
    if (CustomQueryDisplayId) {
        $('#PurchaseOrderModel_TextSearchList').val(CustomQueryDisplayId).trigger('change.select2');
    }
    if (CustomQueryDisplayId && CustomQueryDisplayId !== "0") { 
        $('#posearchListul li').each(function (index, value) {
            if ($(this).attr('id') == CustomQueryDisplayId && $(this).attr('id') != '0') {
                $('#posearchtitle').text($(this).text());
                $(this).addClass('activeState');
            }
        });
        generatePODataTable();
    }
    else {
        $('#posearchListul li').each(function (index, value) {
            if ($(this).attr('id') == 1) {
                CustomQueryDisplayId = $(this).attr('id');
                $('#posearchtitle').text($(this).text());
                $("#posearchListul li").removeClass("activeState");
                $(this).addClass('activeState');
            }
        });
        generatePODataTable();
    }
    //V2-331
    $(".sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $(document).on('click', '.dismiss, .overlay', function () {
        $(document).find('.sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });
    $(document).on('click', '#pinvsidebarCollapse,#sidebarCollapse', function () {
        $('.sidebar').addClass('active');
        $('.overlay').fadeIn();
        $('.collapse.in').toggleClass('in');
        $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    });
});
$(document).on('click', "#brdPO", function () {
    var POId = $(this).attr('data-val');
    swal(CancelAlertSetting, function () {
        RedirectToPORDetail(POId, "overview");
    });
});
//#region Search
var dtPoReceiptTable;
var statussearchval;

$(document).on('change', '#PurchaseOrderModel_TextSearchList', function () {
    ShowbtnLoaderclass("LoaderDrop");
    run = true;
    CustomQueryDisplayId = $('#PurchaseOrderModel_TextSearchList option:selected').val();
    localStorage.setItem("PURCHASEORDERRECEIPTSTATUS", CustomQueryDisplayId);
    dtPoReceiptTable.page('first').draw('page');
});

$(document).on('click', '#liClearAdvSearchFilter', function () {
    run = true;
    $("#PurchaseOrderModel_TextSearchList").val(0).trigger('change.select2');
    localStorage.removeItem("PURCHASEORDERRECEIPTSTATUS");
    clearAdvanceSearch();
    dtPoReceiptTable.page('first').draw('page');
});
$("#btnPODataAdvSrch").on('click', function (e) {
    searchresult = [];
    //dtPoReceiptTable.state.clear();
    run = true;
    PRAdvSearch();
    $('.sidebar').removeClass('active');
    $('.overlay').fadeOut();
    dtPoReceiptTable.page('first').draw('page');//V2-331
});
$(document).on('click', '.btnCross', function () {
    run = true;
    var btnCrossed = $(this).parent().attr('id');
    var searchtxtId = btnCrossed.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    selectCount--;

    if (searchtxtId == "Status") {
        $(document).find("#Status").val("").trigger('change.select2');
    }
    if (searchtxtId == "VendorClientLookupId") {
        $(document).find("#VendorClientLookupId").val("").trigger('change.select2');
    }

    PRAdvSearch();
    dtPoReceiptTable.page('first').draw('page');//V2-331
});

$(document).on('click', '#liPdf', function () {
    $(".buttons-pdf")[0].click();
    $('#mask').trigger('click');
});
$(document).on('click', '#liCsv', function () {
    $(".buttons-csv")[0].click();
    $('#mask').trigger('click');
});
$(document).on('click', "#liExcel", function () {
    $(".buttons-excel")[0].click();
    $('#mask').trigger('click');
});
$(document).on('click', '#liPrint', function () {
    $(".buttons-print")[0].click();
    $('#mask').trigger('click');
});

function PRAdvSearch() {
    var InactiveFlag = false;
    var searchitemhtml = "";
    selectCount = 0;
    $(document).find('#txtColumnSearch').val('');
    $('#advsearchsidebar').find('.adv-item').each(function (index, item) {
        if ($(this).hasClass('dtpicker')) {
            $(this).val(ValidateDate($(this).val()));
        }
        if ($(this).val()) {
            selectCount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
            //searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    dtPoReceiptTable.page('first').draw('page');
    $("#advsearchfilteritems").html(searchitemhtml);
    $(".filteritemcount").text(selectCount);
    $('#liSelectCount').text(selectCount + ' filters applied');
}
function clearAdvanceSearch() {
    selectCount = 0;
    $("#PurchaseOrder").val("");
    $("#Status").val("").trigger('change.select2');
    $("#VendorClientLookupId").val("").trigger('change.select2');
    $("#VendorName").val("");
    $("#CreateDate").val("");
    $("#Attention").val("");
    $("#advsearchfilteritems").html('');
    $(".filteritemcount").text(selectCount);
}
var order = '0';//V2-331 Sorting
var orderDir = 'asc';

function generatePODataTable() {
    if ($(document).find('#purchaseOrderReceiptSearchTable').hasClass('dataTable')) {
        dtPoReceiptTable.destroy();
    }
    dtPoReceiptTable = $("#purchaseOrderReceiptSearchTable").DataTable({
        colReorder: {
            fixedColumnsLeft: 1 //V2-331
        },
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
                    data.order[0][0] = order;
                    data.order[0][1] = orderDir;
                }//V2-331 Sorting   
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
                        orderDir = LayoutInfo.order[0][1];                        //V2-331 Sorting
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

        },
        scrollX: true,
        fixedColumns: {
            leftColumns: 1
        },
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true
        },
        sDom: 'Btlipr',
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'Purchase Order Receipt'
            },
            {
                extend: 'print',
                title: 'Purchase Order Receipt',
            },
            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Purchase Order Receipt',
                extension: '.csv'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                orientation: 'portrait',
                pageSize: 'A4',
                title: 'Purchase Order Receipt'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/PurchaseOrderReceipt/GetPOGridData",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.CustomQueryDisplayId = CustomQueryDisplayId;
                d.PurchaseOrder = LRTrim($("#PurchaseOrder").val());
                d.Status = LRTrim($("#Status").val());
                d.VendorClientLookupId = LRTrim($("#VendorClientLookupId").val());
                d.VendorName = LRTrim($('#VendorName').val());
                d.CreateDate = ValidateDate($("#CreateDate").val());
                d.Attention = LRTrim($("#Attention").val());
                d.txtSearchval = LRTrim($(document).find('#txtColumnSearch').val());//V2-331
                d.Order = order;//V2-331 Sorting
                //d.orderDir = orderDir;//V2-331 Sorting
                
            },
            "dataSrc": function (result) {
                let colOrder = dtPoReceiptTable.order();
                orderDir = colOrder[0][1];
                if (result.data.length < 1) {
                    $(document).find('.import-export').prop('disabled', true);
                }
                else {
                    $(document).find('.import-export').prop('disabled', false);
                }
                HidebtnLoader("btnsortmenu");
                HidebtnLoaderclass("LoaderDrop");
                return result.data;
            },
            global: true
        },
        "columns":
            [
                {
                    "data": "ClientLookupId",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": true,
                    "className": "text-left",
                    "name": "0",
                    "mRender": function (data, type, row) {
                        return '<a class=lnk_poSearch href="javascript:void(0)">' + data + '</a>';
                    }
                },
                { "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "VendorClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2" },
                { "data": "VendorName", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3" },
                {
                    "data": "CreateDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "type": "date"
                },
                { "data": "Attention", "autoWidth": true, "bSearchable": true, "bSortable": true }
            ],
        "columnDefs": [
            {
                targets: [0],
                className: 'noVis'
            }
        ],
        initComplete: function () {
            SetPageLengthMenu();
            //var currestsortedcolumn = $('#purchaseOrderReceiptSearchTable').dataTable().fnSettings().aaSorting[0][0];
            //var currestsortedorder = $('#purchaseOrderReceiptSearchTable').dataTable().fnSettings().aaSorting[0][1];//V2-331
            //var column = this.api().column(currestsortedcolumn);
            //var columnId = $(column.header()).attr('id');
        
            //$(document).find('.srtPOReceiptcolumn').removeClass('sort-active');    //V2-331
            //$(document).find('.srtPOReceiptorder').removeClass('sort-active');    //V2-331
           
            //switch (columnId) {
            //    case "thPoPurchaseOrder":
            //        $(document).find('.srtPOReceiptcolumn').eq(0).addClass('sort-active');
            //        break;
            //    case "thPoVendor":
            //        $(document).find('.srtPOReceiptcolumn').eq(1).addClass('sort-active');
            //        break;
            //    case "thPoVendorName":
            //        $(document).find('.srtPOReceiptcolumn').eq(2).addClass('sort-active');
            //        break;
               
            //}          
            //switch (currestsortedorder) {
            //    case "asc":
            //        $(document).find('.srtPOReceiptorder').eq(0).addClass('sort-active');
            //        break;
            //    case "desc":
            //        $(document).find('.srtPOReceiptorder').eq(1).addClass('sort-active');
            //        break;
            //}//V2-331
            
            //$('#btnsortmenu').text(getResourceValue("spnSorting") + " : " + column.header().innerHTML);
            $("#POReceiptGridAction :input").removeAttr("disabled");
            $("#POReceiptGridAction :button").removeClass("disabled");
            DisableExportButton($("#purchaseOrderReceiptSearchTable"), $(document).find('.import-export'));
        }
    });
}

$(document).on('click', '#purchaseOrderReceiptSearchTable_paginate .paginate_button', function () {
    run = true;
});
$(document).on('change', '#purchaseOrderReceiptSearchTable_length .searchdt-menu', function () {
    run = true;
});
$('#purchaseOrderReceiptSearchTable').find('th').click(function () {
    run = true;
    order = $(this).data('col');
});
function EnablePurchaseOrderIdColumnSorting() {
    $('.DTFC_LeftWrapper').find('#thPoPurchaseOrder').css('pointer-events', 'auto');
    document.getElementById('thPoVendor').style.pointerEvents = 'none';
    document.getElementById('thPoVendorName').style.pointerEvents = 'none';
}
function EnableVendorNameColumnSorting() {
    document.getElementById('thPoVendorName').style.pointerEvents = 'auto';
    $('.DTFC_LeftWrapper').find('#thPoPurchaseOrder').css('pointer-events', 'none');
    document.getElementById('thPoVendor').style.pointerEvents = 'none';
}
function EnableVendorColumnSorting() {
    document.getElementById('thPoVendor').style.pointerEvents = 'auto';
    $('.DTFC_LeftWrapper').find('#thPoPurchaseOrder').css('pointer-events', 'none');
    document.getElementById('thPoVendorName').style.pointerEvents = 'none';
}

//$(document).find('.srtPOReceiptcolumn').click(function () {
//    ShowbtnLoader("btnsortmenu");
//    order = $(this).data('col');//V2-331 Sorting
//    var txtColumnSearch = LRTrim($(document).find('#txtColumnSearch').val());
//    if (txtColumnSearch != "") {
//        TextSearch();//POR Sorting
//    }
//    else {
//        $('#purchaseOrderReceiptSearchTable').DataTable().draw();//V2-331 Sorting
//    }
//    $('#btnsortmenu').text(getResourceValue("spnSorting") + " : " + $(this).text());
//    $(document).find('.srtPOReceiptcolumn').removeClass('sort-active');
//    $(this).addClass('sort-active');
//    run = true;
//});//V2-331
//$(document).find('.srtPOReceiptorder').click(function () {
//    ShowbtnLoader("btnsortmenu");
//    orderDir = $(this).data('mode');//V2-331 Sorting
//    var txtColumnSearch = LRTrim($(document).find('#txtColumnSearch').val());
//    if (txtColumnSearch != "") {
//        TextSearch();//POR Sorting
//    }
//    else {
//        $('#purchaseOrderReceiptSearchTable').DataTable().draw();//V2-331 Sorting
//    }
//    $(document).find('.srtPOReceiptorder').removeClass('sort-active');
//    $(this).addClass('sort-active');
//    run = true;
//});//V2-331


$(function () {
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            var optionval = $('#PurchaseOrderModel_TextSearchList option:selected').val();
            var PurchaseOrder = LRTrim($("#PurchaseOrder").val());
            var Status = $("#Status").val();
            var VendorClientLookupId = $("#VendorClientLookupId").val();
            var VendorName = LRTrim($('#VendorName').val());
            var CreateDate = $("#CreateDate").val();
            var Attention = LRTrim($("#Attention").val());
            var colname = order;
            var coldir = orderDir;//V2-331 Sorting
            var txtSearchval = LRTrim($(document).find('#txtColumnSearch').val()); //V2-331
            var jsonResult = $.ajax({
                url: '/PurchaseOrderReceipt/GetPOPrintData?page=all',
                data: {
                    CustomQueryDisplayId: CustomQueryDisplayId,//V2-331                   
                    PurchaseOrder: PurchaseOrder,
                    Status: Status,
                    VendorClientLookupId: VendorClientLookupId,
                    VendorName: VendorName,
                    CreateDate: CreateDate,
                    Attention: Attention,
                    colname: colname,
                    coldir: coldir,
                    txtSearchval: txtSearchval
                },
                success: function (result) {

                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data;
            var visiblecolumnsIndex = $("#purchaseOrderReceiptSearchTable thead tr th").map(function (key) {
                return this.getAttribute('data-th-index');
            }).get();
            var d = [];
            $.each(thisdata, function (index, item) {
                if (item.CreateDate != null) {
                    item.CreateDate = item.CreateDate;
                }
                else {
                    item.CreateDate = "";
                }
                if (item.CompleteDate != null) {
                    item.CompleteDate = item.CompleteDate;
                }
                else {
                    item.CompleteDate = "";
                }
                var fData = [];
                $.each(visiblecolumnsIndex, function (index, inneritem) {
                    var key = Object.keys(item)[inneritem];
                    var value = item[key]
                    fData.push(value);
                });
                d.push(fData);
            })
            return {
                body: d,
                header: $("#purchaseOrderReceiptSearchTable thead tr th div").map(function (key) {
                    return this.innerHTML;
                }).get()
            };
        }
    });
});
$(document).on('click', '.lnk_poSearch', function (e) {
    e.preventDefault();
    var index_row = $('#purchaseOrderReceiptSearchTable tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = dtPoReceiptTable.row(row).data();
    var PurchaseOrderId = data.PurchaseOrderId;
    var titletext = $('#posearchtitle').text(); //V2-331
    localStorage.setItem("poreceiptstatustext", titletext); //V2-331
    $.ajax({
        url: "/PurchaseOrderReceipt/Details",
        type: "POST",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        data: { PurchaseOrderId: PurchaseOrderId },
        success: function (data) {
            $('#renderpurchaseOrderReceipt').html(data);
            $(document).find('#spnlinkToSearch').text(titletext);//V2-331
        },
        complete: function () {
            var IsExternal = $(document).find('#PurchaseOrderModel_IsExternal').val();
            generateLineiItemdataTable(false, IsExternal);
            $("#HistoryLineItems").hide();
            CloseLoader();
            SetPOReceiptDetailsControls()//V2-331
        }
    });
});
function RedirectDetail(PurchaseOrderId, mode) {
    var titletext = localStorage.getItem("poreceiptstatustext");//V2-331
    $.ajax({
        url: "/PurchaseOrderReceipt/Details",
        type: "POST",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        data: { PurchaseOrderId: PurchaseOrderId },
        success: function (data) {
            $('#renderpurchaseOrderReceipt').html(data);
            $(document).find('#spnlinkToSearch').text(titletext);//V2-331
        },
        complete: function () {
            if (mode === "receipthistory") {
                $(document).find('#PurchasingOverview').hide();
                $(document).find('#POReceiptHistory').show();
                $(document).find('#tabReceiptHistorySidebar').addClass('active');
                $(document).find('#PurchasingOrderSidebar').removeClass('active');
                GenerateReceiptHistoryGrid(PurchaseOrderId);
                $("#LineItems").hide();
                $("#HistoryLineItems").show();
                generateLineiItemdataTable();
            }
            else {
                generateLineiItemdataTable();
            }
            CloseLoader();
        }
    });
}
//#endregion
//#region Line Item
var dtLineItemTable;
var lItemfilteritemcount = 0;
function generateLineiItemdataTable(statesave, IsExternal) {
    if (statesave === false) {
        statesave = statesave;
    }
    else {
        statesave = false;
    }
    $('#poridselectall').prop("checked", false);
    var visibilityCheckbox;
    var PurchaseOrderId = $(document).find('#PurchaseOrderModel_PurchaseOrderId').val();
    var searchText = LRTrim($('#txtsearchbox').val());
    var LineNumber = LRTrim($("#LineNo").val());
    var PurchaseOrderLineItemId = LRTrim($("#PurchaseOrderLineItemId").val());
    var PartId = LRTrim($("#PartID").val());
    var Description = LRTrim($("#Description").val());
    var Part_ManufacturerID = LRTrim($("#PartManufacturerID").val());
    var Quantity = LRTrim($("#OrderQty").val());
    var UOM = LRTrim($('#UOM').val());
    var UnitCost = LRTrim($("#UnitCost").val());
    var Status = $("#Status").val();
    if (Status) {
        Status = LRTrim(Status);
    }
    else {
        Status = "";
    }
    var visibility = lineNumberGridSecurity;
    var LineItemStaus = lineNumberGridStatus;
    var QuantityReceived = LRTrim($("#QuantityReceived").val());
    var QtyToDate = LRTrim($("#QuantityToDate").val());
    var BackOrdered = LRTrim($("#QuantityBackOrdered").val());
    var ChargeType = LRTrim($("#ChargeType").val());
    var ChargeToId = LRTrim($("#ChargeToId").val());
    var UnitOfMeasure = LRTrim($("#UnitOfMeasure").val());
    var StockType = LRTrim($("#StockType").val());
    var StoreroomId = LRTrim($("#StoreroomId").val());
    var Creator_PersonnelId = LRTrim($("#Creator_PersonnelId").val());
    var AccountId = LRTrim($("#AccountId").val());
    var CurrentAverageCost = LRTrim($("#CurrentAverageCost").val());
    var CurrentAppliedCost = LRTrim($("#CurrentAppliedCost").val());
    var CurrentOnHandQuantity = LRTrim($("#CurrentOnHandQuantity").val());
    var OrderQuantity = LRTrim($("#OrderQuantity").val());
    var PartStoreroomId = LRTrim($("#PartStoreroomId").val());
    IsExternal = $(document).find('#PurchaseOrderModel_IsExternal').val();
    if ($(document).find('#tblLineItem').hasClass('dataTable')) {
        dtLineItemTable.destroy();
    }
    dtLineItemTable = $("#tblLineItem").DataTable({
        colReorder: true,
        rowGrouping: true,
        serverSide: true,
        searching: true,
        "bProcessing": true,
        "order": [[1, "asc"]],
        "pagingType": "full_numbers",
        stateSave: false,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/PurchaseOrderReceipt/PopulateLineItem",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.PurchaseOrderId = PurchaseOrderId;
                d.searchText = searchText;
                d.LineNumber = LineNumber;
                d.PartId = PartId;
                d.Description = Description;
                d.Quantity = Quantity;
                d.UOM = UOM;
                d.UnitCost = UnitCost;
                d.QtyReceived = QuantityReceived;
                d.QtyToDate = QtyToDate;
                d.Status = Status;
                d.BackOrdered = BackOrdered;
                d.PurchaseOrderLineItemId = PurchaseOrderLineItemId;
                d.ChargeType = ChargeType;
                d.ChargeToId = ChargeToId;
                d.UnitOfMeasure = UnitOfMeasure;
                d.StockType = StockType;
                d.StoreroomId = StoreroomId;
                d.Creator_PersonnelId = Creator_PersonnelId;
                d.AccountId = AccountId;
                d.CurrentAverageCost = CurrentAverageCost;
                d.CurrentAppliedCost = CurrentAppliedCost;
                d.CurrentOnHandQuantity = CurrentOnHandQuantity;
                d.OrderQuantity = OrderQuantity;
                d.PartStoreroomId = PartStoreroomId;
                d.Part_ManufacturerID = Part_ManufacturerID;
            },
            "dataSrc": function (result) {
                var option = "";
                var statusList = result.statuslist;
                if (statusList) {
                    option += '<option value="">--Select--</option>';
                    for (var i = 0; i < statusList.length; i++) {
                        option += '<option value="' + statusList[i] + '">' + statusList[i] + '</option>';
                    }
                }
                if (totalcount < result.recordsTotal)
                    totalcount = result.recordsTotal;
                if (totalcount != result.recordsTotal)
                    selectedcount = result.recordsTotal;
                $(document).find('#Status').empty().html(option);
                $(document).find('.select2picker').select2({});
                return result.data;
            },
            global: true
        },
        columnDefs: [
            {
                "data": null,
                targets: [12], render: function (a, b, data, d) {
                    if (data.PartId != undefined && data.PartId != 0) {
                        return '<a class="btn btn-outline-primary printPOLineItemBttn gridinnerbutton" id="printPOLineItemBtnId" title= "Print" data-partid="' + data.PartId + '" > <i class="fa fa-print"></i></a>';
                    }
                    else
                        return '';
                },

            },
            { className: 'text-right', targets: [5, 7, 9, 11] }
        ],
        "columns":
            [
                {
                    "data": "PartId",
                    orderable: false,
                    "bSortable": false,
                    className: 'select-checkbox',
                    targets: 0,
                    'className': 'dt-body-center',
                    'render': function (data, type, full, meta) {
                        if ($('#poridselectall').is(':checked') && totalcount == selectedcount) {
                            return '<input type="checkbox" checked="checked" name="id[]" data-partid="' + data + '" class="chkfrow ' + data + '"  value="'
                                + $('<div/>').text(data).html() + '">';
                        }

                        else if (AcceptAllStatus == false && AcceptSelectedStatus == false && ClearAllStatus == false && ClearSelectedStatus == false &&
                            PurchaseOrderReceiptLineItemArray.indexOf(full.PurchaseOrderLineItemId) != -1) {
                            return '<input type="checkbox" checked="checked" name="id[]" data-partid="' + data + '" class="chkfrow ' + data + '"  value="'
                                + $('<div/>').text(data).html() + '">';
                        }
                        else {
                            return '<input type="checkbox" name="id[]" data-partid="' + data + '" class="chkfrow ' + data + '"  value="'
                                + $('<div/>').text(data).html() + '">';
                        }

                    }
                },
                { "data": "LineNumber", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "PartClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        if (row.ChargeToId === 0) {
                            return "<div class='text-wrap width-200'>" + data + "</div>";
                        }
                        else {
                            return "<div class='text-wrap width-200'>" + data + " <p>" + "Charge To: " + row.ChargeType  + " - " + row.ChargeToClientLookupId + "</p></div>";
                        }
                    }
                },
                { "data": "Part_ManufacturerID", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "OrderQuantity", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "UnitOfMeasure", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "UnitCost", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "QuantityReceived", "autoWidth": true, "bSearchable": true, "bSortable": false,
                    'render': function (data, type, full, meta) {
                        if (AcceptClearStatus == true) {
                            if (AcceptAllStatus == true) {
                                if (PurchaseOrderReceiptLineItemArray.indexOf(full.PurchaseOrderLineItemId) == -1)
                                    PurchaseOrderReceiptLineItemArray.push(full.PurchaseOrderLineItemId);

                                var exist = $.grep(QuantitySelectedItemArray, function (obj) {
                                    return obj.PurchaseOrderLineItemId === full.PurchaseOrderLineItemId;
                                });
                                var d = full;
                                var OrderQuantity = d.OrderQuantity;
                                var QtyToDate = d.QuantityToDate;
                                var Status = d.Status;
                                var QuantityReceived = OrderQuantity - QtyToDate;
                                if (exist.length == 0) {
                                    var item1 = new GetQuantitySelectedItem(d.PurchaseOrderLineItemId, d.PartStoreroomId, d.CurrentAverageCost, d.CurrentAppliedCost, d.CurrentOnHandQuantity, d.UnitCost, d.AccountId, d.Creator_PersonnelId, d.StoreroomId, d.Description, d.StockType, d.UnitOfMeasure, d.ChargeToId, d.ChargeType, d.PartId, d.OrderQuantity, d.PurchaseOrderId, QuantityReceived, d.LineNumber, d.QuantityToDate, 0, d.UOMConversion, d.PurchaseUOM, d.EstimatedCostsId);
                                    QuantitySelectedItemArray.push(item1);
                                }
                                if (Status == 'Complete') {
                                    return "<input type='number' style='background-color:#ffffff;' class = 'qntyval form-control search decimalinput cls-qtyrecv', autocomplete = 'off' min='0'  value='" + QuantityReceived + "' >";
                                }
                                else {
                                    return "<input type='number' style='background-color:#d7f9c7;' class = 'qntyval form-control search decimalinput cls-qtyrecv', autocomplete = 'off' min='0'  value='" + QuantityReceived + "' >";
                                }

                            }
                            else if (AcceptSelectedStatus == true) {
                                var exist = $.grep(PurchaseOrderReceiptLineItemArray, function (obj) {
                                    return obj === full.PurchaseOrderLineItemId;
                                });
                                if (exist.length > 0) {
                                    var d = full;
                                    var Status = d.Status;
                                    var OrderQuantity = d.OrderQuantity;
                                    var QtyToDate = d.QuantityToDate;
                                    var QuantityReceived = OrderQuantity - QtyToDate;
                                    var index = -1;
                                    for (var i = 0; i < QuantitySelectedItemArray.length; ++i) {
                                        if (QuantitySelectedItemArray[i].PurchaseOrderLineItemId == d.PurchaseOrderLineItemId) {
                                            index = i;
                                            break;
                                        }
                                    }
                                    if (index >= 0) {
                                        QuantitySelectedItemArray[index].QuantityReceived = QuantityReceived;
                                    }

                                    if (Status == 'Complete') {
                                        return "<input type='number' style='background-color:#ffffff;' class = 'qntyval form-control search decimalinput cls-qtyrecv', autocomplete = 'off' min='0'  value='" + QuantityReceived + "' >";
                                    }
                                    else {
                                        return "<input type='number' style='background-color:#d7f9c7;' class = 'qntyval form-control search decimalinput cls-qtyrecv', autocomplete = 'off' min='0'  value='" + QuantityReceived + "' >";
                                    }

                                }
                                else {
                                    var QuantityReceived = 0
                                    return "<input type='number' style='background-color:#ffffff;' class = 'qntyval form-control search decimalinput cls-qtyrecv', autocomplete = 'off' min='0'  value='" + QuantityReceived + "'>";
                                }
                            }
                            else if (ClearAllStatus == true) {
                                var QuantityReceived = 0
                                return "<input type='number' style='background-color:#ffffff;' class = 'qntyval form-control search decimalinput cls-qtyrecv', autocomplete = 'off' min='0'  value='" + QuantityReceived + "'>";

                            }
                            else if (ClearSelectedStatus == true) {
                                var exist = $.grep(QuantitySelectedItemArray, function (obj) {
                                    return obj.PurchaseOrderLineItemId === full.PurchaseOrderLineItemId;
                                });
                                var existClear = $.grep(ClearSelectedItemArray, function (obj) {
                                    return obj.PurchaseOrderLineItemId === full.PurchaseOrderLineItemId;
                                });
                                if (exist.length > 0) {
                                    var QuantityReceived = exist[0].QuantityReceived;
                                    return "<input type='number' style='background-color:#d7f9c7;' class = 'qntyval form-control search decimalinput cls-qtyrecv', autocomplete = 'off' min='0'  value='" + QuantityReceived + "'>";
                                }
                                else if (existClear.length > 0) {
                                    var QuantityReceived = 0
                                    return "<input type='number' style='background-color:#ffffff;' class = 'qntyval form-control search decimalinput cls-qtyrecv', autocomplete = 'off' min='0'  value='" + QuantityReceived + "'>";
                                }
                                else {
                                    var QuantityReceived = 0
                                    return "<input type='number' style='background-color:#ffffff;' class = 'qntyval form-control search decimalinput cls-qtyrecv', autocomplete = 'off' min='0'  value='" + QuantityReceived + "'>";
                                }
                            }
                            else {
                                var QuantityReceived = 0
                                return "<input type='number' style='background-color:#ffffff;' class = 'qntyval form-control search decimalinput cls-qtyrecv', autocomplete = 'off' min='0'  value='" + QuantityReceived + "'>";
                            }
                        }
                        else {
                            var QuantityReceived;
                            var exist = $.grep(QuantitySelectedItemArray, function (obj) {
                                return obj.PurchaseOrderLineItemId === full.PurchaseOrderLineItemId;
                            });
                            if (exist.length > 0) {
                                QuantityReceived = exist[0].QuantityReceived;
                            }
                            else {
                                QuantityReceived = 0
                            }
                            return "<input type='number' style='background-color:#ffffff;' class = 'qntyval form-control search decimalinput cls-qtyrecv', autocomplete = 'off' min='0'  value='" + QuantityReceived + "'>";
                        }
                    }
                },
                { "data": "QuantityToDate", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Status_Display", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "QuantityBackOrdered", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "ObjectId", defaultContent: "", "bSearchable": false, "bSortable": false }

            ],
        'rowCallback': function (row, data, index) {
            if (AcceptAllStatus == true) {
                if ($('td', row).eq(0).find('.chkfrow').is(':checked')) {
                    $('td', row).eq(0).find('.chkfrow').prop('checked', false);
                }
            }
            if (AcceptSelectedStatus == true) {

                var exist = $.grep(PurchaseOrderReceiptLineItemArray, function (obj) {
                    return obj === data.PurchaseOrderLineItemId;
                });
                if (exist.length > 0) {
                    if ($('td', row).eq(0).find('.chkfrow').is(':checked')) {
                        $('td', row).eq(0).find('.chkfrow').prop('checked', false);
                    }
                }
            }
        },
        initComplete: function () {
            ClearSelectedItemArray = [];
            if (IsExternal == "True") {
                var column = this.api().column(0);
                column.visible(false);
                $(document).find('.cls-qtyrecv').attr('disabled', 'disabled');
            }
            else {
                var column = this.api().column(0);
                column.visible(true);
                $(document).find('.cls-qtyrecv').removeAttr('disabled');
            }
        }
    });
}
$(document).on('click', "#btnLitemSearch", function () {
    clearLineItemAdvanceSearch();
    dtLineItemTable.state.clear();
    generateLineiItemdataTable();
});
$(document).on('click', '#lineitemClearAdvSearchFilter', function () {
    $("#txtsearchbox").val("");
    $('#poridselectall').prop("checked", false);
    clearLineItemAdvanceSearch();
    generateLineiItemdataTable();
});
$(document).on('click', "#btnLItemDataAdvSrch", function () {
    dtLineItemTable.state.clear();
    var searchitemhtml = "";
    filteritemcount = 0;
    lItemfilteritemcount = 0;
    $('#txtsearchbox').val('');
    $('#litemadvsearchsidebar').find('.adv-item').each(function (index, item) {
        if ($(this).val()) {
            lItemfilteritemcount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times litembtnCross" aria-hidden="true"></a></span>';
            //searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times litembtnCross" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $('#lineitemadvsearchfilteritems').html(searchitemhtml);
    $('.sidebar').removeClass('active');
    $('.overlay').fadeOut();
    LineItemAdvanceSearch();
    $('.lifilteritemcount').text(lItemfilteritemcount);
});
function clearLineItemAdvanceSearch() {
    var filteritemcount = 0;
    $('#litemadvsearchsidebar').find('input:text').val('');
    $('.lifilteritemcount').text(filteritemcount);
    $('#lineitemadvsearchfilteritems').html('');
}
function LineItemAdvanceSearch() {
    generateLineiItemdataTable();
}
$(document).on('click', '.litembtnCross', function () {
    var btnCrossedId = $(this).parent().attr('id');
    var searchtxtId = btnCrossedId.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    lItemfilteritemcount--;
    LineItemAdvanceSearch();
    $('.lifilteritemcount').text(lItemfilteritemcount);
});
$(document).on('click', '#lineitemsidebarCollapse', function () {
    $('.sidebar').addClass('active');
    $('.overlay').fadeIn();
    $('.collapse.in').toggleClass('in');
    $('a[aria-expanded=true]').attr('aria-expanded', 'false');
});
$(document).on('click', '#poridselectall', function (e) {
    PurchaseOrderReceiptLineItemArray = [];
    var checked = this.checked;
    var PurchaseOrderId = $(document).find('#PurchaseOrderModel_PurchaseOrderId').val();
    var LineNumber = LRTrim($("#LineNo").val());
    var PurchaseOrderLineItemId = LRTrim($("#PurchaseOrderLineItemId").val());
    var PartId = LRTrim($("#PartID").val());
    var Description = LRTrim($("#Description").val());
    var Part_ManufacturerID = LRTrim($("#PartManufacturerID").val());
    var Quantity = LRTrim($("#OrderQty").val());
    var UOM = LRTrim($('#UOM').val());
    var UnitCost = LRTrim($("#UnitCost").val());
    var Status = $("#Status").val();
    if (Status) {
        Status = LRTrim(Status);
    }
    else {
        Status = "";
    }
    var visibility = lineNumberGridSecurity;
    var LineItemStaus = lineNumberGridStatus;
    var QtyReceived = LRTrim($("#QuantityReceived").val());
    var QtyToDate = LRTrim($("#QuantityToDate").val());
    var BackOrdered = LRTrim($("#QuantityBackOrdered").val());
    var ChargeType = LRTrim($("#ChargeType").val());
    var ChargeToId = LRTrim($("#ChargeToId").val());
    var UnitOfMeasure = LRTrim($("#UnitOfMeasure").val());
    var StockType = LRTrim($("#StockType").val());
    var StoreroomId = LRTrim($("#StoreroomId").val());
    var Creator_PersonnelId = LRTrim($("#Creator_PersonnelId").val());
    var AccountId = LRTrim($("#AccountId").val());
    var CurrentAverageCost = LRTrim($("#CurrentAverageCost").val());
    var CurrentAppliedCost = LRTrim($("#CurrentAppliedCost").val());
    var CurrentOnHandQuantity = LRTrim($("#CurrentOnHandQuantity").val());
    var OrderQuantity = LRTrim($("#OrderQuantity").val());
    var PartStoreroomId = LRTrim($("#PartStoreroomId").val());
    searchresult = [];
    var checked = this.checked;
    $.ajax({
        url: '/PurchaseOrderReceipt/PopulateLineItemdata',
        data: {
            PurchaseOrderId: PurchaseOrderId,
            LineNumber: LineNumber,
            PartId: PartId,
            Description: Description,
            Quantity: Quantity,
            UOM: UOM,
            UnitCost: UnitCost,
            QtyReceived: QtyReceived,
            QtyToDate: QtyToDate,
            Status: Status,
            BackOrdered: BackOrdered,
            PurchaseOrderLineItemId: PurchaseOrderLineItemId,
            ChargeType: ChargeType,
            ChargeToId: ChargeToId,
            UnitOfMeasure: UnitOfMeasure,
            StockType: StockType,
            StoreroomId: StoreroomId,
            Creator_PersonnelId: Creator_PersonnelId,
            AccountId: AccountId,
            CurrentAverageCost: CurrentAverageCost,
            CurrentAppliedCost: CurrentAppliedCost,
            CurrentOnHandQuantity: CurrentOnHandQuantity,
            OrderQuantity: OrderQuantity,
            PartStoreroomId: PartStoreroomId,
            Part_ManufacturerID: Part_ManufacturerID
        },
        async: true,
        type: "GET",
        beforeSend: function () {
            ShowLoader();
        },
        datatype: "json",
        success: function (data) {
            if (data) {
                $.each(data, function (index, item) {
                    searchresult.push(item.PartId);
                    if (checked) {
                        if (PurchaseOrderReceiptLineItemArray.indexOf(item.PurchaseOrderLineItemId) == -1)
                            PurchaseOrderReceiptLineItemArray.push(item.PurchaseOrderLineItemId);

                        var exist = $.grep(QuantitySelectedItemArray, function (obj) {
                            return obj.PurchaseOrderLineItemId === item.PurchaseOrderLineItemId;
                        });

                        if (exist.length == 0) {
                            var item1 = new GetQuantitySelectedItem(item.PurchaseOrderLineItemId, item.PartStoreroomId, item.CurrentAverageCost, item.CurrentAppliedCost, item.CurrentOnHandQuantity, item.UnitCost, item.AccountId, item.Creator_PersonnelId, item.StoreroomId, item.Description, item.StockType, item.UnitOfMeasure, item.ChargeToId, item.ChargeType, item.PartId, item.OrderQuantity, item.PurchaseOrderId, item.QuantityReceived, item.LineNumber, item.QuantityToDate, 0, item.UOMConversion, item.PurchaseUOM, item.EstimatedCostsId);
                            QuantitySelectedItemArray.push(item1);
                        }
                    }
                    else {
                        var i = PurchaseOrderReceiptLineItemArray.indexOf(item.PurchaseOrderLineItemId);
                        PurchaseOrderReceiptLineItemArray.splice(i, 1);
                        QuantitySelectedItemArray = $.grep(QuantitySelectedItemArray, function (obj) {
                            return obj.PurchaseOrderLineItemId !== item.PurchaseOrderLineItemId;
                        });
                    }
                });
            }
        },
        complete: function () {
            dtLineItemTable.column(0).nodes().to$().each(function (index, item) {
                if (checked) {
                    $(this).find('.chkfrow').prop('checked', 'checked');
                } else {
                    $(this).find('.chkfrow').prop('checked', false);
                }
            });
            CloseLoader();
        }
    });
});
$(document).on('change', '.chkfrow', function () {
    var thisTr = $(this).closest("tr");
    var data = dtLineItemTable.row($(this).parents('tr')).data();
    if (!this.checked) {
        var el = $('#poridselectall').get(0);
        if (el && el.checked && ('indeterminate' in el)) {
            el.indeterminate = true;
        }
        selectedcount--;
        var index = PurchaseOrderReceiptLineItemArray.indexOf(data.PurchaseOrderLineItemId);
        var indexClearSelectedarray = ClearSelectedItemArray.indexOf(data.PurchaseOrderLineItemId);
        PurchaseOrderReceiptLineItemArray.splice(index, 1);
        ClearSelectedItemArray.splice(indexClearSelectedarray, 1);
        thisTr.removeClass("checked");
        QuantitySelectedItemArray = $.grep(QuantitySelectedItemArray, function (obj) {
            return obj.PurchaseOrderLineItemId !== data.PurchaseOrderLineItemId;
        });
    }
    else {
        var existInLineItemArray = $.grep(PurchaseOrderReceiptLineItemArray, function (obj) {
            return obj === data.PurchaseOrderLineItemId;
        });
        if (existInLineItemArray.length == 0) {
            PurchaseOrderReceiptLineItemArray.push(data.PurchaseOrderLineItemId);
        }
        var existInClearSelectedItemArray = $.grep(ClearSelectedItemArray, function (obj) {
            return obj === data.PurchaseOrderLineItemId;
        });
        if (existInClearSelectedItemArray.length == 0) {
            ClearSelectedItemArray.push(data.PurchaseOrderLineItemId);
        }
        var exist = $.grep(QuantitySelectedItemArray, function (obj) {
            return obj.PurchaseOrderLineItemId === data.PurchaseOrderLineItemId;
        });

        if (QuantitySelectedItemArray.length > 0 && exist.length > 0) {

        }
        else {
            var thisPart = new GetQuantitySelectedItem(data.PurchaseOrderLineItemId, data.PartStoreroomId, data.CurrentAverageCost, data.CurrentAppliedCost, data.CurrentOnHandQuantity, data.UnitCost, data.AccountId, data.Creator_PersonnelId, data.StoreroomId, data.Description, data.StockType, data.UnitOfMeasure, data.ChargeToId, data.ChargeType, data.PartId, data.OrderQuantity, data.PurchaseOrderId, data.QuantityReceived, data.LineNumber, data.QuantityToDate, 0, data.UOMConversion, data.PurchaseUOM, data.EstimatedCostsId);
            QuantitySelectedItemArray.push(thisPart);
        }
    }
});
$(document).on('change', '.qntyval', function () {
    var data = dtLineItemTable.row($(this).parents('tr')).data();
    var PurchaseOrderLineItemId = data.PurchaseOrderLineItemId;
    var PartStoreroomId = data.PartStoreroomId;
    var CurrentAverageCost = data.CurrentAverageCost;
    var CurrentAppliedCost = data.CurrentAppliedCost;
    var CurrentOnHandQuantity = data.CurrentOnHandQuantity;
    var UnitCost = data.UnitCost;
    var AccountId = data.AccountId;
    var Creator_PersonnelId = data.Creator_PersonnelId;
    var StoreroomId = data.StoreroomId;
    var Description = data.Description;
    var StockType = data.StockType;
    var UnitOfMeasure = data.UnitOfMeasure;
    var ChargeToId = data.ChargeToId;
    var ChargeType = data.ChargeType;
    var PartId = data.PartId;
    var PurchaseOrderId = data.PurchaseOrderId;
    var OrderQuantity = data.OrderQuantity;
    var LineNumber = data.LineNumber;
    var QtyToDate = data.QuantityToDate;
    var QuantityReceived = $(this).val();
    var UOMConversion = data.UOMConversion;
    var PurchaseUOM = data.PurchaseUOM;
    var EstimatedCostsId = data.EstimatedCostsId;
    var exist = $.grep(QuantitySelectedItemArray, function (obj) {
        return obj.PurchaseOrderLineItemId === PurchaseOrderLineItemId;
    });
    if (QuantitySelectedItemArray.length > 0 && exist.length > 0) {
        var index = -1;
        for (var i = 0; i < QuantitySelectedItemArray.length; ++i) {
            if (QuantitySelectedItemArray[i].PurchaseOrderLineItemId == PurchaseOrderLineItemId) {
                index = i;
                break;
            }
        }
        QuantitySelectedItemArray[index].QuantityReceived = QuantityReceived;
    }
    else {
        var item = new GetQuantitySelectedItem(PurchaseOrderLineItemId, PartStoreroomId, CurrentAverageCost, CurrentAppliedCost, CurrentOnHandQuantity, UnitCost, AccountId, Creator_PersonnelId, StoreroomId, Description, StockType, UnitOfMeasure, ChargeToId, ChargeType, PartId, OrderQuantity, PurchaseOrderId, QuantityReceived, LineNumber, QtyToDate, 0, UOMConversion, PurchaseUOM, EstimatedCostsId);
        QuantitySelectedItemArray.push(item);
    }
});
function GetQuantitySelectedItem(PurchaseOrderLineItemId, PartStoreroomId, CurrentAverageCost, CurrentAppliedCost, CurrentOnHandQuantity, UnitCost, AccountId, Creator_PersonnelId, StoreroomId, Description, StockType, UnitOfMeasure, ChargeToId, ChargeType, PartId, OrderQuantity, PurchaseOrderId, QuantityReceived, LineNumber, QtyToDate, POReceiptHeaderId, UOMConversion, PurchaseUOM, EstimatedCostsId) {
    this.PurchaseOrderLineItemId = PurchaseOrderLineItemId;
    this.PartStoreroomId = PartStoreroomId;
    this.CurrentAverageCost = CurrentAverageCost;
    this.CurrentAppliedCost = CurrentAppliedCost;
    this.CurrentOnHandQuantity = CurrentOnHandQuantity;
    this.UnitCost = UnitCost;
    this.AccountId = AccountId;
    this.Creator_PersonnelId = Creator_PersonnelId;
    this.StoreroomId = StoreroomId;
    this.Description = Description
    this.StockType = StockType;
    this.UnitOfMeasure = UnitOfMeasure;
    this.ChargeToId = ChargeToId;
    this.ChargeType = ChargeType;
    this.PartId = PartId;
    this.OrderQuantity = OrderQuantity;
    this.PurchaseOrderId = PurchaseOrderId;
    this.QuantityReceived = QuantityReceived;
    this.LineNumber = LineNumber;
    this.QtyToDate = QtyToDate;
    this.POReceiptHeaderId = POReceiptHeaderId;
    this.UOMConversion = UOMConversion;
    this.PurchaseUOM = PurchaseUOM;
    this.EstimatedCostsId = EstimatedCostsId;
};
function funcPOReceiveLineItem(PurchaseOrderLineItemId, PurchaseOrderId, PartStoreroomId, OrderQuantity, QuantityReceived, QuantityToDate, CurrentAverageCost, CurrentAppliedCost, CurrentOnHandQuantity, UnitCost, AccountId, Creator_PersonnelId, PartId, StoreroomId, Description, StockType, UnitOfMeasure, ChargeToId, ChargeType, POReceiptHeaderId, UOMConversion, PurchaseUOM, EstimatedCostsId) {
    this.PurchaseOrderLineItemId = PurchaseOrderLineItemId;
    this.PurchaseOrderId = PurchaseOrderId;
    this.PartStoreroomId = PartStoreroomId;
    this.OrderQuantity = OrderQuantity;
    this.QuantityReceived = QuantityReceived;
    this.QuantityToDate = QuantityToDate;
    this.CurrentAverageCost = CurrentAverageCost;
    this.CurrentAppliedCost = CurrentAppliedCost;
    this.CurrentOnHandQuantity = CurrentOnHandQuantity;
    this.UnitCost = UnitCost;
    this.AccountId = AccountId;
    this.Creator_PersonnelId = Creator_PersonnelId;
    this.PartId = PartId;
    this.StoreroomId = StoreroomId;
    this.Description = Description;
    this.StockType = StockType;
    this.UnitOfMeasure = UnitOfMeasure;
    this.ChargeToId = ChargeToId;
    this.ChargeType = ChargeType;
    this.POReceiptHeaderId = POReceiptHeaderId;
    this.UOMConversion = UOMConversion;
    this.PurchaseUOM =PurchaseUOM;
    this.EstimatedCostsId = EstimatedCostsId;
}
//#endregion
//#region Common
function SetPOControls() {
    CloseLoader();
    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse(document);
    $('input, form').blur(function () {
        $(this).valid();
    });
    $('.select2picker, form').change(function () {
        var areaddescribedby = $(this).attr('aria-describedby');
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
    });
    $(document).find('.select2picker').select2({});
    $(document).find('.dtpicker').datepicker({
        "dateFormat": "mm/dd/yy",
        autoclose: true
    });
}
//#endregion
//#region SelectPartgrid
var spartgridselectCount = 0;
var PartNotInInventorySelectedItemArray = [];
var FinalGridSelectedItemArray = [];
var finalSelectPartsTable;
var SelectPartsTable;
$(document).on('change', '.chksearch', function () {
    var data = SelectPartsTable.row($(this).parents('tr')).data();
    if (!this.checked) {
        PartNotInInventorySelectedItemArray = PartNotInInventorySelectedItemArray.filter(function (el) {
            return el.PartId != data.PartId;
        });
    }
    else {
        var item = new PartNotInInventorySelectedItem(data.PartId, data.ClientLookupId, data.Description, data.Manufacturer, data.Quantity);
        var found = PartNotInInventorySelectedItemArray.some(function (el) {
            return el.PartId == data.PartId;
        });
        if (!found) { PartNotInInventorySelectedItemArray.push(item); }
    }
});
$(document).on('click', "#btnAcceptAll", function () {
    ClearSelectedItemArray = [];
    AcceptAllStatus = true;
    AcceptSelectedStatus = false;
    AcceptClearStatus = true;
    ClearAllStatus = false;
    ClearSelectedStatus = false;
    generateLineiItemdataTable();
});
$(document).on('click', "#btnClearAll", function () {
    PurchaseOrderReceiptLineItemArray = [];
    QuantitySelectedItemArray = [];
    ClearSelectedItemArray = [];
    AcceptClearStatus = true;
    AcceptSelectedStatus = false;
    AcceptAllStatus = false;
    ClearAllStatus = true;
    ClearSelectedStatus = false;
    generateLineiItemdataTable();
});
$(document).on('click', "#btnAcceptSelected", function () {
    var chkboxlength = 0;
    dtLineItemTable.column(0).nodes().to$().each(function (index, item) {
        var ischecked = $(this).find('.chkfrow')[0].checked;
        if (ischecked) {
            chkboxlength = chkboxlength + 1;
        }

    });
    if (chkboxlength > 0) {
        if (PurchaseOrderReceiptLineItemArray.length > 0) {
            ClearSelectedItemArray = [];
            AcceptAllStatus = false;
            AcceptSelectedStatus = true;
            AcceptClearStatus = true;
            ClearAllStatus = false;
            ClearSelectedStatus = false;
            generateLineiItemdataTable();
        }
    }
    else {
        ShowGridItemSelectionAlert();
        return false;
    }
    $('#poridselectall').prop("checked", false);
});
$(document).on('click', "#btnClearSelected", function () {
    var chkboxlength = 0;
    dtLineItemTable.column(0).nodes().to$().each(function (index, item) {
        var ischecked = $(this).find('.chkfrow')[0].checked;
        if (ischecked) {
            chkboxlength = chkboxlength + 1;
        }
    });
    if (chkboxlength > 0) {
        if (PurchaseOrderReceiptLineItemArray.length > 0) {
            PurchaseOrderReceiptLineItemArray = PurchaseOrderReceiptLineItemArray.filter(function (id) {
                return ClearSelectedItemArray.indexOf(id) < 0;
            });

            QuantitySelectedItemArray = QuantitySelectedItemArray.filter(function (item) {
                return ClearSelectedItemArray.indexOf(item.PurchaseOrderLineItemId) < 0;
            });
            AcceptAllStatus = false;
            AcceptSelectedStatus = false;
            ClearSelectedStatus = true;
            AcceptClearStatus = true;
            ClearAllStatus = false;
            generateLineiItemdataTable();
        }
    }
    else {
        ShowGridItemSelectionAlert();
        return false;
    }
    $('#poridselectall').prop("checked", false);
});
$(document).on('click', "#btnSearch", function () {
    window.location.href = '/PurchaseOrderReceipt/Index?page=Procurement_Receipts';
});
$(document).on('click', "#btnReceive", function () {
    var returnExit = false;
    var NoOfItems = 0;
    var ItemsReceived = 0;
    var ItemsIssued = 0;
    $.each(QuantitySelectedItemArray, function (index, item) {
        var PurchaseOrderLineItemId = item.PurchaseOrderLineItemId;
        var PartStoreroomId = item.PartStoreroomId;
        var CurrentAverageCost = item.CurrentAverageCost;
        var CurrentAppliedCost = item.CurrentAppliedCost;
        var CurrentOnHandQuantity = item.CurrentOnHandQuantity;
        var UnitCost = item.UnitCost;
        var AccountId = item.AccountId;
        var Creator_PersonnelId = item.Creator_PersonnelId;
        var StoreroomId = item.StoreroomId;
        var Description = item.Description;
        var StockType = item.StockType;
        var UnitOfMeasure = item.UnitOfMeasure;
        var ChargeToId = item.ChargeToId;
        var ChargeType = item.ChargeType;
        var POReceiptHeaderId = item.POReceiptHeaderId;
        var PartId = item.PartId;
        var LineNumber = item.LineNumber;
        var PurchaseOrderId = item.PurchaseOrderId;
        var QuantityReceived = item.QuantityReceived;
        var QuantityToDate = item.QtyToDate;
        var OrderQuantity = item.OrderQuantity;
        var NewQuantityReceived = parseInt(QuantityReceived) + parseInt(QuantityToDate);
        //var NewQuantityReceived = parseInt(QuantityReceived) + Math.round(QuantityToDate / item.UOMConversion);
        var UOMConversion = item.UOMConversion;
        var PurchaseUOM = item.PurchaseUOM;
        var EstimatedCostsId = item.EstimatedCostsId;
        if (returnExit == false) {
            if (PartId > 0 && QuantityReceived > 0) {
                ItemsReceived++;
                if (ChargeToId > 0) {
                    ItemsIssued++;
                }
            }
            else if (PartId <= 0 && QuantityReceived > 0) {
                ItemsIssued++;
            }
            if (QuantityReceived > 0) {
                NoOfItems++;
            }
            if ((QuantityReceived > OrderQuantity && QuantityReceived > 0) || (QuantityToDate == OrderQuantity && QuantityReceived > 0) || (NewQuantityReceived > OrderQuantity)) {
                returnExit = true;
                ShowErrorAlert(getResourceValue("ReceiveMoreOrderAlert") + "  " + LineNumber + ")");
                return false;
            }
            else {
                var item = new funcPOReceiveLineItem(PurchaseOrderLineItemId,
                    PurchaseOrderId,
                    PartStoreroomId,
                    OrderQuantity,
                    QuantityReceived,
                    QuantityToDate,
                    CurrentAverageCost,
                    CurrentAppliedCost,
                    CurrentOnHandQuantity,
                    UnitCost,
                    AccountId,
                    Creator_PersonnelId,
                    PartId,
                    StoreroomId,
                    Description,
                    StockType,
                    UnitOfMeasure,
                    ChargeToId,
                    ChargeType,
                    POReceiptHeaderId,
                    UOMConversion,
                    PurchaseUOM,
                    EstimatedCostsId
                );
                PurchaseorderReceiveLineItemSelectedArray.push(item);
            }
        }
    });
    if (returnExit == false) {
        var PORData = PurchaseorderReceiveLineItemSelectedArray;
        var PurchaseOrderId = $(document).find('#PurchaseOrderModel_PurchaseOrderId').val();
        if (NoOfItems == 0) {
            swal({
                title: getResourceValue("CommonErrorAlert"),
                text: getResourceValue("btnReceivedAlert"),
                type: "error",
                showCancelButton: false,
                confirmButtonClass: "btn-sm btn-danger",
                cancelButtonClass: "btn-sm",
                confirmButtonText: getResourceValue("SaveAlertOk"),
                cancelButtonText: getResourceValue("CancelAlertNo")
            }, function () {
            });
            return false;
        }
        else {
            $.ajax({
                url: '/PurchaseOrderReceipt/AddPurchaseOrderReceipt',
                data: { PurchaseOrderId: PurchaseOrderId, ItemsReceived: ItemsReceived, ItemsIssued: ItemsIssued, NoOfItems: NoOfItems },
                dataType: 'html',
                type: "post",
                beforeSend: function () {
                    ShowLoader();
                },
                success: function (data) {
                    $('#renderpurchaseOrderReceipt').html(data);
                   
                },
                complete: function () {
                  
                    if ($('#purchaseOrderReceiptModel_PrintReceiptCheck').val() == 'True') {
                        $('#chkPrint').prop('checked', true);
                    }
                    SetPOControls();
                }
            });
        }
    }
});
function PurchaseOrderReceiptAddOnSuccess(data) {
    if (data.Result == "success") {
        var message;
        var POReceiptHeaderIdval = data.POReceiptHeaderId;
        var PORData = PurchaseorderReceiveLineItemSelectedArray;
        var PurchaseOrderId = 0;
        for (var i in PORData) {
            PORData[i].POReceiptHeaderId = POReceiptHeaderIdval;
            PurchaseOrderId = PORData[i].PurchaseOrderId;
        }
        if (PurchaseorderReceiveLineItemSelectedArray.length == 0) {
            CloseLoader();
            return;
        }
        PORData = JSON.stringify({ 'PORData': PORData });
        PurchaseorderReceiveLineItemSelectedArray = [];
        QuantitySelectedItemArray = [];
        ClearSelectedItemArray = [];
        PurchaseOrderReceiptLineItemArray = [];
        AcceptClearStatus = false;
        AcceptSelectedStatus = false;
        ClearAllStatus = false;
        ClearSelectedStatus = false;
        AcceptAllStatus = false;
        $.ajax({
            url: '/PurchaseOrderReceipt/AddPurchaseOrderReceiptLineItem',
            data: PORData,
            contentType: 'application/json; charset=utf-8',
            type: "POST",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                SuccessAlertSetting.text = getResourceValue("ItemReceiveAlert");
                CloseLoader();
                swal(SuccessAlertSetting, function () {
                    
                    if ($('#chkPrint').prop('checked')) {
                        /* PrintPOReceipt(POReceiptHeaderIdval, PurchaseOrderId);*/
                        $.ajax({
                            url: '/PurchaseOrderReceipt/SetPrintPORFromIndex',
                            data: JSON.stringify({ 'PurchaseOrderId': PurchaseOrderId,'POReceiptHeaderId': POReceiptHeaderIdval}),
                            type: "POST",
                            datatype: "json",
                            contentType: 'application/json; charset=utf-8',
                            //responseType: 'arraybuffer',
                            beforeSend: function () {
                                ShowLoader();
                            },
                            success: function (result) {
                                if (result.success) {
                                    window.open("/PurchaseOrderReceipt/GeneratePurchaseOrderReceiptPrint", "_blank");
                                }
                            },
                            complete: function () {
                                CloseLoader();
                                $(".updateArea").hide();
                                $(".actionBar").fadeIn();
                                $(document).find('.chksearch').prop('checked', false);
                                $('.itemcount').text(0);
                                SelectedWoIdToCancel = [];
                                RedirectToPORDetail(PurchaseOrderId, "overview");
                               
                            }
                        });
                    }
                    else {
                        RedirectToPORDetail(PurchaseOrderId, "overview");
                    }
                });
            },
            complete: function () {
                //CloseLoader();
               
            },
            error: function (jqXHR, exception) {
                CloseLoader();
            }
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
        CloseLoader();
    }
}
function RedirectToPORDetail(PurchaseOrderId, mode) {
    var titletext = localStorage.getItem("poreceiptstatustext");//V2-331
    $.ajax({
        url: "/PurchaseOrderReceipt/Details",
        type: "POST",
        dataType: 'html',
        data: { PurchaseOrderId: PurchaseOrderId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpurchaseOrderReceipt').html(data);
            $(document).find('#spnlinkToSearch').text(titletext);//V2-331
        },
        complete: function () {
            CloseLoader();
            if (mode === "overview") {
                $('#PurchasingOverview').trigger('click');
            }
            generateLineiItemdataTable();
            $("#HistoryLineItems").hide();
        },
        error: function () {
            CloseLoader();
        }
    });

}
$(document).on('click', "#btnsPORLcancel", function () {
    //V2-938 Start
    if (PurchaseorderReceiveLineItemSelectedArray.length > 0) {
        PurchaseorderReceiveLineItemSelectedArray = [];
    }
    //V2-938 End
    var PurchaseOrderId = $(document).find('#purchaseOrderReceiptModel_PurchaseOrderId').val();
    RedirectToPORDetail(PurchaseOrderId, "overview");
});
//#endregion
//#region QR Code
$(document).on('change', '.chksearch', function () {
    var data = partsSearchdt.row($(this).parents('tr')).data();
    if (!this.checked) {
        selectedcount--;
        var index = partToQR.indexOf(data.PartId);
        partToQR.splice(index, 1);
        PartToClientLookupIdQRcode.splice(index, 1);
    }
    else {
        partToQR.push(data.PartId);
        selectedcount = selectedcount + partToQR.length;
        PartToClientLookupIdQRcode.push(data.ClientLookupId + '][' + data.Description + '][' + data.Location1_5 + '][' + data.Consignment + '][' + data.RepairablePart);
    }
    if (partToQR.length > 0) {
        $('#printQrcode').removeAttr("disabled");
    }
    else {
        $('#printQrcode').prop("disabled", "disabled");
    }
    $('.itemcount').text(partToQR.length);
});
$('#printQrcode').on('click', function () {
    var partClientLookups = PartToClientLookupIdQRcode;
    if (partClientLookups.length > 50) {
        var datamsg = getResourceValue("PrintLimitFiftyItemsAlert");
        var msg = getResourceValue(datamsg);
        GenericSweetAlertMethod(msg);
        return false;
    }
    else {
        $.ajax({
            //url: "/PurchaseOrderReceipt/PartQRcode",//v2-724
            url: "/PurchaseOrderReceipt/POReceiptDetailPartQRcode",
            data: {
                PartClientLookups: partClientLookups
            },
            type: "POST",
            beforeSend: function () {
                ShowLoader();
            },
            datatype: "html",
            success: function (data) {
                $('#printPartqrcodemodalcontainer').html(data);
            },
            complete: function () {
                //$('#printPartQrCodeModal').modal('show');
                $('#printPOReceiptQrCodeModal').modal('show');
                $('#btnGenerateQr').prop("disabled", "disabled");
                $(document).find('.itemQRcount').text($('.QRrcode').length);
                CloseLoader();
            },
            error: function () {
                CloseLoader();
            }
        });
    }
});
$(document).on('click', '.printPOLineItemBttn', function () {
    var partid = $(this).attr('data-partid');
    //#region V2-1115
    var datarow = dtLineItemTable.row($(this).parents('tr')).data();
    var workorder = datarow.ChargeToClientLookupId;
    var requestor = datarow.RequestorName; 
    var UOMConversion = datarow.UOMConversion; 
    var description = datarow.Description; 
    var PartStoreroomId = datarow.PartStoreroomId; 
    //#endregion
    PartToClientLookupIdQRcode = [];
    $.ajax({
        url: "/PurchaseOrderReceipt/GetQRcodeInfo",
        data: {
            partid: partid,
            PartStoreroomId: PartStoreroomId
        },
        type: "POST",
        beforeSend: function () {
            ShowLoader();
        },
        datatype: "html",
        success: function (data) {           
            PartToClientLookupIdQRcode.push(data.ClientLookupId + '][' + data.Description + '][' + (data.PTLocation == null ? " " : data.PTLocation) + '][' + data.UOM + '][' + data.Minimum + '][' + data.Maximum + '][' + data.Manufacturer + '][' + data.ManufacturerId + '][' + workorder + '][' + UOMConversion + '][' + description + '][' + requestor);
            var partClientLookups = PartToClientLookupIdQRcode;
            if ($('#EPMInvoiceImportInUse').val() == "True") {
                generateQRforEPM(partClientLookups);
            }
            else {
                $.ajax({
                    url: "/PurchaseOrderReceipt/POReceiptDetailPartQRcode",
                    data: {
                        PartClientLookups: partClientLookups
                    },
                    type: "POST",
                    beforeSend: function () {
                        ShowLoader();
                    },
                    datatype: "html",
                    success: function (data) {
                        $('#printPartqrcodemodalcontainer').html(data);
                    },
                    complete: function () {
                        $('#printPOReceiptQrCodeModal').modal('show');
                        $(document).find('.itemQRcount').text($('.QRrcode').length);
                        CloseLoader();
                    },
                    error: function () {
                        CloseLoader();
                    }
                });
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
//#endregion
//#region ColumnVisibility
$(document).on('click', '#liCustomize', function () {
    funCustomizeBtnClick(dtPoReceiptTable,true);
});
$(document).on('click', '.saveConfig', function () {
    var colOrder = [0];
    funCustozeSaveBtn(dtPoReceiptTable, colOrder);
    run = true;
    dtPoReceiptTable.state.save(run);
});
//#endregion

$(document).on('click', '#PurchasingOrderSidebar', function () {
    $('#PurchasingOverview').show();
    $("#LineItems").show();
    $("#HistoryLineItems").hide();
    $('#DetailsTab').show();
    $('#btnDetails').addClass('active');
});

function PrintPOReceipt(poReceiptHeaderId, PurchaseOrderId) {
    var data = poReceiptHeaderId;
    $.ajax({
        url: '/PurchaseOrderReceipt/PrintPOReceipt',
        type: "POST",
        data: { POReceiptHeaderId: data },
        responseType: 'arraybuffer',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (result) {
            if (result.success) {
                PdfPrintPOReceipt(result.pdf);
            }
        },
        complete: function () {
            CloseLoader();
            $(".updateArea").hide();
            $(".actionBar").fadeIn();
            $(document).find('.chksearch').prop('checked', false);
            $('.itemcount').text(0);
            SelectedWoIdToCancel = [];
            RedirectToPORDetail(PurchaseOrderId, "overview");
        }
    });
};
function PdfPrintPOReceipt(pdf) {
    var blob = b64StrtoBlob(pdf, 'application/pdf');
    var blobUrl = URL.createObjectURL(blob);
    window.open(blobUrl, "_blank");
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
    var blob = new Blob(byteArrays, { type: contentType });
    return blob;
}


//#region V2-389
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
                    searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + item.key + '">' + $("label[for='" + item.key + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
                }
            }
        }
        
        if (item.key == 'Status') {
            $("#Status").trigger('change.select2');
        }
        if (item.key == 'VendorClientLookupId') {
            $("#VendorClientLookupId").trigger('change.select2');
        }
        advcountercontainer.text(selectCount);
    });
    advcountercontainer.text(selectCount);
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    searchstringcontainer.html(searchitemhtml);

}
//#endregion


//#region Dropdown toggle V2-331  
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

$(document).on('click', '.posearchdrpbox', function (e) {
    $(".updateArea").hide();
    $(".actionBar").fadeIn();
    $(document).find('.chksearch').prop('checked', false);
    $('.itemcount').text(0);
    SelectPOId = [];
    run = true;
  
    if ($(this).attr('id') != '0') {
        $('#posearchtitle').text($(this).text());
    }
    else {
        $('#posearchtitle').text(getResourceValue("OpenAlert"));
    }
    $(".searchList li").removeClass("activeState");
    $(this).addClass('activeState');
    $(document).find('#searcharea').hide("slide");
    var optionval = $(this).attr('id');
    localStorage.setItem("PURCHASEORDERRECEIPTSTATUS", optionval);
    CustomQueryDisplayId = optionval;
    ShowbtnLoaderclass("LoaderDrop");
    PRAdvSearch();
    if ($(document).find('#txtColumnSearch').val() !== '')
        $("#advsearchfilteritems").html('');
    $(document).find('#txtColumnSearch').val('');
    dtPoReceiptTable.page('first').draw('page');
});

//#endregion

//#region New Search button V2-331 
$(document).on('click', "#SrchBttnNew", function () {
    $.ajax({
        url: '/Base/PopulateNewSearchList',
        type: 'GET',
        data: { tableName: 'PurchaseOrderReceipt' },
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
    var data = localStorage.getItem("PURCHASEORDERRECEIPTSTATUS");
    CustomQueryDisplayId = data;
    $.ajax({
        url: '/Base/ModifyNewSearchList',
        type: 'POST',
        data: { tableName: 'PurchaseOrderReceipt', searchText: txtSearchval, isClear: isClear },
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
                dtPoReceiptTable.page('first').draw('page');
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

//#region Search V2-331 
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
    var data = localStorage.getItem("PURCHASEORDERRECEIPTSTATUS");
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
        CustomQueryDisplayId = data;
        dtPoReceiptTable.page('first').draw('page');
    }
    var container = $(document).find('#searchBttnNewDrop');
    container.hide("slideToggle");
}

function SetPOReceiptDetailsControls() {
    CloseLoader();
    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse(document);
    $(document).find('.select2picker').select2({});
    SetFixedHeadStyle();
}
//#endregion

//#region V2-1115
function generateQRforEPM(partClientLookups) {
    $.ajax({
        type: "POST",
        url: "/PurchaseOrderReceipt/SetPartIdlistforEPM",
        data: {
            PartClientLookups: partClientLookups
        },
        success: function (data) {
            if (data.success === 0) {
                window.open('/PurchaseOrderReceipt/GenerateEPMPartQRcode', '_blank');
                partClientLookups = [];
            }
        },
        error: function (xhr, status, error) {
            console.error("Error generating QR code:", error);
        }
    });
}
//#endregion
