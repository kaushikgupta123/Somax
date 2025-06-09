var dtClientTable;
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
var gridname = "Client_Search";
function filterinfo(id, value) {
    this.key = id;
    this.value = value;
}

$.validator.setDefaults({ ignore: null });
$(document).ready(function () {
    ShowbtnLoaderclass("LoaderDrop");
    $("#ClientGridAction :input").attr("disabled", "disabled");
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
    generateClientDataTable();
    $(".actionDrop2 ul li a").click(function () {
        $(".actionDrop2").fadeOut();
    });
    SetFixedHeadStyle();
    $(document).on('click', '.link_client_detail', function (e) {
        e.preventDefault();
        titleText = $('#clientsearchtitle').text();
        var index_row = $('#clientSearch tr').index($(this).closest('tr')) - 1;
        var row = $(this).parents('tr');
        var td = $(this).parents('tr').find('td');
        var data = dtClientTable.row(row).data();
        $.ajax({
            url: "/Admin/Client/ClientDetails",
            type: "POST",
            dataType: "html",
            beforeSend: function () {
                ShowLoader();
            },
            data: { ClientId: data.ClientId },
            success: function (data) {

                $('#clientmaincontainer').html(data);
                $(document).find('#spnlinkToSearch').text(titleText);
            },
            complete: function () {
                SetClientDetailEnvironment();
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
    $(document).on('click', "#btnCancelAddClient", function () {
        swal(CancelAlertSetting, function () {
            window.location.href = "/Admin/Client/Index?page=Clients";
        });
    });

    //#region Add 
    $(document).on('click', '.AddClient', function () {
        $.ajax({
            url: "/Admin/Client/AddClient",
            type: "GET",
            dataType: 'html',
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                $('#clientmaincontainer').html(data);
            },
            complete: function () {
                SetClientControls();
            },
            error: function () {
                CloseLoader();
            }
        });
    });

    //#endregion
    //#region Edit 
    $(document).on('click', "#btnclientedit", function () {
        var clientid = LRTrim($(document).find('#ClientModel_ClientId').val());
        $.ajax({
            url: '/Admin/Client/EditClient',
            data: { ClientId: clientid },
            type: "POST",
            datatype: "html",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                $('#clientmaincontainer').html(data);
            },
            complete: function () {
                CloseLoader();
                $.validator.unobtrusive.parse(document);
                $('.select2picker, form').change(function () {
                    var areaddescribedby = $(this).attr('aria-describedby');
                    if (typeof areaddescribedby != 'undefined') {
                        $('#' + areaddescribedby).show();
                    }
                });
                $(document).find('.select2picker').select2({});
                SetFixedHeadStyle();
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
            if (errorTab == "Identifications") {
                $('#ClientInformationtab').trigger('click');
            }
            else if (errorTab == "DefualtLocalizationInformation") {
                $('#DefualtLocalizationInformationtab').trigger('click');
            }
            else if (errorTab == "DefaultUIConfigurationInformation") {
                $('#DefaultUIConfigurationInformationtab').trigger('click');
            }
            else if (errorTab == "SystemInformation") {
                $('#SystemInformationtab').trigger('click');
            }
        }
    });

    $(document).find('.select2picker').select2({});
    var clientstatus = localStorage.getItem("CLIENTSEARCHGRIDDISPLAYSTATUS");
    if (clientstatus) {
        activeStatus = clientstatus;
        $('#clientsearchListul li').each(function (index, value) {
            if ($(this).attr('id') == activeStatus && $(this).attr('id') != '0') {
                $('#clientsearchtitle').text($(this).text());
                $(".searchList li").removeClass("activeState");
                $(this).addClass('activeState');
            }
        });
    }
    else {
        localStorage.setItem("CLIENTSEARCHGRIDDISPLAYSTATUS", "1");
        clientstatus = localStorage.getItem("CLIENTSEARCHGRIDDISPLAYSTATUS");
        activeStatus = clientstatus;
        $('#clientsearchListul li').first().addClass('activeState');
        $('#clientsearchtitle').text(getResourceValue("AlertActive"));
    }
});

//#region Search
function generateClientDataTable() {
    if ($(document).find('#clientSearch').hasClass('dataTable')) {
        dtClientTable.destroy();
    }
    dtClientTable = $("#clientSearch").DataTable({
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
                title: 'Client List'
            },
            {
                extend: 'print',
                title: 'Client List'
            },

            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Client List',
                extension: '.csv'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                exportOptions: {
                    columns: ':visible'
                },
                css: 'display:none',
                title: 'Client List',
                orientation: 'landscape',
                pageSize: 'A3'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/Admin/Client/GetClientGridData",
            "type": "post",
            "datatype": "json",
            cache: false,
            data: function (d) {
                d.customQueryDisplayId = localStorage.getItem("CLIENTSEARCHGRIDDISPLAYSTATUS");
                d.ClientId = LRTrim($("#ClientID").val());
                d.Name = LRTrim($("#Name").val());
                d.Contact = LRTrim($("#Contact").val());
                d.Mail = LRTrim($("#Email").val());
                d.SearchText = LRTrim($(document).find('#txtColumnSearch').val());
                d.order = order;
                d.orderDir = orderDir;
            },
            "dataSrc": function (result) {
                searchcount = result.recordsTotal;
                if (result.data.length < 1) {
                    $(document).find('#btnClientExport').prop('disabled', true);
                }
                else {
                    $(document).find('#btnClientExport').prop('disabled', false);
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
        "columns":
        [
            {
                "data": "ClientId",
                "autoWidth": false,
                "bSearchable": true,
                "bSortable": false,
                className: 'text-left',
                "name": "0",
                "mRender": function (data, type, row) {
                    return '<a class=link_client_detail href="javascript:void(0)">' + data + '</a>';
                }
            },
            {
                "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": false, "name": "1",
                mRender: function (data, type, full, meta) {
                    return "<div class='text-wrap width-200'>" + data + "</div>";
                }
            },
            {
                "data": "Contact", "autoWidth": true, "bSearchable": true, "bSortable": false, "name": "2",
                mRender: function (data, type, full, meta) {
                    return "<div class='text-wrap width-200'>" + data + "</div>";
                }
            },
            { "data": "Email", "autoWidth": true, "bSearchable": true, "bSortable": false, "name": "3" },
            { "data": "BusinessType", "autoWidth": true, "bSearchable": true, "bSortable": false },
            { "data": "PackageLevel", "autoWidth": true, "bSearchable": true, "bSortable": false },
            { "data": "CreateDate", "autoWidth": true, "bSearchable": true, "bSortable": false }



        ],
        columnDefs: [
            {
                targets: [0],
                className: 'noVis'
            }
        ],
        initComplete: function () {
            SetPageLengthMenu();
            var currestsortedcolumn = $('#clientSearch').dataTable().fnSettings().aaSorting[0][0];
            var currestsortedorder = $('#clientSearch').dataTable().fnSettings().aaSorting[0][1];
            var column = this.api().column(currestsortedcolumn);
            var columnId = $(column.header()).attr('id');
            $(document).find('.srtclientcolumn').removeClass('sort-active');
            $(document).find('.srtclientcolumnorder').removeClass('sort-active');
            switch (columnId) {
                case "thClientId":
                    $(document).find('.srtclientcolumn').eq(0).addClass('sort-active');
                    break;
                case "thName":
                    $(document).find('.srtclientcolumn').eq(1).addClass('sort-active');
                    break;
                case "thContact":
                    $(document).find('.srtclientcolumn').eq(2).addClass('sort-active');
                    break;
                default:
                    $(document).find('.srtclientcolumn').eq(0).addClass('sort-active');
                    break;
            }
            switch (currestsortedorder) {
                case "asc":
                    $(document).find('.srtclientcolumnorder').eq(0).addClass('sort-active');
                    break;
                case "desc":
                    $(document).find('.srtclientcolumnorder').eq(1).addClass('sort-active');
                    break;
            }
            $('#btnsortmenu').text(getResourceValue("spnSorting") + " : " + column.header().innerHTML);
            $("#ClientGridAction :input").removeAttr("disabled");
            $("#ClientGridAction :button").removeClass("disabled");
            DisableExportButton($("#clientSearch"), $(document).find('#btnClientExport'));
        }
    });
}
$(document).on('click', '#clientSearch_paginate .paginate_button', function () {
    run = true;
});
$(document).on('change', '#clientSearch_length .searchdt-menu', function () {
    run = true;
});
$(document).find('.srtclientcolumn').click(function () {
    ShowbtnLoader("btnsortmenu");
    order = $(this).data('col');
    dtClientTable.page('first').draw('page');
    $('#btnsortmenu').text(getResourceValue("spnSorting") + " : " + $(this).text());
    $(document).find('.srtclientcolumn').removeClass('sort-active');
    $(this).addClass('sort-active');
    run = true;
});
$(document).find('.srtclientcolumnorder').click(function () {
    ShowbtnLoader("btnsortmenu");
    orderDir = $(this).data('mode');
    dtClientTable.page('first').draw('page');
    $(document).find('.srtclientcolumnorder').removeClass('sort-active');
    $(this).addClass('sort-active');
    run = true;
});
//#endregion

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
$(function () {
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            var clientd = LRTrim($("#ClientID").val());
            var name = LRTrim($("#Name").val());
            var contact = LRTrim($("#Contact").val());
            var email = LRTrim($("#Email").val());
            dtClientTable = $("#clientSearch").DataTable();
            var info = dtClientTable.page.info();
            var start = info.start;
            var lengthMenuSetting = info.length;
            var length = $('#clientSearch').dataTable().length;
            var searchText = LRTrim($(document).find('#txtColumnSearch').val());
            var jsonResult = $.ajax({
                "url": "/Admin/Client/GetClientPrintData",
                "type": "get",
                "datatype": "json",
                data: {
                    customQueryDisplayId: localStorage.getItem("CLIENTSEARCHGRIDDISPLAYSTATUS"),
                    start: start,
                    length: lengthMenuSetting,
                    _ClientId: clientd,
                    _Name: name,
                    _Contact: contact,
                    _Mail: email,
                    _colname: order,
                    _coldir: orderDir,
                    _searchText: searchText
                },
                success: function (result) {
                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data;
            var visiblecolumnsIndex = $("#clientSearch thead tr th").map(function (key) {
                return this.getAttribute('data-th-index');
            }).get();
            var d = [];
            $.each(thisdata, function (index, item) {
                if (item.ClientId != null) {
                    item.ClientId = item.ClientId;
                }
                else {
                    item.ClientId = 0;
                }
                if (item.Name != null) {
                    item.Name = item.Name;
                }
                else {
                    item.Name = "";
                }
                if (item.Contact != null) {
                    item.Contact = item.Contact;
                }
                else {
                    item.Email = "";
                }
                if (item.Email != null) {
                    item.Email = item.Email;
                }
                else {
                    item.Email = "";
                }
                if (item.BusinessType != null) {
                    item.BusinessType = item.BusinessType;
                }
                else {
                    item.BusinessType = "";
                }
                if (item.CreateDate != null) {
                    item.CreateDate = item.CreateDate;
                }
                else {
                    item.CreateDate = "";
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
                header: $("#clientSearch thead tr th").find('div').map(function (key) {
                    return this.innerHTML;
                }).get()
            };
        }
    });

});
//#endregion

//#region Advance Search
function ClientAdvSearch() {
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
$("#btnClientDataAdvSrch").on('click', function (e) {
    run = true;
    $(document).find('#txtColumnSearch').val('');
    $('#sidebar').removeClass('active');
    $('.overlay').fadeOut();
    ClientAdvSearch();
    dtClientTable.page('first').draw('page');
});
$(document).on('click', '.btnCross', function () {
    run = true;
    var btnCrossed = $(this).parent().attr('id');
    var searchtxtId = btnCrossed.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    selectCount--;
    ClientAdvSearch();
    dtClientTable.page('first').draw('page');
});
//#endregion

function RedirectToClientDetail(ClientId, mode) {
    $.ajax({
        url: "/Admin/Client/ClientDetails",
        type: "POST",
        dataType: 'html',
        data: { ClientId: ClientId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#clientmaincontainer').html(data);
            $(document).find('#spnlinkToSearch').text(titleText);
            if (titleText == "Active") {
                localStorage.setItem("CLIENTSEARCHGRIDDISPLAYSTATUS", "1");
            }
            else {
                localStorage.setItem("CLIENTSEARCHGRIDDISPLAYSTATUS", "2");
            }
        },
        complete: function () {
            SetClientDetailEnvironment();
            if (mode === "AzureImageReload") {
                $('#Clienttab').hide();
                $('.tabcontent2').hide();
                $('#auditlogcontainer').hide();
                $('.imageDropZone').show();
                $(document).find('#btnnblock').removeClass("col-xl-6");
                $(document).find('#btnnblock').addClass("col-xl-12");
            }
        },
        error: function () {
            CloseLoader();
        }
    });
}
function SetClientDetailEnvironment() {
    CloseLoader();
    SetFixedHeadStyle();
}
function ClientEditOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("ClientUpdateAlert");
        swal(SuccessAlertSetting, function () {
            if (data.Status == "Active") {
                titleText = getResourceValue("AlertActive");
            }
            else {
                titleText = getResourceValue("AlertInactive");
            }
            RedirectToClientDetail(data.ClientId, "client");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
function ClientAddOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("ClientAddAlert");
        if (data.Command == "save") {
            swal(SuccessAlertSetting, function () {
                if (data.Status == "Active") {
                    titleText = getResourceValue("AlertActive");
                }
                else {
                    titleText = getResourceValue("AlertInactive");
                }
                RedirectToClientDetail(data.ClientId, "client");
            });
        }
        else {
            ResetErrorDiv();
            $(document).find('#ClientInformationtab').addClass('active').trigger('click');
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
function SetClientControls() {
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
    SetFixedHeadStyle();
}


//#region ColumnVisibility
$(document).on('click', '#liCustomize', function () {
    funCustomizeBtnClick(dtClientTable);
});
$(document).on('click', '.saveConfig', function () {
    var colOrder = [0];
    funCustozeSaveBtn(dtClientTable, colOrder);
    run = true;
    dtClientTable.state.save(run);
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

//#region New Search button
$(document).on('keyup', '#clientsearctxtbox', function (e) {
    var tagElems = $(document).find('#clientsearchListul').children();
    $(tagElems).hide();
    for (var i = 0; i < tagElems.length; i++) {
        var tag = $(tagElems).eq(i);
        if ($(tag).text().toLowerCase().includes($(this).val().toLowerCase()) == true || $(this).val().toLowerCase().includes($(tag).text().toLowerCase()) == true) {
            $(tag).show();
        }
    }
});
$(document).on('click', '.clientsearchdrpbox', function (e) {
    run = true;
    if ($(document).find('#txtColumnSearch').val() !== '')
        $("#dvFilterSearchSelect2").html('');
    $(document).find('#txtColumnSearch').val('');
    run = true;
    if ($(this).attr('id') != '0') {
        $('#clientsearchtitle').text($(this).text());
    }
    else {
        $('#clientsearchtitle').text("Client");
    }
    $(".searchList li").removeClass("activeState");
    $(this).addClass('activeState');
    $(document).find('#searcharea').hide("slide");
    var optionval = $(this).attr('id');
    localStorage.setItem("CLIENTSEARCHGRIDDISPLAYSTATUS", optionval);
    if (optionval.length !== 0) {
        ShowbtnLoaderclass("LoaderDrop");
        dtClientTable.page('first').draw('page');
    }
});
$(document).on('click', "#SrchBttnNew", function () {
    $.ajax({
        url: '/Admin/Base/PopulateNewSearchList',
        type: 'GET',
        data: { tableName: 'Client' },
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
        data: { tableName: 'Client', searchText: txtSearchval, isClear: isClear },
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
                dtClientTable.page('first').draw('page');
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
        generateClientDataTable();
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
    window.location.href = "../Client/Index?page=Clients";
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

//#region TestConnectionStrings
$(document).on('click', '#btnTestConnection', function () {
    if ($("#ClientModel_ConnectionString").val() !== "") {
        $.ajax({
            url: "/Admin/Client/TestConnectionString",
            type: "POST",
            dataType: 'json',
            data: { conStr: $("#ClientModel_ConnectionString").val() },
            beforeSend: function () {
                ShowLoader();
            },

            success: function (data) {
                if (data.Result == 'success') {
                    var msgText = getResourceValue('ConnectionStringValidationtrueMsg');
                    SuccessAlertSetting.text = msgText;
                    swal(SuccessAlertSetting, function () {
                    });
                }
                if (data.Result == 'failed') {
                    var msgText = getResourceValue('ConnectionStringValidationfalseMsg');
                    ErrorAlertSetting.text = msgText;
                    swal(ErrorAlertSetting, function () {
                    });
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
    else {
        var msgText = getResourceValue('ConnectionStringErrorMsg');
        ErrorAlertSetting.text = msgText;
        swal(ErrorAlertSetting, function () {
        });
    }
});
$(document).on('click', '#SiteControl', function (e) {
    $(document).find('#ClientModel_MaxAppUsers').val('');
    $(document).find('#ClientModel_MaxWorkRequestUsers').val('');
    $(document).find('#ClientModel_MaxLimitedUsers').val('');
    $(document).find('#ClientModel_MaxSuperUsers').val('');
    var checked = this.checked;
    if (checked) {
        $('#ClientModel_MaxAppUsers').prop("readonly", true).addClass('readonly').removeClass('input-validation-error');
        $('#ClientModel_MaxWorkRequestUsers').prop("readonly", true).addClass('readonly');
        $('#ClientModel_MaxLimitedUsers').prop("readonly", true).addClass('readonly').removeClass('input-validation-error');
        $('#ClientModel_MaxSuperUsers').prop("readonly", true).addClass('readonly');       
        $(document).find("#MaxAppUsersReqId").css('display', 'none');      
        $(document).find("#MaxLimitedUsersReqId").css('display', 'none');
       
    } else {
        $('#ClientModel_MaxAppUsers').prop("readonly", false).removeClass('readonly');
        $('#ClientModel_MaxWorkRequestUsers').prop("readonly", false).removeClass('readonly');
        $('#ClientModel_MaxLimitedUsers').prop("readonly", false).removeClass('readonly');
        $('#ClientModel_MaxSuperUsers').prop("readonly", false).removeClass('readonly');       
        $(document).find("#MaxAppUsersReqId").css('display', 'Inline');  
        $(document).find("#MaxLimitedUsersReqId").css('display', 'Inline');     
    }
});

//#endregion