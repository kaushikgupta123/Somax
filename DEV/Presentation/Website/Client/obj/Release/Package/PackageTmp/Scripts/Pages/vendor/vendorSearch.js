//#region Commonn 
var dtVendorSearch;
var selectCount = 0;
var gridname = "Vendor_Search";
var activeStatus;
var run = false;
//#endregion

function filterinfo(id, value) {
    this.key = id;
    this.value = value;
}
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
$(document).on('keyup', '#vendorsearctxtbox', function (e) {
    var tagElems = $(document).find('#vendorsearchListul').children();
    $(tagElems).hide();
    for (var i = 0; i < tagElems.length; i++) {
        var tag = $(tagElems).eq(i);
        if ($(tag).text().toLowerCase().includes($(this).val().toLowerCase()) == true || $(this).val().toLowerCase().includes($(tag).text().toLowerCase()) == true) {
            $(tag).show();
        }
    }
});
$(document).on('click', '.vendorsearchdrpbox', function (e) {
    if ($(document).find('#txtColumnSearch').val() !== '')
        $("#dvFilterSearchSelect2").html('');
    $(document).find('#txtColumnSearch').val('');
    run = true;
    $('#vendorsearctxtbox').text($(this).text());
    $(".searchList li").removeClass("activeState");
    $(this).addClass('activeState');
    $(document).find('#searcharea').hide("slide");
    var optionval = $(this).attr('id');
    localStorage.setItem("VENDORSEARCHGRIDDISPLAYSTATUS", optionval);
    activeStatus = optionval;
    $(document).find('#vendorsearchtitle').text($(this).text());
    if (optionval.length !== 0) {
        ShowbtnLoaderclass("LoaderDrop");
        dtVendorSearch.page('first').draw('page');
    }
});
$(document).on('click', '#linkToSearch', function () {
    window.location.href = "../Vendor/Index?page=Inventory_Vendors";
});
//#endregion

//#region New Search button
$(document).on('click', "#SrchBttnNew", function () {
    $.ajax({
        url: '/Base/PopulateNewSearchList',
        type: 'GET',
        data: { tableName: 'Vendor' },
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
        data: { tableName: 'Vendor', searchText: txtSearchval, isClear: isClear },
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
                dtVendorSearch.page('first').draw('page');
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
    clearAdvanceSearch();
    $("#gridadvsearchstatus").val('');
    activeStatus = localStorage.getItem("VENDORSEARCHGRIDDISPLAYSTATUS");
    var txtSearchval = LRTrim($(document).find('#txtColumnSearch').val());
    if (txtSearchval) {
        GenerateSearchList(txtSearchval, false);
        var searchitemhtml = "";
        searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $('#txtColumnSearch').attr('id') + '">Search: ' + txtSearchval + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
        searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
        $("#dvFilterSearchSelect2").html(searchitemhtml);
    }
    else {
        dtVendorSearch.page('first').draw('page');
    }
    var container = $(document).find('#searchBttnNewDrop');
    container.hide("slideToggle");
    CloseLoader();
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

//#region New sort
//$(document).find('.srtVendorcolumn').click(function () {
//    ShowbtnLoader("btnsortmenu");
//    order = $(this).data('col');//Vendor Sorting
//    var txtColumnSearch = LRTrim($(document).find('#txtColumnSearch').val());
//    if (txtColumnSearch != "") {
//         TextSearch();//Vendor Sorting
//    }
//    else {
//        $("#vendorSearch").DataTable().draw();//Vendor Sorting
//    } 
//    $('#btnsortmenu').text(getResourceValue("spnSorting") + " : " + $(this).text());
//    $(document).find('.srtVendorcolumn').removeClass('sort-active');
//    $(this).addClass('sort-active');
//    run = true;
//});
//$(document).find('.srtVendororder').click(function () {
//    ShowbtnLoader("btnsortmenu");
//    orderDir = $(this).data('mode');//Vendor Sorting
//    var txtColumnSearch = LRTrim($(document).find('#txtColumnSearch').val());
//    if (txtColumnSearch != "") {
//        TextSearch();//Vendor Sorting
//    }
//    else {
//        $("#vendorSearch").DataTable().draw();//Vendor Sorting
//    }  
//    $(document).find('.srtVendororder').removeClass('sort-active');
//    $(this).addClass('sort-active');
//    run = true;
//});
//#endregion

//#region Search
$(document).ready(function () {
    $("#ActionGridBar :input").attr("disabled", "disabled");
    ShowbtnLoader("btnsortmenu");
    ShowbtnLoaderclass("LoaderDrop");
    $(document).on('click', '#sidebarCollapse', function () {
        $('#sidebar').addClass('active');
        $('.overlay').fadeIn();
        $('.collapse.in').toggleClass('in');
        $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    });
    $("#btnVendorDataAdvSrch").on('click', function (e) {
        run = true;
        $("#txtVendorDataSrch").val('');
        AWBAdvSearch();
        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();

        dtVendorSearch.page('first').draw('page');
    });
    //#region Load Grid With Status
    var displayState = localStorage.getItem("VENDORSEARCHGRIDDISPLAYSTATUS");
    if (displayState) {
        activeStatus = displayState;
        if (activeStatus == 1) {
            $(document).find('#vendorsearchtitle').text($('#vendorsearchListul').find('li').eq(0).text());
            $("#vendorsearchListul li").eq(0).addClass('activeState');
        }
        else if (activeStatus == 2) {
            $(document).find('#vendorsearchtitle').text($('#vendorsearchListul').find('li').eq(1).text());
            $("#vendorsearchListul li").eq(1).addClass('activeState');
        }
    }
    else {
        activeStatus = 1;
        $(document).find('#vendorsearchtitle').text($('#vendorsearchListul').find('li').eq(0).text());
        $("#vendorsearchListul li").eq(0).addClass('activeState');
    }
    $('#advsearchsidebar').find('select').val("").trigger('change');
    generateVendorDataTable();
    //#endregion

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
    $(document).on('click', "#editvendor", function () {
         var vendorId = $('#vendors_VendorId').val();
        $.ajax({
            url: "/Vendor/VendorEditDynamic", /*"/Vendor/VendorEdit",*/ /*implementation of V2-642*/
            type: "POST",
            dataType: 'html',
            data: { vendorId: vendorId },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                $('#vendorsearchcontainer').empty().html(data);
            },
            complete: function () {
                CloseLoader();
                $.validator.setDefaults({ ignore: null });
                $.validator.unobtrusive.parse(document);
                $(document).find('.select2picker').select2({});

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
            },
            error: function () {
                CloseLoader();
            }
        });
    });
});
$(document).on('click', '.btnCross', function () {
    run = true;
    var btnCrossed = $(this).parent().attr('id');
    var searchtxtId = btnCrossed.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    selectCount--;
    if (searchtxtId == "ddlType") {
        $(document).find("#ddlType").val("").trigger('change.select2');
    }
    else if (searchtxtId == "ddlTerm") {
        $(document).find("#ddlTerm").val("").trigger('change.select2');
    }
    if (searchtxtId == "ddlFob") {
        $(document).find("#ddlFob").val("").trigger('change.select2');
    }
    AWBAdvSearch();
    dtVendorSearch.page('first').draw('page');
});
var titleArray = [];
var classNameArray = [];
var order = '0';//Vendor Sorting
var orderDir = 'asc';//Vendor Sorting
function generateVendorDataTable() {
    if ($(document).find('#vendorSearch').hasClass('dataTable')) {
        dtVendorSearch.destroy();
    }
    dtVendorSearch = $("#vendorSearch").DataTable({
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
                if (data.order) {
                    data.order[0][0] = order;
                    data.order[0][1] = orderDir;
                }//Vendor Sorting
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

                    selectCount = 0;
                    if (json.LayoutInfo) {
                        var LayoutInfo = JSON.parse(json.LayoutInfo);//Vendor Sorting
                        order = LayoutInfo.order[0][0];//Vendor Sorting
                        orderDir = LayoutInfo.order[0][1]; //Vendor Sorting
                        callback(JSON.parse(json.LayoutInfo));
                        if (json.FilterInfo) {
                            setsearchui(JSON.parse(json.FilterInfo), $("#txtColumnSearch"), $("#spnControlCounter"), $("#dvFilterSearchSelect2"));
                        }
                    }
                    else {
                        callback(json.LayoutInfo);
                    }
                }
            });
        },
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        scrollX: true,
        fixedColumns: {
            leftColumns: 1,
        },
        sDom: 'Btlipr',
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'Vendors'
            },
            {
                extend: 'print',
                title: 'Vendors'
            },
            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Vendors',
                extension: '.csv',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                orientation: 'portrait',
                pageSize: 'A4',
                title: 'Vendors'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/Vendor/GetVendors",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                activeStatus = localStorage.getItem("VENDORSEARCHGRIDDISPLAYSTATUS");
                d._addresscity = LRTrim($('#AddressCity').val());
                d._addressstate = LRTrim($('#AddressState').val());
                d._vendor = LRTrim($('#Vendor').val());
                d._name = LRTrim($('#Name').val());
                d._type = LRTrim($('#ddlType').val());
                d._terms = LRTrim($('#ddlTerm').val());
                d._fobcode = LRTrim($('#ddlFob').val());
                d.inactiveFlag = activeStatus;
                d.isExternal = LRTrim($("#isExternal").val());
                d.srcData = LRTrim($("#txtColumnSearch").val());
                d.Order = order;//Vendor Sorting
                //d.orderDir = orderDir;//Vendor Sorting
            },
            "dataSrc": function (result) {
                let colOrder = dtVendorSearch.order();
                orderDir = colOrder[0][1];

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
                    "orderable": true,
                    "className": "text-left",
                    "name": "0",
                    "mRender": function (data, type, row) {
                        return '<a class=lnk_vendorrow href="javascript:void(0)">' + data + '</a>';
                    }
                },

                { "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1" },
                { "data": "AddressCity", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "AddressState", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Type", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "4" },
                { "data": "Terms", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "FOBCode", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "IsExternal", "autoWidth": true, "bSearchable": false, "bSortable": true, className: 'text-center', "name": "7",
                    "mRender": function (data, type, row) {
                        if (data == true) {
                            return '<label class="m-checkbox m-checkbox--air m-checkbox--solid m-checkbox--state-brand somax-checkbox lbl-innergrid-checkbox">' +
                                '<input type="checkbox" checked="checked" class="status" onclick="return false"><span></span></label>';
                        }
                        else {

                            return '<label class="m-checkbox m-checkbox--air m-checkbox--state-brand somax-checkbox lbl-innergrid-checkbox">' +
                                '<input type="checkbox" class="status" onclick="return false"><span></span></label>';
                        }

                    }
                }

            ],
        "columnDefs": [
            {
                render: function (data, type, full, meta) {
                    return "<div class='text-wrap width-50'>" + data + "</div>";
                },
                targets: [1, 2]
            },
            {
                targets: [0],
                className: 'noVis'
            },
            {
                targets: -1,
                visible: false
            }
        ],
        initComplete: function (settings, json) {
            SetPageLengthMenu();
            //----------conditional column hiding-------------//
            var api = new $.fn.dataTable.Api(settings);
            var columns = dtVendorSearch.settings().init().columns;
            var arr = [];
            var j = 0;
            while (j < json.hiddenColumnList.length) {
                var clsname = '.' + json.hiddenColumnList[j];
                var title = dtVendorSearch.columns(clsname).header();
                titleArray.push(title[0].innerHTML);
                classNameArray.push(clsname);
                dtVendorSearch.columns(clsname).visible(false);
                var sortMenuItem = '.dropdown-menu' + ' ' + clsname;
                $(sortMenuItem).remove();

                //---hide adv search items---
                var advclsname = '.' + "vnd-" + json.hiddenColumnList[j];
                $(document).find(advclsname).hide();
                j++;
            }
            //----------------------------------------------//

            $("#ActionGridBar :input").removeAttr("disabled");
            $("#ActionGridBar :button").removeClass("disabled");
            DisableExportButton($("#vendorSearch"), $(document).find('.import-export'));
        }
    });
}
$(document).on('click', '#vendorSearch_paginate .paginate_button', function () {
    run = true;
});
$(document).on('change', '#vendorSearch_length .searchdt-menu', function () {
    run = true;
});
$('#vendorSearch').find('th').click(function () {
    run = true;
    order = $(this).data('col');
});
function clearAdvanceSearch() {
    $('#advsearchsidebar').find('input:text').val('');
    $(document).find("#ddlType").val("").trigger('change.select2');
    $(document).find("#ddlTerm").val("").trigger('change.select2');
    $(document).find("#ddlFob").val("").trigger('change.select2');
    selectCount = 0;
    $("#spnControlCounter").text(selectCount);
    $('#liSelectCount').text(selectCount + ' filters applied');
    $('#dvFilterSearchSelect2').find('span').html('');
    $('#dvFilterSearchSelect2').find('span').removeClass('tagTo');
}

