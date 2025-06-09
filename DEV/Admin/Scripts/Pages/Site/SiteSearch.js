var dtSiteTable;
var selectCount = 0;
var selectedcount = 0;
var totalcount = 0;
var searchcount = 0;
var activeStatus;
var run = false;
var titleText = '';
var order = '0';
var orderDir = 'asc';
//Search Retention
var gridname = "Site_Search";
function filterinfo(id, value) {
    this.key = id;
    this.value = value;
}
var isRedirectFromDetails = false;

$.validator.setDefaults({ ignore: null });
$(document).ready(function () {  
    ShowbtnLoaderclass("LoaderDrop");   
    $("#SiteGridAction :input").attr("disabled", "disabled");
    ShowbtnLoader("btnsortmenu");
    $("#action").click(function () {
        $(".actionDrop").slideToggle();
    });
    $(".actionDrop ul li a").click(function () {
        $(".actionDrop").fadeOut();
    });
    $("#action").focusout(function () {
        $(".actionDrop").fadeOut();
    });
    $("#sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $('#dismiss, .overlay').on('click', function () {
        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });

    $(".actionDrop2 ul li a").click(function () {
        $(".actionDrop2").fadeOut();
    });
    $(document).find('.select2picker').select2({});
    var ClientLookUp;
    var ClientlocalStorageDesc = localStorage.getItem("CLIENTLOOKUP");
    if (ClientlocalStorageDesc) {
        ClientLookUp = ClientlocalStorageDesc;
        $(document).find("#ClientList").val(ClientLookUp);
        localStorage.removeItem("CLIENTLOOKUP");
        isRedirectFromDetails = true;
    }
    else {
        ClientLookUp = $(document).find("#ClientList").val();
        isRedirectFromDetails = false;
    }
    $(document).find('#ClientList').trigger('change');  
    $("#btnSiteDataAdvSrch").on('click', function (e) {
        run = true;
        $(document).find('#txtColumnSearch').val('');
        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();
        SiteAdvSearch();
        dtSiteTable.page('first').draw('page');
    });

    $(document).on('click', '.link_site_detail', function (e) {
        e.preventDefault();
        clientId = $(document).find("#ClientList").val();
        var row = $(this).parents('tr');
        var data = dtSiteTable.row(row).data();
        $.ajax({
            url: "/Admin/Site/SiteDetails",
            type: "POST",
            dataType: "html",
            beforeSend: function () {
                ShowLoader();
            },
            data: { ClientId: clientId, SiteId: data.SiteId, },
            success: function (data) {
                $('#sitemaincontainer').html(data);
            },
            complete: function () {
                SetSiteControls();
                CloseLoader();
                SetFixedHeadStyle();
            }
        });
    });
    $(document).on('click', "ul.vtabs li", function () {
        $(document).find("ul.vtabs li").removeClass("active");
        $(document).find(this).addClass("active");
        $(document).find(".tabsArea").hide();
        var activeTab = $(this).find("a").attr("href");
        $(document).find(activeTab).fadeIn();
        return false;
    });
   
});
//#region Print
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
//#endregion

