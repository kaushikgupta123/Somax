//#region Global Variable
var dtKbTopicsTable;
var selectCount = 0;
var selectedcount = 0;
var totalcount = 0;
var searchcount = 0;
var searchresult = [];
var run = false;
var titleText = '';
var filterinfoarray = [];
function filterinfo(id, value) {
    this.key = id;
    this.value = value;
}
$.validator.setDefaults({ ignore: null });
//#endregion
//#region Ready Function
$(document).ready(function () {

    ShowbtnLoaderclass("LoaderDrop");
    $(".updateArea").hide();
    $(".actionBar").fadeIn();
    $("#kbtopicsGridAction :input").attr("disabled", "disabled");
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
    if (IsAdd != "True") {
        generateKnowledgebaseDataTable();
    }
    SetControls();
});
//#endregion

//#region Export Button Click
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

//#region Datatable
var order = '0';
var orderDir = 'asc';
function generateKnowledgebaseDataTable() {
    if ($(document).find('#kbtopicsSearch').hasClass('dataTable')) {
        dtKbTopicsTable.destroy();
    }
    dtKbTopicsTable = $("#kbtopicsSearch").DataTable({
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
                var filterinfoarray = getfilterinfoarray($("#txtColumnSearch"), $('#advsearchsidebar'));
                $.ajax({
                    "url": "/Admin/Base/CreateUpdateState",
                    "data": {
                        GridName: "Knowledgebase_Search",
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
                "url": "/Admin/Base/GetLayout",
                "data": {
                    GridName: "Knowledgebase_Search"
                },
                "async": false,
                "dataType": "json",
                "success": function (json) {
                    selectCount = 0;
                    if (json.LayoutInfo !== '') {
                        var LayoutInfo = JSON.parse(json.LayoutInfo);
                        order = LayoutInfo.order[0][0];
                        orderDir = LayoutInfo.order[0][1];
                        callback(JSON.parse(json.LayoutInfo));
                        if (json.FilterInfo !== '') {
                            setsearchui(JSON.parse(json.FilterInfo), $("#txtColumnSearch"), $("#spnControlCounter"), $("#dvFilterSearchSelect2"));
                        }
                        TagInput();

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
            url: "/Admin/Base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'Knowledge Base'
            },
            {
                extend: 'print',
                title: 'Knowledge Base'
            },

            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Knowledge Base',
                extension: '.csv'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                exportOptions: {
                    columns: ':visible'
                },
                css: 'display:none',
                title: 'Knowledge Base',
                orientation: 'Portarait',
                pageSize: 'A4'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/Admin/Knowledgebase/GetkbtopicsGridData",
            "type": "post",
            "datatype": "json",
            cache: false,
            data: function (d) {
                d.customQueryDisplayId = 1;
                d.Category = LRTrim($("#ddlCategory").val());
                d.Title = LRTrim($("#Title").val());
                d.Description = LRTrim($("#Description").val());
                d.Tags = $("#ddlUser").val();
                d.Folder = LRTrim($("#Folder").val());
                d.SearchText = LRTrim($(document).find('#txtColumnSearch').val());
                d.order = order;
                d.orderDir = orderDir;
            },
            "dataSrc": function (result) {
                searchcount = result.recordsTotal;
                if (result.data.length < 1) {
                    $(document).find('#btnkbtopicsExport').prop('disabled', true);
                }
                else {
                    $(document).find('#btnkbtopicsExport').prop('disabled', false);
                }

                $.each(result.data, function (index, item) {
                    searchresult.push(item.KBTopicsId);
                });
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
        "columns":
            [
                {
                    "data": "KBTopicsId",
                    "autoWidth": false,
                    "bSearchable": true,
                    "bSortable": false,
                    className: 'text-left',
                    "name": "0",
                    "mRender": function (data, type, row) {
                        return '<a class=link_kbtopics_detail href="javascript:void(0)">' + data + '</a>';
                    }
                },
                { "data": "Title", "autoWidth": true, "bSearchable": true, "bSortable": false, "name": "1" },
                { "data": "Category", "autoWidth": true, "bSearchable": true, "bSortable": false, "name": "2" },
                { "data": "Folder", "autoWidth": true, "bSearchable": true, "bSortable": false, "name": "3" }

            ],
        columnDefs: [
            {
                targets: [0],
                className: 'noVis'
            }
        ],
        initComplete: function () {
            SetPageLengthMenu();
            var currestsortedcolumn = $('#kbtopicsSearch').dataTable().fnSettings().aaSorting[0][0];
            var currestsortedorder = $('#kbtopicsSearch').dataTable().fnSettings().aaSorting[0][1];
            var column = this.api().column(currestsortedcolumn);
            var columnId = $(column.header()).attr('id');
            $(document).find('.srtkbtopicscolumn').removeClass('sort-active');
            $(document).find('.srtkbtopicsorder').removeClass('sort-active');
            switch (columnId) {
                case "thKBTopicsId":
                    $(document).find('.srtkbtopicscolumn').eq(0).addClass('sort-active');
                    break;
                case "thTitle":
                    $(document).find('.srtkbtopicscolumn').eq(1).addClass('sort-active');
                    break;
                case "thCategory":
                    $(document).find('.srtkbtopicscolumn').eq(2).addClass('sort-active');
                    break;
                default:
                    $(document).find('.srtkbtopicscolumn').eq(0).addClass('sort-active');
                    break;

            }
            switch (currestsortedorder) {
                case "asc":
                    $(document).find('.srtkbtopicsorder').eq(0).addClass('sort-active');
                    break;
                case "desc":
                    $(document).find('.srtkbtopicsorder').eq(1).addClass('sort-active');
                    break;
            }
            $('#btnsortmenu').text(getResourceValue("spnSorting") + " : " + column.header().innerHTML);
            $("#kbtopicsGridAction :input").removeAttr("disabled");
            $("#kbtopicsGridAction :button").removeClass("disabled");
            DisableExportButton($("#kbtopicsSearch"), $(document).find('#btnkbtopicsExport,#eqsearch-select-all'));
        }
    });
}
$(document).on('click', '#kbtopicsSearch_paginate .paginate_button', function () {
    run = true;
});
$(document).on('change', '#kbtopicsSearch_length .searchdt-menu', function () {
    run = true;
});

$(document).find('.srtkbtopicscolumn').click(function () {
    ShowbtnLoader("btnsortmenu");
    order = $(this).data('col');
    $('#btnsortmenu').text(getResourceValue("spnSorting") + " : " + $(this).text());
    $(document).find('.srtkbtopicscolumn').removeClass('sort-active');
    $(this).addClass('sort-active');
    run = true;
    $('#kbtopicsSearch').DataTable().draw();
});
$(document).find('.srtkbtopicsorder').click(function () {
    ShowbtnLoader("btnsortmenu");
    orderDir = $(this).data('mode');
    $(document).find('.srtkbtopicsorder').removeClass('sort-active');
    $(this).addClass('sort-active');
    run = true;
    $('#kbtopicsSearch').DataTable().draw();
});
//#endregion

//#region Filter Array
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
            if (item.value && item.value.length > 0) {
                txtsearchelement.val(txtSearchval);
                searchitemhtml = "";
                searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + txtsearchelement.attr('id') + '">Search: ' + txtSearchval + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
            }
            return false;
        }
        else {
            if ($('#' + item.key).parent('div').is(":visible")) {
                $('#' + item.key).val(item.value);
                if (item.value && item.value.length > 0) {
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

//#region New Search button
$(document).on('keyup', '#knbsearctxtbox', function (e) {
    var tagElems = $(document).find('#knbsearchListul').children();
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
        data: { tableName: 'KBTopics' },
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
        url: '/Admin/Base/ModifyNewSearchList',
        type: 'POST',
        data: { tableName: 'KBTopics', searchText: txtSearchval, isClear: isClear },
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
                dtKbTopicsTable.page('first').draw('page');
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
        generateKnowledgebaseDataTable();
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
    window.location.href = "../Knowledgebase/Index?page=Knowledgebase_Search";
});

function clearAdvanceSearch() {
    $("#ddlCategory").val("").trigger('change');
    $("#ddlUser").val("").trigger('change');
    $('.adv-item').val("");
    $('.tagify__tag').remove();
}
$(document).on('click', '.btnCross', function () {
    run = true;
    var btnCrossed = $(this).parent().attr('id');
    var searchtxtId = btnCrossed.split('_')[1];
    if (searchtxtId == 'ddlUser') {
        $('.tagify__tag').remove();
    }
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    selectCount--;
    KBTopicsAdvSearch();
    dtKbTopicsTable.page('first').draw('page');
});


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

//#region AdvancedSearch
function KBTopicsAdvSearch() {
    $('#txtColumnSearch').val('');
    var searchitemhtml = "";
    selectCount = 0;
    $('#advsearchsidebar').find('.adv-item').each(function (index, item) {
        if ($(this).val() && $(this).val().length > 0) {
            selectCount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';

    $("#dvFilterSearchSelect2").html(searchitemhtml);
    $("#spnControlCounter").text(selectCount);
    $('#liSelectCount').text(selectCount + ' filters applied');
}
$(document).on('click', '#sidebarCollapse', function () {
    $('#sidebar').addClass('active');
    $('.overlay').fadeIn();
    $('.collapse.in').toggleClass('in');
    $('a[aria-expanded=true]').attr('aria-expanded', 'false');

});
$("#btnKnbDataAdvSrch").on('click', function (e) {
    run = true;
    $(document).find('#txtColumnSearch').val('');
    searchresult = [];
    $('#sidebar').removeClass('active');
    $('.overlay').fadeOut();
    KBTopicsAdvSearch();
    dtKbTopicsTable.page('first').draw('page');
});
//#endregion

//#region Add and Update
function checkformvalid() {
    $("#topicaddform").submit(function () {
        var messageLength = "";
        if ($(this).valid()) {
            //ck-rounded-corners
            const data = getDataFromTheEditor();
            messageLength = data.replace(/<[^>]*>/gi, '').length;
            if (!messageLength) {
                $(".content-container").addClass("input-validation-error");
            }
            else {
                $(".content-container").removeClass("input-validation-error");
            }
            return;
        }
        else {
            const data = getDataFromTheEditor();
            messageLength = data.replace(/<[^>]*>/gi, '').length;
            if (!messageLength) {
                $(".content-container").addClass("input-validation-error");
            }
            else {
                $(".content-container").removeClass("input-validation-error");
            }
        }
    });
}
function checkformEditvalid() {
    $("#topiceditform").submit(function () {
        var messageLength = "";
        if (getDataFromTheEditor() != '') {
            $('#KBTopicsModel_Description').val(getDataFromTheEditor());
        }
        if ($(this).valid()) {
            const data = getDataFromTheEditor();
            messageLength = data.replace(/<[^>]*>/gi, '').length;
            if (!messageLength) {
                $(".content-container").addClass("input-validation-error");
            }
            else {
                $(".ck-editor__editable").removeClass("input-validation-error");
            }
            return;
        }
        else {
            const data = getDataFromTheEditor();
            messageLength = data.replace(/<[^>]*>/gi, '').length;
            if (!messageLength) {
                $(".content-container").addClass("input-validation-error");
            }
            else {
                $(".content-container").removeClass("input-validation-error");
            }

        }
    });
}
$(document).on('click', '.AddKBTopics', function () {
    $.ajax({
        url: "/Admin/Knowledgebase/AddKbTopics",
        type: "GET",
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#kbtopicsmaincontainer').html(data);
            TagInput();
            LoadCkEditor();
            $('#lblTopicDesc').css('height', '520px');
        },
        complete: function () {
            checkformvalid();
            SetControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});

function TopicAddOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("KBTopicsAddAlert");
        if (data.Command == "save") {
            swal(SuccessAlertSetting, function () {
                titleText = getResourceValue("AlertKnowledgebaseTopic");
                RedirectToKbTopicsDetail(data.KbtopicsId);
            });
        }
        else {
            ResetErrorDiv();
            $(document).find('#topicaddidtab').addClass('active').trigger('click');
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

$(document).on('click', "#btnCancelAddKBTopics", function () {
    swal(CancelAlertSetting, function () {
        window.location.href = "../Knowledgebase/Index?page=Knowledgebase_Search";
    });
});
$(document).on('click', "#btnkbtopicseditcancel", function () {
    var KBTopicsId = $(document).find('#KBTopicsModel_KBTopicsId').val();
    swal(CancelAlertSetting, function () {
        RedirectToKbTopicsDetail(KBTopicsId);
    });
});
$(document).on('click', "#editKbTopics", function () {
    var KBTopicsId = LRTrim($(document).find('#KBTopicsModel_KBTopicsId').val());
    $.ajax({
        url: '/Admin/Knowledgebase/KbTopicsEdit',
        data: { KBTopicsId: KBTopicsId },
        type: "POST",
        datatype: "html",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            var getTexttoHtml = textToHTML(data);
            $('#kbtopicsmaincontainer').html(getTexttoHtml);
            TagInput();
            LoadCkEditor();
            $('#lblTopicDesc').css('height', '520px');
        },
        complete: function () {
            checkformEditvalid();
            SetControls();
        }
    });
});

function KbTopicsEditOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("KBTopicsUpdateAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToKbTopicsDetail(data.KBTopicsId);
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', '.lithisKbTopics', function () {
    var KBTopicsId = $(this).attr('data-val');
    RedirectToKbTopicsDetail(KBTopicsId);
});
//#endregion

//#endregion

//#region Export
$(function () {
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            var Title = LRTrim($("#Title").val());
            var Description = LRTrim($("#Description").val());
            var Tags = $("#ddlUser").val();
            var Category = LRTrim($("#ddlCategory").val());
            var Folder = LRTrim($("#Folder").val());
            dtKbTopicsTable = $("#kbtopicsSearch").DataTable();
            var info = dtKbTopicsTable.page.info();
            var start = info.start;
            var lengthMenuSetting = info.length;
            var jsonResult = $.ajax({
                "url": "/Admin/Knowledgebase/GetkbtopicsPrintData",
                "type": "get",
                "datatype": "json",
                data: {
                    start: start,
                    length: lengthMenuSetting,
                    customQueryDisplayId: 1,
                    Category: Category,
                    Title: Title,
                    Folder: Folder,
                    Description: Description,
                    Tags: Tags,
                    SearchText: LRTrim($(document).find('#txtColumnSearch').val()),
                    order: order,
                    orderDir: orderDir
                },
                success: function (result) {
                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data;
            var visiblecolumnsIndex = $("#kbtopicsSearch thead tr th").map(function (key) {
                return this.getAttribute('data-th-index');
            }).get();
            var d = [];
            $.each(thisdata, function (index, item) {
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
                header: $("#kbtopicsSearch thead tr th").find('div').map(function (key) {
                    return this.innerHTML;
                }).get()
            };
        }
    });

});
//#endregion


//#region Details
function RedirectToKbTopicsDetail(KbTopicsId) {
    $.ajax({
        url: "/Admin/Knowledgebase/KnowledgebaseDetails",
        type: "POST",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        data: { KBTopicsId: KbTopicsId },
        success: function (data) {
            var getTexttoHtml = textToHTML(data);
            $('#kbtopicsmaincontainer').html(getTexttoHtml);
            $(document).find('#spnlinkToSearch').text(titleText);
        },
        complete: function () {
            CloseLoader();
            SetFixedHeadStyle();
            $('.descspan a').attr('target', '_blank');
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', '.link_kbtopics_detail', function (e) {
    e.preventDefault();
    titleText = $('#kbtopicssearchtitle').text();
    var row = $(this).parents('tr');
    var data = dtKbTopicsTable.row(row).data();
    $.ajax({
        url: "/Admin/Knowledgebase/KnowledgebaseDetails",
        type: "POST",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        data: { KBTopicsId: data.KBTopicsId },
        success: function (data) {
            var getTexttoHtml = textToHTML(data);
            $('#kbtopicsmaincontainer').html(getTexttoHtml);
            $(document).find('#spnlinkToSearch').text(titleText);
        },
        complete: function () {
            CloseLoader();
            SetFixedHeadStyle();
            $('.descspan a').attr('target', '_blank');
        }
    });
});
//#endregion
//#region CKEditor



//#endregion

//#region ColumnVisibility
$(document).on('click', '#liCustomize', function () {
    funCustomizeBtnClick(dtKbTopicsTable);
});
$(document).on('click', '.saveConfig', function () {
    var colOrder = [0, 1];
    funCustozeSaveBtn(dtKbTopicsTable, colOrder);
    run = true;
    dtKbTopicsTable.state.save(run);
});
//#endregion


function SetControls() {
    CloseLoader();
    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse(document);
    $('input, form').blur(function () {
        $(this).valid();
    });
    $('.select2picker, form').change(function () {
        var areaddescribedby = $(this).attr('aria-describedby');
        if ($(this).parents().find('.sidebar-content').attr('id') != "advsearchsidebar") {
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
    SetFixedHeadStyle();

}
//#region taginput
function TagInput() {
    var intvalue = '';
    if ($(document).find('#ddlUser').val()) {
        intvalue = $(document).find('#ddlUser').val().trim().split(/\s*,\s*/);
    }
    else {
        intvalue = [];
    }
    var input = document.querySelector('.som-tagify'),
        // init Tagify script on the above inputs
        tagify = new Tagify(input, {
            whitelist: intvalue

        });
    tagify.on('input', onTagifyInputAdd);
    function onTagifyInputAdd(e) {
        tagify.settings.whitelist.length = 0; // reset current whitelist
        tagify.loading(true); // show the loader animation

        $.ajax({
            url: "/Admin/Knowledgebase/RetrieveTagName",
            type: "GET",
            dataType: 'json',
            success: function (data) {
                tagify.settings.whitelist.length = 0;
                $.each(data, function (index, item) {
                    tagify.settings.whitelist.push(item);
                });

                tagify.loading(false).dropdown.show.call(tagify, e.detail.value);

            },
            complete: function () { },
            error: function () {
                tagify.dropdown.hide.call(tagify);
            }
        });
    }

}
//#endregion


//#region CustomAdapter CkEditor
class MyUploadAdapter {
    constructor(loader) {
        // The file loader instance to use during the upload.
        this.loader = loader;
    }
    // Starts the upload process.
    upload() {
        return this.loader.file
            .then(file => new Promise((resolve, reject) => {
                this._initRequest();
                this._initListeners(resolve, reject, file);
                this._sendRequest(file);
            }));
    }

    // Aborts the upload process.
    abort() {
        // Reject the promise returned from the upload() method.
        // server.abortUpload();
        if (this.xhr) {
            this.xhr.abort();
        }
    }

    // Initializes the XMLHttpRequest object using the URL passed to the constructor.
    _initRequest() {
        const xhr = this.xhr = new XMLHttpRequest();
        // Note that your request may look different. It is up to you and your editor
        // integration to choose the right communication channel. This example uses
        // a POST request with JSON as a data structure but your configuration
        // could be different. 
        xhr.open('POST', '/Admin/Base/SaveUploadedFileToServer', true);
        xhr.responseType = 'json';
    }
    // Initializes XMLHttpRequest listeners.
    _initListeners(resolve, reject, file) {
        const xhr = this.xhr;
        const loader = this.loader;
        const genericErrorText = `Couldn't upload file: ${file.name}.`;

        xhr.addEventListener('error', () => reject(genericErrorText));
        xhr.addEventListener('abort', () => reject());
        xhr.addEventListener('load', () => {
            const response = xhr.response;

            // This example assumes the XHR server's "response" object will come with
            // an "error" which has its own "message" that can be passed to reject()
            // in the upload promise.
            //
            // Your integration may handle upload errors in a different way so make sure
            // it is done properly. The reject() function must be called when the upload fails.
            if (!response || response.error) {
                return reject(response && response.error ? response.error.message : genericErrorText);
            }

            // If the upload is successful, resolve the upload promise with an object containing
            // at least the "default" URL, pointing to the image on the server.
            // This URL will be used to display the image in the content. Learn more in the
            // UploadAdapter#upload documentation.
            resolve({
                default: response
            });
        });

        // Upload progress when it is supported. The file loader has the #uploadTotal and #uploaded
        // properties which are used e.g. to display the upload progress bar in the editor
        // user interface.
        if (xhr.upload) {
            xhr.upload.addEventListener('progress', evt => {
                if (evt.lengthComputable) {
                    loader.uploadTotal = evt.total;
                    loader.uploaded = evt.loaded;
                }
            });
        }
    }

    // Prepares the data and sends the request.
    _sendRequest(file) {

        // Prepare the form data.
        const data = new FormData();

        data.append('file', file);
        // Important note: This is the right place to implement security mechanisms
        // like authentication and CSRF protection. For instance, you can use
        // XMLHttpRequest.setRequestHeader() to set the request headers containing
        // the CSRF token generated earlier by your application.

        // Send the request.
        this.xhr.send(data);
    }

}
//#endregion


let theEditor;
function LoadCkEditor() {

    //let server;
    DecoupledDocumentEditor
        .create(document.querySelector('#editor'), {
            toolbar: ['heading', '|', 'bold', 'italic', 'numberedList', 'bulletedList', 'underline', 'alignment', 'outdent', 'indent', 'link', '|', 'fontFamily', 'fontSize', 'fontColor', 'fontBackgroundColor', '|', 'insertTable', 'imageUpload', 'mediaEmbed', 'removeFormat'],
            extraPlugins: [MyCustomUploadAdapterPlugin],
            mediaEmbed: { previewsInData: true },
            fontSize: {
                options: [
                    8,
                    9,
                    10,
                    11,
                    12,
                    13,
                    14,
                    16,
                    18, 24, 30, 36, 48, 60, 72, 96

                ]
            }
        })
        .then(editor => {
            const toolbarContainer = document.querySelector('main .toolbar-container');
            toolbarContainer.prepend(editor.ui.view.toolbar.element);
            theEditor = editor;
            editor.model.document.on('change:data', () => {

                if (getDataFromTheEditor() != '') {
                    $('#KBTopicsModel_Description').val(getDataFromTheEditor());
                    $(".content-container").removeClass("input-validation-error");

                    var areaddescribedby = $(this).attr('aria-describedby');
                    if ($(this).parents().find('.sidebar-content').attr('id') != "advsearchsidebar") {
                        $('#' + areaddescribedby).show();

                    }
                }
                else {
                    $('#KBTopicsModel_Description').val('');
                    $(".content-container").addClass("input-validation-error");
                }
            });
        })
        .catch(err => {
            console.error(err.stack);
        });
}

function getDataFromTheEditor() {
    return theEditor.getData();
}

function MyCustomUploadAdapterPlugin(theEditor) {
    theEditor.plugins.get('FileRepository').createUploadAdapter = (loader) => {
        // Configure the URL to the upload script in your back-end here!
        return new MyUploadAdapter(loader);
    };
}

//#region
//return only html
var support = (function () {
    if (!window.DOMParser) return false;
    var parser = new DOMParser();
    try {
        parser.parseFromString('x', 'text/html');
    } catch (err) {
        return false;
    }
    return true;
})();

var textToHTML = function (str) {

    // check for DOMParser support
    if (support) {
        var parser = new DOMParser();
        var doc = parser.parseFromString(str, 'text/html');
        return doc.body.innerHTML;
    }

    // Otherwise, create div and append HTML
    var dom = document.createElement('div');
    dom.innerHTML = str;
    return dom;

};
//#endregion