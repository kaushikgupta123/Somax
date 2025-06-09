var dtPersonneltable;
var selectCount = 0;
var personnelGridName = 'Personnel_Search';
var run = false;
var filterinfoarray = [];
var order = '1';
var orderDir = 'asc';
function filterinfo(id, value) {
    this.key = id;
    this.value = value;
}

$(function () {
    //#region Load Grid With Status
    var displayState = localStorage.getItem("PERSONNELSEARCHGRIDDISPLAYSTATUS");
    if (displayState) {
        activeStatus = displayState;
        if (activeStatus == 1) {
            $(document).find('#personnelsearchtitle').text($('#personnelsearchListul').find('li').eq(0).text());
            $("#personnelsearchListul li").eq(0).addClass('activeState');
        }
        else if (activeStatus == 2) {
            $(document).find('#personnelsearchtitle').text($('#personnelsearchListul').find('li').eq(1).text());
            $("#personnelsearchListul li").eq(1).addClass('activeState');
        }
    }
    else {
        activeStatus = 1;
        $(document).find('#personnelsearchtitle').text($('#personnelsearchListul').find('li').eq(0).text());
        $("#personnelsearchListul li").eq(0).addClass('activeState');
    }
    GeneratePersonnelTable();
    $(document).find('.select2picker').select2({});
    $("#sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $('#dismiss, .overlay').on('click', function () {
        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });

    $("#btnPersonnelDataAdvSrch").on('click', function (e) {
        run = true;
        $(document).find('#txtColumnSearch').val('');
        searchresult = [];
        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();
        PersonnelAdvSearch();
        dtPersonneltable.page('first').draw('page');
    });
});