//#region Search
$(document).on('change', "#ClientList", function (e) { 
    var clientName = "";
    if (dtSiteTable != null && dtSiteTable != undefined) {
        dtSiteTable.state.clear();
    }
    var ClientID = "0";
    if ($(document).find("#ClientList").val() != "")
    {
        ClientID = $(document).find("#ClientList").val();
        clientName = $(document).find("#ClientList :selected").text();
    }

    var ClientLookUp = $(document).find("#ClientList").val();  
    localStorage.setItem("CLIENTLOOKUP", ClientLookUp);
    generateSiteDataTable(ClientID, clientName);
    if (!isRedirectFromDetails) {
        $('#txtColumnSearch').val('');
        clearAdvanceSearch();  
    }
    isRedirectFromDetails = false;
});
function generateSiteDataTable(ClientNameVal, ClientNameText) {
    if ($(document).find('#siteSearch').hasClass('dataTable')) {
        dtSiteTable.destroy();
    }
    dtSiteTable = $("#siteSearch").DataTable({
        colReorder: {
          fixedColumnsLeft: 1
        },
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        "stateSave": true,
        "stateSaveCallback": function (settings, data) {
            if (run == true) {
                if (data.order) {
                    data.order[0][0] = order;
                    data.order[0][1] = orderDir;
                }
                //Search Retention
                var filterinfoarray = getfilterinfoarray($("#txtColumnSearch"), $('#advsearchsidebar'));
                $.ajax({
                    "url": "/Admin/Base/CreateUpdateState",
                    "data": {
                        GridName: gridname,
                        LayOutInfo: JSON.stringify(data),
                        FilterInfo: JSON.stringify(filterinfoarray)
                    },
                    "dataType": "json",
                    "type": "POST",
                    "success": function () { return; }
                });
                //Search Retention
            }
            run = false;
        },
        "stateLoadCallback": function (settings, callback) {

            //Search Retention
            $.ajax({
                "url": "/Admin/Base/GetLayout",
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
                        callback(LayoutInfo);
                        if (json.FilterInfo) {
                            setsearchui(JSON.parse(json.FilterInfo), $("#txtColumnSearch"), $("#spnControlCounter"), $("#dvFilterSearchSelect2"));

                        }
                    }
                    else {
                        callback(json);
                    }
                }
            });
            //Search Retention
        },
        scrollX: true,
        fixedColumns: {
            leftColumns: 1
        },
        language: {
            url: "/Admin/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'Site List'
            },
            {
                extend: 'print',
                title: 'Site List'
            },

            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Site List',
                extension: '.csv'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                exportOptions: {
                    columns: ':visible'
                },
                css: 'display:none',
                title: 'Site List',
                orientation: 'landscape',
                pageSize: 'A3'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/Admin/Site/GetSiteGridData",
            "type": "post",
            "datatype": "json",
            cache: false,
            data: function (d) {                
                d.SiteId = LRTrim($("#SiteID").val());
                d.ClientId = ClientNameVal;
                d.Name = LRTrim($("#Name").val());
                d.AddressCity = LRTrim($("#AddressCity").val());
                d.AddressState = LRTrim($("#AddressState").val());
                d.SearchText = LRTrim($(document).find('#txtColumnSearch').val());
                d.order = order;
                d.orderDir = orderDir;
            },
            "dataSrc": function (result) {
                searchcount = result.recordsTotal;
                if (result.data.length < 1) {
                    $(document).find('#btnSiteExport').prop('disabled', true);
                }
                else {
                    $(document).find('#btnSiteExport').prop('disabled', false);
                }
                if (totalcount < result.recordsTotal)
                    totalcount = result.recordsTotal;
                if (totalcount != result.recordsTotal)
                    selectedcount = result.recordsTotal;

                HidebtnLoader("btnsortmenu");
                HidebtnLoaderclass("LoaderDrop");
                return result.data;
            }
        },
        select: {
            style: 'os',
            selector: 'td:first-child'
        },
        columnDefs: [
            {
                targets: [4], render: function (a, b, data, d) {                   
                    return '<a class="btn btn-outline-primary AddSite gridinnerbutton"  data-clientname="' + ClientNameText + '"  data-clientid="' + ClientNameVal + '" title="Add"><i class="fa fa-plus"></i></a>' +
                        '<a class="btn btn-outline-success EditSite gridinnerbutton"  data-clientid="' + ClientNameVal + '" data-siteid="' + data.SiteId + '" title= "Edit"> <i class="fa fa-pencil"></i></a>';
                },
                targets: [0, 4],
                className: 'noVis'
            }
        ],
        "columns":
        [

            {
                "data": "SiteId",
                "autoWidth": false,
                "bSearchable": true,
                "bSortable": false,
                className: 'text-left',
                "name": "0",
                "mRender": function (data, type, row) {
                    return '<a class=link_site_detail href="javascript:void(0)">' + data + '</a>';
                }
            },            
            { "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": false, "name": "1" },
            { "data": "AddressCity", "autoWidth": true, "bSearchable": true, "bSortable": false, "name": "2" },
            { "data": "AddressState", "autoWidth": true, "bSearchable": true, "bSortable": false, "name": "3" },
            { "data": "SiteId", "autoWidth": true, "bSearchable": false, "bSortable": false, className: 'text-center notPrint' }

        ],       
        initComplete: function () {
            SetPageLengthMenu();
            var currestsortedcolumn = $('#siteSearch').dataTable().fnSettings().aaSorting[0][0];
            var currestsortedorder = $('#siteSearch').dataTable().fnSettings().aaSorting[0][1];
            var column = this.api().column(currestsortedcolumn);
            var columnId = $(column.header()).attr('id');
            $(document).find('.srtsitecolumn').removeClass('sort-active');
            $(document).find('.srtsitecolumnorder').removeClass('sort-active');
            switch (columnId) {
                case "thSiteId":
                    $(document).find('.srtsitecolumn').eq(0).addClass('sort-active');
                    break;
                case "thSiteName":
                    $(document).find('.srtsitecolumn').eq(1).addClass('sort-active');
                    break;
                case "thCity":
                    $(document).find('.srtsitecolumn').eq(2).addClass('sort-active');
                    break;
                case "thState":
                    $(document).find('.srtsitecolumn').eq(3).addClass('sort-active');
                    break;
                default:
                    $(document).find('.srtsitecolumn').eq(0).addClass('sort-active');
                    break;
            }
            switch (currestsortedorder) {
                case "asc":
                    $(document).find('.srtsitecolumnorder').eq(0).addClass('sort-active');
                    break;
                case "desc":
                    $(document).find('.srtsitecolumnorder').eq(1).addClass('sort-active');
                    break;
            }
            $('#btnsortmenu').text(getResourceValue("spnSorting") + " : " + column.header().innerHTML);
            $("#SiteGridAction :input").removeAttr("disabled");
            $("#SiteGridAction :button").removeClass("disabled");
            DisableExportButton($("#siteSearch"), $(document).find('#btnSiteExport'));           
            if (searchcount > 0 || ClientNameText=="") { $("#btnAddSearchSite").hide(); }
            else {
                $("#btnAddSearchSite").show();
            }
        }
    });
}
$(document).on('click', '#siteSearch_paginate .paginate_button', function () {
    run = true;
});
$(document).on('change', '#siteSearch_length .searchdt-menu', function () {
    run = true;
});
$(document).find('.srtsitecolumn').click(function () {
    ShowbtnLoader("btnsortmenu");
    order = $(this).data('col');
    dtSiteTable.page('first').draw('page');
    $('#btnsortmenu').text(getResourceValue("spnSorting") + " : " + $(this).text());
    $(document).find('.srtsitecolumn').removeClass('sort-active');
    $(this).addClass('sort-active');
    run = true;
});
$(document).find('.srtsitecolumnorder').click(function () {
    ShowbtnLoader("btnsortmenu");    
    orderDir = $(this).data('mode');
    dtSiteTable.page('first').draw('page');   
    $(document).find('.srtsitecolumnorder').removeClass('sort-active');
    $(this).addClass('sort-active'); AddSite
    run = true;
});

