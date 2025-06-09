var run = false;
//#region Common
var activeStatus = false;
$(document).on('click', '#emsidebarCollapse', function () {
    $('#sidebar').addClass('active');
    $('.overlay').fadeIn();
    $('.collapse.in').toggleClass('in');
    $('a[aria-expanded=true]').attr('aria-expanded', 'false');
});
$(function () {
    $(".actionBar").fadeIn();
    $("#equipmentGridAction :input").attr("disabled", "disabled");
    generateVendorCatTable();
    $("#sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $('#dismiss, .overlay').on('click', function () {
        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });
    $('#sidebarCollapse').on('click', function () {
        $('#sidebar').addClass('active');
        $('.overlay').fadeIn();
        $('.collapse.in').toggleClass('in');
        $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    });
});
//#endregion Common
//#region Search
var vendorCatTable;
function generateVendorCatTable() {
    var printCounter = 0;
    if ($(document).find('#vendorCatalogSearch').hasClass('dataTable')) {
        vendorCatTable.destroy();
    }
    vendorCatTable = $("#vendorCatalogSearch").DataTable({
        colReorder: {
            fixedColumnsLeft: 0
        },
        rowGrouping: true,
        colReorder: true,
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
                        GridName: "VendorCatalog_Search",
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
                    GridName: "VendorCatalog_Search",
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
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'Vendor Catalog'
            },
            {
                extend: 'print',
                title: 'Vendor Catalog'
            },
            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Vendor Catalog',
                extension: '.csv'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                css: 'display:none',
                title: 'Vendor Catalog',
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/VendorCatalog/GetVendorCatalogGrid",
            "type": "POST",
            "datatype": "json",
            data: function (d) {
                d.srchText = LRTrim($('#txtSearch').val());
                d.PartID = LRTrim($('#PartID').val());
                d.Description = LRTrim($("#Description").val());
                d.Manufacturer = LRTrim($("#Manufacturer").val());
                d.ManufacturerPartNumber = LRTrim($('#ManufacturerPartNumber').val());
                d.Category = LRTrim($('#Category').val());
                d.CategoryDescription = LRTrim($('#CategoryDescription').val());
                d.UnitCost = LRTrim($('#UnitCost').val());
                d.PurchaseUnit = LRTrim($('#PurchaseUnit').val());
                d.VendorPartNumber = LRTrim($('#VendorPartNumber').val());
                d.VendorName = LRTrim($('#VendorName').val());
                d.VendorID = LRTrim($('#VendorID').val());
            },
            "datasrc": function (result) {
            },
            global: true
        },
        "columns":
        [
            { "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "0" },
            { "data": "LongDescription", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1" },
            { "data": "Manufacturer", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2" },
            { "data": "ManufacturerId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3" },
            { "data": "Category", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "4" },
            { "data": "CM_Description", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "5" },
            { "data": "UnitCost", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "6" },
            { "data": "VI_PurchaseUOM", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "7" },
            { "data": "VCI_PartNumber", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "8" },
            { "data": "VendorName", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "9" },
            { "data": "VendorClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "10" }
        ],
        columnDefs: [
            {
                targets: [0],
                className: 'noVis'
            }
        ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
};
$(document).on('click', "#btnTxtSearch", function (e) {
    run = true;
    clearAdvanceSearch();
    generateVendorCatTable();
});
//#endregion Search
//#region AdvSearch
var hGridfilteritemcount = 0;
$(document).on('click', "#btnVenCatAdvSrch", function (e) {
    $(document).find('#txtSearch').val('');
  //  vendorCatTable.state.clear();
    var searchitemhtml = "";
    hGridfilteritemcount = 0;
    $('#advsearchsidebar').find('.adv-item').each(function (index, item) {
        if ($(this).val() && $(this).val() != "0") {
            hGridfilteritemcount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossSODL" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $('#advsearchfilteritems').html(searchitemhtml);
    $('#sidebar').removeClass('active');
    $(document).find('.overlay').fadeOut();
    GridAdvanceSearch();
});
function GridAdvanceSearch() {
    run = true;
    vendorCatTable.page('first').draw('page');
   // generateVendorCatTable();
    $('.filteritemcount').text(hGridfilteritemcount);
}
$(document).on('click', '.btnCrossSODL', function () {
    var btnCrossedId = $(this).parent().attr('id');
    var searchtxtId = btnCrossedId.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    hGridfilteritemcount--;
    GridAdvanceSearch();
});
$(document).on('click', '#liClearAdvSearchFilter', function () {
   run = true;
     $(document).find('#txtSearch').val('');
     clearAdvanceSearch();
     vendorCatTable.page('first').draw('page');
    //generateVendorCatTable();
});
function clearAdvanceSearch() {
    var filteritemcount = 0;   
    $('#advsearchsidebar').find('input:text').val('');
    $('.filteritemcount').text(filteritemcount);
    $('#advsearchfilteritems').find('span').html('');
    $('#advsearchfilteritems').find('span').removeClass('tagTo');
}
//#endregion AdvSearch


$(document).on('click', '#vendorCatalogSearch_paginate .paginate_button', function () {
    run = true;
});
$(document).on('change', '#vendorCatalogSearch_length .searchdt-menu', function () {
    run = true;
});

$(document).on('click', '#vendorCatalogSearch_wrapper th', function () {
    run = true;
});