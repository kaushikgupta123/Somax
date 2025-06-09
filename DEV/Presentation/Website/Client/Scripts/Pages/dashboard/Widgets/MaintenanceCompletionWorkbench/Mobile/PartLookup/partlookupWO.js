var run = false;
var selectedcount = 0;
var tablecarItemarr = [];
var myproductCartArr = [];
var partLokkUpdt;
var layoutTypeWO = 1;
var layoutVal;
var pLookUpGridName = "PartLookUpWO_Search";
var cardviewstartvalue = 0;
var cardviewlwngth = 10;
var grdcardcurrentpage = 1;
var currentorderedcolumn = 1;
var currentorder = 'asc';
var orderbycol = 1;
var orderDir = 'asc';
var dtpart = '<div class="symbol tableImageSrc"><img src="#val0" /></div><div class="dtlsBox tablePartId"><h2>#val1</h2><p>#val2</p><h3 style="display:none">#val3</h3><h4 style="display:none">#val4</h4></div>';
var colstr = '<div class="dtlsBox menufacDtl"><h2>#val1</h2><p>#val2</p><p class="PurchaseUOM" style="display:none">#val3</p><p class="UOMConvRequired" style="display:none">#val4</p><p class="IssueOrder" style="display:none">#val5</p><p class="VendorCatalogItemId" style="display:none">#val6</p><p class="QtyMaximum" style="display:none">#val7</p><p class="VendorId" style="display:none">#val8</p><p class="PartCategoryMasterId" style="display:none">#val9</p><p class="Indexid" style="display:none">#val10</p></div>';
var colpricestr = '<div class="dtlsBox priceDtl"><h2>#val1</h2><p>#val2</p></div>';
var actionvendorcatlogstr = '<input type="text" data-Qty-Id="tablepartQtyCard" data-PartId="#valPartId" data-indexid="#valIndexId" class="form-control search qtyothertext qtytableItemQty allownumericWithdecimal decimalinputupto2places " maskedFormat="8,2" onkeyup="getCurQty(this)" value="#valQty" maxlength="11"  placeholder="Qty">' +
    '<input type="text" data-Price-Tab-Id="tablepartPrice" data-PartId="#valPartIdNew" class="form-control search pricetext tableUnitPrice allownumericWithdecimal" onkeyup="getCurPrize(this)" style="display:none" placeholder="Price" maxlength="10"  value="#val0">' +
    '<input type="hidden" class="form-control search pricetext tableUnitRate" value="#val1">' +
    '<input type="hidden" class="form-control search tableUOM" placeholder="Price" value="#val2">' +
    '<input type="hidden" class="form-control search hidtableTotalPrice" placeholder="Price" value="#val2">' +
    '#RequiredHtmlContent' +
    '<input type="hidden" class="form-control search hidtableStoreroomId" placeholder="StoreroomId" value="#val3">' +
    '<input type="hidden" class="form-control search hidtablePartStoreroomId" placeholder="PartStoreroomId" value="#val4">' +
    '<input type="hidden" class="form-control search hidtableAccountId" placeholder="AccountId" value="#val5">' +
    '<input type="hidden" class="form-control search hidtableUnitOfMeasure" placeholder="UnitOfMeasure" value="#val6">' +
    '<button type="button" class="btn btn-danger btn-cart btn-addToCartTable" data-AddtoCart-Id="btn-MyaddToCartTable" title="Add to Cart"><i class="fa fa-shopping-cart"></i></button>';

var actionnotvcstr = '<input type="text" data-Qty-Id="tablepartQtyCard" data-PartId="#valPartId" data-indexid="#valIndexId" class="form-control search qtyothertext qtytableItemQty allownumericWithdecimal decimalinputupto2places " maskedFormat="8,2" onkeyup="getCurQty(this)" value="#valQty" maxlength="11"  placeholder="Qty">' +
    '<input type="text" data-Price-Tab-Id="tablepartPrice" data-PartId="#valPartIdNew" class="form-control search pricetext tableUnitPrice allownumericWithdecimal" onkeyup="getCurPrize(this)" style="display:none"  placeholder="Price" maxlength="10" value="#val0">' +
    '<input type="hidden" class="form-control search pricetext tableUnitRate" value="#val1">' +
    '<input type="hidden" class="form-control search tableUOM" placeholder="Price" value="#val2">' +
    '<input type="hidden" class="form-control search hidtableTotalPrice" placeholder="Price" value="#val2">' +
    '#RequiredHtmlContent' +
    '<input type="hidden" class="form-control search hidtableStoreroomId" placeholder="StoreroomId" value="#val3">' +
    '<input type="hidden" class="form-control search hidtablePartStoreroomId" placeholder="PartStoreroomId" value="#val4">' +
    '<input type="hidden" class="form-control search hidtableAccountId" placeholder="AccountId" value="#val5">' +
    '<input type="hidden" class="form-control search hidtableUnitOfMeasure" placeholder="UnitOfMeasure" value="#val6">' +
    '<button type="button" class="btn btn-danger btn-cart btn-addToCartTable" data-AddtoCart-Id="btn-MyaddToCartTable" title="Add to Cart"><i class="fa fa-shopping-cart"></i></button>';
var Vendorid = 0;
var RequiredHtmlContent = '<input type="hidden" class="form-control search hidRequiredDate" value="#valRequiredDate">';
var ReplaceRequiredHtmlContent = '';
$(document).ready(function () {
    $(document).on('click', "#tableviewLayoutPartLookupWO", function () {
        if (layoutTypeWO == 2) {
            return;
        }
        layoutTypeWO = 2;
        ShowbtnLoader("layoutsortmenuPartLookupWO");
        layoutVal = $(document).find('#tableviewLayoutPartLookupWO').text();
        $('#layoutsortmenuPartLookupWO').text("Layout" + " : " + layoutVal);
        $(document).find('#ActiveCardPartLookupWO').hide();
        $(document).find('#tableViewPartLookupWO').show();
        $(document).find('#btnAllAddToCartPartLookupWO').show();
        HidebtnLoader("layoutsortmenuPartLookupWO");
        if (partLokkUpdt) {
            partLokkUpdt.page.len(cardviewlwngth).order([[currentorderedcolumn, currentorder]]).page(grdcardcurrentpage - 1).draw('page');
            $(document).find('#tblpartlookup_length .searchdt-menu').val(cardviewlwngth).trigger('change.select2');
            if ($('.sidebarCartList').length > 0) {
                FillDataFromLayoutView('TableView');
            }
            if (tablecarItemarr.length > 0) {
                $(".updateArea").fadeIn();
            }
            else {
                $(".updateArea").hide();
            }
        }
        else {
            GeneratePartLookUpGrid();
            if ($('.sidebarCartList').length > 0) {
                FillDataFromLayoutView('TableView');
            }

            if (tablecarItemarr.length > 0) {
                $(".updateArea").fadeIn();
            }
            else {
                $(".updateArea").hide();
            }
        }

    });
});
function cardviewstate(currentpage, start, length, currentorderedcolumn, sorttext, order) {
    this.currentpage = currentpage;
    this.start = start;
    this.length = length;
    this.currentorderedcolumn = currentorderedcolumn;
    this.sorttext = sorttext;
    this.order = order;
}

