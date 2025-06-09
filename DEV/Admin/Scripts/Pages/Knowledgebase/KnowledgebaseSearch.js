//#region Global Variable
var dtTable;
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
    $('#sampleDatepicker').datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: "dd/mm/yy",
        yearRange: "-90:+00"
    });
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
var order = '1';
var orderDir = 'asc';
function generateKnowledgebaseDataTable() {
    if ($(document).find('#kbtopicsSearch').hasClass('dataTable')) {
        dtTable.destroy();
    }
    dtTable = $("#kbtopicsSearch").DataTable({
        colReorder: {
            fixedColumnsLeft: 1
        },
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[1, "asc"]],
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
                d.Title = LRTrim($("#Title").val()),
                    d.Description = LRTrim($("#Description").val()),
                    //d.Tags = $("#ddlUser").val().join(),
                    d.Tags = $("#ddlUser").val(),
                    d.Folder = LRTrim($("#Folder").val()),
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
    var temp2 = "Select Type";
    $("#ddlCategory").val("").trigger('change');
    $("#ddlUser").val("").trigger('change');
    $('.adv-item').val("");
    $('.tagify__tag').remove();
}
$(document).on('click', '.btnCross', function () {
    run = true;
    var btnCrossed = $(this).parent().attr('id');
    var searchtxtId = btnCrossed.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    selectCount--;
    KBTopicsAdvSearch();
    $('.tagify__tag').remove();
    dtTable.page('first').draw('page');
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
    dtTable.page('first').draw('page');
});
//#endregion

//#region Add and Update
function checkformvalid() {
    $("#topicaddform").submit(function () {
        var messageLength = "";
        if ($(this).valid()) {

            messageLength = CKEDITOR.instances['KBTopicsModel_Description'].getData().replace(/<[^>]*>/gi, '').length;
            if (!messageLength) {
                $("#cke_KBTopicsModel_Description").addClass("input-validation-error");
            }
            else {
                $("#cke_KBTopicsModel_Description").removeClass("input-validation-error")
            }
            return;
        }
        else {
            var activetagid = $(document).find('.vtabs li.active').attr('id');
            messageLength = CKEDITOR.instances['KBTopicsModel_Description'].getData().replace(/<[^>]*>/gi, '').length;
            if (!messageLength) {
                $("#cke_KBTopicsModel_Description").addClass("input-validation-error");
            }
            else {
                $("#cke_KBTopicsModel_Description").removeClass("input-validation-error");
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
            
            LoadEditor();
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
                CKEDITOR.instances.KBTopicsModel_Description.setData('');
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
            $('#kbtopicsmaincontainer').html(data);
            TagInput();
            
            
            
           
        },
        complete: function () {
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
            dtTable = $("#kbtopicsSearch").DataTable();
            var info = dtTable.page.info();
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
            $('#kbtopicsmaincontainer').html(data);
            $(document).find('#spnlinkToSearch').text(titleText);
        },
        complete: function () {
            CloseLoader();
            SetFixedHeadStyle();

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
    var data = dtTable.row(row).data();
    $.ajax({
        url: "/Admin/Knowledgebase/KnowledgebaseDetails",
        type: "POST",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        data: { KBTopicsId: data.KBTopicsId },
        success: function (data) {
            $('#kbtopicsmaincontainer').html(data);
            $(document).find('#spnlinkToSearch').text(titleText);
        },
        complete: function () {
            CloseLoader();
            SetFixedHeadStyle();
        }
    });
});
//#endregion
//#region CKEditor

function LoadEditor() {
    for (var instanceName in CKEDITOR.instances)
        CKEDITOR.instances[instanceName].updateElement();

    CKEDITOR.config.removePlugins = 'elementspath';

    CKEDITOR.config.colorButton_colors = '1ABC9C,2ECC71,3498DB,9B59B6,4E5F70,F1C40F,' +
        '16A085,27AE60,2980B9,8E44AD,2C3E50,F39C12,' +
        'E67E22,E74C3C,ECF0F1,95A5A6,DDD,FFF,' +
        'D35400,C0392B,BDC3C7,7F8C8D,999,000';
    CKEDITOR.config.colorButton_enableAutomatic = false;
    CKEDITOR.config.colorButton_colorsPerRow = 6;
    // This is actually the default value.
    CKEDITOR.config.colorButton_backStyle = {
        element: 'span',
        styles: { 'background-color': '#(color)' }
    };
  
    CKEDITOR.config.colorButton_backStyle = {
        element: 'span',
        styles: { 'background-color': '#(color)' }
    };

    CKEDITOR.config.colorButton_foreStyle = {
        element: 'span',
        styles: { color: '#(color)' }
    };
      
    // allow these tags to accept classes
    CKEDITOR.config.extraAllowedContent = 'hr(*)';
    CKEDITOR.config.extraAllowedContent = 'audio(*)';
    CKEDITOR.config.extraAllowedContent = 'source(*)';
    CKEDITOR.config.extraAllowedContent = 'code';
    CKEDITOR.config.extraAllowedContent = 'a(*)';
    CKEDITOR.config.extraAllowedContent = 'iframe[*]';
  
    var editor=CKEDITOR.replace('KBTopicsModel_Description', {
        extraPlugins: 'justify,font,panelbutton,colorbutton,colordialog,liststyle,oembed,widget,widgetselection,clipboard,lineutils,dialog,image,videodetector,format,richcombo,uploadimage,uploadwidget,filetools,notificationaggregator,notification,filebrowser,popup,floatpanel,panel,listblock,button',
        codeSnippet_theme: 'monokai_sublime',
        toolbar: [
            { name: 'document', items: ['Source'] },
            { name: 'basicstyles', items: ['Bold', 'Italic', 'Underline','CopyFormatting','RemoveFormat'] },
            { name: 'paragraph', items: ['NumberedList', 'BulletedList', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock', '-', 'richcombo'] },
            { name: 'styles', items: ['Font','Format', 'FontSize'] },
            { name: 'colors', items: ['TextColor', 'BGColor'] },
            { name: 'links', items: ['Link', 'Unlink'] },
            { name: 'insert', items: ['Flash', 'Table', 'VideoDetector', 'uploadimage','Image'] }

        ],
        on:
        {
            instanceReady: function (ev) {
                $('#lblTopicDesc').css('height', '270px');
                //set keyup event
                this.document.on("keyup", function () { CKEDITOR.instances['KBTopicsModel_Description'].updateElement(); });
                //and paste event
                this.document.on("paste", function () { CKEDITOR.instances['KBTopicsModel_Description'].updateElement(); });
                this.document.on("keypress", function () { CKEDITOR.instances['KBTopicsModel_Description'].updateElement(); });
                this.document.on("blur", function () { CKEDITOR.instances['KBTopicsModel_Description'].updateElement(); });
                this.document.on("change", function () { CKEDITOR.instances['KBTopicsModel_Description'].updateElement(); });
                this.focus();
            },
            change: function (ev) {
               // var messageLength = CKEDITOR.instances['KBTopicsModel_Description'].getData().replace(/<[^>]*>/gi, '').length;
                var messageLength = CKEDITOR.instances['KBTopicsModel_Description'].getData().length;
                if (!messageLength) {
                    $("#cke_KBTopicsModel_Description").addClass("input-validation-error");
                    return false;
                }
                else {
                    CKEDITOR.instances['KBTopicsModel_Description'].updateElement();
                    $("#cke_KBTopicsModel_Description").removeClass("input-validation-error");
                    $("#KBTopicsModel_Description").removeClass("input-validation-error");
                    return true;
                }
                    //$("#cke_KBTopicsModel_Description").removeClass("input-validation-error");
                    //return true;
            }
        }
    });
    editor.addCommand('videodetector', new CKEDITOR.dialogCommand('videoDialog'));
    editor.ui.addButton && editor.ui.addButton('VideoDetector', {
        label: 'Insert a Youtube, Vimeo or Dailymotion video',
        command: 'videodetector'
    });
}

//#endregion

//#region ColumnVisibility
$(document).on('click', '#liCustomize', function () {
    funCustomizeBtnClick(dtTable);
});
$(document).on('click', '.saveConfig', function () {
    var colOrder = [0, 1];
    funCustozeSaveBtn(dtTable, colOrder);
    run = true;
    dtTable.state.save(run);
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
                //$.each(tagify.value, function (index, item) {
                //    tagify.settings.whitelist.push(item, item.value);
                    
                //});

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

