var StoreroomSearchTBL;
var run = false;
var siteid;
var selectCount = 0;
var CustomQueryDisplayId = 0;
//#region OnPageLoadJs
$(document).ready(function () {

    if (localStorage.getItem("sitevalue") == null || localStorage.getItem("sitevalue") == 0) {
        siteid = 0
        $("#ddlsitename").val("");
    }
    else {
        siteid = localStorage.getItem("sitevalue");
        $("#ddlsitename").val(localStorage.getItem("sitevalue"));
    }
    $(document).find('.select2picker').select2({});
    $(document).on('click', "#action", function () {
        $(".actionDrop").slideToggle();
    });
    $(".actionBar").fadeIn();
    $("#StoreroomGridAction :input").attr("disabled", "disabled");
    $(".actionDrop ul li a").click(function () {
        $(".actionDrop").fadeOut();
    });
    $(document).on('focusout', "#action", function () {
        $(".actionDrop").fadeOut();
    });
    $(document).find("#sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $(document).on('click', '#dismiss, .overlay', function () {
        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });

    $('#advsearchsidebar').find('select').val("").trigger('change');

    //#region Load Grid With Status
    var storeroomcurrentstatus = localStorage.getItem("CURRENTACTIVESTATUS");
    if (storeroomcurrentstatus != 'undefined' && storeroomcurrentstatus != null && storeroomcurrentstatus != "") {
        CustomQueryDisplayId = storeroomcurrentstatus;
        $('#storeroomsearchListul li').each(function (index, value) {
            if ($(this).attr('id') == CustomQueryDisplayId && $(this).attr('id') != '0') {
                $('#Storeroomsearchtitle').text($(this).text());
                $(".searchList li").removeClass("activeState");
                $(this).addClass('activeState');
            }
        });
    }
    else {
        CustomQueryDisplayId = "1";
        $('#Storeroomsearchtitle').text(getResourceValue("AlertActive"));
        $("#storeroomsearchListul li").first().addClass("activeState");
    }
    //#endregion
    generateStoreroomDataTable();
});
$(document).find('#sidebarCollapse').on('click', function () {
    $('#sidebar').addClass('active');
    $('.overlay').fadeIn();
    $('.collapse.in').toggleClass('in');
    $('a[aria-expanded=true]').attr('aria-expanded', 'false');
});
//#endregion