function GeneratePartLookUpGrid() {
    var ShoppingCart = $(document).find('#ShoppingCart').val();
    var WorkOrderID = $(document).find('#WorkOrderID').val();
    var StoreroomId = $('#StoreroomId').val();
    orderbycol = $('li.srtWOcolumn.sort-active').data('col');
    orderDir = $('li.srtWOorder.sort-active').data('mode');
    if ($(document).find('#tblpartlookup').hasClass('dataTable')) {
        partLokkUpdt.destroy();
    }
    partLokkUpdt = $("#tblpartlookup").DataTable({
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
                    data.order[0][0] = orderbycol;
                    data.order[0][1] = orderDir;
                }
                var filterinfoarray = getfilterinfoarray($("#txtsearch"));
                $.ajax({
                    "url": gridStateSaveUrl,
                    "data": {
                        GridName: pLookUpGridName,
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
                //"url": gridStateLoadUrl,
                "url": "/Base/GetLayout",
                "data": {
                    GridName: pLookUpGridName
                },
                "async": false,
                "dataType": "json",
                "success": function (json) {
                    //if (json) {
                    //    callback(JSON.parse(json));
                    //}
                    //else {
                    //    callback(json);
                    //}
                    if (json) {
                        if (json.LayoutInfo !== '') {
                            var LayoutInfo = JSON.parse(json.LayoutInfo);
                            orderbycol = LayoutInfo.order[0][0];
                            orderDir = LayoutInfo.order[0][1];
                            callback(JSON.parse(json.LayoutInfo));
                            if (json.FilterInfo !== '') {
                                setsearchui(JSON.parse(json.FilterInfo), $("#txtsearch"), $("#searchfilteritems"));
                            }
                        }
                        else {
                            callback(json.LayoutInfo);
                        }
                    }
                    else {
                        callback(json);
                    }
                }
            });
        },
        scrollX: true,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Dashboard/GetPartLookUpGridWO_Mobile",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.orderbycol = $('li.srtWOcolumn.sort-active').data('col');
                d.orderDir = $('li.srtWOorder.sort-active').data('mode');
                d.searchString = LRTrim($(document).find('#txtsearch').val());
                d.StoreroomId = StoreroomId;
            },
            "dataSrc": function (result) {
                if (result.data.length < 1) {
                    $(document).find('#btnAllAddToCartPartLookupWO').hide();
                }
                else {
                    $(document).find('#btnAllAddToCartPartLookupWO').show();
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

                    "data": "PartId",
                    orderable: false,
                    "bSortable": false,
                    className: 'select-checkbox dt-body-center',
                    'render': function (data, type, full, meta) {
                        var myNewdata = "" + data + "";
                        var myIndexdata = "" + full.indexid + "";
                        var className = 'index_' + data + '_' + full.indexid;
                        if (getIndexIfObjWithAttr(tablecarItemarr, 'PartId', myNewdata, 'Indexid', myIndexdata) != -1) {
                            return '<input type="checkbox" checked="checked" id="chkItemMe_' + data + '"  data-eqid="' + data + '"  class="chksearch chkcheckItem ' + data + ' ' + full.indexid + ' ' + className + '" onclick=ClickMeItem(this)>';
                        }
                        else {
                            return '<input type="checkbox" id="chkItemMe_' + data + '"  data-eqid="' + data + '"  class="chksearch chkcheckItem ' + data + ' ' + full.indexid + ' ' + className + '" onclick=ClickMeItem(this)>';
                        }
                    }
                },
                {
                    "data": "ClientLookupId",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": false,
                    "className": "text-left",
                    "name": "0",
                    "mRender": function (data, type, full) {
                        return dtpart.replace('#val0', full.ImageUrl).replace('#val1', full.ClientLookupId).replace('#val2', full.Description).replace('#val3', full.PartId).replace('#val4', full.InVendorCatalog);
                    }
                },
                {
                    "data": "Manufacturer", "autoWidth": true, "bSearchable": true, "bSortable": false, "name": "2",
                    mRender: function (data, type, full, meta) {
                        return colstr.replace('#val1', full.Manufacturer).replace('#val2', full.ManufacturerId).replace('#val3', full.PurchaseUOM).replace('#val4', full.UOMConvRequired).replace('#val5', full.IssueOrder).replace('#val6', full.VendorCatalogItemId).replace('#val7', full.QtyMaximum).replace('#val8', full.VendorId).replace('#val9', full.PartCategoryMasterId).replace('#val10', full.indexid);
                    }
                },
                {
                    "data": "ManufacturerId", "autoWidth": true, "bSearchable": true, "bSortable": false, "name": "3", className: 'text-right',
                    mRender: function (data, type, full, meta) {
                        return colpricestr.replace('#val1', full.Price).replace('#val2', full.Unit);

                    }
                },
                {
                    "className": "text-center lastActiontd", "bSortable": false,
                    mRender: function (data, type, full, meta) {
                        var myNewdata = "" + full.PartId + "";
                        var myIndexdata = "" + full.indexid + "";
                        ReplaceRequiredHtmlContent = '';
                        if (full.InVendorCatalog) {
                            if (getIndexIfObjWithAttr(tablecarItemarr, 'PartId', myNewdata, 'Indexid', myIndexdata) != -1) {
                                var index = getIndexIfObjWithAttr(tablecarItemarr, 'PartId', myNewdata, 'Indexid', myIndexdata);
                                if (ShoppingCart && ShoppingCart === "True" && WorkOrderID === "0") {
                                    ReplaceRequiredHtmlContent = RequiredHtmlContent.replace('#valRequiredDate', tablecarItemarr[index].RequiredDate);
                                }
                                return actionvendorcatlogstr.replace('#val0', tablecarItemarr[index].UnitCost).replace('#val1', tablecarItemarr[index].UnitCost).replace('#val2', tablecarItemarr[index].UnitofMeasure).replace('#valQty', tablecarItemarr[index].OrderQuantity).replace('#valPartId', full.PartId).replace('#valPartIdNew', full.PartId).replace('#RequiredHtmlContent', ReplaceRequiredHtmlContent).replace('#valIndexId', full.indexid).replace('#val3', tablecarItemarr[index].StoreroomId).replace('#val4', tablecarItemarr[index].PartStoreroomId).replace('#val5', tablecarItemarr[index].AccountId).replace('#val6', tablecarItemarr[index].UnitOfMeasureIssue);
                            }
                            else {
                                if (ShoppingCart && ShoppingCart === "True" && WorkOrderID === "0") {
                                    ReplaceRequiredHtmlContent = RequiredHtmlContent.replace('#valRequiredDate', full.RequiredDate);
                                }
                                return actionvendorcatlogstr.replace('#val0', full.Price).replace('#val1', full.Price).replace('#val2', full.Unit).replace('#valQty', 1).replace('#valPartId', full.PartId).replace('#valPartIdNew', full.PartId).replace('#RequiredHtmlContent', ReplaceRequiredHtmlContent).replace('#valIndexId', full.indexid).replace('#val3', full.StoreroomId).replace('#val4', full.PartStoreroomId).replace('#val5', full.AccountId).replace('#val6', full.UnitOfMeasure);
                            }
                        }
                        else {
                            if (getIndexIfObjWithAttr(tablecarItemarr, 'PartId', myNewdata, 'Indexid', myIndexdata) != -1) {
                                var mindex = getIndexIfObjWithAttr(tablecarItemarr, 'PartId', myNewdata, 'Indexid', myIndexdata);

                                if (ShoppingCart && ShoppingCart === "True" && WorkOrderID === "0") {
                                    ReplaceRequiredHtmlContent = RequiredHtmlContent.replace('#valRequiredDate', tablecarItemarr[mindex].RequiredDate);
                                }
                                if (isNaN(tablecarItemarr[mindex].OrderQuantity)) {
                                    return actionnotvcstr.replace('#val0', tablecarItemarr[mindex].UnitCost).replace('#val1', tablecarItemarr[mindex].UnitCost).replace('#val2', tablecarItemarr[mindex].UnitofMeasure).replace('#valQty', '').replace('#valPartId', full.PartId).replace('#valPartIdNew', full.PartId).replace('#RequiredHtmlContent', ReplaceRequiredHtmlContent).replace('#valIndexId', full.indexid).replace('#val3', tablecarItemarr[mindex].StoreroomId).replace('#val4', tablecarItemarr[mindex].PartStoreroomId).replace('#val5', tablecarItemarr[mindex].AccountId).replace('#val6', tablecarItemarr[mindex].UnitOfMeasureIssue);
                                }
                                else {
                                    if (full.UOMConvRequired) {
                                        return actionvendorcatlogstr.replace('#val0', tablecarItemarr[mindex].UnitCost).replace('#val1', tablecarItemarr[mindex].UnitCost).replace('#val2', tablecarItemarr[mindex].UnitofMeasure).replace('#valQty', tablecarItemarr[mindex].OrderQuantity).replace('#valPartId', full.PartId).replace('#valPartIdNew', full.PartId).replace('#RequiredHtmlContent', ReplaceRequiredHtmlContent).replace('#valIndexId', full.indexid).replace('#val3', tablecarItemarr[mindex].StoreroomId).replace('#val4', tablecarItemarr[mindex].PartStoreroomId).replace('#val5', tablecarItemarr[mindex].AccountId).replace('#val6', tablecarItemarr[mindex].UnitOfMeasureIssue);
                                    }
                                    else {
                                        return actionnotvcstr.replace('#val0', tablecarItemarr[mindex].UnitCost).replace('#val1', tablecarItemarr[mindex].UnitCost).replace('#val2', tablecarItemarr[mindex].UnitofMeasure).replace('#valQty', tablecarItemarr[mindex].OrderQuantity).replace('#valPartId', full.PartId).replace('#valPartIdNew', full.PartId).replace('#RequiredHtmlContent', ReplaceRequiredHtmlContent).replace('#valIndexId', full.indexid).replace('#val3', tablecarItemarr[mindex].StoreroomId).replace('#val4', tablecarItemarr[mindex].PartStoreroomId).replace('#val5', tablecarItemarr[mindex].AccountId).replace('#val6', tablecarItemarr[mindex].UnitOfMeasureIssue);
                                    }
                                }

                            }
                            else {
                                if (ShoppingCart && ShoppingCart === "True" && WorkOrderID === "0") {
                                    ReplaceRequiredHtmlContent = RequiredHtmlContent.replace('#valRequiredDate', full.RequiredDate);
                                }
                                if (full.UOMConvRequired) {
                                    return actionvendorcatlogstr.replace('#val0', full.Price).replace('#val1', full.Price).replace('#val2', full.Unit).replace('#valQty', 1).replace('#valPartId', full.PartId).replace('#valPartIdNew', full.PartId).replace('#RequiredHtmlContent', ReplaceRequiredHtmlContent).replace('#valIndexId', full.indexid).replace('#val3', full.StoreroomId).replace('#val4', full.PartStoreroomId).replace('#val5', full.AccountId).replace('#val6', full.UnitOfMeasure);
                                }
                                else {
                                    return actionnotvcstr.replace('#val0', full.Price).replace('#val1', full.Price).replace('#val2', full.Unit).replace('#valQty', 1).replace('#valPartId', full.PartId).replace('#valPartIdNew', full.PartId).replace('#RequiredHtmlContent', ReplaceRequiredHtmlContent).replace('#valIndexId', full.indexid).replace('#val3', full.StoreroomId).replace('#val4', full.PartStoreroomId).replace('#val5', full.AccountId).replace('#val6', full.UnitOfMeasure);
                                }
                            }
                        }
                    }
                }
            ],
        initComplete: function () {
            SetPageLengthMenu();
            switch (orderbycol) {
                case 1:
                    $(document).find('.srtWOcolumn').eq(0).addClass('sort-active');
                    sorttext = $(document).find('.srtWOcolumn').eq(0).text();
                    break;
                case 2:
                    $(document).find('.srtWOcolumn').eq(1).addClass('sort-active');
                    sorttext = $(document).find('.srtWOcolumn').eq(1).text();
                    break;
                case 3:
                    $(document).find('.srtWOcolumn').eq(2).addClass('sort-active');
                    sorttext = $(document).find('.srtWOcolumn').eq(2).text();
                    break;
                case 5:
                    $(document).find('.srtWOcolumn').eq(3).addClass('sort-active');
                    sorttext = $(document).find('.srtWOcolumn').eq(3).text();
                    break;
                case 6:
                    $(document).find('.srtWOcolumn').eq(4).addClass('sort-active');
                    sorttext = $(document).find('.srtWOcolumn').eq(4).text();
                    break;
            }
            switch (orderDir) {
                case "asc":
                    $(document).find('.srtWOorder').eq(0).addClass('sort-active');
                    break;
                case "desc":
                    $(document).find('.srtWOorder').eq(1).addClass('sort-active');
                    break;
            }

            HideAddtoCartButton($('#tblpartlookup'), $(document).find('#btnAllAddToCartPartLookupWO'));
        }
    });
}
function HideAddtoCartButton(dataTbl, btnExort) {
    if (dataTbl.dataTable().fnGetData().length < 1)
        btnExort.hide();
    else
        btnExort.show();
}