function AWBAdvSearch() {
    var searchitemhtml = "";
    $(document).find('#txtColumnSearch').val('');
    selectCount = 0;
    $('#advsearchsidebar').find('.adv-item').each(function (index, item) {
        if ($(this).val()) {
            selectCount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $("#dvFilterSearchSelect2").html(searchitemhtml);

    $("#spnControlCounter").text(selectCount);
}
$(document).on('click', '.lnk_vendorrow', function (e) {
    e.preventDefault();
    var row = $(this).parents('tr');
    var data = dtVendorSearch.row(row).data();
    var titletext = $('#vendorsearchtitle').text();
    localStorage.setItem("vendorstatustext", titletext);
    $.ajax({
        url: "/Vendor/VendorDetail",
        type: "POST",
        dataType: 'html',
        data: { vendorId: data.VendorId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#vendorsearchcontainer').html(data);
            $('#frmvendoeadd').removeData("validator");
            $('#frmvendoeadd').removeData("unobtrusiveValidation");
            $.validator.unobtrusive.parse('#frmvendoeadd');
            $(document).find('#spnlinkToSearch').text(localStorage.getItem("vendorstatustext"));
        },
        complete: function () {
            SetFixedHeadStyle();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(function () {
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            if ($(document).find('#vendorSearch').hasClass('dataTable')) {
                var valVendor = LRTrim($('#Vendor').val());
                var srcData = LRTrim($("#txtColumnSearch").val());
                var valName = LRTrim($('#Name').val());
                var valAddressCity = LRTrim($('#AddressCity').val());
                var valAddressState = LRTrim($('#AddressState').val());
                var vendorTypeData = $('#ddlType').val();
                var valTerms = LRTrim($('#ddlTerm').val());
                var valFobCode = LRTrim($('#ddlFob').val());
                var valInactiveFlag = activeStatus;
                var valIsExternal = LRTrim($("#isExternal").val());
                var colname = order;//Vendor Sorting
                var coldir = orderDir;//Vendor Sorting
                var jsonResult = $.ajax({
                    "url": "/Vendor/GetVendorPrintData",
                    "type": "get",
                    "datatype": "json",
                    data: {
                        _vendor: valVendor,
                        _name: valName,
                        _addresscity: valAddressCity,
                        _addressstate: valAddressState,
                        _type: vendorTypeData,
                        _terms: valTerms,
                        _fobcode: valFobCode,
                        _inactiveFlag: valInactiveFlag,
                        _isExternal: valIsExternal,
                        _srcData: srcData,
                        _colname: colname,
                        _coldir: coldir
                    },
                    success: function (result) {
                    },
                    async: false
                });
                var thisdata = JSON.parse(jsonResult.responseText).data;
                var visiblecolumnsIndex = $("#vendorSearch thead tr th").map(function (key) {
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
                    if (item.Name != null) {
                        item.Name = item.Name;
                    }
                    else {
                        item.Name = "";
                    }
                    if (item.AddressCity != null) {
                        item.AddressCity = item.AddressCity;
                    }
                    else {
                        item.AddressCity = "";
                    }
                    if (item.AddressState != null) {
                        item.AddressState = item.AddressState;
                    }
                    else {
                        item.AddressState = "";
                    }
                    if (item.Type != null) {
                        item.Type = item.Type;
                    }
                    else {
                        item.Type = "";
                    }
                    if (item.Terms != null) {
                        item.Terms = item.Terms;
                    }
                    else {
                        item.Terms = "";
                    }
                    if (item.FOBCode != null) {
                        item.FOBCode = item.FOBCode;
                    }
                    else {
                        item.FOBCode = "";
                    }
                    if (item.IsExternal != null) {
                        if (item.IsExternal == true) {
                            item.IsExternal = getResourceValue("CancelAlertYes");
                        }
                        else if (item.IsExternal == false) {
                            item.IsExternal = getResourceValue("CancelAlertNo");
                        }
                    }
                    else {
                        item.isExternal = "";
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
                    header: $("#vendorSearch thead tr th div").map(function (key) {
                        return this.innerHTML;
                    }).get()
                };
            }
            else {
                var vendorid = $(document).find('#vendors_VendorId').val();
                var colname = order;
                var coldir = orderDir;
                var jsonResult = $.ajax({
                    "url": "/Vendor/GetVendorPartsPrintData",
                    "type": "get",
                    "datatype": "json",
                    data: {
                        _vendorId: vendorid,
                        _colname: colname,
                        _coldir: coldir
                    },
                    success: function (result) {
                    },
                    async: false
                });
                var thisdata = JSON.parse(jsonResult.responseText).data;
                var visiblecolumnsIndex = $("#partsTable thead tr th").not(":eq(8)").map(function (key) {
                    return this.getAttribute('data-th-index');
                }).get();
                var d = [];
                $.each(thisdata, function (index, item) {
                    if (item.Part != null) {
                        item.Part = item.Part;
                    }
                    else {
                        item.Part = "";
                    }
                    if (item.PartDescription != null) {
                        item.PartDescription = item.PartDescription;
                    }
                    else {
                        item.PartDescription = "";
                    }
                    if (item.CatalogNumber != null) {
                        item.CatalogNumber = item.CatalogNumber;
                    }
                    else {
                        item.CatalogNumber = "";
                    }
                    if (item.Manufacturer != null) {
                        item.Manufacturer = item.Manufacturer;
                    }
                    else {
                        item.Manufacturer = "";
                    }
                    if (item.ManufacturerID != null) {
                        item.ManufacturerID = item.ManufacturerID;
                    }
                    else {
                        item.ManufacturerID = "";
                    }
                    if (item.OrderUnit != null) {
                        item.OrderUnit = item.OrderUnit;
                    }
                    else {
                        item.OrderUnit = "";
                    }
                    if (item.OrderQuantity != null) {
                        item.OrderQuantity = item.OrderQuantity;
                    }
                    else {
                        item.OrderQuantity = "";
                    }
                    if (item.Price != null) {
                        item.Price = item.Price;
                    }
                    else {
                        item.Price = "";
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
                    header: $("#partsTable thead tr th").not(":eq(8)").map(function (key) {
                            return this.innerHTML;
                    }).get()
                };

            }
        }
    });
})
//#endregion

//#region Commonn
$(document).ready(function () {
    $("#action").click(function () {
        $(".actionDrop").slideToggle();
    });
    $(".actionDrop ul li a").click(function () {
        $(".actionDrop").fadeOut();
    });
    $("#action").focusout(function () {
        $(".actionDrop").fadeOut();
    });
    $(document).find('.select2picker').select2({
    });
    $(".actionBar").fadeIn();
    $("#ActionGridBar :input").attr("disabled", "disabled");
});

$.validator.setDefaults({ ignore: null });
$(document).ready(function () {
    var dtContactTable;
    var dtNotesTable;
    var dtAttachmentTable;
    var dtPartsTable;
    var _ObjectId;
    $(document).on('click', "#btnSaveAnotherOpenVendor,#btnSaveVendor", function () {
        if ($(document).find("form").valid()) {
            return;
        }
        else {
            var activetagid = $('.vtabs li.active').attr('id');
            var errorTabId = $(document).find(".input-validation-error").parents('div:eq(0)').attr('id');
            if (errorTabId == "Details" && activetagid !=='vendordetailstab') {
                $('#vendordetailstab').trigger('click');
            }
            else if (errorTabId == "PunchOutSetup" && activetagid !== 'vendorpunchoutsetuptab') {
                $('#vendorpunchoutsetuptab').trigger('click');
            }
        }
    });
    $(document).on('click', "#btnvendorEdit", function () {
        if ($(document).find("form").valid()) {
            return;
        }
        else {
            var activetagid = $('.vtabs li.active').attr('id');
            var errorTabId = $(document).find(".input-validation-error").parents('div:eq(0)').attr('id');
            if (errorTabId == "Details" && activetagid !== 'vendordetailstabedit') {
                $('#vendordetailstabedit').trigger('click');
            }
            else if (errorTabId == "PunchOutSetup" && activetagid !== 'vendorpunchoutsetuptabedit') {
                $('#vendorpunchoutsetuptabedit').trigger('click');
            }
        }
    });
    $(document).on('change', '#colorselector', function (evt) {
        $('.tabsArea').hide();
        openCity(evt, $(this).val());
        $('#' + $(this).val()).show();
    });
    $("#sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $('#dismiss, .overlay').on('click', function () {
        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });
    $(document).on('click', "ul.vtabs li", function () {
        $("ul.vtabs li").removeClass("active");
        $(this).addClass("active");
        $(".tabsArea").hide();
        var activeTab = $(this).find("a").attr("href");
        $(activeTab).fadeIn();
        return false;
    });
    $(document).on('click', "#btneditcancel", function () {
        var vendorId = $(document).find('#vendors_VendorId').val();
        swal(CancelAlertSetting, function () {
            RedirectToVendorDetail(vendorId);
        });
    });
});
function openCity(evt, cityName) {
    evt.preventDefault();
    switch (cityName) {
        case "HideExport":
            $(document).find('.import-export').hide();
            break;
        case "Contact":
            GenerateContactGrid();
            $(document).find('.import-export').hide();
            break;
        case "Notes":
            GenerateNotesGrid();
            $(document).find('.import-export').hide();
            break;
        case "Attachment":
            GenerateAttachmentGrid();
            $(document).find('.import-export').hide();
            break;
        case "PartsContainer":
            GeneratePartsGrid();
            $(document).find('.import-export').show();
            break;
        case "Insurance":
            GetVendorInsuranceWidget();
            $(document).find('.import-export').hide();
        case "AssetManagement":
            GetVendorAssetManagementWidget();
            $(document).find('.import-export').hide();
            break;
    }
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent2");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(cityName).style.display = "block";
    evt.currentTarget.className += " active";
}

$(document).on('click', "#livendor", function () {
    $(document).find('#btndetails').addClass('active');
    $(document).find('#Details').show();
});

//#endregion

//#region Contact
function GenerateContactGrid() {
    var vendorid = $(document).find('#vendors_VendorId').val();
    if ($(document).find('#contactTable').hasClass('dataTable')) {
        dtContactTable.destroy();
    }
    dtContactTable = $("#contactTable").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        stateSave: false,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Vendor/PopulateContacts?_vendorId=" + vendorid,
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [
            {
                targets: [4], render: function (a, b, data, d) {
                    return '<a class="btn btn-outline-success editedVendorContact gridinnerbutton" title="Edit"> <i class="fa fa-pencil"></i></a>' +
                        '<a class="btn btn-outline-danger delVendorContact gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                }
            }
        ],
        "columns":
            [
                { "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Phone1", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Email1", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "OwnerName", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "VendorId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center"
                }
            ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', '.editedVendorContact', function () {
    var data = dtContactTable.row($(this).parents('tr')).data();
    var vendorClientLookupId = $(document).find('#vendors_ClientLookupId').val();
    EditVendorContact(data.VendorId, data.ContactId, data.updatedindex, vendorClientLookupId);
});
$(document).on('click', '.delVendorContact', function () {
    var data = dtContactTable.row($(this).parents('tr')).data();
    DeleteVendorContact(data.ContactId);
});
$(document).on('click', "#btnAddContact", function () {
    var vendorId = $(document).find('#vendors_VendorId').val();
    var vendorClientLookupId = $(document).find('#vendors_ClientLookupId').val();
    $.ajax({
        url: "/Vendor/AddContact",
        type: "GET",
        dataType: 'html',
        data: { vendorId: vendorId, vendorClientLookupId: vendorClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#vendorsearchcontainer').html(data);
        },
        complete: function () {
            CloseLoader();
            $.validator.setDefaults({ ignore: null });
            $.validator.unobtrusive.parse(document);
            $('input, form').blur(function () {
                $(this).valid();
            });
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', "#btncontactcancel", function () {
    var vendorId = $(document).find('#contactModel_VendorId').val();
    swal(CancelAlertSetting, function () {
        RedirectToVendorDetail(vendorId, "contact");
    });
});
function EditVendorContact(vendorid, contactid, updatedindex, vendorClientLookupId) {
    $.ajax({
        url: "/Vendor/EditContact",
        type: "GET",
        dataType: 'html',
        data: { vendorId: vendorid, contactId: contactid, updatedIndex: updatedindex, vendorClientLookupId: vendorClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#vendorsearchcontainer').html(data);
        },
        complete: function () {
            CloseLoader();
            $.validator.setDefaults({ ignore: null });
            $.validator.unobtrusive.parse(document);
            $('input, form').blur(function () {
                $(this).valid();
            });
        },
        error: function () {
            CloseLoader();
        }
    });
}
function DeleteVendorContact(contactid) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/Vendor/DeleteContacts',
            data: {
                _contactId: contactid
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    ShowDeleteAlert(getResourceValue("contactDeleteSuccessAlert"));
                    dtContactTable.state.clear();
                }
            },
            complete: function () {
                GenerateContactGrid();
                CloseLoader();
            }
        });
    });
}
function ContactAddOnSuccess(data) {
    if (data.Result == "success") {
        var message = "";
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("ContactAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("ContactUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToVendorDetail(data.vendorid, "contact");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
//#endregion

//#region Notes
function GenerateNotesGrid() {
    var vendorid = $(document).find('#vendors_VendorId').val();
    if ($(document).find('#notesTable').hasClass('dataTable')) {
        dtNotesTable.destroy();
    }
    dtNotesTable = $("#notesTable").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        stateSave: false,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Vendor/PopulateNotes?_vendorId=" + vendorid,
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [
            {
                targets: [3], render: function (a, b, data, d) {
                    return '<a class="btn btn-outline-success editedVendorNotes gridinnerbutton" title="Edit"> <i class="fa fa-pencil"></i></a>' +
                        '<a class="btn btn-outline-danger delVendorNotes gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                }
            }
        ],
        "columns":
            [
                { "data": "Subject", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "OwnerName", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "ModifiedDate", "type": "date" },
                { "data": "ObjectId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center" }
            ],
        initComplete: function () { SetPageLengthMenu(); }
    });
}
$(document).on('click', "#btnAddNote", function () {
    var vendorId = $(document).find('#vendors_VendorId').val();
    var vendorClientLookupId = $(document).find('#vendors_ClientLookupId').val();
    $.ajax({
        url: "/Vendor/AddNotes",
        type: "GET",
        dataType: 'html',
        data: { vendorId: vendorId, vendorClientLookupId: vendorClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#vendorsearchcontainer').html(data);
        },
        complete: function () {
            CloseLoader();
            $.validator.setDefaults({ ignore: null });
            $.validator.unobtrusive.parse(document);
            $('input, form').blur(function () {
                $(this).valid();
            });
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', '.editedVendorNotes', function () {
    var data = dtNotesTable.row($(this).parents('tr')).data();
    var vendorClientLookupId = $(document).find('#vendors_ClientLookupId').val();
    EditVendorNote(data.ObjectId, data.NotesId, data.updatedindex, vendorClientLookupId);
});
$(document).on('click', '.delVendorNotes', function () {
    var data = dtNotesTable.row($(this).parents('tr')).data();
    DeleteVendorNote(data.NotesId);
});
function NotesAddOnSuccess(data) {
    if (data.Result == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("AddNoteAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("UpdateNoteAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToVendorDetail(data.vendorid, "notes");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
function EditVendorNote(vendorid, notesid, updatedIndex, vendorClientLookupId) {
    $.ajax({
        url: "/Vendor/EditNotes",
        type: "GET",
        dataType: 'html',
        data: { _vendorId: vendorid, _notesId: notesid, _updatedIndex: updatedIndex, vendorClientLookupId: vendorClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#vendorsearchcontainer').html(data);
        },
        complete: function () {
            CloseLoader();
            $.validator.setDefaults({ ignore: null });
            $.validator.unobtrusive.parse(document);
            $('input, form').blur(function () {
                $(this).valid();
            });
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
function DeleteVendorNote(notesId) {
    var vendorId = $(document).find('#vendors_VendorId').val();
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/Vendor/DeleteNotes',
            data: {
                _notesId: notesId, vendorId: vendorId
            },
            beforeSend: function () {
                ShowLoader();
            },
            type: "POST",
            datatype: "json",
            success: function (data) {
                if (data.Result == "success") {
                    ShowDeleteAlert(getResourceValue("noteDeleteSuccessAlert"));
                    dtNotesTable.state.clear();
                }
            },
            complete: function () {
                GenerateNotesGrid();
                CloseLoader();
            }
        });
    });
}
$(document).on('click', "#btnnotescancel", function () {
    var vendorId = $(document).find('#notesModel_VendorId').val();
    swal(CancelAlertSetting, function () {
        RedirectToVendorDetail(vendorId, "notes");
    });
});
//#endregion

//#region Attachment
function GenerateAttachmentGrid() {
    var vendorid = $(document).find('#vendors_VendorId').val();
    if ($(document).find('#attachTable').hasClass('dataTable')) {
        dtAttachmentTable.destroy();
    }
    var visibility;
    var attchCount = 0;
    dtAttachmentTable = $("#attachTable").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        "bProcessing": true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bDeferRender": true,
        "order": [[0, "asc"]],
        stateSave: false,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Vendor/PopulateAttachment?_vendorId=" + vendorid,
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (response) {
                attchCount = response.recordsTotal;
                if (attchCount > 0) {
                    $(document).find('#vendorAttachmentCount').show();
                    $(document).find('#vendorAttachmentCount').html(attchCount);
                }
                else {
                    $(document).find('#vendorAttachmentCount').hide();
                }
                return response.data;
            }
        },
        columnDefs: [
            {
                targets: [5], render: function (a, b, data, d) {

                    return '<a class="btn btn-outline-danger delVendorAttachment gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                }
            }
        ],
        "columns":
            [
                { "data": "Subject", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "FileName",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<a class=lnk_sensor_1 href="' + '/Vendor/DownloadAttachment?_fileinfoId=' + row.FileInfoId + '"  target="_blank">' + row.FullName + '</a>'
                    }
                },
                { "data": "FileSizeWithUnit", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "OwnerName", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "CreateDate", "type": "date " },
                {
                    "data": "ObjectId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center"
                }
            ],
        initComplete: function () {
            SetPageLengthMenu();
            if (visibility == "False") {
                var column = this.api().column(5);
                column.visible(false);
            }
            else {
                var column = this.api().column(5);
                column.visible(true);
            }
        }
    });
}
$(document).on('click', '.lnk_sensor_1', function (e) {
    e.preventDefault();
    var row = $(this).parents('tr');
    var data = dtAttachmentTable.row(row).data();
    var FileAttachmentId = data.FileAttachmentId;
    $.ajax({
        type: "post",
        url: '/Base/IsOnpremiseCredentialValid',
        success: function (data) {
            if (data === true) {
                window.location = '/Vendor/DownloadAttachment?_fileinfoId=' + FileAttachmentId;
            }
            else {
                ShowErrorAlert(getResourceValue("NotAuthorisedDownloadFileAlert"));
            }
        }

    });

});
$(document).on('click', '.delVendorAttachment', function () {
    var data = dtAttachmentTable.row($(this).parents('tr')).data();
    DeleteVendorAttachment(data.FileAttachmentId);
});
function DeleteVendorAttachment(fileAttachmentId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/Vendor/DeleteAttachment',
            data: {
                _fileAttachmentId: fileAttachmentId
            },
            beforeSend: function () {
                ShowLoader();
            },
            type: "POST",
            datatype: "json",
            success: function (data) {
                if (data.Result == "success") {
                    dtAttachmentTable.state.clear();
                    ShowDeleteAlert(getResourceValue("attachmentDeleteSuccessAlert"));
                }
                else {
                    ShowErrorAlert(data.Message);
                }
            },
            complete: function () {
                GenerateAttachmentGrid();
                CloseLoader();
            }
        });
    });
}
$(document).on('click', "#btnAddAttachment", function () {
    var vendorId = $(document).find('#vendors_VendorId').val();
    var vendorClientLookupId = $(document).find('#vendors_ClientLookupId').val();
    $.ajax({
        url: "/Vendor/AddAttachment",
        type: "GET",
        dataType: 'html',
        data: { vendorId: vendorId, vendorClientLookupId: vendorClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#vendorsearchcontainer').html(data);
        },
        complete: function () {
            CloseLoader();
            $.validator.setDefaults({ ignore: null });
            $.validator.unobtrusive.parse(document);
            $('input, form').blur(function () {
                $(this).valid();
            });
        },
        error: function () {
            CloseLoader();
        }
    });
});
function AttachmentAddOnSuccess(data) {
    if (data.Result == "success") {
        RedirectToVendorDetail(data.vendorid, "attachment");
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', "#btnattachmentcancel", function () {
    var vendorId = $(document).find('#attachmentModel_VendorId').val();
    swal(CancelAlertSetting, function () {
        RedirectToVendorDetail(vendorId, "attachment");
    });
});
//#endregion

//#region Parts
function GeneratePartsGrid() {
    var vendorid = $(document).find('#vendors_VendorId').val();
    var visibility = Sec_Part_Vendor_XRef;
    var rCount = 0;
    if ($(document).find('#partsTable').hasClass('dataTable')) {
        dtPartsTable.destroy();
    }
    dtPartsTable = $("#partsTable").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        stateSave: false,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'Vendors Parts'
            },
            {
                extend: 'print',
                title: 'Vendors Parts'
            },
            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Vendors Parts',
                extension: '.csv',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                orientation: 'portrait',
                pageSize: 'A4',
                title: 'Vendors Parts'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/Vendor/PopulateParts?_vendorId=" + vendorid,
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (json) {
                let colOrder = dtPartsTable.order();
                orderDir = colOrder[0][1];
                order = colOrder[0][0];
                rCount = json.data.length;
                if (json.data.length == "0") {
                    $(document).find('.import-export').attr('disabled', 'disabled');
                }
                else {
                    $(document).find('.import-export').removeAttr('disabled');
                }
                return json.data;
            }
        },
        columnDefs: [
            {
                targets: [8], render: function (a, b, data, d) {
                    return '<a class="btn btn-outline-primary addVendorParts gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                        '<a class="btn btn-outline-success editedVendorParts gridinnerbutton" title="Edit"> <i class="fa fa-pencil"></i></a>' +
                        '<a class="btn btn-outline-danger delVendorParts gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                }
            }
        ],
        "columns":
            [
                {
                    "data": "Part", "autoWidth": false, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<a class=lnk_part_detail href="javascript:void(0)">' + data + '</a>';
                    }
                },
                {
                    "data": "PartDescription", "autoWidth": false, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-100'>" + data + "</div>";
                    }
                },
                { "data": "CatalogNumber", "autoWidth": false, "bSearchable": true, "bSortable": true },
                { "data": "Manufacturer", "autoWidth": false, "bSearchable": true, "bSortable": true },
                { "data": "ManufacturerID", "autoWidth": false, "bSearchable": true, "bSortable": true },
                { "data": "OrderQuantity", "autoWidth": false, "bSearchable": true, "bSortable": true },
                { "data": "OrderUnit", "autoWidth": false, "bSearchable": true, "bSortable": true },
                { "data": "Price", "autoWidth": false, "bSearchable": true, "bSortable": true },
                {
                    "data": "ObjectId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center"

                }
            ],
        initComplete: function () {
            var column = this.api().column(8);
            SetPageLengthMenu();
            if (visibility == "False") {
                column.visible(false);
            }
            else {
                column.visible(true);
            }
            if (rCount > 0 || visibility == "False") {
                $("#btnAddParts").hide();
            }
            else {
                $("#btnAddParts").show();
            }
            DisableExportButton($("#partsTable"), $(document).find('.import-export'));
        }
    });
}

$(document).on('click', '.lnk_part_detail', function (e) {
    var row = $(this).parents('tr');
    var data = dtPartsTable.row(row).data();
    var partId = data.PartID;
    clearDropzone();
    window.location.href = "../Parts/PartsDetailFromVendor?partId=" + partId;
});

$(document).on('click', '.addVendorParts', function () {
    var data = dtPartsTable.row($(this).parents('tr')).data();
    AddParts(data.VendorId, data.VendorClientLookupId);
});
$(document).on('click', '.editedVendorParts', function () {
    var data = dtPartsTable.row($(this).parents('tr')).data();
    EditVendorParts(data.VendorId, data.PartVendorXrefId, data.updatedindex);
});
$(document).on('click', '.delVendorParts', function () {
    var data = dtPartsTable.row($(this).parents('tr')).data();
    DeleteVendorParts(data.PartVendorXrefId);
});
function EditVendorParts(VendorId, _PartVendorXrefId, updatedIndex) {
    $.ajax({
        url: "/Vendor/PartsEdit",
        type: "GET",
        dataType: 'html',
        data: { vendorId: VendorId, PartVendorXrefId: _PartVendorXrefId, updatedIndex: updatedIndex },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#vendorsearchcontainer').html(data);
        },
        complete: function () {
            CloseLoader();
            $.validator.setDefaults({ ignore: null });
            $.validator.unobtrusive.parse(document);
            $(document).find('.select2picker').select2({
            });
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
        },
        error: function () {
            CloseLoader();
        }
    });
}
function DeleteVendorParts(part_Vendor_XrefId) {
    var vendorId = $(document).find('#vendors_VendorId').val();
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/Vendor/PartsDelete',
            data: {
                _PartVendorXrefId: part_Vendor_XrefId, vendorId: vendorId
            },
            type: "POST",
            beforeSend: function () {
                ShowLoader();
            },
            datatype: "json",
            success: function (data) {
                if (data.Result == "success") {
                    ShowDeleteAlert(getResourceValue("partDeleteSuccessAlert"));
                    dtPartsTable.state.clear();
                }
            },
            complete: function () {
                GeneratePartsGrid();
                CloseLoader();
            }
        });
    });
}
$(document).on('click', "#btnAddParts", function () {
    var vendorId = $(document).find('#vendors_VendorId').val();
    var vendorClientLookupId = $(document).find('#vendors_ClientLookupId').val();
    AddParts(vendorId, vendorClientLookupId);
});
function AddParts(vendorId, ClientLookupId) {
    $.ajax({
        url: "/Vendor/PartsAdd",
        type: "GET",
        dataType: 'html',
        data: { vendorId: vendorId, ClientLookupId: ClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#vendorsearchcontainer').html(data);
        },
        complete: function () {
            CloseLoader();
            $.validator.setDefaults({ ignore: null });
            $.validator.unobtrusive.parse(document);
            $(document).find('.select2picker').select2({
            });
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
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', "#btnpartscancel", function () {
    var vendorId = $(document).find('#partVendorXrefModel_VendorId').val();
    swal(CancelAlertSetting, function () {
        RedirectToVendorDetail(vendorId, "parts");
    });
});
function PartsAddOnSuccess(data) {
    if (data.Result == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("PartAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("PartUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToVendorDetail(data.vendorid, "parts");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', "#opengrid", function () {
    generatePartsXrefDataTable();
});
//#endregion

//#region Add-Edit Vendor
function VendorEditOnSuccess(data) {
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("VendorUpdateAlert");
        swal(SuccessAlertSetting, function () {
            $.ajax({
                url: "/Vendor/VendorDetail",
                type: "POST",
                dataType: 'html',
                data: { vendorId: data.vendorid },
                beforeSend: function () {
                    ShowLoader();
                },
                success: function (data) {
                    $('#vendorsearchcontainer').html(data);
                    $(document).find('#spnlinkToSearch').text(localStorage.getItem("vendorstatustext"));
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
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}

//#region V2-375
function ShowAddVendor() {
    $.ajax({
        url: "/Vendor/ShowAddVendorDynamic", /*"/Vendor/ShowAddVendor"*/ /*implementation of V2-642*/
        type: "GET",
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#vendorsearchcontainer').html(data);
        },
        complete: function () {
            CloseLoader();
            $.validator.setDefaults({ ignore: null });
            $.validator.unobtrusive.parse(document);
            $(document).find('.select2picker').select2({

            });
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
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', '.addvendor', function () {
    var v1 = $(document).find('#usevendormaster').val();
    var v2 = $(document).find('#vendormasterallowlocal').val();
    if (v1 == "False") {
        ShowAddVendor();
    }
    else if (v1 == "True" && v2 == "False") {
        $(document).find("#spnvendormasteradd").trigger("click");
    }
    else if (v1 == "True" && v2 == "True") {
        $.ajax({
            url: '/Vendor/ShowAddVendorPopup',
            type: "GET",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                $(document).find('#AddVendorPopupModal').modal('show');
            },
            complete: function () {
                CloseLoader();
            }
        });
    }
});
$(document).on('click', '#SelectFromVenMast', function () {
    $(document).find('#AddVendorPopupModal').modal('hide');
    VMPopupGrid();
});
$(document).on('click', '#AddLocalVend', function () {
    $(document).find('#AddVendorPopupModal').modal('hide');
    ShowAddVendor();
});
$(document).on('change', '#chkIsBusiness', function () {
    if (this.checked) {
        $(document).find('#vendors_RemitAddress1').val($('#vendors_Address1').val());
        $(document).find('#vendors_RemitAddress2').val($('#vendors_Address2').val());
        $(document).find('#vendors_RemitAddress3').val($('#vendors_Address3').val());
        $(document).find('#vendors_RemitAddressCity').val($('#vendors_AddressCity').val());
        $(document).find('#vendors_RemitAddressState').val($('#vendors_AddressState').val());
        $(document).find('#vendors_RemitPostalCode').val($('#vendors_PostalCode').val());
        $(document).find('#vendors_RemitCountry').val($('#vendors_Country').val());
        $(document).find('#vendors_RemitAddress1').attr('readonly', true);
        $(document).find('#vendors_RemitAddress2').attr('readonly', true);
        $(document).find('#vendors_RemitAddress3').attr('readonly', true);
        $(document).find('#vendors_RemitAddressCity').attr('readonly', true);
        $(document).find('#vendors_RemitAddressState').attr('readonly', true);
        $(document).find('#vendors_RemitPostalCode').attr('readonly', true);
        $(document).find('#vendors_RemitCountry').attr('readonly', true);
    }
    else {
        $(document).find('#vendors_RemitAddress1').val('');
        $(document).find('#vendors_RemitAddress2').val('');
        $(document).find('#vendors_RemitAddress3').val('');
        $(document).find('#vendors_RemitAddressCity').val('');
        $(document).find('#vendors_RemitAddressState').val('');
        $(document).find('#vendors_RemitPostalCode').val('');
        $(document).find('#vendors_RemitCountry').val('');
        $(document).find('#vendors_RemitAddress1').attr('readonly', false);
        $(document).find('#vendors_RemitAddress2').attr('readonly', false);
        $(document).find('#vendors_RemitAddress3').attr('readonly', false);
        $(document).find('#vendors_RemitAddressCity').attr('readonly', false);
        $(document).find('#vendors_RemitAddressState').attr('readonly', false);
        $(document).find('#vendors_RemitPostalCode').attr('readonly', false);
        $(document).find('#vendors_RemitCountry').attr('readonly', false);
    }
});
//#endregion

//#region Commonn
function RedirectToVendorDetail(vendorId, mode) {
    $.ajax({
        url: "/Vendor/VendorDetail",
        type: "POST",
        dataType: 'html',
        data: { vendorId: vendorId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#vendorsearchcontainer').html(data);
            $(document).find('#spnlinkToSearch').text(localStorage.getItem("vendorstatustext"));
        },
        complete: function () {
            SetFixedHeadStyle();
            CloseLoader();
            if (mode === "contact") {
                $('#licontact').trigger('click');
                $('#colorselector').val('Contact');
                GenerateContactGrid();
            }
            if (mode === "notes") {
                $('#linotes').trigger('click');
                $('#colorselector').val('Notes');
                GenerateNotesGrid();
            }
            if (mode === "parts") {
                $('#liparts').trigger('click');
                $(document).find('.import-export').show();
                $('#colorselector').val('PartsContainer');
                GeneratePartsGrid();
            }
            if (mode === "attachment") {
                $('#liattachment').trigger('click');
                $('#colorselector').val('Attachment');
                GenerateAttachmentGrid();
            }
            if (mode === "insurance") {
                $('#liinsurance').trigger('click');
                $('#colorselector').val('Insurance');
                GetVendorInsuranceWidget();
            }
            if (mode === "assetmanagement") {
                $('#liassetmanagement').trigger('click');
                $('#colorselector').val('AssetManagement');
                GetVendorAssetManagementWidget();
            }
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', "#brdvendor", function () {
    var vendorId = $(this).attr('data-val');
    RedirectToVendorDetail(vendorId);
});
//#endregion


//#region liCustomize1
$(document).on('click', '#liCustomize', function () {
    funCustomizeBtnClick(dtVendorSearch, true, titleArray);
    //funVendCustomizeBtnClick(dtVendorSearch, null, titleArray);
});
var colOrder = [0];
$(document).on('click', '.saveConfig', function () {
    var colOrder = [0];
    funCustozeSaveBtn(dtVendorSearch, colOrder);
    run = true;
    dtVendorSearch.state.save(run);
    if (classNameArray != null && classNameArray.length > 0) {
        var j = 0;
        while (j < classNameArray.length) {
            dtVendorSearch.columns(classNameArray[j]).visible(false);
            j++;
        }
    }
});
//#endregion

//#region ActivateInActivate
$(document).on('click', '#actinctivatevendor', function () {
    var vendorId = $('#vendors_VendorId').val();
    var clientLookupId = $(document).find('#vendors_ClientLookupId').val();
    var InActiveFlag = $(document).find('#vendorhiddeninactiveflag').val();
    $.ajax({
        url: "/Vendor/ValidateForActiveInactive",
        type: "POST",
        dataType: "json",
        data: { InActiveFlag: InActiveFlag, VendorId: vendorId, ClientLookupId: clientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.validationStatus == true) {
                if (InActiveFlag == "True") {
                    CancelAlertSetting.text = getResourceValue("ActivateVendorAlert");
                }
                else {
                    CancelAlertSetting.text = getResourceValue("InActivateVendorAlert");
                }
                swal(CancelAlertSetting, function (isConfirm) {
                    if (isConfirm == true) {
                        $.ajax({
                            url: "/Vendor/MakeActiveInactive",
                            type: "POST",
                            dataType: "json",
                            data: { InActiveFlag: InActiveFlag, VendorId: vendorId },
                            beforeSend: function () {
                                ShowLoader();
                            },
                            success: function (data) {
                                if (data.Result == 'success') {
                                    if (InActiveFlag == "True") {
                                        SuccessAlertSetting.text = getResourceValue("VendorActiveSuccessAlert");
                                        //localStorage.setItem("VENDORSEARCHGRIDDISPLAYSTATUS", "1");
                                        //$(document).find('#spnlinkToSearch').text(getResourceValue("ActiveVendorsAlert"));
                                    }
                                    else {
                                        SuccessAlertSetting.text = getResourceValue("VendorInActiveSuccessAlert");
                                        //localStorage.setItem("VENDORSEARCHGRIDDISPLAYSTATUS", "2");
                                        //$(document).find('#spnlinkToSearch').text(getResourceValue("InactiveVendorsAlert"));
                                    }
                                    swal(SuccessAlertSetting, function () {
                                        //window.location.href = "../Vendor/Index?page=Inventory_Vendors";
                                        RedirectToVendorDetail(vendorId);
                                    });
                                }
                                else {
                                    ShowGenericErrorOnAddUpdate(data);
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
            }
            else {
                GenericSweetAlertMethod(data);
            }
        },
        complete: function () {
            CloseLoader();
        },
        error: function (jqxhr) {
            CloseLoader();
        }
    });
});
//#endregion

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
                    searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + item.key + '">' + $("label[for='" + item.key + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
                }
            }
            advcountercontainer.text(selectCount);
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    searchstringcontainer.html(searchitemhtml);

}
//#endregion

//#region Change Vendor Id V2-404
$(document).on('click', '#optchangevendorid', function (e) {
    var NewClientLookupId = $(document).find('#OldClientLookupId').val();
    $(document).find('#NewClientLookupId').val(NewClientLookupId).removeClass('input-validation-error');
    $('#changeVendorIDModalDetailsPage').modal('show');
    $.validator.unobtrusive.parse(document);
    $(this).blur();
});
function ChangeVendorIdSuccess(data) {
    $('#changeVendorIDModalDetailsPage').modal('hide');
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("VendorUpdateAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToVendorDetail($('#vendors_VendorId').val(), "vendor");
        });
    }
    else {
        ShowErrorAlert(data);
    }
}
//#endregion

//#region Punchoutsupport
$(document).on('click', '#openpunchoutmodal', function (e) {
    $('#vendorpunchoutsetupmodal').modal('show');
    $('#vendorpunchoutsetupmodal').find('.select2picker').select2({});
    $.validator.unobtrusive.parse(document);
    $(this).blur();
});
function PunchOutSetupSuccess(data) {
    if (data.Result == "success") {
        SuccessAlertSetting.text = "Punch out setup updated successfully.";
        swal(SuccessAlertSetting, function () {
            $('#vendorpunchoutsetupmodal').modal('hide');
            RedirectToVendorDetail(data.vendorid);
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
//#endregion
//#region V2-642 Edit success
$(document).on('click', "#editcanceldynamic", function () {
    var vendorId = $(document).find('#EditVendor_VendorId').val();
    swal(CancelAlertSetting, function () {
        RedirectToVendorDetail(vendorId);
    });
});
function VendorEditDynamicOnSuccess(data) {
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("VendorUpdateAlert");
        swal(SuccessAlertSetting, function () {
            $.ajax({
                url: "/Vendor/VendorDetail",
                type: "POST",
                dataType: 'html',
                data: { vendorId: data.vendorid },
                beforeSend: function () {
                    ShowLoader();
                },
                success: function (data) {
                    $('#vendorsearchcontainer').html(data);
                    $(document).find('#spnlinkToSearch').text(localStorage.getItem("vendorstatustext"));
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
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
//#endregion

//#region Email Configuration V2-750
$(document).on('click', '#openemailconfigurationmodal', function (e) {
    var NewEmail = $(document).find('#OldEmail').val();
    $(document).find('#NewEmail').val(NewEmail).removeClass('input-validation-error');
    var NewAutoEmailPO = $(document).find('#OldAutoEmailPO').val();
    console.log(NewAutoEmailPO);
    if (NewAutoEmailPO === "True") {
        $('#NewAutoEmailPO').prop('checked', true);
    }
    else {
        $('#NewAutoEmailPO').prop('checked', false);
    }
    //$('#NewAutoEmailPO').checked(NewAutoEmailPO);
    //$('#NewAutoEmailPO').prop('checked', NewAutoEmailPO);
    //$(document).find('#NewAutoEmailPO').val(NewAutoEmailPO).removeClass('input-validation-error');
    $('#vendoremailconfigurationsetupmodal').modal('show');
    $('#vendoremailconfigurationsetupmodal').find('.select2picker').select2({});
    $.validator.unobtrusive.parse(document);
    $(this).blur();
});
function EmailConfigurationSetupSuccess(data) {
    $('#vendoremailconfigurationsetupmodal').modal('hide');
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("EmailConfigurationUpdatedAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToVendorDetail(data.vendorid);
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
//#endregion
//#region V2-853 Reset Grid
$('#liResetGridClearBtn').click(function (e) {
    CancelAlertSetting.text = getResourceValue("ResetGridAlertMessage");
    swal(CancelAlertSetting, function () {
        var localstorageKeys = [];
        localstorageKeys.push("VENDORSEARCHGRIDDISPLAYSTATUS");
        localstorageKeys.push("vendorstatustext");
        DeleteGridLayout('Vendor_Search', dtVendorSearch, localstorageKeys);
        GenerateSearchList('', true);
        window.location.href = "../Vendor/Index?page=Inventory_Vendors";
    });
});
//#endregion
//#region V2-929
//#region VendorInsurance Grid
function GenerateVendorInsuranceGrid() {
    var vendorid = $(document).find('#vendors_VendorId').val();
    var visibility;
    var rCount = 0;
    if ($(document).find('#vendorInsuranceTable').hasClass('dataTable')) {
        dtVendorInsuranceTable.destroy();
    }
    dtVendorInsuranceTable = $("#vendorInsuranceTable").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        stateSave: false,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Vendor/PopulateVendorInsurance?_vendorId=" + vendorid,
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (json) {
                let colOrder = dtVendorInsuranceTable.order();
                orderDir = colOrder[0][1];
                order = colOrder[0][0];
                rCount = json.data.length;
                return json.data;
            }
        },
        columnDefs: [
            {
                targets: [4], render: function (a, b, data, d) {
                    var hdnvendorinsuranceaddsecurity = $(document).find('#hdnvendorinsuranceaddsecurity').val();
                    var hdnvendorinsuranceeditsecurity = $(document).find('#hdnvendorinsuranceeditsecurity').val();
                    var hdnvendorinsurancedelsecurity = $(document).find('#hdnvendorinsurancedelsecurity').val();
                    var actiontags = '';
                    if (hdnvendorinsuranceaddsecurity == 'True') {
                        actiontags = actiontags + '<a class="btn btn-outline-primary addVendorInsurance gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>';
                    }
                    if (hdnvendorinsuranceeditsecurity == 'True') {
                        actiontags = actiontags + '<a class="btn btn-outline-success editedVendorInsurance gridinnerbutton" title="Edit"> <i class="fa fa-pencil"></i></a>';
                    }
                    if (data.VendorInsuranceId != data.Vendor_InsuranceSource && hdnvendorinsurancedelsecurity == 'True') {
                        actiontags = actiontags + '<a class="btn btn-outline-danger delVendorInsurance gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                    return actiontags;
                }
            }
        ],
        "columns":
            [
                {
                    "data": "Company", "autoWidth": false, "bSearchable": true, "bSortable": true
                },
                {
                    "data": "Contact", "autoWidth": false, "bSearchable": true, "bSortable": true
                },
                { "data": "ExpireDate", "autoWidth": false, "bSearchable": true, "bSortable": true, "type": "date" },
                { "data": "Amount", "autoWidth": false, "bSearchable": true, "bSortable": true },
                {
                    "data": "VendorInsuranceId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center"

                }
            ],
        initComplete: function () {
            SetPageLengthMenu();
            if (rCount > 0) {
                $("#btnAddVendorInsurance").hide();
            }
            else {
                $("#btnAddVendorInsurance").show();
            }
        }
    });
}
$(document).on('click', ".addVendorInsurance", function (e) {
    e.preventDefault();
    var vendorid = $(document).find('#vendors_VendorId').val();
    GoToAddVendorInsurance(vendorid);
});

function GoToAddVendorInsurance(vendorid) {
    $.ajax({
        url: "/Vendor/AddVendorInsurance",
        type: "GET",
        dataType: 'html',
        data: { VendorId: vendorid },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#VendorInsurancePopUp').html(data);
            $('#AddVendorInsuranceModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            SetControls();
            CloseLoader();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
$(document).on('click', ".editedVendorInsurance", function (e) {
    e.preventDefault();
    var data = dtVendorInsuranceTable.row($(this).parents('tr')).data();
    var vendorid = $(document).find('#vendors_VendorId').val();
    GoToEditVendorInsurance(data.VendorInsuranceId, data.Vendor_InsuranceSource,vendorid);
});

function GoToEditVendorInsurance(VendorInsuranceId, Vendor_InsuranceSource,vendorid) {
    $.ajax({
        url: "/Vendor/EditVendorInsurance",
        type: "GET",
        dataType: 'html',
        data: { VendorInsuranceId: VendorInsuranceId, Vendor_InsuranceSource: Vendor_InsuranceSource, VendorId: vendorid },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#VendorInsurancePopUp').html(data);
            $('#AddVendorInsuranceModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            SetControls();
            CloseLoader();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}

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
    $(document).find('.dtpicker').datepicker({
        changeMonth: true,
        changeYear: true,
        beforeShow: function (i) { if ($(i).attr('readonly')) { return false; } },
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy');
    SetFixedHeadStyle();
};
$(document).on('click', ".btnCancelInsurance", function () {
    $(document).find('#AddVendorInsuranceModalpopup').modal("hide");
});

function AddVendorInsuranceOnSuccess(data) {
    CloseLoader();
    var VendorId = $(document).find('#vendors_VendorId').val();
    if (data.Result == "success") {
        $(document).find('#AddVendorInsuranceModalpopup').modal("hide");
        if (data.Mode == 'add') {
            SuccessAlertSetting.text = getResourceValue("VendorInsuranceAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("VendorInsuranceEditAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToVendorDetail(VendorId, "insurance");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}

$(document).on('click', '.delVendorInsurance', function () {
    var data = dtVendorInsuranceTable.row($(this).parents('tr')).data();
    DeleteVendorInsurance(data.VendorInsuranceId);
});
function DeleteVendorInsurance(VendorInsuranceId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/Vendor/VendorInsuranceDelete',
            data: {
                VendorInsuranceId: VendorInsuranceId
            },
            type: "POST",
            beforeSend: function () {
                ShowLoader();
            },
            datatype: "json",
            success: function (data) {
                if (data.Result == "success") {
                    ShowDeleteAlert(getResourceValue("VendorInsuranceDeleteAlert"));
                    dtVendorInsuranceTable.state.clear();
                }
            },
            complete: function () {
                GenerateVendorInsuranceGrid();
                CloseLoader();
            }
        });
    });
}
//#endregion
//#region Vendor Insurance Widget
function GetVendorInsuranceWidget() {
    var vendorid = $(document).find('#vendors_VendorId').val();
    $.ajax({
        url: "/Vendor/VendorInsuranceWidgetDetails",
        type: "GET",
        dataType: 'html',
        data: { VendorId: vendorid },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#VendorInsuranceWidget').html(data);
        },
        complete: function () {
            GenerateVendorInsuranceGrid();
            SetControls();
            CloseLoader();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
$(document).on('click', '#overridevendorinsurance', function () {
    var vendorid = $(document).find('#vendors_VendorId').val();
    UpdateVendorInsuranceWdget(vendorid, true);
});
$(document).on('click', '#removeoverridevendorinsurance', function () {
    var vendorid = $(document).find('#vendors_VendorId').val();
    UpdateVendorInsuranceWdget(vendorid, false);
});
function UpdateVendorInsuranceWdget(VendorId, InsuranceOverride) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/Vendor/UpdateVendorInsuranceWdget',
            data: {
                VendorId: VendorId, InsuranceOverride: InsuranceOverride
            },
            type: "POST",
            beforeSend: function () {
                ShowLoader();
            },
            datatype: "json",
            success: function (data) {
                if (data.Result == "success") {
                    if (InsuranceOverride == true) {
                        SuccessAlertSetting.text = getResourceValue("VendorInsuranceOverrideAlert");
                    }
                    else {
                        SuccessAlertSetting.text = getResourceValue("VendorInsuranceRemoveOverrideAlert");
                    }
                    swal(SuccessAlertSetting, function () {
                        RedirectToVendorDetail(VendorId, "insurance");
                    });
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}

$(document).on('click', "#editvendorinsurance", function () {
    var vendorId = $('#vendors_VendorId').val();
    $.ajax({
        url: "/Vendor/EditVendorInsuranceInfo",
        type: "POST",
        dataType: 'html',
        data: { vendorId: vendorId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#vendorsearchcontainer').empty().html(data);
        },
        complete: function () {
            CloseLoader();
            SetControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
function UpdateVendorInsuranceInfoOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("VendorInfoUpdateAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToVendorDetail(data.vendorid, "insurance");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', "#btnCancelVIInfo,#brdvendorinfo", function () {
    swal(CancelAlertSetting, function () {
        var vendorid = $(document).find('#VendorInsuranceWidgetModel_VendorId').val();
        RedirectToVendorDetail(vendorid, "insurance");
    });
});
//#endregion

//#endregion

//#region V2-933
var dtVendorAssetManagementTable;
//#region VendorAssetManagement Grid
function GenerateVendorAssetManagementGrid() {
    var vendorid = $(document).find('#vendors_VendorId').val();
    var visibility;
    var rCount = 0;
    if ($(document).find('#vendorAssetManagementTable').hasClass('dataTable')) {
        dtVendorAssetManagementTable.destroy();
    }
    dtVendorAssetManagementTable = $("#vendorAssetManagementTable").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        stateSave: false,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Vendor/PopulateVendorAssetManagement?_vendorId=" + vendorid,
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (json) {
                let colOrder = dtVendorAssetManagementTable.order();
                orderDir = colOrder[0][1];
                order = colOrder[0][0];
                rCount = json.data.length;
                return json.data;
            }
        },
        columnDefs: [
            {
                targets: [4], render: function (a, b, data, d) {
                    var hdnvendorassetmgtaddsecurity = $(document).find('#hdnvendorassetmgtaddsecurity').val();
                    var hdnvendorassetmgteditsecurity = $(document).find('#hdnvendorassetmgteditsecurity').val();
                    var hdnvendorassetmgtdelsecurity = $(document).find('#hdnvendorassetmgtdelsecurity').val();
                    var actiontags = '';
                    if (hdnvendorassetmgtaddsecurity == 'True') {
                        actiontags = actiontags + '<a class="btn btn-outline-primary addVendorAssetManagement gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>';
                    }
                    if (hdnvendorassetmgteditsecurity == 'True') {
                        actiontags = actiontags + '<a class="btn btn-outline-success editedVendorassetmgt gridinnerbutton" title="Edit"> <i class="fa fa-pencil"></i></a>';
                    }
                    if (data.VendorAssetMgtId != data.AssetMgtSource && hdnvendorassetmgtdelsecurity == 'True') {
                        actiontags = actiontags + '<a class="btn btn-outline-danger delVendorassetmgt gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                    return actiontags;
                }
            }
        ],
        "columns":
            [
                {
                    "data": "Company", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "0"
                },
                {
                    "data": "Contact", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1"
                },
                {
                    "data": "Contract", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2"
                },
                {
                    "data": "ExpireDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3", "type": "date"
                },
                {
                    "data": "VendorAssetMgtId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center"

                }
            ],
        initComplete: function () {
            SetPageLengthMenu();
            if (rCount > 0) {
                $("#btnAddVendorAssetManagement").hide();
            }
            else {
                $("#btnAddVendorAssetManagement").show();
            }
        }
    });
}


//#endregion

//#region Vendor Asset Management Widget
function GetVendorAssetManagementWidget() {
    var vendorid = $(document).find('#vendors_VendorId').val();
    $.ajax({
        url: "/Vendor/VendorAssetManagementWidgetDetails",
        type: "GET",
        dataType: 'html',
        data: { VendorId: vendorid },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#VendorAssetManagementWidget').html(data);
        },
        complete: function () {
            GenerateVendorAssetManagementGrid();
            SetControls();
            CloseLoader();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}

//#endregion

//#endregion
//#region addVendorAssetManagement

$(document).on('click', ".addVendorAssetManagement", function (e) {
    e.preventDefault();
    var vendorid = $(document).find('#vendors_VendorId').val();
    GoToAddVendorAssetManagement(vendorid);
});

function GoToAddVendorAssetManagement(vendorid) {
    $.ajax({
        url: "/Vendor/AddVendorAssetManagement",
        type: "GET",
        dataType: 'html',
        data: { VendorId: vendorid },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#VendorAssetMgtPopUp').html(data);
            $('#AddVendorAssetMgtModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            SetControls();
            CloseLoader();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
$(document).on('click', ".editedVendorassetmgt", function (e) {
    e.preventDefault();
    var data = dtVendorAssetManagementTable.row($(this).parents('tr')).data();
    var vendorid = $(document).find('#vendors_VendorId').val();
    GoToEditVendorAssetManagement(data.VendorAssetMgtId, data.AssetMgtSource, vendorid);
});

function GoToEditVendorAssetManagement(VendorAssetMgtId, AssetMgtSource, vendorid) {
    $.ajax({
        url: "/Vendor/EditVendorAssetMgt",
        type: "GET",
        dataType: 'html',
        data: { VendorAssetMgtId: VendorAssetMgtId, AssetMgtSource: AssetMgtSource, VendorId: vendorid },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#VendorAssetMgtPopUp').html(data);
            $('#AddVendorAssetMgtModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
          },
        complete: function () {
            SetControls();
            CloseLoader();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}

function AddVendorAssetManagementOnSuccess(data) {
    CloseLoader();
    var VendorId = $(document).find('#vendors_VendorId').val();
    if (data.Result == "success") {
        $(document).find('#AddVendorAssetMgtModalpopup').modal("hide");
        if (data.Mode == 'add') {
            SuccessAlertSetting.text = getResourceValue("VendorAssetMgtAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("VendorAssetMgtUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToVendorDetail(VendorId, "assetmanagement");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', ".btnCancelAssetMgt", function () {
    $(document).find('#AddVendorAssetMgtModalpopup').modal("hide");
});
$(document).on('click', '.delVendorassetmgt', function () {
    var data = dtVendorAssetManagementTable.row($(this).parents('tr')).data();
    DeleteVendorassetmgt(data.VendorAssetMgtId);
});
function DeleteVendorassetmgt(VendorAssetMgtId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/Vendor/VendorAssetMgtDelete',
            data: {
                VendorAssetMgtId: VendorAssetMgtId
            },
            type: "POST",
            beforeSend: function () {
                ShowLoader();
            },
            datatype: "json",
            success: function (data) {
                if (data.Result == "success") {
                    ShowDeleteAlert(getResourceValue("VendorAssetMgtDeleteAlert"));
                    dtVendorAssetManagementTable.state.clear();
                }
            },
            complete: function () {
                GenerateVendorAssetManagementGrid();
                CloseLoader();
            }
        });
    });
}

$(document).on('click', '#overridevendorAssetMgt', function () {
    var vendorid = $(document).find('#vendors_VendorId').val();
    UpdateVendorAssetMgtWdget(vendorid, true);
});
$(document).on('click', '#removeoverridevendorAssetMgt', function () {
    var vendorid = $(document).find('#vendors_VendorId').val();
    UpdateVendorAssetMgtWdget(vendorid, false);
});
function UpdateVendorAssetMgtWdget(VendorId, AssetMgtOverride) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/Vendor/UpdateVendorAssetMgtOverrideWdget',
            data: {
                VendorId: VendorId, AssetMgtOverride: AssetMgtOverride
            },
            type: "POST",
            beforeSend: function () {
                ShowLoader();
            },
            datatype: "json",
            success: function (data) {
                if (data.Result == "success") {
                    if (AssetMgtOverride == true) {
                        SuccessAlertSetting.text = getResourceValue("VendorInsuranceOverrideAlert");
                    }
                    else {
                        SuccessAlertSetting.text = getResourceValue("VendorInsuranceRemoveOverrideAlert");
                    }
                    swal(SuccessAlertSetting, function () {
                        RedirectToVendorDetail(VendorId, "assetmanagement");
                    });
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}

//endregion