//#region Generate Storeroom func()
$(document).on('click', '#liPrint', function () {
    $(".buttons-print")[0].click();
    funcCloseExportbtn();
});
$(document).on('click', '#liCsv', function () {
    $(".buttons-csv")[0].click();
    funcCloseExportbtn();
});
$(document).on('click', '#liPdf', function () {
    $(".buttons-pdf")[0].click();
    funcCloseExportbtn();
});
$(document).on('click', "#liExcel", function () {
    $(".buttons-excel")[0].click();
    funcCloseExportbtn();
});
function generateStoreroomDataTable() {

    if ($(document).find('#StoreroomSearchTBL').hasClass('dataTable')) {
        StoreroomSearchTBL.destroy();
    }
    StoreroomSearchTBL = $("#StoreroomSearchTBL").DataTable({
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
                var filterinfoarray = getfilterinfoarray($("#txtColumnSearch"), $('#advsearchsidebar'));
                $.ajax({
                    "url": gridStateSaveUrl,
                    "data": {
                        GridName: "Storeroom_Search",
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
                    GridName: "Storeroom_Search",
                },
                "async": false,
                "dataType": "json",
                "success": function (json) {
                    if (json.LayoutInfo !== '') {
                        callback(JSON.parse(json.LayoutInfo));
                        if (json.FilterInfo !== '') {
                            setsearchui(JSON.parse(json.FilterInfo), $("#txtColumnSearch"), $(".filteritemcount"), $("#advsearchfilteritems"));
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
        sDom: 'Btlipr',
        buttons: [

            'excelHtml5',
            {
                extend: 'print'
            },
            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Storeroom List',
                extension: '.csv'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                css: 'display:none',
                title: 'Storeroom List',
                orientation: 'landscape',
                pageSize: 'A4'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/StoreroomSetup/GetGridDataforStoreroomSetup",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.Name = LRTrim($("#txtName").val());
                d.Description = LRTrim($("#txtDescription").val());
                d.SiteId = siteid;
                d.SearchText = LRTrim($("#txtColumnSearch").val());
                d.Case = CustomQueryDisplayId;
            },
            "dataSrc": function (result) {
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
        "columns":
            [
                {
                    "data": "Name",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": true,
                    "name": "0",
                    "mRender": function (data, type, row) {
                        return '<a class=lnk_storeroomdetails href="javascript:void(0)">' + data + '</a>';
                    }
                },
                { "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1", "className": "text-left", "visible": true, },
                { "data": "SiteName", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2", "className": "text-left", "visible": true, },
                {
                    "data": "InactiveFlag", "autoWidth": true, "bSearchable": true, "bSortable": false, "name": "3", "className": "text-center",
                    "mRender": function (data, type, row) {
                        if (data == false) {
                            return '<label class="m-checkbox m-checkbox--air m-checkbox--state-brand somax-checkbox lbl-innergrid-checkbox">' +
                                '<input type="checkbox" class="status" onclick="return false"><span></span></label>';
                        }
                        else {
                            return '<label class="m-checkbox m-checkbox--air m-checkbox--solid m-checkbox--state-brand somax-checkbox lbl-innergrid-checkbox">' +
                                '<input type="checkbox" checked="checked" class="status" onclick="return false"><span></span></label>';
                        }
                    }
                },


            ],
        columnDefs: [
            {
                targets: [0],
                className: 'noVis'
            },
        ],

        initComplete: function (settings, json) {
            SetPageLengthMenu();
            $("#StoreroomGridAction :input").removeAttr("disabled");
            $("#StoreroomGridAction :button").removeClass("disabled");
            DisableExportButton($("#StoreroomSearchTBL"), $(document).find('.import-export'));
        }
    });
};

var filterinfoarray = [];
function filterinfo(id, value) {
    this.key = id;
    this.value = value;
}
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
$(function () {

    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            dtTable = $("#StoreroomSearchTBL").DataTable();
            var currestsortedcolumn = $('#StoreroomSearchTBL').dataTable().fnSettings().aaSorting[0][0];
            var coldir = $('#StoreroomSearchTBL').dataTable().fnSettings().aaSorting[0][1];
            var colname = $('#StoreroomSearchTBL').dataTable().fnSettings().aoColumns[currestsortedcolumn].name;
            var _SiteId = siteid;
            var _Name = LRTrim($("#txtName").val());
            var _Description = LRTrim($("#txtDescription").val());
            var jsonResult = $.ajax({
                "url": "/StoreroomSetup/GetStoreroomPrintData",
                "type": "get",
                "datatype": "json",
                data: {
                    colname: colname,
                    coldir: coldir,
                    SiteId: _SiteId,
                    Name: _Name,
                    Description: _Description,
                    CustomQueryDisplayId: CustomQueryDisplayId,
                    SearchText: LRTrim($("#txtColumnSearch").val())
                },
                success: function (result) {
                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data;
            var visiblecolumnsIndex = $("#StoreroomSearchTBL thead tr th").map(function (key) {
                return this.getAttribute('data-th-index');
            }).get();
            var d = [];
            $.each(thisdata, function (index, item) {

                if (item.Name != null) {
                    item.Name = item.Name;
                }
                else {
                    item.Name = "";
                }
                if (item.Description != null) {
                    item.Description = item.Description;
                }
                else {
                    item.Description = "";
                }

                if (item.SiteName != null) {
                    item.SiteName = item.SiteName;
                }
                else {
                    item.SiteName = "";
                }

                if (item.InactiveFlag != null) {
                    if (item.InactiveFlag == true) {
                        item.InactiveFlag = "Yes";
                    }
                    else if (item.InactiveFlag == false) {
                        item.InactiveFlag = "No";
                    }
                }
                else {
                    item.InactiveFlag = "";
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
                header: $("#StoreroomSearchTBL thead tr th").map(function (key) {
                    return this.innerHTML;
                }).get()
            };
        }
    });
})
//#endregion

//#region Search
$('#btntextSearch').on('click', function () {
    run = true;
    clearAdvanceSearch();
    StoreroomSearchTBL.page('first').draw('page');
});

$("#ddlsitename").change(function () {
    run = true;
    siteid = $(this).val();
    localStorage.setItem("sitevalue", siteid);
    StoreroomSearchTBL.page('first').draw('page');

});
$("#btnStoreroomDataAdvSrch").on('click', function (e) {
    run = true;
    StoreroomAdvSearch();
    $('#sidebar').removeClass('active');
    $('.overlay').fadeOut();
    StoreroomSearchTBL.page('first').draw('page');
});
$(document).on('click', '.btnCross', function () {
    run = true;
    var btnCrossed = $(this).parent().attr('id');
    var searchtxtId = btnCrossed.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    selectCount--;
    StoreroomAdvSearch();
    StoreroomSearchTBL.page('first').draw('page');
});
$(document).on('click', '#liClearAdvSearchFilter', function () {
    run = true;
    $("#txtColumnSearch").val("");
    clearAdvanceSearch();
    StoreroomSearchTBL.page('first').draw('page');
});
function clearAdvanceSearch() {
    selectCount = 0;
    $('#advsearchsidebar').find('select').val("").trigger('change');
    $(document).find('#txtDescription').val("");
    $(document).find('#txtName').val("");
    $(".filteritemcount").text(selectCount);
    $('#advsearchfilteritems').find('span').html('');
    $('#advsearchfilteritems').find('span').removeClass('tagTo');
}
function StoreroomAdvSearch() {
    $('#txtColumnSearch').val('');
    var searchitemhtml = "";
    selectCount = 0;
    $('#advsearchsidebar').find('.adv-item').each(function (index, item) {
        if ($(this).hasClass('dtpicker')) {
            $(this).val(ValidateDate($(this).val()));
        }
        if ($(this).val()) {
            selectCount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $("#advsearchfilteritems").html(searchitemhtml);
    $(".filteritemcount").text(selectCount);
}
$(document).on('click', '.lnk_storeroomdetails', function (e) {
    e.preventDefault();
    var row = $(this).parents('tr');
    var data = StoreroomSearchTBL.row(row).data();
    var storeromId = data.StoreroomId;
    RedirectToDetail(storeromId);

});
//#endregion



//#region Common
function SetStoreroomEnvironmentPage() {
    $.validator.unobtrusive.parse(document);
    $('input, form').blur(function () {
        $(this).valid();
    });
    $('.select2picker, form').change(function () {
        var areaddescribedby = $(this).attr('aria-describedby');
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
    });
    $(document).find('.select2picker').select2({});
}
function RedirectToDetail(srID) {
    $.ajax({
        url: "/StoreroomSetup/StoreroomDetail",
        type: "POST",
        dataType: 'html',
        data: { StoreroomId: srID },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderStoreroomSetup').html(data);
        },
        complete: function () {
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
//#endregion


//#region Storeroom
$(document).on('click', "#editstoreroom", function () {
    var storeroomId = $('#storeroomModel_StoreroomId').val();
    $.ajax({
        url: "/StoreroomSetup/AddOrEditStoreroom",
        type: "GET",
        dataType: 'html',
        data: { StoreroomId: storeroomId, IsAdd: false },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderStoreroomSetup').html(data);
        },
        complete: function () {
            CloseLoader();
            SetStoreroomEnvironmentPage();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', "#addStoreroomFromIndex", function () {
    var srId = $('#storeroomModel_StoreroomId').val();
    $.ajax({
        url: "/StoreroomSetup/AddOrEditStoreroom",
        type: "GET",
        dataType: 'html',
        data: { StorroomId: srId, IsAdd: true },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderStoreroomSetup').html(data);
        },
        complete: function () {
            CloseLoader();
            SetStoreroomEnvironmentPage();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', "#btnCancelStoreroom", function () {
    var StoreroomId = $(document).find('#storeroomModel_StoreroomId').val();
    swal(CancelAlertSetting, function () {
        if (StoreroomId == 0) {
            window.location.href = "/StoreroomSetup/Index?page=StoreroomSetup";
        }
        else {
            RedirectToDetail(StoreroomId);
        }
    });
});
$(document).on('click', "#brdStoreroom", function () {
    var storeroomId = $(this).attr('data-val');
    RedirectToDetail(storeroomId);
});
function OnStoreroomAddOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        var message;
        if (data.Command == "save") {
            if (data.mode == "add") {
                SuccessAlertSetting.text = getResourceValue("StoreroomAddAlert");
            }
            else {
                SuccessAlertSetting.text = getResourceValue("StoreroomUpdateAlert");
            }
            swal(SuccessAlertSetting, function () {
                RedirectToDetail(data.StoreroomID);
            });
        }
        else {
            SuccessAlertSetting.text = getResourceValue("StoreroomAddAlert");
            ResetErrorDiv();
            swal(SuccessAlertSetting, function () {
                $(document).find('form').trigger("reset");
                $(document).find('form').find("select").val("").trigger('change');
                $(document).find('form').find("#storeroomModel_Name").removeClass("input-validation-error");
                $(document).find('form').find("#storeroomModel_SiteId").removeClass("input-validation-error");
                $(document).find('form').find("#storeroomModel_Description").removeClass("input-validation-error");
                $(document).find('form').find("input").removeClass("input-validation-error");
                $(document).find('form').find("textarea").removeClass("input-validation-error");
                $(document).find('form').find("select").removeClass("select-validation-error");
            });
        }
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}





$(document).on('click', '#StoreroomSearchTBL_paginate .paginate_button', function () {
    run = true;
});
$(document).on('change', '#StoreroomSearchTBL_length .searchdt-menu', function () {
    run = true;
});


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


$(document).on('keyup', '#storeroomsearchtxtbox', function (e) {
    var tagElems = $(document).find('#storeroomsearchListul').children();
    $(tagElems).hide();
    for (var i = 0; i < tagElems.length; i++) {
        var tag = $(tagElems).eq(i);
        if ($(tag).text().toLowerCase().includes($(this).val().toLowerCase()) == true || $(this).val().toLowerCase().includes($(tag).text().toLowerCase()) == true) {
            $(tag).show();
        }
    }
});
$(document).on('click', '.storeroomsearchdrpbox', function (e) {
    if ($(document).find('#txtColumnSearch').val() !== '')
        $("#advsearchfilteritems").html('');
    $(document).find('#txtColumnSearch').val('');
    run = true;
    if ($(this).attr('id') != '0') {
        $('#Storeroomsearchtitle').text($(this).text());
    }
    else {
        $('#Storeroomsearchtitle').text("Storeroom");
    }
    $(".searchList li").removeClass("activeState");
    $(this).addClass('activeState');
    $(document).find('#searcharea').hide("slide");
    var optionval = $(this).attr('id');
    localStorage.setItem("CURRENTACTIVESTATUS", optionval);
    if (optionval == '1') {
        CustomQueryDisplayId = 1;
    }
    else if (optionval == '2') {
        CustomQueryDisplayId = 2;
    }

    StoreroomSearchTBL.page('first').draw('page');
});

//#endregion


//#region New Search button
$(document).on('click', "#SrchBttnNew", function () {
    $.ajax({
        url: '/Base/PopulateNewSearchList',
        type: 'GET',
        data: { tableName: 'Storeroom' },
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
        data: { tableName: 'Storeroom', searchText: txtSearchval, isClear: isClear },
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
                StoreroomSearchTBL.page('first').draw('page');
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
    var storeroomcurrentstatus = localStorage.getItem("CURRENTACTIVESTATUS");
    if (storeroomcurrentstatus) {
        CustomQueryDisplayId = storeroomcurrentstatus;
        $('#storeroomsearchListul li').each(function (index, value) {
            if ($(this).attr('id') == CustomQueryDisplayId && $(this).attr('id') != '0') {
                $('#Storeroomsearchtitle').text($(this).text());
                $(".searchList li").removeClass("activeState");
                $(this).addClass('activeState');
            }
        });
    }
    else { $('#Storeroomsearchtitle').text(getResourceValue("AlertActive")); }
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
        generateStoreroomDataTable();
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

