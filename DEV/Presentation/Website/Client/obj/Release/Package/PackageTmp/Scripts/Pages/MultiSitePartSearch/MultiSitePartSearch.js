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
    generateMultiSitePartSearchTable();
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
    ShowbtnLoader("btnsortmenu");
});
//#endregion Common
//#region Search
var MultiSitePartSearchTable;
function generateMultiSitePartSearchTable() {
    var printCounter = 0;
    if ($(document).find('#MultiSitePartSearch').hasClass('dataTable')) {
        MultiSitePartSearchTable.destroy();
    }
    MultiSitePartSearchTable = $("#MultiSitePartSearch").DataTable({
        colReorder: {
            fixedColumnsLeft: 0
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
                        GridName: "MultiSitePart_Search",
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
                    GridName: "MultiSitePart_Search",
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
         
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/MultiSitePartSearch/GetMultiSitePartSearchGrid",
            "type": "POST",
            "datatype": "json",
            data: function (d) {
                d.srchText = LRTrim($('#txtSearch').val());
                d.ClientLookupId = LRTrim($('#ClientLookupId').val());
                d.Description = LRTrim($("#Description").val());
                d.Quantity = LRTrim($("#Quantity").val());
                d.Manufacturer = LRTrim($('#Manufacturer').val());
                d.ManufacturerId = LRTrim($('#ManufacturerId').val());                
                d.City = LRTrim($('#City').val());
                d.State = LRTrim($('#State').val());
                d.Name = LRTrim($('#Name').val());
            },
            "dataSrc": function (result) {
                HidebtnLoader("btnsortmenu");
                return result.data;
            },
            global: true
        },
        "columns":
        [
            { "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "0" },
            {
                "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1",
                "mRender": function (data, type, row) {
                    return "<div class='text-wrap width-200'>" + data + "</div>";
                }
            },
            { "data": "Quantity", "autoWidth": true, "bSearchable": true, "bSortable": false },
            { "data": "Manufacturer", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3" },
            { "data": "ManufacturerId", "autoWidth": true, "bSearchable": true, "bSortable": false },           
            { "data": "City", "autoWidth": true, "bSearchable": true, "bSortable": false },
            { "data": "State", "autoWidth": true, "bSearchable": true, "bSortable": false },
            { "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": false },
        ],       
        initComplete: function () {
            SetPageLengthMenu();
            var currestsortedcolumn = $('#MultiSitePartSearch').dataTable().fnSettings().aaSorting[0][0];
            var column = this.api().column(currestsortedcolumn);
            var columnId = $(column.header()).attr('id');
            switch (columnId) {
                case "thpartid":
                    EnablePartIdColumnSorting();
                    break;
                case "thdescription":
                    EnableDescriptionColumnSorting();
                    break;
                case "thmanufacturer":
                    EnableManufacturerColumnSorting();
                    break;
            }
            $('#btnsortmenu').text(getResourceValue("spnSorting") + " : " + column.header().innerHTML);
            $("#PartGridAction :input").removeAttr("disabled");
            $("#PartGridAction :button").removeClass("disabled");
        }
    });
};
$(document).on('click', "#btnTxtSearch", function (e) {
    run = true;
    clearAdvanceSearch();
    MultiSitePartSearchTable.page('first').draw('page');
});
function EnablePartIdColumnSorting() {
    $('.DTFC_LeftWrapper').find('#thpartid').css('pointer-events', 'auto');
    document.getElementById('thpartid').style.pointerEvents = 'auto';
    document.getElementById('thdescription').style.pointerEvents = 'none';
    document.getElementById('thmanufacturer').style.pointerEvents = 'none';
}
function EnableDescriptionColumnSorting() {
    $(document).find('.th-partid').css('pointer-events', 'none');
    document.getElementById('thdescription').style.pointerEvents = 'auto';
    document.getElementById('thpartid').style.pointerEvents = 'none';
    document.getElementById('thmanufacturer').style.pointerEvents = 'none';
}
function EnableManufacturerColumnSorting() {
    $(document).find('.th-partid').css('pointer-events', 'none');
    document.getElementById('thmanufacturer').style.pointerEvents = 'auto';
    document.getElementById('thpartid').style.pointerEvents = 'none';
    document.getElementById('thdescription').style.pointerEvents = 'none';
}
$(document).find('.srtpartcolumn').click(function () {
    ShowbtnLoader("btnsortmenu");
    var col = $(this).data('col');
    switch (col) {
        case 0:
            EnablePartIdColumnSorting();
            $('#thpartid').trigger('click');
            break;
        case 1:
            EnableDescriptionColumnSorting();
            $('#thdescription').trigger('click');
            break;
        case 3:
            EnableManufacturerColumnSorting();
            $('#thmanufacturer').trigger('click');
            break;
    }
    $('#btnsortmenu').text(getResourceValue("spnSorting") + " : " + $(this).text());
    $(document).find('.srtpartcolumn').removeClass('sort-active');
    $(this).addClass('sort-active');
    run = true;
});
//#endregion Search
//#region AdvSearch
var hGridfilteritemcount = 0;
$(document).on('click', "#btnMultiSitePartAdvSrch", function (e) {
    $(document).find('#txtSearch').val('');
    MultiSitePartSearchTable.state.clear();
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
    MultiSitePartSearchTable.page('first').draw('page');
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
$(document).on('click', '#liClearAdvSearchFilter,#clearSearch', function () {
    run = true;
    $(document).find('#txtSearch').val('');
    clearAdvanceSearch();
    MultiSitePartSearchTable.page('first').draw('page');
});
function clearAdvanceSearch() {
    var filteritemcount = 0;
    $('#advsearchsidebar').find('input:text').val('');
    $('.filteritemcount').text(filteritemcount);
    $('#advsearchfilteritems').find('span').html('');
    $('#advsearchfilteritems').find('span').removeClass('tagTo');
}
//#endregion AdvSearch
$(document).on('click', '#MultiSitePartSearch_paginate .paginate_button', function () {
    run = true;
});
$(document).on('change', '#MultiSitePartSearch_length .searchdt-menu', function () {
    run = true;
});

$(document).on('click', '#MultiSitePartSearch_wrapper th', function () {
    run = true;
});