function ShowCardViewWO() {
    GetLayoutWO();
    if (cardviewstartvalue >= cardviewlwngth) {
        grdcardcurrentpage = parseInt(cardviewstartvalue / cardviewlwngth) + 1;
    }
    else {
        grdcardcurrentpage = 1;
    }
    Vendorid = $(document).find("#VendorId").val();
    if ($('#ModeForRedirect').val() == null || $('#ModeForRedirect').val() == undefined) {
        var ModeForRedirect = 'WorkOrder';
    }
    else {
        var ModeForRedirect = $('#ModeForRedirect').val();
    }
    if (!orderbycol) {
        orderbycol = 1;
    }

    if (!orderDir) {
        orderDir = 'asc';
    }
    var sorttext = '';
    $.ajax({
        url: '/Dashboard/GetCardViewDataWO_Mobile',
        type: 'POST',
        data: {
            currentpage: grdcardcurrentpage,
            start: cardviewstartvalue,
            length: cardviewlwngth,
            currentorderedcolumn: currentorderedcolumn,
            currentorder: currentorder,
            searchString: LRTrim($(document).find('#txtsearch').val()),
            WorkOrderID: $(document).find('#WorkOrderID').val(),
            StoreroomId: $(document).find('#StoreroomId').val(),
            ModeForRedirect: ModeForRedirect
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#ActiveCardPartLookupWO').html(data).show();
            $(document).find('#tblpartlookup_paginate li').each(function (index, value) {
                $(this).removeClass('active');
                if ($(this).data('currentpage') == grdcardcurrentpage) {
                    $(this).addClass('active');
                }
            });
        },
        complete: function () {

            switch (orderbycol) {
                case 1:
                    $(document).find('.srtWOcolumn').eq(0).addClass('sort-active');
                    sorttext = $(document).find('.srtWOcolumn').eq(0).text();
                    break;
                case 2:
                    $(document).find('.srtWOcolumn').eq(1).addClass('sort-active');
                    sorttext = $(document).find('.srtWOcolumn').eq(1).text();
                    break;
                case 3:
                    $(document).find('.srtWOcolumn').eq(2).addClass('sort-active');
                    sorttext = $(document).find('.srtWOcolumn').eq(2).text();
                    break;
                case 5:
                    $(document).find('.srtWOcolumn').eq(3).addClass('sort-active');
                    sorttext = $(document).find('.srtWOcolumn').eq(3).text();
                    break;
                case 6:
                    $(document).find('.srtWOcolumn').eq(4).addClass('sort-active');
                    sorttext = $(document).find('.srtWOcolumn').eq(4).text();
                    break;
            }
            switch (currentorder) {
                case "asc":
                    $(document).find('.srtWOorder').eq(0).addClass('sort-active');
                    break;
                case "desc":
                    $(document).find('.srtWOorder').eq(1).addClass('sort-active');
                    break;
            }
            $(document).find('#wocardviewpagelengthdrp').select2({ minimumResultsForSearch: -1 }).val(cardviewlwngth).trigger('change.select2');
            HidebtnLoader("btnsortmenu");
            HidebtnLoader("layoutsortmenuPartLookupWO");
            $('#btnsortmenu').text(getResourceValue("spnSorting") + " : " + sorttext);
            if ($('.sidebarCartList').length > 0) {
                FillDataFromLayoutView('CardView');
            }
            $(document).find('.dtpicker').datepicker({
                changeMonth: true,
                changeYear: true,
                beforeShow: function (i) { if ($(i).attr('readonly')) { return false; } },
                "dateFormat": "mm/dd/yy",
                autoclose: true,
                minDate: new Date()
            }).inputmask('mm/dd/yyyy');
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}

function ShowCardViewWO_FromDashboard(WorkOrderID) {
    GetLayoutWO();
    if (cardviewstartvalue >= cardviewlwngth) {
        grdcardcurrentpage = parseInt(cardviewstartvalue / cardviewlwngth) + 1;
    }
    else {
        grdcardcurrentpage = 1;
    }
    //  Vendorid = $(document).find("#VendorId").val();
    if ($('#ModeForRedirect').val() == null || $('#ModeForRedirect').val() == undefined) {
        var ModeForRedirect = 'Dashboard';
    }
    else {
        var ModeForRedirect = $('#ModeForRedirect').val();
    }
    if (!orderbycol) {
        orderbycol = 1;
    }

    if (!orderDir) {
        orderDir = 'asc';
    }
    var sorttext = '';
    $.ajax({
        url: '/Dashboard/GetCardViewDataWO_Mobile',
        type: 'POST',
        data: {
            currentpage: grdcardcurrentpage,
            start: cardviewstartvalue,
            length: cardviewlwngth,
            currentorderedcolumn: currentorderedcolumn,
            currentorder: currentorder,
            searchString: LRTrim($(document).find('#txtsearch').val()),
            WorkOrderID: WorkOrderID,
            StoreroomId: $(document).find('#StoreroomId').val(),
            ModeForRedirect: ModeForRedirect
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#ActiveCardPartLookupWO').html(data).show();
            $(document).find('#tblpartlookup_paginate li').each(function (index, value) {
                $(this).removeClass('active');
                if ($(this).data('currentpage') == grdcardcurrentpage) {
                    $(this).addClass('active');
                }
            });
        },
        complete: function () {

            switch (orderbycol) {
                case 1:
                    $(document).find('.srtWOcolumn').eq(0).addClass('sort-active');
                    sorttext = $(document).find('.srtWOcolumn').eq(0).text();
                    break;
                case 2:
                    $(document).find('.srtWOcolumn').eq(1).addClass('sort-active');
                    sorttext = $(document).find('.srtWOcolumn').eq(1).text();
                    break;
                case 3:
                    $(document).find('.srtWOcolumn').eq(2).addClass('sort-active');
                    sorttext = $(document).find('.srtWOcolumn').eq(2).text();
                    break;
                case 5:
                    $(document).find('.srtWOcolumn').eq(3).addClass('sort-active');
                    sorttext = $(document).find('.srtWOcolumn').eq(3).text();
                    break;
                case 6:
                    $(document).find('.srtWOcolumn').eq(4).addClass('sort-active');
                    sorttext = $(document).find('.srtWOcolumn').eq(4).text();
                    break;
            }
            switch (currentorder) {
                case "asc":
                    $(document).find('.srtWOorder').eq(0).addClass('sort-active');
                    break;
                case "desc":
                    $(document).find('.srtWOorder').eq(1).addClass('sort-active');
                    break;
            }
            $(document).find('#wocardviewpagelengthdrp').select2({ minimumResultsForSearch: -1 }).val(cardviewlwngth).trigger('change.select2');
            HidebtnLoader("btnsortmenu");
            HidebtnLoader("layoutsortmenuPartLookupWO");
            $('#btnsortmenu').text(getResourceValue("spnSorting") + " : " + sorttext);
            if ($('.sidebarCartList').length > 0) {
                FillDataFromLayoutView('CardView');
            }
            $(document).find('.dtpicker').datepicker({
                changeMonth: true,
                changeYear: true,
                beforeShow: function (i) { if ($(i).attr('readonly')) { return false; } },
                "dateFormat": "mm/dd/yy",
                autoclose: true,
                minDate: new Date()
            }).inputmask('mm/dd/yyyy');
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}

$(document).on('click', "#cardviewLayoutPartLookupWO", function () {
    if (layoutTypeWO == 1) {
        return;
    }
    if (!orderbycol) {
        orderbycol = 1;
    }
    if (!orderDir) {
        orderDir = 'asc';
    }
    ShowbtnLoader("layoutsortmenuPartLookupWO");
    layoutTypeWO = 1;
    layoutVal = $(document).find('#cardviewLayoutPartLookupWO').text();
    $('#layoutsortmenuPartLookupWO').text("Layout" + " : " + layoutVal);
    $(document).find('#tableViewPartLookupWO').hide();
    $(document).find('#ActiveCardPartLookupWO').show();
    $(document).find('#btnAllAddToCartPartLookupWO').hide();
    HidebtnLoader("layoutsortmenuPartLookupWO");
    $(document).find('#liCustomize').prop("disabled", true);
    if (partLokkUpdt) {
        var info = partLokkUpdt.page.info();
        var pageclicked = info.page;
        cardviewlwngth = info.length;
        cardviewstartvalue = cardviewlwngth * pageclicked;
        grdcardcurrentpage = pageclicked + 1;
        currentorderedcolumn = orderbycol;
        currentorder = orderDir;
    }
    else {
        cardviewstartvalue = 0;
        cardviewlwngth = 10;
        grdcardcurrentpage = 1;
        currentorderedcolumn = 1;
        currentorder = 'asc';
        LayoutFilterinfoUpdateWO();
    }
    ShowCardViewWO();
    $(".updateArea").hide();
});
$(document).on('change', '#wocardviewpagelengthdrp', function () {
    cardviewlwngth = $(this).val();
    grdcardcurrentpage = parseInt(cardviewstartvalue / cardviewlwngth) + 1;
    cardviewstartvalue = parseInt((grdcardcurrentpage - 1) * cardviewlwngth);
    //GetAndSaveState();
    LayoutUpdateWO('pagelength');
    ShowCardViewWO();

});
$(document).on('click', '#tblpartlookup_paginate .paginate_button', function () {
    if (layoutTypeWO == 1) {
        var currentselectedpage = parseInt($(document).find('#tblpartlookup_paginate .pagination').find('.active').text());
        cardviewlwngth = $(document).find('#wocardviewpagelengthdrp').val();
        cardviewstartvalue = cardviewlwngth * (parseInt($(this).find('.page-link').text()) - 1);
        var lastpage = parseInt($(this).prev('li').data('currentpage'));

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
            grdcardcurrentpage = parseInt($(this).prevAll('li').eq(1).text());
            cardviewstartvalue = cardviewlwngth * (grdcardcurrentpage - 1);
        }
        else {
            grdcardcurrentpage = $(this).data('currentpage');
        }
        //GetAndSaveState();
        LayoutUpdateWO('pagination');
        ShowCardViewWO();

    }
    else {
        run = true;
    }
});
$(document).on('change', '#tblpartlookup_length .searchdt-menu', function () {
    run = true;
});
$(document).on('click', '.srtWOcolumn', function () {
    ShowbtnLoader("btnsortmenu");
    $(document).find('.srtWOcolumn').removeClass('sort-active');
    $(this).addClass('sort-active');
    orderbycol = $(this).data('col');
    orderDir = $('li.srtWOorder.sort-active').data('mode');
    if (layoutTypeWO == 1) {
        cardviewstartvalue = 0;
        grdcardcurrentpage = 1;
        currentorderedcolumn = orderbycol;
        currentorder = orderDir;
        //GetAndSaveState();
        LayoutUpdateWO('column');
        ShowCardViewWO();

    }
    else {
        $('#tblpartlookup').DataTable().order([orderbycol, orderDir]).draw();
    }
    $('#btnsortmenu').text(getResourceValue("spnSorting") + " : " + $(this).text());

    run = true;
});
$(document).on('click', '.srtWOorder', function () {
    $(document).find('.srtWOorder').removeClass('sort-active');
    $(this).addClass('sort-active');
    orderbycol = $(this).parent('ul').find('li.srtWOcolumn.sort-active').data('col');
    orderDir = $(this).data('mode');
    if (layoutTypeWO == 1) {
        cardviewstartvalue = 0;
        grdcardcurrentpage = 1;
        currentorderedcolumn = orderbycol;
        currentorder = orderDir;
        //GetAndSaveState();
        LayoutUpdateWO('order');
        ShowCardViewWO();

    }
    else {
        ShowbtnLoader("btnsortmenu");
        $('#tblpartlookup').DataTable().order([orderbycol, orderDir]).draw();
    }
    run = true;
});
$(document).on('click', '#icon-search', function () {
    run = true;
    if (layoutTypeWO == 1) {
        cardviewstartvalue = 0;
        grdcardcurrentpage = 1;
        LayoutFilterinfoUpdateWO();
        ShowCardViewWO();
    }
    else {
        partLokkUpdt.page('first').draw('page');
    }
    $('#spnsearchbtn').html('Search: ' + LRTrim($(document).find('#txtsearch').val()) + '&nbsp;<a href="javascript:void(0);" class="fa fa-times" aria-hidden="true" id="btnclearsearch"></a>');
    if ($(document).find('#txtsearch').val()) {
        $('#searchfilteritems').show();
    }
    else {
        $('#searchfilteritems').hide();
    }
});
$(document).on('click', '#btnclearsearch', function () {
    $(document).find('#txtsearch').val('');
    $(document).find('#icon-search').trigger('click');
});
$(document).on('keyup', '#txtsearch', function (event) {
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if (keycode == 13) {
        $(document).find('#icon-search').trigger('click');
    }
    else {
        event.preventDefault();
    }
});
function GetAndSaveState() {
    $.ajax({
        "url": gridStateLoadUrl,
        "data": {
            GridName: pLookUpGridName
        },
        "async": false,
        "dataType": "json",
        "success": function (json) {
            var gridstate = JSON.parse(json);
            gridstate.order[0] = [currentorderedcolumn, currentorder];
            $.ajax({
                "url": gridStateSaveUrl,
                "data": {
                    GridName: pLookUpGridName,
                    LayOutInfo: JSON.stringify(gridstate)
                },
                "dataType": "json",
                "type": "POST",
                "success": function () { return; }
            });
        }
    });
}

//#region Part Lookup for Line Item Add 
function ClickMeItem(r) {
    if ($(r).is(":checked")) {
        var eqId = '.' + $(r).attr('data-eqid');
        var tblClientLookUpPartId = $(r).closest('tr').children('td').find('.tablePartId').children('h2').text();
        var tblPartId = $(r).closest('tr').children('td').find('.tablePartId').children('h3').text();
        var tblInVendorCatalog = $(r).closest('tr').children('td').find('.tablePartId').children('h4').text();
        var tblPartDesc = $(r).closest('tr').children('td').find('.tablePartId').children('p').text();
        var tblPartImageUrl = $(r).closest('tr').children('td').find('.tableImageSrc').find('img').attr('src');
        var tblitemQty = $(r).closest('tr').children('td').find('.qtytableItemQty').val();
        var tblUnitPrice = $(r).closest('tr').children('td').find('.tableUnitPrice').val();
        var tblTotalUnitPrice = parseFloat(tblitemQty) * parseFloat(tblUnitPrice);
        var tblUOM = $(r).closest('tr').children('td').find('.tableUOM').val();
        var tblStoreroomId = $(r).closest('tr').children('td').find('.hidtableStoreroomId').val();//V2-732
        var tblPartStoreroomId = $(r).closest('tr').children('td').find('.hidtablePartStoreroomId').val();//V2-732
        var tblPurchaseUOM = $(r).closest('tr').children('td').find('.menufacDtl').children('p.PurchaseUOM').text();
        var tblUOMConvRequired = $(r).closest('tr').children('td').find('.menufacDtl').children('p.UOMConvRequired').text();
        var tblIssueOrder = $(r).closest('tr').children('td').find('.menufacDtl').children('p.IssueOrder').text();
        var tblVendorCatalogItemId = $(r).closest('tr').children('td').find('.menufacDtl').children('p.VendorCatalogItemId').text();
        var tblRequiredDate = $(r).closest('tr').find('.hidRequiredDate').val();
        var tblQtyMaximum = $(r).closest('tr').children('td').find('.menufacDtl').children('p.QtyMaximum').text();
        var tblVendorId = $(r).closest('tr').children('td').find('.menufacDtl').children('p.VendorId').text();
        var tblPartCategoryMasterId = $(r).closest('tr').children('td').find('.menufacDtl').children('p.PartCategoryMasterId').text();
        var tblIndexid = $(r).closest('tr').children('td').find('.menufacDtl').children('p.Indexid').text();

        var tblAccountId = $(r).closest('tr').children('td').find('.hidtableAccountId').val();//V2-1068
        var tblUnitOfMeasure = $(r).closest('tr').children('td').find('.hidtableUnitOfMeasure').val();//V2-1068

        tablecarItemarr.push({
            PartId: tblPartId,
            ClientLookUpId: tblClientLookUpPartId,
            InVendorCatalog: tblInVendorCatalog,
            Description: tblPartDesc,
            PartImageUrl: tblPartImageUrl,
            OrderQuantity: parseFloat(tblitemQty),
            UnitCost: parseFloat(tblUnitPrice),
            TotalUnitCost: parseFloat(tblTotalUnitPrice),
            UnitofMeasure: tblUOM,
            PurchaseUOM: tblPurchaseUOM,
            UOMConvRequired: tblUOMConvRequired,
            IssueOrder: tblIssueOrder,
            VendorCatalogItemId: tblVendorCatalogItemId,
            RequiredDate: tblRequiredDate,
            QtyMaximum: tblQtyMaximum,
            VendorId: tblVendorId,
            PartCategoryMasterId: tblPartCategoryMasterId,
            Indexid: tblIndexid,
            StoreroomId: tblStoreroomId,
            PartStoreroomId: tblPartStoreroomId,
            AccountId: tblAccountId,
            UnitOfMeasure: tblUnitOfMeasure

        });
        uniqueArrayWO(tablecarItemarr);
        selectedcount = selectedcount + tablecarItemarr.length;
    }
    else {
        selectedcount--;
        var tblPartId = $(r).closest('tr').children('td').find('.tablePartId').children('h3').text();
        var tblIndexid = $(r).closest('tr').children('td').find('.menufacDtl').children('p.Indexid').text();
        var myNewdata = "" + tblPartId + "";
        var myIndexdata = "" + tblIndexid + "";
        var index = getIndexIfObjWithAttr(tablecarItemarr, 'PartId', myNewdata, 'Indexid', myIndexdata);
        tablecarItemarr.splice(index, 1);
    }
    if (tablecarItemarr.length > 0) {
        $(".updateArea").fadeIn();
    }
    else {
        $(".updateArea").hide();
    }
    $('.chkitemcount').text(tablecarItemarr.length);
}
function getCurQty(r) {
    var UnitCost = 0;
    if ($(r).siblings('.tableUnitPrice').val() != '') {
        UnitCost = parseFloat($(r).siblings('.tableUnitPrice').val());
    }
    var TotalCost = parseFloat(UnitCost) * parseFloat($(r).val());
    var dataPartId = $(r).attr('data-PartId');
    var myNewdata = "" + dataPartId + "";
    var dataIndexId = $(r).attr('data-indexid');
    var myIndexdata = "" + dataIndexId + "";
    if (getIndexIfObjWithAttr(tablecarItemarr, 'PartId', myNewdata, 'Indexid', myIndexdata) != -1) {
        var index = getIndexIfObjWithAttr(tablecarItemarr, 'PartId', myNewdata, 'Indexid', myIndexdata);
        tablecarItemarr[index].OrderQuantity = parseFloat($(r).val());
        tablecarItemarr[index].TotalUnitCost = TotalCost;
    }
}
function getCurPrize(r) {
    var Qty = 0;
    if ($(r).siblings('.qtytableItemQty').val() != '') {
        Qty = parseFloat($(r).siblings('.qtytableItemQty').val());
    }
    var TotalCost = parseFloat(Qty) * parseFloat($(r).val());
    var dataPartId = $(r).attr('data-PartId');
    var myNewdata = "" + dataPartId + "";
    var dataIndexId = $(r).attr('data-indexid');
    var myIndexdata = "" + dataIndexId + "";
    if (getIndexIfObjWithAttr(tablecarItemarr, 'PartId', myNewdata, 'Indexid', myIndexdata) != -1) {
        var index = getIndexIfObjWithAttr(tablecarItemarr, 'PartId', myNewdata, 'Indexid', myIndexdata);
        tablecarItemarr[index].UnitCost = parseFloat($(r).val());
        tablecarItemarr[index].TotalUnitCost = TotalCost;
    }
}
function FillAddtoCartSide(_data, IsDivExists, PartId, Indexid) {
    $.ajax({
        url: '/Dashboard/GetAddToCartDataWO_Mobile',
        type: 'POST',
        data: _data,
        success: function (data) {
            if (IsDivExists) {
                /* $("#advsearchsidebarWorkorder").find($('#' + PartId)).replaceWith(data);*/
                $("#advsearchsidebarWorkorder").find($('.clsIndex_' + Indexid)).replaceWith(data);
                $(".sidebar").mCustomScrollbar({
                    theme: "minimal"
                });
            }
            else {
                var exitlen = $("#advsearchsidebarWorkorder").find($('.clsSBarList_' + PartId)).length;
                var clsIndexexitlen = $("#advsearchsidebarWorkorder").find($('.clsIndex_' + Indexid)).length;

                if (exitlen == 0 || clsIndexexitlen == 0) {
                    $("#advsearchsidebarWorkorder").append(data);
                    var cartDataLength = $('.sidebarCartList').length;
                    $('.cartBadges').text(cartDataLength);
                    $('.filteritemcount').text(cartDataLength);
                }
                $(".sidebar").mCustomScrollbar({
                    theme: "minimal"
                });
            }
        },
        complete: function () {
        },
        error: function () {
        }
    });
}

function FillAllAddtoCartSide(_data) {
    $.ajax({
        url: '/Dashboard/GetAllAddToCartDataWO_Mobile',
        type: 'POST',
        contentType: "application/json",
        data: _data,
        success: function (data) {
            $("#advsearchsidebarWorkorder").html(data);
            var cartDataLength = $('.sidebarCartList').length;
            $('.cartBadges').text(cartDataLength);
            $('.filteritemcount').text(cartDataLength);
            $(".sidebar").mCustomScrollbar({
                theme: "minimal"
            });
        },
        complete: function () {
        },
        error: function () {
        }
    });
}
function uniqueArrayWO(myArray) {
    var newArray = [];
    $.each(myArray, function (key, value) {
        var exists = false;
        $.each(newArray, function (k, val2) {
            if (value.PartId == val2.PartId && value.Indexid == val2.Indexid) { exists = true };
        });
        if (exists == false && value.Indexid != "") { newArray.push(value); }
    });
    return newArray;
}
function AddtoCartItem(rItem) {
    var IsCardView = $("#ActiveCardPartLookupWO").is(":visible");
    var IsTableView = $("#tableViewPartLookupWO").is(":visible");
    var WorkOrderID = $('#WorkOrderID').val();


    if (IsCardView && $(rItem).attr('data-AddmyCart-Id') == 'btn-AddmyCart-Id') {
        var PartClientLookupId = $(rItem).siblings('.clsClientLookupId').text();
        var PartId = $(rItem).siblings('.qtyBox').find('.hidmyPartId').val();
        var InVendorCatalog = $(rItem).siblings('.qtyBox').find('.hidVendorCatalog').val();
        var PartDesc = $(rItem).siblings('.qtyBox').find('.hiddescription').val();
        var PartImageUrl = $(rItem).siblings('.clsproductImg').attr('src');
        var itemQty = $(rItem).siblings('.qtyBox').find('.clsPartQuantity').val();
        var UnitPrice = $(rItem).siblings('.qtyBox').find('.partPriceMe').val();
        var TotalUnitPrice = parseFloat(itemQty) * parseFloat(UnitPrice);
        var PurchaseUnitofMeasure = $(rItem).siblings('.qtyBox').find('.hidpartUOM').val();
        var IsDivExists = false;
        var PurchaseUOM = $(rItem).siblings('.qtyBox').find('.hidPurchaseUOM').val();
        var UOMConvRequired = $(rItem).siblings('.qtyBox').find('.hidUOMConvRequired').val();
        var IssueOrder = $(rItem).siblings('.qtyBox').find('.hidIssueOrder').val();
        var VendorCatalogItemId = $(rItem).siblings('.qtyBox').find('.hidVendorCatalogItemId').val();
        var RequiredDate = $(rItem).siblings('.requiredBox').find('.requiredDate').val();
        //V2-665 
        var QtyMaximum = $(rItem).siblings('.qtyBox').find('.hidQtyMaximum').val();
        //V2-665
        //V2-690
        var VendorId = $(rItem).siblings('.qtyBox').find('.hidVendorId').val();
        var PartCategoryMasterId = $(rItem).siblings('.qtyBox').find('.hidPartCategoryMasterId').val();
        var Indexid = $(rItem).siblings('.qtyBox').find('.hidindexid').val();
        var StoreroomId = $(rItem).siblings('.qtyBox').find('.hidstoreroomid').val();//V2-732
        var PartStoreroomId = $(rItem).siblings('.qtyBox').find('.hidpartstoreroomid').val();//V2-732
        //var dataindexid = $(rItem).siblings('.qtyBox').find('.hidindexid').val();	
        //V2-1068
        var AccountId = $(rItem).siblings('.qtyBox').find('.hidAccountId').val();//V2-1068
        var UnitOfMeasure = $(rItem).siblings('.qtyBox').find('.hidUnitOfMeasure').val();//V2-1068
        //V2-690	
        if (RequiredDate === undefined || RequiredDate === "") {
            RequiredDate = null;
        }
        if ($('#' + PartId).length && $('.clsIndex_' + Indexid).length) {
            IsDivExists = true;
        } else {
            IsDivExists = false;
        }
        var _data =
        {
            model: {
                ImageUrl: PartImageUrl,
                PartId: PartId,
                ClientLookUpId: PartClientLookupId,
                InVendorCatalog: InVendorCatalog,
                Description: PartDesc,
                PartQty: parseFloat(itemQty),
                UnitPrice: parseFloat(UnitPrice),
                TotalUnitPrice: parseFloat(TotalUnitPrice),
                PurchaseUnitofMeasure: PurchaseUnitofMeasure,
                PurchaseUOM: PurchaseUOM,
                UOMConvRequired: UOMConvRequired,
                IssueOrder: IssueOrder,
                VendorCatalogItemId: VendorCatalogItemId,
                RequiredDate: RequiredDate,
                WorkOrderID: WorkOrderID,
                VendorId: VendorId,
                PartCategoryMasterId: PartCategoryMasterId,
                Indexid: Indexid,
                StoreroomId: StoreroomId,
                PartStoreroomId: PartStoreroomId,
                AccountId: AccountId,
                UnitOfMeasure: UnitOfMeasure
            }
        };
        if (parseFloat(itemQty) > parseFloat(QtyMaximum)) {
            swal({
                title: getResourceValue("ConfirmationAlert"),
                type: "warning",
                text: getResourceValue("OrderQuantityAlert") + "(" + itemQty + ")" + getResourceValue("ResultOverMaxAlert") + "(" + QtyMaximum + ")" + getResourceValue("DoYouContinueAlert"),
                html: true,
                showCancelButton: true,
                closeOnConfirm: false,
                confirmButtonClass: "btn-sm btn-primary",
                cancelButtonClass: "btn-sm",
                confirmButtonText: getResourceValue("CancelAlertYes"),
                cancelButtonText: getResourceValue("CancelAlertNo")
            },
                function () {
                    $('.sweet-overlay').fadeOut();
                    $('.showSweetAlert').fadeOut();
                    FillAddtoCartSide(_data, IsDivExists, PartId, Indexid);
                    var btnText = '<i class="fa fa-check addtoCartchkBoxColor" aria-hidden="true"></i>' + ' ' + $(rItem).text();
                    $(rItem).html(btnText);
                    if (!IsDivExists) {

                        $('div.notification').remove();
                        $.notify({
                            icon: 'glyphicon glyphicon-warning-sign',
                            title: getResourceValue("ItemAdded"),
                            timer: 500,
                            message: getResourceValue("ItemAddedToTheCart"),
                            url: '#'
                        }, {
                            type: 'success',
                            template: '<div data-notify="container" class="notification col-xs-11 col-sm-3 alert alert-{0}" role="alert">' +
                                '<button type="button" aria-hidden="true" class="close" data-notify="dismiss"></button>' +
                                '<span data-notify="icon"></span> ' +
                                '<span data-notify="title">{1}</span> ' +
                                '<span data-notify="message">{2}</span>' +
                                '<div class="progress" data-notify="progressbar">' +
                                '<div class="progress-bar progress-bar-{0}" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%;"></div>' +
                                '</div>' +
                                '<a href="{3}" target="{4}" data-notify="url"></a>' +
                                '</div>'
                        });
                    }
                });
        }
        else {
            FillAddtoCartSide(_data, IsDivExists, PartId, Indexid);
            var btnText = '<i class="fa fa-check addtoCartchkBoxColor" aria-hidden="true"></i>' + ' ' + $(rItem).text();
            $(rItem).html(btnText);
            if (!IsDivExists) {

                $('div.notification').remove();
                $.notify({
                    icon: 'glyphicon glyphicon-warning-sign',
                    title: getResourceValue("ItemAdded"),
                    timer: 500,
                    message: getResourceValue("ItemAddedToTheCart"),
                    url: '#'
                }, {
                    type: 'success',
                    template: '<div data-notify="container" class="notification col-xs-11 col-sm-3 alert alert-{0}" role="alert">' +
                        '<button type="button" aria-hidden="true" class="close" data-notify="dismiss"></button>' +
                        '<span data-notify="icon"></span> ' +
                        '<span data-notify="title">{1}</span> ' +
                        '<span data-notify="message">{2}</span>' +
                        '<div class="progress" data-notify="progressbar">' +
                        '<div class="progress-bar progress-bar-{0}" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%;"></div>' +
                        '</div>' +
                        '<a href="{3}" target="{4}" data-notify="url"></a>' +
                        '</div>'
                });
            }
        }


    }
    else if (IsTableView && $(rItem).attr('data-AddtoCart-Id') == 'btn-MyaddToCartTable') {
        var tblClientLookUpId = $(rItem).closest('td').siblings('td:eq(1)').find('.tablePartId').children('h2').text();
        var tblPartId = $(rItem).closest('td').siblings('td:eq(1)').find('.tablePartId').children('h3').text();
        var tblInVendorCatalog = $(rItem).closest('td').siblings('td:eq(1)').find('.tablePartId').children('h4').text();
        var tblPartDesc = $(rItem).closest('td').siblings('td:eq(1)').find('.tablePartId').children('p').text();
        var tblPartImageUrl = $(rItem).closest('td').siblings('td:eq(1)').find('.tableImageSrc').find('img').attr('src');
        var tblitemQty = $(rItem).closest('td').find('.qtytableItemQty').val();
        var tblUnitPrice = $(rItem).closest('td').find('.tableUnitPrice').val();
        var tblTotalUnitPrice = parseFloat(tblitemQty) * parseFloat(tblUnitPrice);
        var tblUOM = $(rItem).closest('td').find('.tableUOM').val();
        var IsMyDivExists = false;
        //V2-553
        var tblPurchaseUOM = $(rItem).closest('td').siblings('td:eq(2)').find('.menufacDtl').children('p.PurchaseUOM').text();
        var tblUOMConvRequired = $(rItem).closest('td').siblings('td:eq(2)').find('.menufacDtl').children('p.UOMConvRequired').text();
        var tblIssueOrder = $(rItem).closest('td').siblings('td:eq(2)').find('.menufacDtl').children('p.IssueOrder').text();
        var tblVendorCatalogItemId = $(rItem).closest('td').siblings('td:eq(2)').find('.menufacDtl').children('p.VendorCatalogItemId').text();
        var tblRequiredDate = $(rItem).closest('td').find('.hidRequiredDate').val();
        //V2-665 
        var tblQtyMaximum = $(rItem).closest('td').siblings('td:eq(2)').find('.menufacDtl').children('p.QtyMaximum').text();
        //V2-665
        //V2-690
        var tblVendorId = $(rItem).closest('td').siblings('td:eq(2)').find('.menufacDtl').children('p.VendorId').text();
        var tblPartCategoryMasterId = $(rItem).closest('td').siblings('td:eq(2)').find('.menufacDtl').children('p.PartCategoryMasterId').text();
        var tblIndexid = $(rItem).closest('td').siblings('td:eq(2)').find('.menufacDtl').children('p.Indexid').text();
        var tblStoreroomId = $(rItem).closest('td').find('.hidtableStoreroomId').val();//V2-732
        var tblPartStoreroomId = $(rItem).closest('td').find('.hidtablePartStoreroomId').val();//V2-732

        var tblAccountId = $(rItem).closest('td').find('.hidtableAccountId').val();//V2-1068
        var tblUnitOfMeasure = $(rItem).closest('td').find('.hidtableUnitOfMeasure').val();//V2-1068
        //V2-690	
        if ($('#' + tblPartId).length && $('.clsIndex_' + tblIndexid).length) {
            IsMyDivExists = true;
        } else {
            IsMyDivExists = false;
        }
        var _mydata =
        {
            model: {
                ImageUrl: tblPartImageUrl,
                PartId: tblPartId,
                ClientLookUpId: tblClientLookUpId,
                InVendorCatalog: tblInVendorCatalog,
                Description: tblPartDesc,
                PartQty: parseFloat(tblitemQty),
                UnitPrice: parseFloat(tblUnitPrice),
                TotalUnitPrice: parseFloat(tblTotalUnitPrice),
                PurchaseUnitofMeasure: tblUOM,
                PurchaseUOM: tblPurchaseUOM,
                UOMConvRequired: tblUOMConvRequired,
                IssueOrder: tblIssueOrder,
                VendorCatalogItemId: tblVendorCatalogItemId,
                RequiredDate: tblRequiredDate,
                WorkOrderID: WorkOrderID,
                VendorId: tblVendorId,
                PartCategoryMasterId: tblPartCategoryMasterId,
                Indexid: tblIndexid,
                StoreroomId: tblStoreroomId,
                PartStoreroomId: tblPartStoreroomId,
                AccountId: tblAccountId,
                UnitOfMeasure: tblUnitOfMeasure
            }
        };
        if (parseFloat(tblitemQty) > parseFloat(tblQtyMaximum)) {
            swal({
                title: getResourceValue("ConfirmationAlert"),
                type: "warning",
                text: getResourceValue("OrderQuantityAlert") + "(" + tblitemQty + ")" + getResourceValue("ResultOverMaxAlert") + "(" + tblQtyMaximum + ")" + getResourceValue("DoYouContinueAlert"),
                html: true,
                showCancelButton: true,
                closeOnConfirm: false,
                confirmButtonClass: "btn-sm btn-primary",
                cancelButtonClass: "btn-sm",
                confirmButtonText: getResourceValue("CancelAlertYes"),
                cancelButtonText: getResourceValue("CancelAlertNo")
            },
                function () {
                    $('.sweet-overlay').fadeOut();
                    $('.showSweetAlert').fadeOut();
                    FillAddtoCartSide(_mydata, IsMyDivExists, tblPartId, tblIndexid);
                    tablecarItemarr.push({
                        PartId: tblPartId,
                        ClientLookUpId: tblClientLookUpId,
                        InVendorCatalog: tblInVendorCatalog,
                        Description: tblPartDesc,
                        PartImageUrl: tblPartImageUrl,
                        OrderQuantity: parseFloat(tblitemQty),
                        UnitCost: parseFloat(tblUnitPrice),
                        TotalUnitCost: parseFloat(tblTotalUnitPrice),
                        UnitofMeasure: tblUOM,
                        PurchaseUOM: tblPurchaseUOM,
                        UOMConvRequired: tblUOMConvRequired,
                        IssueOrder: tblIssueOrder,
                        VendorCatalogItemId: tblVendorCatalogItemId,
                        RequiredDate: tblRequiredDate,
                        VendorId: tblVendorId,
                        PartCategoryMasterId: tblPartCategoryMasterId,
                        Indexid: tblIndexid,
                        StoreroomId: tblStoreroomId,
                        PartStoreroomId: tblPartStoreroomId,
                        AccountId: tblAccountId,
                        UnitOfMeasureIssue: tblUnitOfMeasure
                    });
                    tablecarItemarr = uniqueArrayWO(tablecarItemarr);
                    var classvar = 'index_' + tblPartId + '_' + tblIndexid;
                    $('.' + classvar).prop('checked', true);
                    selectedcount = selectedcount + tablecarItemarr.length;
                    $('.chkitemcount').text(tablecarItemarr.length);
                    if (tablecarItemarr.length > 0) {
                        $(".updateArea").fadeIn();
                    }
                    else {
                        $(".updateArea").hide();
                    }
                    $('div.notification').remove();
                    $.notify({
                        icon: 'glyphicon glyphicon-warning-sign',
                        title: getResourceValue("ItemAdded"),
                        timer: 500,
                        message: getResourceValue("ItemAddedToTheCart"),
                        url: '#'
                    }, {
                        type: 'success',
                        template: '<div data-notify="container" class="notification col-xs-11 col-sm-3 alert alert-{0}" role="alert">' +
                            '<button type="button" aria-hidden="true" class="close" data-notify="dismiss"></button>' +
                            '<span data-notify="icon"></span> ' +
                            '<span data-notify="title">{1}</span> ' +
                            '<span data-notify="message">{2}</span>' +
                            '<div class="progress" data-notify="progressbar">' +
                            '<div class="progress-bar progress-bar-{0}" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%;"></div>' +
                            '</div>' +
                            '<a href="{3}" target="{4}" data-notify="url"></a>' +
                            '</div>'
                    });
                });
        }
        else {
            FillAddtoCartSide(_mydata, IsMyDivExists, tblPartId, tblIndexid);
            tablecarItemarr.push({
                PartId: tblPartId,
                ClientLookUpId: tblClientLookUpId,
                InVendorCatalog: tblInVendorCatalog,
                Description: tblPartDesc,
                PartImageUrl: tblPartImageUrl,
                OrderQuantity: parseFloat(tblitemQty),
                UnitCost: parseFloat(tblUnitPrice),
                TotalUnitCost: parseFloat(tblTotalUnitPrice),
                UnitofMeasure: tblUOM,
                PurchaseUOM: tblPurchaseUOM,
                UOMConvRequired: tblUOMConvRequired,
                IssueOrder: tblIssueOrder,
                VendorCatalogItemId: tblVendorCatalogItemId,
                RequiredDate: tblRequiredDate,
                VendorId: tblVendorId,
                PartCategoryMasterId: tblPartCategoryMasterId,
                Indexid: tblIndexid,
                StoreroomId: tblStoreroomId,
                PartStoreroomId: tblPartStoreroomId,
                AccountId: tblAccountId,
                UnitOfMeasureIssue: tblUnitOfMeasure
            });
            tablecarItemarr = uniqueArrayWO(tablecarItemarr);
            var classvar = 'index_' + tblPartId + '_' + tblIndexid;
            $('.' + classvar).prop('checked', true);
            selectedcount = selectedcount + tablecarItemarr.length;
            $('.chkitemcount').text(tablecarItemarr.length);
            if (tablecarItemarr.length > 0) {
                $(".updateArea").fadeIn();
            }
            else {
                $(".updateArea").hide();
            }
            $('div.notification').remove();
            $.notify({
                icon: 'glyphicon glyphicon-warning-sign',
                title: getResourceValue("ItemAdded"),
                timer: 500,
                message: getResourceValue("ItemAddedToTheCart"),
                url: '#'
            }, {
                type: 'success',
                template: '<div data-notify="container" class="notification col-xs-11 col-sm-3 alert alert-{0}" role="alert">' +
                    '<button type="button" aria-hidden="true" class="close" data-notify="dismiss"></button>' +
                    '<span data-notify="icon"></span> ' +
                    '<span data-notify="title">{1}</span> ' +
                    '<span data-notify="message">{2}</span>' +
                    '<div class="progress" data-notify="progressbar">' +
                    '<div class="progress-bar progress-bar-{0}" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%;"></div>' +
                    '</div>' +
                    '<a href="{3}" target="{4}" data-notify="url"></a>' +
                    '</div>'
            });
        }


    }
}
var getIndexIfObjWithAttr = function (array, attr, value, attrindex, valindex) {
    if (attrindex != '') {
        for (var i = 0; i < array.length; i++) {
            if (array[i][attr] === value && array[i][attrindex] === valindex) {
                return i;
            }
        }
    }
    else {
        for (var i = 0; i < array.length; i++) {
            if (array[i][attr] === value) {
                return i;
            }
        }
    }

    return -1;
}
$(document).on('blur', '.Partqtyvalue', function () {
    var IsCardView = $("#ActiveCardPartLookupWO").is(":visible");
    var IsTableView = $("#tableViewPartLookupWO").is(":visible");
    var orderquantity = $(this).val();
    if ($('.sidebarCartList').length > 0) {
        if (IsCardView) {
            FillDataFromLayoutView('CardView');
        }
        else if (IsTableView) {
            var partId = $(this).parent().find('button').data('partid');
            var indexId = $(this).parent().find('button').data('indexid');
            var className = 'index_' + partId + '_' + indexId;
            $(document).find('#tblpartlookup').find('.' + className).closest('tr').find('.qtytableItemQty').val(orderquantity);
            $.each(tablecarItemarr, function (index, item) {
                if (item.PartId == partId && item.Indexid == indexId) {
                    item.OrderQuantity = orderquantity;
                    return false;
                }
            });

        }
    }


});
function DeleteFromCart(me) {
    var PartId = $(me).attr('data-partid');
    var Indexid = $(me).attr('data-indexid');
    $('#tblpartlookup>tbody>tr').each(function (i, row) {

        var tblMyPartId = $(this).closest('tr').find('.tablePartId').children('h3').text();
        var tblIndexId = $(this).closest('tr').find('.Indexid').text();
        if (PartId == tblMyPartId && Indexid == tblIndexId) {
            var classnew = '.chkcheckItem' + '.' + tblMyPartId + '.' + tblIndexId + '.index' + '_' + tblMyPartId + '_' + tblIndexId;
            if ($(classnew).is(':checked')) {
                $(classnew).prop('checked', false);
            }
        }
    });
    selectedcount--;
    var myNewdata = "" + PartId + "";

    var myIndexdata = "" + Indexid + "";
    var index = getIndexIfObjWithAttr(tablecarItemarr, 'PartId', myNewdata, 'Indexid', myIndexdata);
    tablecarItemarr.splice(index, 1);
    myproductCartArr.splice(index, 1);
    $('.chkitemcount').text(tablecarItemarr.length);
    $('.clsCardItem').each(function (index, value) {
        var CardPartId = $(this).find('.qtyBox').find('.hidmyPartId').val();
        var CardIndexId = $(this).find('.qtyBox').find('.hidindexid').val();
        var elementbutton = $(this).find('.btn-addToCart');
        if (PartId == CardPartId && Indexid == CardIndexId) {
            var btnModifiedText = $(elementbutton).attr('title');
            $(elementbutton).html(btnModifiedText);
        }
    });

    $(me).closest('.sidebarCartList').remove();
    var cartDataLength = $('.sidebarCartList').length;
    $('.cartBadges').text(cartDataLength);
    $('.filteritemcount').text(cartDataLength);

    if (tablecarItemarr.length == 0) {
        $(".updateArea").hide();
    }
}


function ProcessCartData() {
   $('.sidebarCartList').each(function (index, value) {
        var PartId = $(this).attr('id');
        var UnitCost = $(this).find('.qtyBox').find('#PartPrice').val();
        var OrderQuantity = $(this).find('.qtyBox').find('.Partqtyvalue').val();
        var TotalUnitCost = parseFloat(OrderQuantity) * parseFloat(UnitCost);
        var UnitofMeasure = $(this).find('.qtyBox').find('#PartUOM').val();
        var Description = $(this).find('.qtyBox').find('.hiddescription').val();
        var ClientLookUpId = $(this).find('.qtyBox').find('#hidClientLookupId').val();
        //V2-553
        var IsVendorCatalog = $(this).find('.qtyBox').find('.MyVendorCatalog').val();
        var PurchaseUOM = $(this).find('.qtyBox').find('.PurchaseUOM').val();
        var UOMConvRequired = $(this).find('.qtyBox').find('.UOMConvRequired').val();
        var IssueOrder = $(this).find('.qtyBox').find('.IssueOrder').val();
        var VendorCatalogItemId = $(this).find('.qtyBox').find('.VendorCatalogItemId').val();
        var RequiredDate = $(this).find('.qtyBox').find('.requiredDate').val();
        //V2-690
        var VendorId = $(this).find('.qtyBox').find('.hidVendorId').val();
        var PartCategoryMasterId = $(this).find('.qtyBox').find('.hidPartCategoryMasterId').val();
        var Indexid = $(this).find('.qtyBox').find('.hidindexid').val();
        //V2-732
        var StoreroomId = $(this).find('.qtyBox').find('.hidstoreroomid').val();//V2-732
        var PartStoreroomId = $(this).find('.qtyBox').find('.hidpartstoreroomid').val();//V2-732
        //V2-1068
        var AccountId = $(this).find('.qtyBox').find('.hidAccountId').val();//V2-1068
        var UnitOfMeasureIssue = $(this).find('.qtyBox').find('.hidUnitOfMeasure').val();//V2-1068
        myproductCartArr.push({
            PartId: PartId,
            ClientLookUpId: ClientLookUpId,
            Description: Description,
            OrderQuantity: parseFloat(OrderQuantity),
            UnitCost: parseFloat(UnitCost),
            TotalUnitCost: parseFloat(TotalUnitCost),
            UnitofMeasure: UnitofMeasure,
            IsVendorCatalog: IsVendorCatalog,
            PurchaseUOM: PurchaseUOM,
            UOMConvRequired: UOMConvRequired,
            IssueOrder: IssueOrder,
            VendorCatalogItemId: VendorCatalogItemId,
            RequiredDate: RequiredDate,
            VendorId: VendorId,
            PartCategoryMasterId: PartCategoryMasterId,
            Indexid: Indexid,
            StoreroomId: StoreroomId,
            PartStoreroomId: PartStoreroomId,
            AccountId: AccountId,
            UnitOfMeasureIssue: UnitOfMeasureIssue
        });
    });
}
function FillDataFromLayoutView(viewName) {
    if (viewName == 'TableView') {
        tablecarItemarr = [];
        $('.sidebarCartList').each(function (index, value) {
            var PartId = $(this).attr('id');
            var Indexid = $(this).find('.qtyBox').find('.hidindexid').val();
            //var UnitCost = $('#' + this.id).find('.qtyBox').find('#PartPrice').val();
            var UnitCost = $('.clsIndex_' + Indexid).find('.qtyBox').find('#PartPrice').val();
            //var OrderQuantity = $('#' + this.id).find('.qtyBox').find('.Partqtyvalue').val();
            var OrderQuantity = $('.clsIndex_' + Indexid).find('.qtyBox').find('.Partqtyvalue').val();
            var TotalUnitCost = parseFloat(OrderQuantity) * parseFloat(UnitCost);
            var UnitofMeasure = $('#' + this.id).find('.qtyBox').find('#PartUOM').val();
            var PartImageUrl = $('#' + this.id).find('.qtyBox').find('#MyPartImage').val();
            /*  var Description = $('#' + this.id).find('.dtlsBox').children('p').text();*/
            var Description = $('#' + this.id).find('.qtyBox').find('.hiddescription').val();
            var ClientLookUpId = $('#' + this.id).find('.qtyBox').find('#hidClientLookupId').val();
            var InVendorCatalog = $('#' + this.id).find('.qtyBox').find('.MyVendorCatalog').val();

            var PurchaseUOM = $('#' + this.id).find('.qtyBox').find('.PurchaseUOM').val();
            var UOMConvRequired = $('#' + this.id).find('.qtyBox').find('.UOMConvRequired').val();
            var IssueOrder = $('#' + this.id).find('.qtyBox').find('.IssueOrder').val();
            var VendorCatalogItemId = $('#' + this.id).find('.qtyBox').find('.VendorCatalogItemId').val();
            var RequiredDate = $('#' + this.id).find('.qtyBox').find('.requiredDate').val();
            //V2-690
            var VendorId = $('#' + this.id).find('.qtyBox').find('.hidVendorId').val();
            var PartCategoryMasterId = $('#' + this.id).find('.qtyBox').find('.hidPartCategoryMasterId').val();
            //V2-732
            var StoreroomId = $('#' + this.id).find('.qtyBox').find('.hidstoreroomid').val();//V2-732
            var PartStoreroomId = $('#' + this.id).find('.qtyBox').find('.hidpartstoreroomid').val();//V2-732

            //V2-1068
            var AccountId = $('#' + this.id).find('.qtyBox').find('.hidAccountId').val();//V2-1068
            var UnitOfMeasureIssue = $('#' + this.id).find('.qtyBox').find('.hidUnitOfMeasure').val();//V2-1068
            if (RequiredDate === undefined || RequiredDate == "") {
                RequiredDate = null;
            }
            tablecarItemarr.push({
                PartId: PartId,
                ClientLookUpId: ClientLookUpId,
                InVendorCatalog: InVendorCatalog,
                Description: Description,
                PartImageUrl: PartImageUrl,
                OrderQuantity: parseFloat(OrderQuantity),
                UnitCost: parseFloat(UnitCost),
                TotalUnitCost: parseFloat(TotalUnitCost),
                UnitofMeasure: UnitofMeasure,
                PurchaseUOM: PurchaseUOM,
                UOMConvRequired: UOMConvRequired,
                IssueOrder: IssueOrder,
                VendorCatalogItemId: VendorCatalogItemId,
                RequiredDate: RequiredDate,
                VendorId: VendorId,
                PartCategoryMasterId: PartCategoryMasterId,
                Indexid: Indexid,
                StoreroomId: StoreroomId,
                PartStoreroomId: PartStoreroomId,
                AccountId: AccountId,
                UnitOfMeasureIssue: UnitOfMeasureIssue
            });
            uniqueArrayWO(tablecarItemarr);
        });

        $('.chkitemcount').text(tablecarItemarr.length);
    }
    else if (viewName == 'CardView') {
        $('.sidebarCartList').each(function (index, value) {
            var PartId = $(this).attr('id');
            var IndexId = $(this).find('.qtyBox').find('.hidindexid').val();
            //var UnitCost = $('#' + this.id).find('.qtyBox').find('#PartPrice').val();
            var UnitCost = $('.clsIndex_' + IndexId).find('.qtyBox').find('#PartPrice').val();
            //var OrderQuantity = $('#' + this.id).find('.qtyBox').find('.Partqtyvalue').val();
            var OrderQuantity = $('.clsIndex_' + IndexId).find('.qtyBox').find('.Partqtyvalue').val();
            var UOM = $('.clsIndex_' + IndexId).find('.qtyBox').find('#PartUOM').val();
            var TotalUnitCost = parseFloat(OrderQuantity) * parseFloat(UnitCost);
            $('.clsCardItem').each(function (index, value) {
                var CardPartId = $(this).find('.qtyBox').find('.hidmyPartId').val();
                var CartIndexId = $(this).find('.qtyBox').find('.hidindexid').val();
                if (PartId == CardPartId && IndexId == CartIndexId) {
                    $(this).find('.clsPartQuantity').val(OrderQuantity);
                    $(this).find('.hidpartPriceRate').val(UnitCost);
                    $(this).find('.partPriceMe').val(UnitCost);
                    $(this).find('.hidTotalPrice').val(TotalUnitCost);
                    var elementbutton = $(this).find('.btn-addToCart');
                    var btnText = '<i class="fa fa-check addtoCartchkBoxColor" aria-hidden="true"></i>' + $(elementbutton).text();
                    $(elementbutton).html(btnText);

                }
            });
        });
    }
}

$(document).on('click', '.btn-addToCart,.btn-addToCartTable', function () {
    AddtoCartItem(this);
});
$(document).on('click', '#btnProcessCartData,#btnprocess', function () {
    myproductCartArr = [];
    var WorkOrderID = $('#WorkOrderID').val();
    var MaterialRequestId = $('#MaterialRequestId').val();
    var cartDataLength = $('.sidebarCartList').length;
    var ModeForRedirect = $('#ModeForRedirect').val();
    if (cartDataLength > 0) {
        ProcessCartData();
        var _mypartdata = JSON.stringify({
            modelData: myproductCartArr,
            WorkOrderID: WorkOrderID,
            MaterialRequestId: MaterialRequestId
        });
        var message = '';
        var myclientlookUpId = '';
        var myclientlookUpAllId = '';
        var qtyErrormsg = '';
        var myQuantityEmptyProductarr = [];
        var myQuantityZeroProductarr = [];
 
        var ShoppingCart = $(document).find('#ShoppingCart').val();
        for (var myi = 0, l = myproductCartArr.length; myi < l; myi++) {
            var product = myproductCartArr[myi];
            if (ShoppingCart === "True" && (product.OrderQuantity <= 0 || product.UnitCost <= 0)) {
                myclientlookUpId += product.ClientLookUpId + ",";
                myQuantityZeroProductarr.push({
                    PartId: product.PartId,
                    ClientLookUpId: product.ClientLookUpId,
                    Description: product.Description,
                    OrderQuantity: parseFloat(product.OrderQuantity),
                    UnitCost: parseFloat(product.UnitCost),
                    TotalUnitCost: parseFloat(product.TotalUnitCost),
                    UnitofMeasure: product.UnitofMeasure
                });
            }
            if (isNaN(product.OrderQuantity)) {
                myclientlookUpAllId += product.ClientLookUpId + ",";
                myQuantityEmptyProductarr.push({
                    PartId: product.PartId,
                    ClientLookUpId: product.ClientLookUpId
                });
            } else if (product.OrderQuantity <= 0) {
                myclientlookUpId += product.ClientLookUpId + ",";
                myQuantityZeroProductarr.push({
                    PartId: product.PartId,
                    ClientLookUpId: product.ClientLookUpId,
                    Description: product.Description,
                    OrderQuantity: parseFloat(product.OrderQuantity),
                    UnitCost: parseFloat(product.UnitCost),
                    TotalUnitCost: parseFloat(product.TotalUnitCost),
                    UnitofMeasure: product.UnitofMeasure
                });
            }
        }
        var lastcommaIndex = myclientlookUpId.lastIndexOf(",");
        myclientlookUpId = myclientlookUpId.substring(0, lastcommaIndex);
        var lastcommaAllIndex = myclientlookUpAllId.lastIndexOf(",");
        myclientlookUpAllId = myclientlookUpAllId.substring(0, lastcommaAllIndex);
        if (myQuantityEmptyProductarr.length != 0) {
            qtyErrormsg = getResourceValue("QuantityEmptyErrMsg") + ' ' + myclientlookUpAllId;
            swal({
                title: getResourceValue("QtyRequiredTitle"),
                type: "warning",
                text: qtyErrormsg,
                confirmButtonClass: "btn-danger",
                showConfirmButton: true
            },
                function () {
                    $('.overlay').fadeOut();
                });
        }
        else {
            message = getResourceValue("QtyPriceNotZeroMsg") + ' ' + myclientlookUpId;
            if (myQuantityZeroProductarr.length == 0) {
                $.ajax({
                    url: '/Dashboard/ProcesssPartCartDataWO_Mobile',
                    contentType: "application/json",
                    type: 'POST',
                    data: _mypartdata,
                    beforeSend: function () {
                        ShowLoader();
                    },
                    success: function (data) {
                        $(".updateArea").hide();
                        //#region ServerSide Validation Quantity & Price
                        if (data.errormessge != "") {
                            $.notify({
                                message: data.errormessge
                            }, {
                                type: 'danger'
                            });
                        }
                        else {
                            if (WorkOrderID != 0) {
                                if (data.status != 'fail') {
                                    SuccessAlertSetting.text = data.status;
                                    CloseLoader();
                                    swal(SuccessAlertSetting, function () {
                                        if (ModeForRedirect == 'WorkOrder') {
                                            RedirectToPmDetail(WorkOrderID, "estimatespart");
                                        }
                                        else if (ModeForRedirect == 'Dashboard') {
                                            var CurrentTabForDashboard = 'MaterialRequests';
                                            RedirectToMaintenanceWorkBenchDetailFromDashboard(WorkOrderID, '', CurrentTabForDashboard);
                                        }
                                        resetpage();
                                    });
                                }
                            }
                            if (MaterialRequestId != 0) {
                                if (data.status != 'fail') {
                                    SuccessAlertSetting.text = data.status;
                                    CloseLoader();
                                    swal(SuccessAlertSetting, function () {
                                        RedirectToMRequestDetail(MaterialRequestId);
                                        resetpage();
                                    });
                                }
                            }

                        }
                        //#region ServerSide Validation Quantity & Price
                    },

                    complete: function () {
                        $('.overlay').fadeOut();
                        CloseLoader();
                    },
                    error: function () {
                        CloseLoader();
                    }
                });
            }
            else {
                $('div.notification').remove();
                $.notify({
                    icon: 'glyphicon glyphicon-warning-sign',
                    title: '',
                    message: message,
                    url: '#'
                }, {
                    type: 'danger',
                    template: '<div data-notify="container" class="notification col-xs-11 col-sm-4 alert alert-{0}" style="overflow: scroll;" role="alert">' +
                        '<button type="button" aria-hidden="true" class="close" data-notify="dismiss"></button>' +
                        '<span data-notify="icon"></span> ' +
                        '<span data-notify="title">{1}</span> ' +
                        '<span data-notify="message">{2}</span>' +
                        '<div class="progress" data-notify="progressbar">' +
                        '<div class="progress-bar progress-bar-{0}" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%;"></div>' +
                        '</div>' +
                        '<a href="{3}" target="{4}" data-notify="url"></a>' +
                        '</div>'
                });
            }
        }
    }
    else {
        var cartMessage = getResourceValue("EmptyCartMessage");
        $('div.notification').remove();
        $.notify({
            icon: 'glyphicon glyphicon-warning-sign',
            title: '',
            message: cartMessage,
            url: '#'
        }, {
            type: 'danger',
            template: '<div data-notify="container" class="notification col-xs-11 col-sm-3 alert alert-{0}" role="alert">' +
                '<button type="button" aria-hidden="true" class="close" data-notify="dismiss"></button>' +
                '<span data-notify="icon"></span> ' +
                '<span data-notify="title">{1}</span> ' +
                '<span data-notify="message">{2}</span>' +
                '<div class="progress" data-notify="progressbar">' +
                '<div class="progress-bar progress-bar-{0}" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%;"></div>' +
                '</div>' +
                '<a href="{3}" target="{4}" data-notify="url"></a>' +
                '</div>'
        });
    }
});
function resetpage() {
    selectedcount = 0;
    layoutTypeWO = 1;
    tablecarItemarr = [];
    myproductCartArr = [];
    partLokkUpdt = '';
}
$(document).on('click', '#btnAllAddToCartPartLookupWO', function () {
    var lstAllModerldataarr = [];
    for (var mei = 0, li = tablecarItemarr.length; mei < li; mei++) {
        var tblClientLookUpPartId = tablecarItemarr[mei].ClientLookUpId;
        var tblPartId = tablecarItemarr[mei].PartId;
        var tblInVendorCatalog = tablecarItemarr[mei].InVendorCatalog;
        var tblPartDesc = tablecarItemarr[mei].Description;
        var tblPartImageUrl = tablecarItemarr[mei].PartImageUrl;
        var tblitemQty = tablecarItemarr[mei].OrderQuantity;
        var tblUnitPrice = tablecarItemarr[mei].UnitCost;
        var tblTotalUnitPrice = parseFloat(tblitemQty) * parseFloat(tblUnitPrice);
        var tblUOM = tablecarItemarr[mei].UnitofMeasure;
        var tblPurchaseUOM = tablecarItemarr[mei].PurchaseUOM;
        var tblUOMConvRequired = tablecarItemarr[mei].UOMConvRequired;
        var tblIssueOrder = tablecarItemarr[mei].IssueOrder;
        var tblVendorCatalogItemId = tablecarItemarr[mei].VendorCatalogItemId;
        var tblRequiredDate = tablecarItemarr[mei].RequiredDate;
        var tblVendorId = tablecarItemarr[mei].VendorId;
        var tblPartCategoryMasterId = tablecarItemarr[mei].PartCategoryMasterId;
        var tblIndexid = tablecarItemarr[mei].Indexid;
        var tblStoreroomId = tablecarItemarr[mei].StoreroomId;
        var tblPartStoreroomId = tablecarItemarr[mei].PartStoreroomId;
        var tblAccountId = tablecarItemarr[mei].AccountId;
        var tblUnitofMeasure = tablecarItemarr[mei].UnitOfMeasureIssue;
        lstAllModerldataarr.push({
            ImageUrl: tblPartImageUrl,
            PartId: tblPartId,
            ClientLookUpId: tblClientLookUpPartId,
            InVendorCatalog: tblInVendorCatalog,
            Description: tblPartDesc,
            PartQty: parseInt(tblitemQty),
            UnitPrice: parseFloat(tblUnitPrice),
            TotalUnitPrice: parseFloat(tblTotalUnitPrice),
            PurchaseUnitofMeasure: tblUOM,
            PurchaseUOM: tblPurchaseUOM,
            UOMConvRequired: tblUOMConvRequired,
            IssueOrder: parseFloat(tblIssueOrder),
            VendorCatalogItemId: tblVendorCatalogItemId,
            RequiredDate: tblRequiredDate,
            VendorId: tblVendorId,
            PartCategoryMasterId: tblPartCategoryMasterId,
            Indexid: tblIndexid,
            WorkOrderID: $(document).find('#WorkOrderID').val(),
            StoreroomId: tblStoreroomId,
            PartStoreroomId: tblPartStoreroomId,
            AccountId: tblAccountId,
            UnitofMeasure: tblUnitofMeasure
        });
    }
    var _mynewdata =
        JSON.stringify({
            model: {
                lstpartAddToCartModels: lstAllModerldataarr
            }
        });

    FillAllAddtoCartSide(_mynewdata);

    $('div.notification').remove();
    $.notify({
        icon: 'glyphicon glyphicon-warning-sign',
        title: getResourceValue("ItemAdded"),
        timer: 500,
        message: getResourceValue("ItemAddedToTheCart"),
        url: '#'
    }, {
        type: 'success',
        template: '<div data-notify="container" class="notification col-xs-11 col-sm-3 alert alert-{0}" role="alert">' +
            '<button type="button" aria-hidden="true" class="close" data-notify="dismiss"></button>' +
            '<span data-notify="icon"></span> ' +
            '<span data-notify="title">{1}</span> ' +
            '<span data-notify="message">{2}</span>' +
            '<div class="progress" data-notify="progressbar">' +
            '<div class="progress-bar progress-bar-{0}" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%;"></div>' +
            '</div>' +
            '<a href="{3}" target="{4}" data-notify="url"></a>' +
            '</div>'
    });
});

$(document).ready(function () {
    $(".updateArea").hide();
    $(document).on("keypress keyup paste blur", ".allownumericWithdecimal", function (event) {
        $(this).val($(this).val().replace(/[^0-9\.]/g, ''));
        if ((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
            event.preventDefault();
        }
    });
});

$(document).on('click', '.showmoredesc', function () {
    $(document).find('#partlookupdetaildesmodal').modal('show');
    $(document).find('#partlookupdetaildesmodaltext').text($(this).data("des"));
    $(document).find('#partDecription').text($(this).data("des-title"));
});
$(document).on('click', '#lnk_psearchdtls', function (e) {
    e.preventDefault();
    resetpage();
    var WorkOrderID = $('#WorkOrderID').val();
    var MaterialRequestId = $('#MaterialRequestId').val();
    if (WorkOrderID != 0) {
        RedirectToPmDetail(WorkOrderID, "overview");
    }
    if (MaterialRequestId != 0) {
        RedirectToMRequestDetail(MaterialRequestId);
    }
});

$(document).on('click', '#lnk_psearchplup', function (e) {
    e.preventDefault();
    resetpage();
    var WorkOrderID = $('#WorkOrderID').val();
    var MaterialRequestId = $('#MaterialRequestId').val();
    if (WorkOrderID != 0) {
        location.href = "/WorkOrder/Index?page=Maintenance_Work_Order_Search";
    }
    if (MaterialRequestId != 0) {
        location.href = "/MaterialRequest/Index?page=MaterialRequest";
    }
});
//#endregion

//#region V2-563

$(document).on('click', '.clsAdditionalCatalog', function () {
    var PartId = $(this).attr('data-partid');
    var clientlookupid = $(this).attr('data-pclientlookupid');
    $.ajax({
        url: "/Dashboard/AddWOAdditionalCatalogGrid_Mobile",
        type: "GET",
        dataType: 'html',
        data: { clientlookupid: clientlookupid },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#additionalCatalogDetails').html('');
            $('#additionalCatalogDetails').html(data);
        },
        complete: function () {
            generateAdditionalCatalogDataTable(PartId);
        },
        error: function () {
            CloseLoader();
        }
    });
});
var dtAddCatTable;
function generateAdditionalCatalogDataTable(PartId) {
    var rCount = 0;
    var visibility;
    if ($(document).find('.additionalCatalogItem').hasClass('dataTable')) {
        dtAddCatTable.destroy();
    }
    dtAddCatTable = $(".additionalCatalogItem").DataTable({
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[1, "asc"]],
        "pageLength": 10,
        stateSave: true,
        language: {
            url: "/base/GetDataTableLanguageJson?InnerGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "columnDefs": [
            { className: 'text-right', targets: [4] }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/Dashboard/GetWOAdditionalCatalogGrid_Mobile",
            data: function (d) {
                d.PartId = PartId;
            },
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (json) {
                $(document).find("#addPR").prop('disabled', true);
                rCount = json.data.length;
                return json.data;
            }
        },
        "columns":
            [

                {
                    "data": "VendorId",
                    orderable: false,
                    "bSortable": false,
                    className: 'select-radio dt-body-center',
                    'render': function (data, type, full, meta) {
                        return '<input type="radio" id="radioItemMe_' + data + '" data-index="' + meta.row + '" name="group1" data-description="' + full.Description + '" data-partstoreroomId="' + full.PartStoreroomId + '" data-IssueUnit="' + full.IssueUnit + '" data-vendorcatalogitemid="' + full.VendorCatalogItemid + '" class="radiosearch radiocheckItem ' + data + '" onclick=RadiobtnClick(this)>';

                    }
                },
                {
                    "data": "VendorClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true,
                },
                {
                    "data": "VendorName", "autoWidth": true, "bSearchable": true, "bSortable": true,
                },
                {
                    "data": "VendorPartNumber", "autoWidth": true, "bSearchable": true, "bSortable": true,
                },
                {
                    "data": "UnitCost", "autoWidth": true, "bSearchable": true, "bSortable": true,
                },
                {
                    "data": "PurchaseUOM", "autoWidth": true, "bSearchable": true, "bSortable": true,
                },
                {
                    "autoWidth": true,
                    "bSearchable": false,
                    "bSortable": false,
                    'render': function (data, type, full, meta) {
                        return '<span class="field-validation-error" data-valmsg-replace="true" style="display: none"><span class="ValidationErr"></span></span><input type="text" style="text-align:right ;" id="orderQuantity_' + meta.row + '"   class="form-control search OrderQuantityItem grd-orderQuantity" autocomplete="off" onKeyUp="ValidateDecimalInputs(this)" disabled>';

                    }
                }
            ],
        initComplete: function () {
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#additionalCatalogItemModal').hasClass('show')) {
                $(document).find('#additionalCatalogItemModal').modal("show");
            }

        }
    });
}
var index_row = 0;
function RadiobtnClick(r) {
    index_row = $(r).attr('data-index');
    $('.OrderQuantityItem').removeClass('input-validation-error');
    $('.OrderQuantityItem').siblings('.field-validation-error').find('.ValidationErr').text('');
    $(document).find(".OrderQuantityItem").prop('disabled', true).val('');
    $(document).find("#orderQuantity_" + index_row).prop('disabled', false).val('1.0');
    $(document).find("#addPR").prop('disabled', false);
}

$(document).on('click', '#addPR', function () {
    var data = dtAddCatTable.row(index_row).data();
    var purchaseRequestModelparam = {
        VendorId: data.VendorId,
        Reason: '',
        Description: data.Description,
        LineNumber: 1,
        PartId: data.PartId,
        PartStoreroomId: data.PartStoreroomId,
        OrderQuantity: $(document).find("#orderQuantity_" + index_row).val(),
        PurchaseUOM: data.PurchaseUOM,
        UnitofMeasure: data.IssueUnit,
        UnitCost: data.UnitCost,
        VendorCatalogItemId: data.VendorCatalogItemid,
        IsAdditionalCatalogItem: true
    };

    purchaseRequestModel = JSON.stringify({ 'purchaseRequestModel': purchaseRequestModelparam });

    $.ajax({
        "url": "/PurchaseRequest/AddPurchaseRequestfromAdditionalCat",
        "data": purchaseRequestModel,
        contentType: 'application/json; charset=utf-8',
        "dataType": "json",
        "type": "POST",
        beforeSend: function () {
            ShowLoader();
        },
        "success": function (d) {
            if (d.purchaserequestid != 0) {
                $(document).find('#additionalCatalogItemModal').modal("hide");
                var msgText = getResourceValue("RequestCreatedAlert");
                var Text = msgText.replace("00", d.PRClientlookupId);
                SuccessAlertSetting.text = Text; //
                swal(SuccessAlertSetting, function () {


                });
            }
            else {
                ShowGenericErrorOnAddUpdate(d);
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

function ValidateDecimalInputs(e) {
    $(document).find("#addPR").prop('disabled', false);
    var beforeDecimal = 9;
    var afterDecimal = 6;

    $('#' + e.id).on('input', function () {
        this.value = this.value
            .replace(/[^\d.]/g, '')
            .replace(new RegExp("(^[\\d]{" + beforeDecimal + "})[\\d]", "g"), '$1')
            .replace(/(\..*)\./g, '$1')
            .replace(new RegExp("(\\.[\\d]{" + afterDecimal + "}).", "g"), '$1');
    });

    var textbox = e.value;
    var thstextbox = e.id;
    $('#' + thstextbox).removeClass('input-validation-error');
    $('#' + thstextbox).siblings('.field-validation-error').find('.ValidationErr').text('');
    if (textbox == "") {
        $('#' + thstextbox).addClass('input-validation-error');
        $('#' + thstextbox).siblings('.field-validation-error').find('.ValidationErr').text("Please enter order quantity");
        $(document).find("#addPR").prop('disabled', true);
    }
    else if (parseInt(textbox) < 1) {
        $('#' + thstextbox).addClass('input-validation-error');
        $('#' + thstextbox).siblings('.field-validation-error').find('.ValidationErr').text("Order quantity must be greater than 1.0");
        $(document).find("#addPR").prop('disabled', true);
    }

}

$(document).on('click', "#sidebarCollapse", function () {
    $('#mainwidget').find('.sidebar').addClass('active');
    $('.overlay').fadeIn();
    $('.collapse.in').toggleClass('in');
    $('a[aria-expanded=true]').attr('aria-expanded', 'false');

});
$(document).on('click', '#dismiss, .overlay', function () {
    $('.sidebar').removeClass('active');
    $('.overlay').fadeOut();
});

$(document).on('click', '.dismiss, .overlay', function () {
    $(document).find('.sidebar').removeClass('active');
    $('.overlay').fadeOut();
});
//#endregion

$(document).on('click', '#lnk_psearchplupdashboard', function () {
    window.location = '/Dashboard/Dashboard';
});

//#region Card view griddatalayout update

function filterinfoWO(id, value) {
    this.key = id;
    this.value = value;
}
//For column , order , page and page length change
function LayoutUpdateWO(area) {
    $.ajax({
        "url": "/Base/GetLayout",
        "data": {
            GridName: pLookUpGridName
        },
        "async": false,
        "dataType": "json",
        "success": function (json) {

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
                    setsearchui(JSON.parse(json.FilterInfo), $("#txtColumnSearch"), $("#searchfilteritems"));
                }
                $.ajax({
                    "url": gridStateSaveUrl,
                    "data": {
                        GridName: pLookUpGridName,
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
function LayoutFilterinfoUpdateWO() {
    $.ajax({
        "url": "/Base/GetLayout",
        "data": {
            GridName: pLookUpGridName
        },
        "async": false,
        "dataType": "json",
        "success": function (json) {
            if (json.LayoutInfo !== '') {
                var gridstate = JSON.parse(json.LayoutInfo);
                gridstate.start = cardviewstartvalue;
                var filterinfoarray = getfilterinfoarray($("#txtsearch"));
                $.ajax({
                    "url": gridStateSaveUrl,
                    "data": {
                        GridName: pLookUpGridName,
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

function getfilterinfoarray(txtsearchelement) {
    var filterinfoarray = [];
    var f = new filterinfoWO('searchstring', LRTrim($(document).find('#txtsearch').val()));
    filterinfoarray.push(f);

    return filterinfoarray;
}
function setsearchui(data, txtsearchelement, searchstringcontainer) {
    $.each(data, function (index, item) {
        if (item.key == 'searchstring' && item.value) {
            var txtSearchval = item.value;
            if (item.value) {
                txtsearchelement.val(txtSearchval);
                searchitemhtml = "";
                $('#spnsearchbtn').html('Search: ' + txtSearchval + '&nbsp;<a href="javascript:void(0);" class="fa fa-times" aria-hidden="true" id="btnclearsearch"></a>');
            }
            return false;
        }
    });
    if ($(document).find('#txtsearch').val()) {
        $('#searchfilteritems').show();
    }
    else {
        $('#searchfilteritems').hide();
    }
}

function GetLayoutWO() {
    $.ajax({
        "url": "/Base/GetLayout",
        "data": {
            GridName: pLookUpGridName
        },
        "async": false,
        "dataType": "json",
        "success": function (json) {

            if (json.LayoutInfo !== '') {
                var gridstate = JSON.parse(json.LayoutInfo);
                cardviewstartvalue = gridstate.start;
                [currentorderedcolumn, currentorder] = gridstate.order[0];
                cardviewlwngth = gridstate.length;

                if (json.FilterInfo !== '') {
                    setsearchui(JSON.parse(json.FilterInfo), $("#txtsearch"), $("#searchfilteritems"));
                }
            }
        }
    });
}
//#endregion