$(function () {
    $(document).on('change', '#colorselector', function (evt) {
        $(document).find('.tabsArea').hide();
        openCity2(evt, $(this).val());
        $('#' + $(this).val()).show();
    });
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            var siteid = LRTrim($("#SiteID").val());
            var clientId = $(document).find("#ClientList").val();
            var name = LRTrim($("#Name").val());
            var addressCity = LRTrim($("#AddressCity").val());
            var addressState = LRTrim($("#AddressState").val());          
            dtSiteTable = $("#siteSearch").DataTable();
            var info = dtSiteTable.page.info();
            var start = info.start;
            var lengthMenuSetting = info.length;           
            var length = $('#siteSearch').dataTable().length;               

            var searchText = LRTrim($(document).find('#txtColumnSearch').val());
            var jsonResult = $.ajax({
                "url": "/Admin/Site/GetSitePrintData",
                "type": "get",
                "datatype": "json",
                data: {                   
                    start: start,
                    length: lengthMenuSetting,
                    _siteid: siteid,
                    _clientId: clientId,
                    _Name: name,
                    _AddressCity: addressCity,
                    _AddressState: addressState,                   
                    _colname: order,
                    _coldir: orderDir,
                    _searchText: searchText
                },
                success: function (result) {
                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data;
            var visiblecolumnsIndex = $("#siteSearch thead tr th").not(".notPrint").map(function (key) {
                return this.getAttribute('data-th-index');
            }).get();
            var d = [];
            $.each(thisdata, function (index, item) {
                if (item.SiteId != null) {
                    item.SiteId = item.SiteId;
                }
                else {
                    item.SiteId = "";
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
                header: $("#siteSearch thead tr th").not(".notPrint").find('div').map(function (key) {
                    return this.innerHTML;
                }).get()
            };
        }
    });
    $("#EqpBulksidebar").mCustomScrollbar({
        theme: "minimal"
    });
});

function SiteAdvSearch() {    
    $('#txtColumnSearch').val('');
    var searchitemhtml = "";
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
    $('#liSelectCount').text(selectCount + ' filters applied');
}
function clearAdvanceSearch() {  
    $('.adv-item').val("");
    selectCount = 0;
    $("#spnControlCounter").text(selectCount);
    $('#dvFilterSearchSelect2').find('span').html('');
    $('#dvFilterSearchSelect2').find('span').removeClass('tagTo');
}
$(document).on('click', '#sidebarCollapse', function () {
    $('#sidebar').addClass('active');
    $('.overlay').fadeIn();
    $('.collapse.in').toggleClass('in');
    $('a[aria-expanded=true]').attr('aria-expanded', 'false');
});
$(document).on('click', '.btnCross', function () {
    run = true;
    var btnCrossed = $(this).parent().attr('id');
    var searchtxtId = btnCrossed.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    selectCount--;    
    SiteAdvSearch();
    dtSiteTable.page('first').draw('page');
});


function funCustozeSiteSaveBtn(dtSiteTable, colOrder) {
    //Maintaing colspan when columns gets added or removed in no data available state in grid
    var colOrderNew = [];
    $('.dataTables_empty').attr('colspan', '100%');
    $("#PresenterList li").each(function () {
        var name = $(this).find('span').text();
        $.each(oCols, function (k, l) {
            if (name === l.Value) {
                colOrderNew.push(l.Id);
                dtSiteTable.columns(l.Idx).visible(true);
            }
        });
    });
    $.each(nselectedCol, function (o, g) {
        $.each(oCols, function (k, l) {
            if (g.Name === l.Value) {
                colOrderNew.push(l.Id);
                console.log(dtSiteTable.settings()[0].aoColumns[l.Idx].sTitle);
                dtSiteTable.columns(l.Idx).visible(false);
            }
        });
    });
    $.each(colOrder, function (index, item) {
        colOrderNew.splice(item, 0, item);
    });
    dtSiteTable.colReorder.reset();
    dtSiteTable.colReorder.order(colOrderNew);
}
$(document).on('click', '#liCustomize', function () {
    funCustomizeBtnClick(dtSiteTable);
});
$(document).on('click', '.saveConfig', function () {
    var colOrder = [0,4];
    funCustozeSiteSaveBtn(dtSiteTable, colOrder);
    run = true;
    dtSiteTable.state.save(run);
});   

//#endregion

//#region New Search button
$(document).on('keyup', '#sitesearctxtbox', function (e) {
    var tagElems = $(document).find('#clientsearchListul').children();
    $(tagElems).hide();
    for (var i = 0; i < tagElems.length; i++) {
        var tag = $(tagElems).eq(i);
        if ($(tag).text().toLowerCase().includes($(this).val().toLowerCase()) == true || $(this).val().toLowerCase().includes($(tag).text().toLowerCase()) == true) {
            $(tag).show();
        }
    }
});

$(document).on('click', "#SrchBttnNew", function () {
    $.ajax({
        url: '/Admin/Base/PopulateNewSearchList',
        type: 'GET',
        data: { tableName: 'Site' },
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
        url: '/Admin/Base/ModifyNewSearchList',
        type: 'POST',
        data: { tableName: 'Site', searchText: txtSearchval, isClear: isClear },
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
                dtSiteTable.page('first').draw('page');
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
    activeStatus = 0;
    var txtSearchval = LRTrim($(document).find('#txtColumnSearch').val());
    if (txtSearchval) {
        GenerateSearchList(txtSearchval, false);
        var searchitemhtml = "";
        searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $('#txtColumnSearch').attr('id') + '">Search: ' + txtSearchval + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
        searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
        $("#dvFilterSearchSelect2").html(searchitemhtml);
    }
    else {
        run = true;
       var  clientID = $(document).find("#ClientList").val();
       var clientName = $(document).find("#ClientList :selected").text();
       generateSiteDataTable(clientID, clientName);   
      
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
$(document).on('click', '#linkToSearch', function () {
    window.location.href = "/Admin/Site/Index?page=Sites";
});

//#endregion
//#region //Search Retention
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
//#region Details
function openCity2(evt, cityName) {
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent2");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tablinks2");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(cityName).style.display = "block";
    evt.currentTarget.className += " active";
}
//#endregion
//#region Add 
$(document).on('click', '.AddSite', function () {
    var ClientName = $(this).data("clientname"); 

    var ClientId = $(this).data("clientid"); 
    $.ajax({
        url: "/Admin/Site/AddSite",
        data: { ClientName: ClientName, ClientId: ClientId},
        type: "GET",
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#sitemaincontainer').html(data);
        },
        complete: function () {
            SetSiteControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', '#btnAddSite', function () {
    var ClientId = LRTrim($(document).find('#SiteModel_ClientId').val());
    var ClientName = LRTrim($(document).find('#SiteModel_ClientName').val());   
    $.ajax({
        url: "/Admin/Site/AddSite",
        data: { ClientName: ClientName, ClientId: ClientId },
        type: "GET",
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#sitemaincontainer').html(data);
        },
        complete: function () {
            SetSiteControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', '#btnAddSearchSite', function () {   
    var ClientId = $(document).find("#ClientList").val();
    var ClientName = $(document).find("#ClientList :selected").text();
    $.ajax({
        url: "/Admin/Site/AddSite",
        data: { ClientName: ClientName, ClientId: ClientId },
        type: "GET",
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#sitemaincontainer').html(data);
        },
        complete: function () {
            SetSiteControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', "#btnCancelAddSite", function () {
    swal(CancelAlertSetting, function () {
        window.location.href = "/Admin/Site/Index?page=Sites";
    });
});
//#endregion
//#region Edit 
$(document).on('click', "#btnCancelEditSite", function () {
    var SiteId = $(document).find('#SiteModel_SiteId').val();
    var ClientId = $(document).find('#SiteModel_ClientId').val();
    swal(CancelAlertSetting, function () {
        RedirectToSiteDetail(SiteId, ClientId);
    });
});
$(document).on('click', ".EditSite", function () {

    var ClientId = $(this).data("clientid"); 
    var SiteId = $(this).data("siteid"); 
    $.ajax({
        url: '/Admin/Site/EditSite',
        data: { SiteId: SiteId, ClientId: ClientId },
        type: "POST",
        datatype: "html",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#sitemaincontainer').html(data);
        },
        complete: function () {
            CloseLoader();
            $.validator.setDefaults({ ignore: '.ignorevalidation' });
            $.validator.unobtrusive.parse(document);
            $('.select2picker, form').change(function () {
                var areaddescribedby = $(this).attr('aria-describedby');
                if (typeof areaddescribedby != 'undefined') {
                    $('#' + areaddescribedby).show();
                }
            });
            $(document).find('.select2picker').select2({});

            $("form").submit(function () {
                var activetagid = $('.vtabs li.active').attr('id');               
            });
        }
    });
});
$(document).on('click', "#btnEditSite", function () {
    var ClientId = LRTrim($(document).find('#SiteModel_ClientId').val());   

    var SiteId = LRTrim($(document).find('#SiteModel_SiteId').val());   
   
    $.ajax({
        url: '/Admin/Site/EditSite',
        data: { SiteId: SiteId, ClientId: ClientId },
        type: "POST",
        datatype: "html",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#sitemaincontainer').html(data);
        },
        complete: function () {
            CloseLoader();
            $.validator.setDefaults({ ignore: '.ignorevalidation' });
            $.validator.unobtrusive.parse(document);
            $('.select2picker, form').change(function () {
                var areaddescribedby = $(this).attr('aria-describedby');
                if (typeof areaddescribedby != 'undefined') {
                    $('#' + areaddescribedby).show();
                }
            });
            $(document).find('.select2picker').select2({});

            $("form").submit(function () {
                var activetagid = $('.vtabs li.active').attr('id');               
            });
        }
    });
});
//#endregion

$(document).on('click', "#btnSaveAnotherOpen,#btnSave", function () {
    if ($(document).find("form").valid()) {      
        return;
    }
    else {

        var errorTab = $(document).find(".input-validation-error").parents('div:eq(0)').attr('id');
        if (errorTab == "SiteInformation") {
            $('#SiteInformationtab').trigger('click');
        }
        else if (errorTab == "AddressInformation") {
            $('#AddressInformationtab').trigger('click');
        }
        else if (errorTab == "SystemInformation") {
            $('#SystemInformationtab').trigger('click');
        }
       

    }
});
function SetSiteControls() {
    CloseLoader();
    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse(document);
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
function SiteAddEditOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {

        if (data.Mode == "Add") {
            SuccessAlertSetting.text = getResourceValue("SiteAddSuccessAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("SiteUpdateSuccessAlert");
        }      
        if (data.Command == "save") {
            swal(SuccessAlertSetting, function () {
                RedirectToSiteDetail(data.SiteId, data.ClientId);
            });
        }
        else {
            ResetErrorDiv();
            $(document).find('#SiteInformationtab').addClass('active').trigger('click');
            swal(SuccessAlertSetting, function () {
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
function RedirectToSiteDetail(SiteId, ClientId) {

    $.ajax({
        url: "/Admin/Site/SiteDetails",
        type: "POST",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        data: { ClientId: ClientId, SiteId: SiteId},
        success: function (data) {

            $('#sitemaincontainer').html(data);
        },
        complete: function () {
            SetSiteControls();
            CloseLoader();
        }
    });   
}

//#region V2-1176
$(document).on('click', '#createGuestURL', function () {
    var ClientId = $("#SiteModel_ClientId").val();
    var SiteId = $("#SiteModel_SiteId").val();
    $.ajax({
        url: "/Admin/Site/CreateGuestURL",
        data: { ClientId: ClientId, SiteId: SiteId },
        type: "GET",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#guestUrl').text(data.data);
            $('#guestURLModal').modal('show');
        },
        complete: function () {
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', '#copyToClipboard', function () {
    var textToCopy = $('#guestUrl').text();
    navigator.clipboard.writeText(textToCopy).then().catch(function (err) {
        console.log('Failed to copy text: ', err);
    });
});
//#endregion

//#region V2-1178
$(document).on('click', '#createPartIssueURL', function () {
    var ClientId = $("#SiteModel_ClientId").val();
    var SiteId = $("#SiteModel_SiteId").val();
    $.ajax({
        url: "/Admin/Site/CreateGuestURL",
        data: { ClientId: ClientId, SiteId: SiteId },
        type: "GET",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#txtUserName').val('');
            $('#userNameValidateModal').modal('show');
        },
        complete: function () {
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});
function createPartIssueURLOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {     
               $('#userNameValidateModal').modal('hide');
                var ClientId = $("#SiteModel_ClientId").val();
                var SiteId = $("#SiteModel_SiteId").val();
                var LoginInfoId = data.LoginInfoId;
                $.ajax({
                    url: "/Admin/Site/CreatePartIssueURL",
                    data: { ClientId: ClientId, SiteId: SiteId, LoginInfoId: LoginInfoId },
                    type: "GET",
                    beforeSend: function () {
                        ShowLoader();
                    },
                    success: function (data) {
                        $('#partIssueUrl').text(data.data);
                        $('#partIssueURLModal').modal('show');
                    },
                    complete: function () {
                        CloseLoader();
                    },
                    error: function () {
                        CloseLoader();
                    }
                });            
        
    }
   else if (data.Result == "failed") {
        var msgText = getResourceValue('spnUserNotExistMsg');
        ErrorAlertSetting.text = msgText;
        swal(ErrorAlertSetting, function () {
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}

$(document).on('click', '#copyToClipboardPartIssue', function () {
    var textToCopy = $('#partIssueUrl').text();
    navigator.clipboard.writeText(textToCopy).then().catch(function (err) {
        console.log('Failed to copy text: ', err);
    });
});
//#endregion