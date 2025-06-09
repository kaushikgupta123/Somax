var run = false;
$(document).ready(function () {
    ShowbtnLoaderclass("LoaderDrop");
    ShowbtnLoader("btnsortmenu");
    $(document).find('.select2picker').select2({});
    $(document).on('click', "#action", function () {
        $(".actionDrop").slideToggle();
    });
    $(document).on('focusout', "#action", function () {
        $(".actionDrop").fadeOut();
    });
    $(".tabsArea").hide();
    $("ul.vtabs li:first").addClass("active").show();
    $(".tabsArea:first").show();
    $(".actionBar").fadeIn();
    $("#POGridAction :input").attr("disabled", "disabled");
    $(document).on('click', "ul.vtabs li", function () {
        $("ul.vtabs li").removeClass("active");
        $(this).addClass("active");
        $(".tabsArea").not('#PurchaseRequestLineItem').hide();
        var activeTab = $(this).find("a").attr("href");
        $(activeTab).fadeIn();
        return false;
    });
    $('#tabselector').change(function () {
        $('.tabcontent').hide();
        $('#' + $(this).val()).show();
    });
    $('#tabselector2').change(function () {
        $('.tabcontent').hide();
        $('#' + $(this).val()).show();
    });
    $('[data-toggle="tooltip"]').tooltip();
    $(document).on('click', '#poOverview', function () {
        $('#PurchasingOverview').show();
        $('#Equipmenttab').show();
        $('.tabcontent').show();
        $('#btndetails').addClass('active');
    });
    $(document).on('change', '#colorselector', function (evt) {
        $(".tabsArea").not('#PurchaseRequestLineItem').hide();
        if ($(this).val() === 'PurchasingOverview') {
            opendiv(evt, 'Detailstab');
        }
        else {
            openCity(evt, $(this).val());
        }
        $('#' + $(this).val()).show();
    });
    function opendiv(evt, cityName) {
        $("#PurchasingOverview").find("#btndetails").addClass('active');
        document.getElementById(cityName).style.display = "block";
    }
});
function openCity(evt, cityName) {
    var PurchaseOrderId = $(document).find('#PurchaseOrderModel_PurchaseOrderId').val();
    switch (cityName) {
        case "PONotes":
            GeneratePONotesGrid();
            break;
        case "POEventLog":
            GenerateSJEventLogGrid();
            break;
        case "POAttachments":
            GeneratePOAttachmentsGrid();
            break;
        case "POverview":
            $('#PurchasingOverview').show();
            $('#Detailstab').show();
            $('#btndetails').addClass('active')
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
var dtTable;
var CustomQueryDisplayId = 0;
$(document).ready(function () {
    var val = localStorage.getItem("PURCHASEORDERSTATUS");
    if (val) {
        CustomQueryDisplayId = val;
        generatePODataTable();
        $('#PurchaseOrderModel_CustomFilter').val(val).trigger('change.select2');
    }
    else {
        val = 0;
        generatePODataTable();
    }
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
$(document).on('click', '#PurchasingOrderSidebar', function () {
    $('#PurchasingOverview').show();
    $('#Detailstab').show();
    $('#btndetails').addClass('active')
});
//#region Search
$(document).on('change', '#PurchaseOrderModel_CustomFilter', function () {
    ShowbtnLoaderclass("LoaderDrop");
    run = true;
    var optionval = $('#PurchaseOrderModel_CustomFilter option:selected').val();
    CustomQueryDisplayId = optionval;
    localStorage.setItem("PURCHASEORDERSTATUS", CustomQueryDisplayId);
    typeValPoStatus = $("#Status").val();
    dtTable.page('first').draw('page');
});
$(document).on('click', '#liClearAdvSearchFilter', function () {
    run = true;
    $("#PurchaseOrderModel_CustomFilter").val(0).trigger('change.select2');
    CustomQueryDisplayId = 0;
    localStorage.removeItem("PURCHASEORDERSTATUS");
    clearAdvanceSearch();
    dtTable.page('first').draw('page');
});
$("#btnPODataAdvSrch").on('click', function (e) {
    searchresult = [];
    typeValPoStatus = $("#Status").val();
    PRAdvSearch();
    $('.sidebar').removeClass('active');
    $('.overlay').fadeOut();
    dtTable.page('first').draw('page');
});
$(document).on('click', '.btnCross', function () {
    var btnCrossed = $(this).parent().attr('id');
    var searchtxtId = btnCrossed.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    selectCount--;
    if (searchtxtId == "Status") {
        typeValPoStatus = null;
    }
    PRAdvSearch();
    dtTable.page('first').draw('page');
});
$(document).on('click', '#liPrint', function () {
    $(".buttons-print")[0].click();
    $('#mask').trigger('click');
});
$(document).on('click', '#liCsv', function () {
    $(".buttons-csv")[0].click();
    $('#mask').trigger('click');
});
$(document).on('click', '#liPdf', function () {
    $(".buttons-pdf")[0].click();
    $('#mask').trigger('click');
});
$(document).on('click', "#liExcel", function () {
    $(".buttons-excel")[0].click();
    $('#mask').trigger('click');
});
function PRAdvSearch() {
    var InactiveFlag = false;
    var searchitemhtml = "";
    selectCount = 0;
    $('#advsearchsidebar').find('.adv-item').each(function (index, item) {
        if ($(this).hasClass('dtpicker')) {
            $(this).val(ValidateDate($(this).val()));
        }
        if ($(this).val()) {
            selectCount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $("#advsearchfilteritems").html(searchitemhtml);
    $(".filteritemcount").text(selectCount);
    $('#liSelectCount').text(selectCount + ' filters applied');
}
function clearAdvanceSearch() {
    selectCount = 0;
    $("#PurchaseOrder").val("");
    $("#Status").val("").trigger('change');
    $('#VendorClientLookupId').val("");
    $("#VendorName").val("");
    $("#CreateDate").val("");
    $("#Attention").val("");
    $("#VendorPhoneNumber").val("");
    $("#CompleteDate").val("");
    $("#Reason").val("");
    $("#BuyerPersonnelName").val("");
    $("#TotalCost").val("");
    $("#advsearchfilteritems").html('');
    $(".filteritemcount").text(selectCount);
    typeValPoStatus = $("#Status").val();
}
var typeValPoStatus;
function generatePODataTable() {
    var printCounter = 0;
    if ($(document).find('#PurchaseOrderSearch').hasClass('dataTable')) {
        dtTable.destroy();
    }
    dtTable = $("#PurchaseOrderSearch").DataTable({
        colReorder: {
            fixedColumnsLeft: 1
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
                $.ajax({
                    "url": gridStateSaveUrl,
                    "data": {
                        GridName: "PurchaseOrder_Search",
                        LayOutInfo: JSON.stringify(data)
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
                "url": gridStateLoadUrl,
                "data": {
                    GridName: "PurchaseOrder_Search",
                },
                "async": false,
                "dataType": "json",
                "success": function (json) {
                    if (json) {
                        callback(JSON.parse(json));
                    }
                    else {
                        callback(json);
                    }
                }
            });

        },
        scrollX: true,
        fixedColumns: {
            leftColumns: 1,
        },
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [
            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Purchase Order List',
                extension: '.csv'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                orientation: 'portrait',
                pageSize: 'A3',
                css: 'display:none',
                title: 'Purchase Order List'
            },
            {
                extend: 'excelHtml5',
                title: 'Purchase Order List'
            },
            {
                extend: 'print',
                title: 'Purchase Order List',
                messageTop: function () {
                    printCounter++;

                    if (printCounter === 1) {
                        return 'This is the first time you have printed this document.';
                    }
                    else {
                        return 'You have printed this document ' + printCounter + ' times';
                    }
                },
                messageBottom: null
            },
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/Purchasing/GetPOGridData",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.CustomQueryDisplayId = CustomQueryDisplayId;
                d.PurchaseOrder = LRTrim($("#PurchaseOrder").val());
                d.Status = $("#Status").val();
                d.VendorClientLookupId = LRTrim($("#VendorClientLookupId").val());
                d.VendorName = LRTrim($('#VendorName').val());
                d.CreateDate = ValidateDate($("#CreateDate").val());
                d.Attention = LRTrim($("#Attention").val());
                d.VendorPhoneNumber = LRTrim($("#VendorPhoneNumber").val());
                d.CompleteDate = LRTrim(ValidateDate($("#CompleteDate").val()));
                d.Reason = LRTrim($("#Reason").val());
                d.Buyer_PersonnelName = LRTrim($("#BuyerPersonnelName").val());
                d.TotalCost = LRTrim($("#TotalCost").val());
            },
            "dataSrc": function (result) {
                if (result.recordsTotal == 0) {
                    $(document).find('.import-export').prop("disabled", true);
                }
                else {
                    $(document).find('.import-export').removeAttr('disabled');
                }
                $("#Status").empty();
                $("#Status").append("<option value=''>" + "--Select--" + "</option>");
                for (var i = 0; i < result.statuslist.length; i++) {
                    var id = result.statuslist[i];
                    var name = result.statuslist[i];
                    $("#Status").append("<option value='" + id + "'>" + getStatusValue(name) + "</option>");
                }

                if (typeValPoStatus && $("#Status option[value='" + typeValPoStatus + "']").length > 0) {
                    $("#Status").val(typeValPoStatus).trigger("change.select2");
                }
                else {
                    $("#Status").val("").trigger("change.select2");
                }
                HidebtnLoader("btnsortmenu");
                HidebtnLoaderclass("LoaderDrop");
                return result.data;
            },
            global: true
        },
        select: {
            style: 'os',
            selector: 'td:first-child'
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
            {
                "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1",
                mRender: function (data, type, full, meta) {
                    if (data == statusCode.Partial) {
                        return "<span class='m-badge m-badge-grid-cell m-badge--yellow m-badge--wide'>" + getStatusValue(data) + "</span >";
                    }
                    else if (data == statusCode.Complete) {
                        return "<span class='m-badge m-badge-grid-cell m-badge--green m-badge--wide'>" + getStatusValue(data) + "</span >";
                    }
                    else if (data == statusCode.Void) {
                        return "<span class='m-badge m-badge-grid-cell m-badge--red m-badge--wide'>" + getStatusValue(data) + "</span >";
                    }
                    else if (data == statusCode.Open) {
                        return "<span class='m-badge m-badge-grid-cell m-badge--blue m-badge--wide'>" + getStatusValue(data) + "</span >";
                    }
                    else {
                        return getStatusValue(data);
                    }
                }
            },
            { "data": "VendorClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2" },
            { "data": "VendorName", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3" },
            { "data": "CreateDate", "autoWidth": true, "bSearchable": true, "bSortable": false, "type": "date" },
            { "data": "Attention", "autoWidth": true, "bSearchable": true, "bSortable": false },
            { "data": "VendorPhoneNumber", "autoWidth": true, "bSearchable": true, "bSortable": false },
            { "data": "CompleteDate", "autoWidth": true, "bSearchable": true, "bSortable": false, "type": "date" },
            {
                "data": "Reason", "autoWidth": true, "bSearchable": true, "bSortable": false,
                "mRender": function (data, type, row) {
                    return "<div class='text-wrap width-200'>" + data + "</div>";
                }
            },
            { "data": "Buyer_PersonnelName", "autoWidth": true, "bSearchable": true, "bSortable": false },
            { "data": "TotalCost", "autoWidth": true, "bSearchable": true, "bSortable": false, className: "text-right" }
        ],
        "columnDefs": [
            {
                targets: [0],
                className: 'noVis'
            }
        ],
        initComplete: function () {
            SetPageLengthMenu();
            var currestsortedcolumn = $('#PurchaseOrderSearch').dataTable().fnSettings().aaSorting[0][0];
            var column = this.api().column(currestsortedcolumn);
            var columnId = $(column.header()).attr('id');
            switch (columnId) {
                case "thPurchaseOrder":
                    EnablePurchaseOrderIdColumnSorting();
                    break;
                case "thPurchaseOrderStatus":
                    EnableStatusColumnSorting();
                    break;
                case "thPurchaseOrderVendor":
                    EnableVendorColumnSorting();
                    break;
                case "thPurchaseOrderVendorName":
                    EnableVendorNameColumnSorting();
                    break;
            }
            $('#btnsortmenu').text(getResourceValue("spnSorting") + " : " + column.header().innerHTML);
            $("#POGridAction :input").removeAttr("disabled");
            $("#POGridAction :button").removeClass("disabled");
        }
    });
}

$(document).on('click', '#PurchaseOrderSearch_paginate .paginate_button', function () {
    run = true;
});
$(document).on('change', '#PurchaseOrderSearch_length .searchdt-menu', function () {
    run = true;
});

function EnablePurchaseOrderIdColumnSorting() {
    $('.DTFC_LeftWrapper').find('#thPurchaseOrder').css('pointer-events', 'auto');
    document.getElementById('thPurchaseOrderStatus').style.pointerEvents = 'none';
    document.getElementById('thPurchaseOrderVendor').style.pointerEvents = 'none';
    document.getElementById('thPurchaseOrderVendorName').style.pointerEvents = 'none';
}
function EnableStatusColumnSorting() {
    document.getElementById('thPurchaseOrderStatus').style.pointerEvents = 'auto';
    $(document).find('.th-PurchaseOrder').css('pointer-events', 'none');
    document.getElementById('thPurchaseOrderVendor').style.pointerEvents = 'none';
    document.getElementById('thPurchaseOrderVendorName').style.pointerEvents = 'none';
}
function EnableVendorColumnSorting() {
    document.getElementById('thPurchaseOrderVendor').style.pointerEvents = 'auto';
    $(document).find('.th-PurchaseOrder').css('pointer-events', 'none');
    document.getElementById('thPurchaseOrderStatus').style.pointerEvents = 'none';
    document.getElementById('thPurchaseOrderVendorName').style.pointerEvents = 'none';
}
function EnableVendorNameColumnSorting() {
    document.getElementById('thPurchaseOrderVendorName').style.pointerEvents = 'auto';
    $(document).find('.th-PurchaseOrder').css('pointer-events', 'none');
    document.getElementById('thPurchaseOrderStatus').style.pointerEvents = 'none';
    document.getElementById('thPurchaseOrderVendor').style.pointerEvents = 'none';
}
$(document).find('.srtPOcolumn').click(function () {
    ShowbtnLoader("btnsortmenu");
    var col = $(this).data('col');
    switch (col) {
        case 0:
            EnablePurchaseOrderIdColumnSorting();
            $('.DTFC_LeftBodyWrapper').find('#thPurchaseOrder').trigger('click');
            break;
        case 1:
            EnableStatusColumnSorting();
            $('#thPurchaseOrderStatus').trigger('click');
            break;
        case 2:
            EnableVendorColumnSorting();
            $('#thPurchaseOrderVendor').trigger('click');
            break;
        case 3:
            EnableVendorNameColumnSorting();
            $('#thPurchaseOrderVendorName').trigger('click');
            break;
    }

    $('#btnsortmenu').text(getResourceValue("spnSorting") + " : " + $(this).text());
    $(document).find('.srtPOcolumn').removeClass('sort-active');
    $(this).addClass('sort-active');
    run = true;
});
$(function () {
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            var optionval = $('#PurchaseOrderModel_CustomFilter option:selected').val();
            var PurchaseOrder = LRTrim($("#PurchaseOrder").val());
            var Status = $("#Status").val();
            var VendorClientLookupId = LRTrim($("#VendorClientLookupId").val());
            var VendorName = LRTrim($('#VendorName').val());
            var CreateDate = $("#CreateDate").val().trim();
            var Attention = LRTrim($("#Attention").val());
            var VendorPhoneNumber = LRTrim($("#VendorPhoneNumber").val());
            var CompleteDate = LRTrim($("#CompleteDate").val());
            var Reason = LRTrim($("#Reason").val());
            var Buyer_PersonnelName = LRTrim($("#BuyerPersonnelName").val());
            var TotalCost = LRTrim($("#TotalCost").val());
            dtTable = $("#PurchaseOrderSearch").DataTable();
            var currestsortedcolumn = $('#PurchaseOrderSearch').dataTable().fnSettings().aaSorting[0][0];
            var coldir = $('#PurchaseOrderSearch').dataTable().fnSettings().aaSorting[0][1];
            var colname = $('#PurchaseOrderSearch').dataTable().fnSettings().aoColumns[currestsortedcolumn].name;
            var jsonResult = $.ajax({
                url: '/Purchasing/GetPOPrintData?page=all',
                data: {
                    CustomQueryDisplayId: CustomQueryDisplayId,
                    PurchaseOrder: PurchaseOrder,
                    Status: Status,
                    VendorClientLookupId: VendorClientLookupId,
                    VendorName: VendorName,
                    CreateDate: CreateDate,
                    Attention: Attention,
                    VendorPhoneNumber: VendorPhoneNumber,
                    CompleteDate: CompleteDate,
                    Reason: Reason,
                    Buyer_PersonnelName: Buyer_PersonnelName,
                    TotalCost: TotalCost,
                    colname: colname,
                    coldir: coldir
                },
                success: function (result) {
                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data;
            var visiblecolumnsIndex = $("#PurchaseOrderSearch thead tr th").map(function (key) {
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
                if (item.Status != null) {
                    item.Status = item.Status;
                }
                else {
                    item.Status = "";
                }
                if (item.VendorClientLookupId != null) {
                    item.VendorClientLookupId = item.VendorClientLookupId;
                }
                else {
                    item.VendorClientLookupId = "";
                }
                if (item.VendorName != null) {
                    item.VendorName = item.VendorName;
                }
                else {
                    item.VendorName = "";
                }
                if (item.CreateDate != null) {
                    item.CreateDate = item.CreateDate;
                }
                else {
                    item.CreateDate = "";
                }
                if (item.Attention != null) {
                    item.Attention = item.Attention;
                }
                else {
                    item.Attention = "";
                }
                if (item.VendorPhoneNumber != null) {
                    item.VendorPhoneNumber = item.VendorPhoneNumber;
                }
                else {
                    item.VendorPhoneNumber = "";
                }
                if (item.CompleteDate != null) {
                    item.CompleteDate = item.CompleteDate;
                }
                else {
                    item.CompleteDate = "";
                }
                if (item.Reason != null) {
                    item.Reason = item.Reason;
                }
                else {
                    item.Reason = "";
                }
                if (item.Buyer_PersonnelName != null) {
                    item.Buyer_PersonnelName = item.Buyer_PersonnelName;
                }
                else {
                    item.Buyer_PersonnelName = "";
                }
                if (item.TotalCost != null) {
                    item.TotalCost = item.TotalCost;
                }
                else {
                    item.TotalCost = "";
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
                header: $("#PurchaseOrderSearch thead tr th div").map(function (key) {
                    return this.innerHTML;
                }).get()
            };
        }
    });
});
$(document).on('click', '.lnk_poSearch', function (e) {
    e.preventDefault();
    var index_row = $('#PurchaseOrderSearch tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = dtTable.row(row).data();
    var PurchaseOrderId = data.PurchaseOrderId;
    $.ajax({
        url: "/Purchasing/Details",
        type: "POST",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        data: { PurchaseOrderId: PurchaseOrderId },
        success: function (data) {
            $('#renderpurchasing').html(data);
        },
        complete: function () {
            generateLineiItemdataTable(PurchaseOrderId, "");
            CloseLoader();
            if ($(document).find('.AddPO').length === 0) { $(document).find('#poactiondiv').css('margin-right', '0px'); }
        }
    });
});
//#endregion
//#region Line Item
var dtLineItemTable;
var lItemfilteritemcount = 0;
var typeValPoLiStatus;
function generateLineiItemdataTable(PurchaseOrderId, searchtext) {
    var rCount = 0;
    var LineNumber = $("#LineNo").val();
    if (LineNumber) {
        LineNumber = LRTrim(LineNumber);
    }
    var PartId = $("#PartID").val();
    if (PartId) {
        PartId = LRTrim(PartId);
    }
    var Description = $("#Description").val();
    if (Description) {
        Description = LRTrim(Description);
    }
    var Quantity = $("#OrderQty").val();
    if (Quantity) {
        Quantity = LRTrim(Quantity);
    }
    var UOM = $('#UOM').val();
    if (UOM) {
        UOM = LRTrim(UOM);
    }
    var UnitCost = $("#UnitCost").val();
    if (UnitCost) {
        UnitCost = LRTrim(UnitCost);
    }
    var TotalCost = $("#TotalCost").val();
    if (TotalCost) {
        TotalCost = LRTrim(TotalCost);
    }
    var Status = $("#PoLiStatus").val();
    if (Status) {
        Status = LRTrim(Status);
    }
    else {
        Status = "";
    }
    var visibility = lineNumberGridSecurity;
    var visibilityAddLineitem = VisibilityAddLineItem;
    var LineItemStaus = lineNumberGridStatus;
    if ($(document).find('#tblLineItem').hasClass('dataTable')) {
        dtLineItemTable.destroy();
    }
    dtLineItemTable = $("#tblLineItem").DataTable({
        colReorder: true,
        rowGrouping: true,
        serverSide: true,
        searching: true,
        "bProcessing": true,
        "order": [[0, "asc"]],
        stateSave: false,
        "pagingType": "full_numbers",
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Purchasing/PopulateLineItem",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.PurchaseOrderId = PurchaseOrderId;
                d.searchText = searchtext;
                d.LineNumber = LineNumber;
                d.PartId = PartId;
                d.Description = Description;
                d.Quantity = Quantity;
                d.UOM = UOM;
                d.UnitCost = UnitCost;
                d.TotalCost = TotalCost;
                d.Status = Status;
            },
            "dataSrc": function (result) {
                rCount = result.data.length;
                $("#PoLiStatus").empty();
                $("#PoLiStatus").append("<option value=''>" + "--Select--" + "</option>");
                for (var i = 0; i < result.statuslist.length; i++) {
                    var id = result.statuslist[i];
                    var name = result.statuslist[i];
                    $("#PoLiStatus").append("<option value='" + id + "'>" + name + "</option>");
                }
                $("#UOM").empty();
                $("#UOM").append("<option value=''>" + "--Select--" + "</option>");
                var unitOfMeasure = [];
                for (var key in result.data) {
                    if (unitOfMeasure.indexOf(result.data[key].UnitOfMeasure) == -1) {
                        unitOfMeasure.push(result.data[key].UnitOfMeasure);
                    }
                }
                for (unitVal in unitOfMeasure) {
                    var name = unitOfMeasure[unitVal];
                    $("#UOM").append("<option value='" + name + "'>" + name + "</option>");
                }
                if (UOM && $("#UOM option[value='" + UOM + "']").length > 0) {
                    $("#UOM").val(UOM).trigger('change.select2');
                }
                else {
                    $("#UOM").val("").trigger('change.select2');
                }
                $(document).find('.select2picker').select2({});
                if (typeValPoLiStatus && $("#PoLiStatus option[value='" + typeValPoLiStatus + "']").length > 0) {
                    $(document).find("#PoLiStatus").val(typeValPoLiStatus).trigger('change.select2');
                }
                else {
                    $(document).find("#PoLiStatus").val("").trigger('change.select2');
                }
                return result.data;
            },
            global: true
        },
        columnDefs: [
            {
                "data": null,
                targets: [8], render: function (a, b, data, d) {
                    if (LineItemStaus == "Void") {
                        return "";
                    }
                    else if (visibility == "True" && visibilityAddLineitem == "False") {

                        return '<a class="btn btn-outline-success editPOLineItemBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delPOLineItemBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                    else if (visibility == "False" && visibilityAddLineitem == "True") {
                        return '<a data-toggle="modal" href="#AddLineItems" class="btn btn-outline-primary addPOLineItemBttn gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>'
                    }
                    else if (visibility == "True" && visibilityAddLineitem == "True") {
                        return '<a data-toggle="modal" href="#AddLineItems" class="btn btn-outline-primary addPOLineItemBttn gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                            '<a class="btn btn-outline-success editPOLineItemBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delPOLineItemBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a></div>';
                    }
                    else {
                        return "";
                    }
                }
            }
        ],
        "columns":
        [
            { "data": "LineNumber", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "PartClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
            {
                "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                "mRender": function (data, type, row) {
                    return "<div class='text-wrap width-200'>" + data + "</div>";
                }
            },
            { "data": "OrderQuantity", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
            { "data": "UnitOfMeasure", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "UnitCost", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
            { "data": "TotalCost", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
            { "data": "Status_Display", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "ObjectId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center" }

        ],
        initComplete: function () {
            var column = this.api().column(8);
            if (rCount > 0) {
                $("#addLineItem").hide();
            }
            else {
                if (LineItemStaus == "Void") {
                    $("#addLineItem").hide();
                }
                else if (visibilityAddLineitem == "True") {
                    $("#addLineItem").show();
                }
                else {
                    $("#addLineItem").hide();
                }
            }

            if (LineItemStaus == "Void") {
                column.visible(false);
            }
            else if (visibility == "False" && visibilityAddLineitem == "False") {
                column.visible(false);
            }
            else if (visibility == "True" || visibilityAddLineitem == "True") {
                column.visible(true);
            }
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', "#btnLitemSearch", function () {
    var PurchaseOrderId = $(document).find('#PurchaseOrderModel_PurchaseOrderId').val();
    clearLineItemAdvanceSearch();
    dtLineItemTable.state.clear();
    var searchText = LRTrim($('#txtsearchbox').val());
    typeValPoLiStatus = $("#PoLiStatus").val();
    generateLineiItemdataTable(PurchaseOrderId, searchText);
});
$(document).on('click', '#lineitemClearAdvSearchFilter', function () {
    var PurchaseOrderId = $(document).find('#PurchaseOrderModel_PurchaseOrderId').val();
    $("#txtsearchbox").val("");
    clearLineItemAdvanceSearch();
    generateLineiItemdataTable(PurchaseOrderId, "");
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
        }
    });
    typeValPoLiStatus = $("#PoLiStatus").val();
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
    $(document).find("#PoLiStatus,#UOM").val("").trigger('change.select2');
    typeValPoLiStatus = $("#PoLiStatus").val();
}
function LineItemAdvanceSearch() {
    var PurchaseOrderId = $(document).find('#PurchaseOrderModel_PurchaseOrderId').val();
    generateLineiItemdataTable(PurchaseOrderId, "");
}
$(document).on('click', '.litembtnCross', function () {
    var btnCrossedId = $(this).parent().attr('id');
    var searchtxtId = btnCrossedId.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    lItemfilteritemcount--;
    if (searchtxtId == "PoLiStatus") {
        typeValPoLiStatus = null;
    }
    LineItemAdvanceSearch();
    $('.lifilteritemcount').text(lItemfilteritemcount);

});
$(document).on('click', '.editPOLineItemBttn', function () {
    var data = dtLineItemTable.row($(this).parents('tr')).data();
    EditPOLineItem(data.PurchaseOrderLineItemId);
});
function EditPOLineItem(lineitemid) {
    var PurchaseOrderId = $(document).find('#PurchaseOrderModel_PurchaseOrderId').val();
    $.ajax({
        url: "/Purchasing/EditLineItem",
        type: "POST",
        dataType: 'html',
        data: { LineItemId: lineitemid, PurchaseOrderId: PurchaseOrderId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpurchasing').html(data);
        },
        complete: function () {
            SetPOControls();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
$(document).on('click', "#btnpolineitemcancel", function () {
    var PurchaseOrderId = $(document).find('#lineItem_PurchaseOrderId').val();
    RedirectToDetailOncancel(PurchaseOrderId, "overview");
});
$(document).on('click', "#btnpoNonPartIncancel", function () {
    var PurchaseOrderId = $(document).find('#lineItem_PurchaseOrderId').val();
    RedirectToDetailOncancel(PurchaseOrderId, "overview");
});
function EditLineItemOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        var message;
        SuccessAlertSetting.text = getResourceValue("LineItemUpdatedAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToPODetail(data.PurchaseOrderId, "overview");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
function AddPartNonInInventoryOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        var message;
        SuccessAlertSetting.text = getResourceValue("LineItemAddedAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToPODetail(data.PurchaseOrderId, "overview");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', '.delPOLineItemBttn', function () {
    var data = dtLineItemTable.row($(this).parents('tr')).data();
    var PurchaseOrderId = data.PurchaseOrderId;
    var PurchaseOrderLineItemId = data.PurchaseOrderLineItemId;
    DeletePRLineItem(PurchaseOrderId, PurchaseOrderLineItemId)
});
function DeletePRLineItem(PurchaseOrderId, PurchaseOrderLineItemId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: "/Purchasing/DeleteLineItem",
            type: "POST",
            dataType: 'json',
            data: { _PurchaseOrderId: PurchaseOrderId, _PurchaseOrderLineItemId: PurchaseOrderLineItemId },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                var message = "";
                if (data == "success") {
                    SuccessAlertSetting.text = getResourceValue("LineItemDeletedAlert");
                    swal(SuccessAlertSetting, function () {
                        if (data == "success") {
                            dtLineItemTable.destroy();
                            generateLineiItemdataTable(PurchaseOrderId, "");
                        }
                    });
                }
                else {
                    message = getResourceValue("LineItemDeleteFailedAlert");
                    swal({
                        title: "Alert",
                        text: message,
                        type: "warning",
                        showCancelButton: false,
                        confirmButtonClass: "btn-sm btn-primary",
                        cancelButtonClass: "btn-sm",
                        confirmButtonText: getResourceValue("SaveAlertOk"),
                        cancelButtonText: getResourceValue("CancelAlertNo")
                    }, function () {
                    });
                }
            },
            complete: function () {
                CloseLoader();
            },
            error: function (jqXHR, exception) {
                CloseLoader();
            }
        });
    });
}
$(document).on('change', '#lineItem_ChargeType', function () {
    $(document).find('#txtChargeToId').val('');
});
$(document).on('change', '#lineItem_ChargeToId', function () {
    var Id = $(this).val();
    var type = $("#lineItem_ChargeType option:selected").val();
    var textChargeToId = $("#lineItem_ChargeToId option:selected").text();
    if (textChargeToId == "--Select--") {
        $("#lineItem_ChargeTo_Name").val('');
        return false;
    }
    else {
        var Name = '';
        $.ajax({
            url: "/Purchasing/GetChargeToName",
            type: "GET",
            dataType: 'json',
            data: { _Id: Id, _type: type },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (type == "Equipment") {
                    Name = data.Name
                }
                else if (type == "WorkOrder") {
                    Name = data.ChargeTo_Name
                }
                else if (type == "Account") {
                    Name = data.Name
                }
            },
            complete: function () {
                CloseLoader();
                $("#lineItem_ChargeTo_Name").val(Name);
                $(document).find('.select2picker').select2({});
            },
            error: function () {
                CloseLoader();
            }
        });
    }
});
$(document).on('click', "#selectidpartnotininventory", function (e) {
    e.preventDefault();
    var PurchaseOrderId = $(document).find('#PurchaseOrderModel_PurchaseOrderId').val();
    $('.modal-backdrop').remove();
    GoToAddNonPartInInventory(PurchaseOrderId);
});
$(document).on('click', "#selectidpartininventory", function (e) {
    e.preventDefault();
    var PurchaseOrderId = $(document).find('#PurchaseOrderModel_PurchaseOrderId').val();
    $('.modal-backdrop').remove();
    GoToAddPartInInventory(PurchaseOrderId);
});
function GoToAddNonPartInInventory(PurchaseOrderId) {
    $.ajax({
        url: "/Purchasing/AddNonPartInInventory",
        type: "POST",
        dataType: "html",
        data: { PurchaseOrderId: PurchaseOrderId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpurchasing').html(data);
        },
        complete: function () {
            CloseLoader();
            SetPOControls();
        },
        error: function () {
            CloseLoader();
        }
    });
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
        changeMonth: true,
        changeYear: true,
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy');
}
//#endregion

//#region Work Flow History -  Removed as JIRA V2-30
//#endregion
//#region SelectPartgrid
var spartgridselectCount = 0;
var PartNotInInventorySelectedItemArray = [];
var FinalGridSelectedItemArray = [];
var finalSelectPartsTable;
var SelectPartsTable;
function PartNotInInventorySelectedItem(PartId, ClientLookupId, Description, Manufacture, Quantity) {
    this.PartId = PartId;
    this.ClientLookupId = ClientLookupId;
    this.Description = Description;
    this.Manufacture = Manufacture;
    this.Quantity = Quantity;
};
function PartNotInInventoryProcessdata(PartId, ClientLookupId, Description, OrderQuantity, PurchaseOrderId) {
    this.PartId = PartId;
    this.ClientLookupId = ClientLookupId;
    this.Description = Description;
    this.OrderQuantity = OrderQuantity;
    this.PurchaseOrderId = PurchaseOrderId;
};
function GoToAddPartInInventory(PurchaseOrderId) {
    $.ajax({
        url: "/Purchasing/AddPartInInventory",
        type: "POST",
        dataType: "html",
        data: { PurchaseOrderId: PurchaseOrderId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpurchasing').html(data);
        },
        complete: function () {
            CloseLoader();
            GenerateSelectPartsGrid();
        },
        error: function () {
            CloseLoader();
        }
    });
}
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
$(document).on('change', '.chkfrow', function () {
    var data = finalSelectPartsTable.row($(this).parents('tr')).data();
    if (!this.checked) {
        FinalGridSelectedItemArray = FinalGridSelectedItemArray.filter(function (el) {
            return el.PartId != data.PartId;
        });
        if ($(this).parents('tr').hasClass('chk-selected')) {
            $(this).parents('tr').removeClass('chk-selected');
        }
    }
    else {
        var item = new PartNotInInventorySelectedItem(data.PartId, data.ClientLookupId, data.Description, data.Manufacturer, data.Quantity);
        var found = FinalGridSelectedItemArray.some(function (el) {
            return el.PartId == data.PartId;
        });
        if (!found) { FinalGridSelectedItemArray.push(item); }
        $(this).parents('tr').addClass('chk-selected');
    }
});
$(document).on('click', "#btnselectinventory", function () {
    if (PartNotInInventorySelectedItemArray.length < 1) {
        ShowGridItemSelectionAlert();
        return false;
    }
    else {
        if (typeof finalSelectPartsTable != 'undefined') {
            finalSelectPartsTable.destroy();
        }
        FinalGridSelectedItemArray = [];
        ScrollToId("btnselectinventory");
        $('#finselectcontainer').show();
        GeneratedfinalSelectPartsTable(PartNotInInventorySelectedItemArray);
        var el = $(document).find('#fgidselectall').get(0);
        if (el && el.checked && ('indeterminate' in el)) {
            el.indeterminate = false;
        }
        $('#fgidselectall').prop('checked', false);
    }
});
$(document).on('click', '#fgidselectall', function (e) {
    var checked = this.checked;
    finalSelectPartsTable.rows().every(function () {
        var d = this.data();
        if (checked) {
            var found = FinalGridSelectedItemArray.some(function (el) {
                return el.PartId === d.PartId;
            });
            if (!found) {
                var item = new PartNotInInventorySelectedItem(d.PartId, d.ClientLookupId, d.Description, d.Manufacturer, d.Quantity);
                FinalGridSelectedItemArray.push(item);
            }
        }
        else {
            FinalGridSelectedItemArray = [];
        }
    });
    finalSelectPartsTable.column(0).nodes().to$().each(function (index, item) {
        var thischk = $(this).parents('tr');
        if (checked) {
            thischk.addClass('chk-selected');
            thischk.find('.chkfrow').prop('checked', 'checked');
        } else {
            thischk.removeClass('chk-selected');
            thischk.find('.chkfrow').prop('checked', false);
        }
    });
    $('#tblfinalSelectPartsGrid').on('change', 'input[type="checkbox"]', function () {
        if (!this.checked) {
            var el = $(document).find('#fgidselectall').get(0);
            if (el && el.checked && ('indeterminate' in el)) {
                el.indeterminate = true;
            }
        }
    });
});
$(document).on('click', "#btnremoverowfgrid", function () {
    if (FinalGridSelectedItemArray.length > 0) {
        if ($('#fgidselectall').is(":checked")) {
            finalSelectPartsTable.clear().draw();
            FinalGridSelectedItemArray = [];
            $('#fgidselectall').prop('checked', false);
        }
        else {
            finalSelectPartsTable.rows().every(function () {
                var d = this.data();
                var IsChecked = this.nodes().to$().find('.chkfrow').is(":checked");
                if (IsChecked) {
                    var d = this.data();
                    var PartId = d.PartId;
                    FinalGridSelectedItemArray = FinalGridSelectedItemArray.filter(function (el) {
                        return el.PartId != PartId;
                    });
                }
            });
            finalSelectPartsTable.rows('.chk-selected').remove().draw(false);
        }
        if (finalSelectPartsTable.rows().count() == 0) {
            $('#finselectcontainer').hide();
        }
    }
    else {
        $('#cancelModal').modal('hide');
        swal({
            title: "No line item selected",
            text: "Please select line item(s) to remove",
            type: "warning",
            confirmButtonClass: "btn-sm btn-primary",
            confirmButtonText: getResourceValue("SaveAlertOk"),
        });
        return false;
    }
});
$(document).on('click', "#btnprocess", function () {
    var PurchaseOrderId = $(document).find('#partInInventoryModel_PurchaseOrderId').val();
    var thisData = [];
    var toReturn;
    finalSelectPartsTable.rows().every(function () {
        var d = this.data();
        var PartId = d.PartId;
        var ClientLookupId = d.ClientLookupId;
        var Description = d.Description;
        var Quantity = this.nodes().to$().find('.decimalinput').val();
        if (!$.isNumeric(Quantity)) {
            this.nodes().to$().find('.decimalinput').addClass('input-validation-error');
            toReturn = false;
        }
        else {
            $(this).find('.decimalinput ').removeClass('input-validation-error');
        }
        if (PartId) {
            var item = new PartNotInInventoryProcessdata(PartId, ClientLookupId, Description, Quantity, PurchaseOrderId);
            thisData.push(item);
        }
    });
    if (toReturn == false) {
        return;
    }
    var list = JSON.stringify({ 'list': thisData });
    $.ajax({
        url: "/Purchasing/SavePartInInventory",
        type: "POST",
        dataType: "json",
        data: list,
        contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.Result == "success") {
                PartNotInInventorySelectedItemArray = [];
                FinalGridSelectedItemArray = [];
                SuccessAlertSetting.text = getResourceValue("LineItemAddedAlert");
                swal(SuccessAlertSetting, function () {
                    RedirectToPODetail(PurchaseOrderId, "overview");
                });
            }
            else {
                var msgEror = getResourceValue("UpdateAlert");
                ShowGenericErrorOnAddUpdate(msgEror);
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
function GenerateSelectPartsGrid(searchtext) {
    var partId = LRTrim($('#PinvPartID').val());
    var description = LRTrim($('#pinvDescription').val());
    if (typeof SelectPartsTable !== "undefined") {
        SelectPartsTable.destroy();
    }
    SelectPartsTable = $("#tblSelectPartsGrid").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[1, "asc"]],
        stateSave: true,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Purchasing/GetSelectPartsGridData",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.searchText = searchtext;
                d.PartId = partId;
                d.Description = description;
            },
            "dataSrc": function (result) {
                return result.data;
            },
            global: true
        },
        select: {
            style: 'os',
            selector: 'td:first-child'
        },
        "columns":
        [
            {
                "data": "PartId",
                orderable: false,
                "bSortable": false,
                className: 'select-checkbox',
                targets: 0,
                'render': function (data, type, full, meta) {
                    return '<input type="checkbox" name="id[]" data-eqid="' + data + '" class="chksearch"  value="'
                        + $('<div/>').text(data).html() + '">';

                }
            },
            { "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
            {
                "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                "mRender": function (data, type, row) {
                    return "<div class='text-wrap width-300'>" + data + "</div>";
                }
            },
            { "data": "ManufacturerId", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "Manufacturer", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "Quantity", "autoWidth": true, "bSearchable": true, "bSortable": true },

        ],
        "columnDefs": [
            { width: "3%", targets: 0 },
            { width: "10%", targets: 1 },
            { width: "47%", targets: 2 },
            { width: "15%", targets: 3 },
            { width: "15%", targets: 4 },
            { width: "10%", targets: 5 },

        ],
        'rowCallback': function (row, data, dataIndex) {
            var found = PartNotInInventorySelectedItemArray.some(function (el) {
                return el.PartId === data.PartId;
            });
            if (found) {
                $(row).find('input[type="checkbox"]').prop('checked', true);
            }
        }
    });
};
function GeneratedfinalSelectPartsTable(datasource1) {
    var data = datasource1;
    if (!$(document).find('#tblfinalSelectPartsGrid').hasClass('dataTable')) {
        finalSelectPartsTable = $("#tblfinalSelectPartsGrid").DataTable({
            colReorder: true,
            rowGrouping: true,
            searching: true,
            "bProcessing": true,
            language: {
                url: "/base/GetDataTableLanguageJson?nGrid=" + true,
            },
            "order": [[1, "asc"]],
            sDom: 'Btipr',
            buttons: [],
            "orderMulti": true,
            "pagingType": "full_numbers",
            data: data,
            "columns":
            [
                {
                    "data": "PartId",
                    orderable: false,
                    "bSortable": false,
                    className: 'select-checkbox dt-body-center',
                    targets: 0,
                    'render': function (data, type, full, meta) {
                        return '<input type="checkbox" name="id[]" data-eqid="' + data + '" class="chkfrow"  value="'
                            + $('<div/>').text(data).html() + '">';

                    }
                },
                { "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                {
                    "data": "Quantity", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<input type="text" class = "form-control search decimalinput", autocomplete = "off"  value="1" id="qnty">';
                    }
                },
            ],
            "columnDefs": [
                { width: "3%", targets: 0 },
                { width: "10%", targets: 1 },
                { width: "57%", targets: 2 },
                { width: "30%", targets: 3 }
            ],
            'rowCallback': function (row, data, dataIndex) {
                var found = FinalGridSelectedItemArray.some(function (el) {
                    return el.PartId === data.PartId;
                });
                if (found) {
                    $(row).find('input[type="checkbox"]').prop('checked', true);
                }
            }
        });
    }
};
$(document).on('click', "#btnSPGSearch", function () {
    PartsGridclearAdvanceSearch();
    var searchtext = LRTrim($('#SPGtxtSearch').val());
    SelectPartsTable.state.clear();
    GenerateSelectPartsGrid(searchtext);
});
$(document).on('click', '#SPGClearSearch', function () {
    SelectPartsTable.state.clear();
    $("#SPGtxtSearch").val("");
    PartsGridclearAdvanceSearch();
    GenerateSelectPartsGrid("");
});
$(document).on('click', '#btnSPGAdvanceSearch', function () {
    SelectPartsTable.state.clear();
    SPGAdvSearch();
    $('.sidebar').removeClass('active');
    $('.overlay').fadeOut();
});
function SPGAdvSearch() {
    $("#SPGtxtSearch").val("");
    var searchitemhtml = "";
    spartgridselectCount = 0;
    $('#SPGadvsearchsidebar').find('.adv-item').each(function (index, item) {
        if ($(this).val()) {
            spartgridselectCount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times SPGbtnCross" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    GenerateSelectPartsGrid("");
    $("#SPGadvsearchfilteritems").html(searchitemhtml);
    $(".pinvgridfilteritemcount").text(spartgridselectCount);
}
function PartsGridclearAdvanceSearch() {
    spartgridselectCount = 0;
    $("#PinvPartID").val("");
    $("#pinvDescription").val("");
    $(".pinvgridfilteritemcount").text(0);
    $("#SPGadvsearchfilteritems").html("");
    $(document).find("#Status").val("").trigger('change');
};
$(document).on('click', '.SPGbtnCross', function () {
    var btnCrossed = $(this).parent().attr('id');
    var searchtxtId = btnCrossed.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    spartgridselectCount--;
    SPGAdvSearch();
});
$(document).on('click', '#brdpolineitem', function () {
    var PurchaseOrderId = $(this).attr('data-val');
    PartNotInInventorySelectedItemArray = [];
    RedirectToPODetail(PurchaseOrderId);
});
$(document).on('click', "#btnAddInventorycancel", function () {
    var PurchaseOrderId = $(document).find('#partInInventoryModel_PurchaseOrderId').val();
    PartNotInInventorySelectedItemArray = [];
    swal(CancelAlertSetting, function () {
        RedirectToPODetail(PurchaseOrderId);
    });

});
//#endregion

//#region ColumnVisibility
$(document).on('click', '#liCustomize', function () {
    funCustomizeBtnClick(dtTable);
});
$(document).on('click', '.saveConfig', function () {
    var colOrder = [0];
    funCustozeSaveBtn(dtTable, colOrder);
    run = true;
    dtTable.state.save(run);
});
//#endregion