function GeneratePersonnelTable() {
    if ($(document).find('#tblPersonnelSearch').hasClass('dataTable')) {
        dtPersonneltable.destroy();
    }
    dtPersonneltable = $("#tblPersonnelSearch").DataTable({
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
                }
                var filterinfoarray = getfilterinfoarray($("#txtColumnSearch"), $('#advsearchsidebarPersonnel'));
                $.ajax({
                    "url": gridStateSaveUrl,
                    "data": {
                        GridName: personnelGridName,
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
                    GridName: personnelGridName
                },
                "async": false,
                "dataType": "json",
                "success": function (json) {
                    hGridfilteritemcount = 0;
                    if (json.LayoutInfo !== '') {
                        var LayoutInfo = JSON.parse(json.LayoutInfo);
                        order = LayoutInfo.order[0][0];
                        orderDir = LayoutInfo.order[0][1];
                        callback(JSON.parse(json.LayoutInfo));
                        if (json.FilterInfo !== '') {
                            setsearchui(JSON.parse(json.FilterInfo), $("#txtColumnSearch"), $(".filteritemcount"), $("#dvFilterSearchSelect2"));
                        }
                    }
                    else {
                        callback(json.LayoutInfo);
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
                title: 'Personnel List',
                orientation: 'landscape',
                pageSize: 'A2'
            },
            {
                extend: 'print',
                title: 'Personnel List'
            },
            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Personnel List',
                extension: '.csv'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                customize: function (doc) {
                    doc.defaultStyle.alignment = 'left';
                    doc.styles.tableHeader.alignment = 'left';
                },
                exportOptions: {
                    columns: ':visible'
                },
                orientation: 'landscape',
                pageSize: 'A4',
                title: 'Personnel List'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/Personnel/GetPersonnelGrid",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.Order = order;
                d.ClientLookupId = LRTrim($("#PersonnelID").val());
                d.Name = LRTrim($("#Name").val());
                d.Shift = LRTrim($("#Shift").val());
                d.SearchText = LRTrim($(document).find('#txtColumnSearch').val());
                d.ActiveStatus = localStorage.getItem("PERSONNELSEARCHGRIDDISPLAYSTATUS");
                //#region 1108
                d.AssignedAssetGroup1 = LRTrim($("#AssignedAssetGroup1").val());
                d.AssignedAssetGroup2 = LRTrim($("#AssignedAssetGroup2").val());
                d.AssignedAssetGroup3 = LRTrim($("#AssignedAssetGroup3").val());
                //#endregion
            },
            "dataSrc": function (result) {
                let colOrder = dtPersonneltable.order();
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
                    "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-left",
                    "mRender": function (data, type, row) {
                        return '<a class=link_Personnel_Detail href="javascript:void(0)">' + data + '</a>';
                    }
                },
                { "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "ShiftDescription", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "CraftClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                //#region 1108
                { "data": "AssetGroup1Names", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "AssetGroup2Names", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "AssetGroup3Names", "autoWidth": true, "bSearchable": true, "bSortable": true },
                //#endregion
            ],
        columnDefs: [
            {
                targets: [0],
                className: 'noVis'
            }
        ],
        initComplete: function () {
            SetPageLengthMenu();
            $("#woGridAction :input").removeAttr("disabled");
            $("#woGridAction :button").removeClass("disabled");
            DisableExportButton($('#tblPersonnelSearch'), $(document).find('.import-export'));
        }
    });
}
//#region Pagination
$(document).on('click', '#tblPersonnelSearch_paginate .paginate_button', function () {
    run = true;
});
$(document).on('change', '#tblPersonnelSearch_length .searchdt-menu', function () {
    run = true;
});
//#endregion
//#region Sort
$('#tblPersonnelSearch').find('th').click(function () {
    if ($(this).data('col') !== undefined && $(this).data('col') !== '') {
        run = true;
        order = $(this).data('col');
    }
});
//#endregion
//#region ColumnVisibility
$(document).on('click', '#liCustomize', function () {
    funCustomizeBtnClick(dtPersonneltable, true);
});
$(document).on('click', '.saveConfig', function () {
    var colOrder = [0];
    funCustozeSaveBtn(dtPersonneltable, colOrder);
    run = true;
    dtPersonneltable.state.save(run);
});
//#endregion
//#region Export
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

$(function () {
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
       
        if (this.context.length) {
            var personnelId = LRTrim($("#PersonnelID").val());
            var name = LRTrim($("#Name").val());
            var shift = $('#Shift').val();
            if (!shift) {
                shift = "";
            }
            //#region 1108
            var assignedAssetGroup1Id = $('#AssignedAssetGroup1').val();
            var assignedAssetGroup2Id = $('#AssignedAssetGroup2').val();
            var assignedAssetGroup3Id = $('#AssignedAssetGroup3').val();
            //#endregion
            dtTable = $("#tblPersonnelSearch").DataTable();
         
            var info = dtTable.page.info();
            var start = info.start;
            var lengthMenuSetting = info.length;
            var currestsortedcolumn = order;
            var length = $('#tblPersonnelSearch').dataTable().length;
            var coldir = orderDir;
            var searchText = LRTrim($(document).find('#txtColumnSearch').val());
            var jsonResult = $.ajax({
                "url": "/Personnel/GetPersonnelPrintData",
                "type": "get",
                "datatype": "json",
                data: {
                    _colname: currestsortedcolumn,
                    _coldir: coldir,
                    start: start,
                    length: lengthMenuSetting,
                    _ClientLookupId: personnelId,
                    _Name: name,
                    _Shift: shift,
                    _searchText: searchText,
                    _activeStatus: localStorage.getItem("PERSONNELSEARCHGRIDDISPLAYSTATUS"),
                     // #region 1108
                    _assignedAssetGroup1: assignedAssetGroup1Id,
                    _assignedAssetGroup2: assignedAssetGroup2Id,
                    _assignedAssetGroup3: assignedAssetGroup3Id
                    //#endregion
                },
                success: function (result) {
                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data;
            var visiblecolumnsIndex = $("#tblPersonnelSearch thead tr th").map(function (key) {
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
                if (item.ShiftDescription != null) {
                    item.ShiftDescription = item.ShiftDescription;
                }
                else {
                    item.ShiftDescription = "";
                }
                if (item.CraftClientLookupId != null) {
                    item.CraftClientLookupId = item.CraftClientLookupId;
                }
                else {
                    item.CraftClientLookupId = "";
                }
                //#region 1108
                if (item.AssetGroup1Names != null) {
                    item.AssetGroup1Names = item.AssetGroup1Names;
                }
                else {
                    item.AssetGroup1Names = "";
                }
                if (item.AssetGroup2Names != null) {
                    item.AssetGroup2Names = item.AssetGroup2Names;
                }
                else {
                    item.AssetGroup2Names = "";
                }
                if (item.AssetGroup3Names != null) {
                    item.AssetGroup3Names = item.AssetGroup3Names;
                }
                else {
                    item.AssetGroup3Names = "";
                }
                //#endregion
                
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
                header: $("#tblPersonnelSearch thead tr th").find('div').map(function (key) {
                    return this.innerHTML;
                }).get()
            };
        }
    });
});
//#endregion
//#region TextSearch
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
$(document).mouseup(function (e) {
    var container = $(document).find('#searchBttnNewDrop');
    if (!container.is(e.target) && container.has(e.target).length === 0) {
        container.hide("slideToggle");
    }
});
$(document).on('click', "#personaltextsearch", function () {
    $.ajax({
        url: '/Base/PopulateNewSearchList',
        type: 'GET',
        data: { tableName: 'Personnel' },
        beforeSend: function () {
            ShowbtnLoader("personaltextsearch");
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
            HidebtnLoader("personaltextsearch");
        }
    });
});
function GenerateSearchList(txtSearchval, isClear) {
    $.ajax({
        url: '/Base/ModifyNewSearchList',
        type: 'POST',
        data: { tableName: 'Personnel', searchText: txtSearchval, isClear: isClear },
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
                dtPersonneltable.page('first').draw('page');
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
    run = true;
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
        GeneratePersonnelTable();
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

//#region AdvanceSearch
function PersonnelAdvSearch() {
    $('#txtColumnSearch').val('');
    var searchitemhtml = "";
    selectCount = 0;
    $('#advsearchsidebarPersonnel').find('.adv-item').each(function (index, item) {
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
$(document).on('click', '#liClearAdvSearchFilter', function () {
    run = true;
    $('#txtColumnSearch').val('');
    clearAdvanceSearch();
    dtPersonneltable.page('first').draw('page');
});
function clearAdvanceSearch() {
    $('.adv-item').val("").trigger('change.select2');
    selectCount = 0;
    $("#spnControlCounter").text(selectCount);
    $('#liSelectCount').text(selectCount + ' filters applied');
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
    PersonnelAdvSearch();
    dtPersonneltable.page('first').draw('page');
});
//#endregion

//#region Details
$(document).on('click', '.link_Personnel_Detail', function (e) {    
    var row = $(this).parents('tr');
    var data = dtPersonneltable.row(row).data();
    $.ajax({
        url: "/Personnel/PersonnelDetails",
        type: "POST",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        data: { PersonnelId: data.PersonnelId },
        success: function (data) {
            $('#personnelcontainer').html(data);
        },
        complete: function () {
            CloseLoader();
        }
    });
});
//#endregion

//#region V2-1098 Status Toggle
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
$(document).on('keyup', '#personnelsearctxtbox', function (e) {
    var tagElems = $(document).find('#personnelsearchListul').children();
    $(tagElems).hide();
    for (var i = 0; i < tagElems.length; i++) {
        var tag = $(tagElems).eq(i);
        if ($(tag).text().toLowerCase().includes($(this).val().toLowerCase()) == true || $(this).val().toLowerCase().includes($(tag).text().toLowerCase()) == true) {
            $(tag).show();
        }
    }
});
$(document).on('click', '.personnelsearchdrpbox', function (e) {
    if ($(document).find('#txtColumnSearch').val() !== '')
        $("#dvFilterSearchSelect2").html('');
    $(document).find('#txtColumnSearch').val('');
    run = true;
    $('#personnelsearctxtbox').text($(this).text());
    $(".searchList li").removeClass("activeState");
    $(this).addClass('activeState');
    $(document).find('#searcharea').hide("slide");
    var optionval = $(this).attr('id');
    localStorage.setItem("PERSONNELSEARCHGRIDDISPLAYSTATUS", optionval);
    activeStatus = optionval;
    $(document).find('#personnelsearchtitle').text($(this).text());
    if (optionval.length !== 0) {
        ShowbtnLoaderclass("LoaderDrop");
        dtPersonneltable.page('first').draw('page');
    }
});

//#endregion
