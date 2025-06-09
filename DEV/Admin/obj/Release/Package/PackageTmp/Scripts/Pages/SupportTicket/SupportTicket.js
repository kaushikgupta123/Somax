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
var gridname = "SupportTicket_Search";
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
    generateSupportTicketDataTable();
    $(".actionDrop2 ul li a").click(function () {
        $(".actionDrop2").fadeOut();
    });

    $(document).on('click', '#sidebarCollapse', function () {
        $('#sidebar').addClass('active');
        $('.overlay').fadeIn();
        $('.collapse.in').toggleClass('in');
        $('a[aria-expanded=true]').attr('aria-expanded', 'false');
        $(document).find('.dtpicker').datepicker({
            changeMonth: true,
            changeYear: true,
            "dateFormat": "mm/dd/yy",
            autoclose: true
        });
    });
    SetFixedHeadStyle();
    $(document).on('click', '.link_SupportTicket_detail', function (e) {
        e.preventDefault();
        titleText = $('#stsearchtitle').text();
        var index_row = $('#SupportTicketSearch tr').index($(this).closest('tr')) - 1;
        var row = $(this).parents('tr');
        var td = $(this).parents('tr').find('td');
        var data = dtClientTable.row(row).data();
        var ClientId = data.ClientId;
        $.ajax({
            url: "/Admin/SupportTicket/TicketDetails",
            type: "POST",
            dataType: "html",
            beforeSend: function () {
                ShowLoader();
            },
            data: { SupportTicketId: data.SupportTicketId, ClientId: ClientId },
            success: function (data) {

                $('#supportticketmaincontainer').html(data);
                $(document).find('#spnlinkToSearch').text(titleText);
            },
            complete: function () {
                LoadTicketResponses(data.SupportTicketId);
                SetDetailEnvironment();
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
        var ClientId = $(document).find('#EditPageClientId').val();
        var SupportTicketId = $('#SupportTicketModel_SupportTicketId').val();
        swal(CancelAlertSetting, function () {
            if (SupportTicketId != 0) {
                RedirectToTicketDetail(SupportTicketId, ClientId);
            }
            else {
                window.location.href = "/Admin/SupportTicket/Index?page=SupportTicket";
            }
        });
    });

    //#region Add 
    $(document).on('click', '.AddSupportTicket', function () {
        $.ajax({
            url: "/Admin/SupportTicket/AddSupportTicket",
            type: "GET",
            dataType: 'html',
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                $('#supportticketmaincontainer').html(data);
                TagInput();
                LoadCkEditorDesc();
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

    //#endregion
    //#region Edit 
    $(document).on('click', "#btnSTEdit", function () {
        var ClientId = $(document).find('#SupportTicketModel_ClientId').val();
        var SupportTicketId = $('#SupportTicketModel_SupportTicketId').val();
        $.ajax({
            url: '/Admin/SupportTicket/EditSupportTicket',
            data: { SupportTicketId: SupportTicketId, ClientId: ClientId },
            type: "GET",
            datatype: "html",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                $('#supportticketmaincontainer').html(data);
                var SiteId = $('#EditPageSiteId').val();
                GetSites(ClientId);
                GetPersonnelList(ClientId, SiteId);
                TagInput();
                LoadCkEditorDesc();
            },
            complete: function () {
                checkformEditvalid();
                SetControls();
            }
        });
    });

    $(document).on('click', "#brdsupportticket", function () {
        var SupportTicketId = $(this).attr('data-val');
        var ClientId = $(document).find('#EditPageClientId').val();
        RedirectToTicketDetail(SupportTicketId, ClientId);
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
    var clientstatus = localStorage.getItem("SUPPORTTICKETSEARCHGRIDDISPLAYSTATUS");
    if (clientstatus) {
        activeStatus = clientstatus;
        $('#stsearchListul li').each(function (index, value) {
            if ($(this).attr('id') == activeStatus && $(this).attr('id') != '0') {
                $('#stsearchtitle').text($(this).text());
                $(".searchList li").removeClass("activeState");
                $(this).addClass('activeState');
                titleText = $('#stsearchtitle').text();
            }
        });
    }
    else {
        localStorage.setItem("SUPPORTTICKETSEARCHGRIDDISPLAYSTATUS", "3");
        clientstatus = localStorage.getItem("SUPPORTTICKETSEARCHGRIDDISPLAYSTATUS");
        activeStatus = clientstatus;
        $('#stsearchListul li').first().addClass('activeState');
        //$('#stsearchtitle').text(getResourceValue("AlertActive"));
        titleText = $('#stsearchtitle').text();
    }
});

//#region Search
function generateSupportTicketDataTable() {
    if ($(document).find('#SupportTicketSearch').hasClass('dataTable')) {
        dtClientTable.destroy();
    }
    dtClientTable = $("#SupportTicketSearch").DataTable({
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
            "url": "/Admin/SupportTicket/GetSupportTicketGridData",
            "type": "post",
            "datatype": "json",
            cache: false,
            data: function (d) {
                d.customQueryDisplayId = localStorage.getItem("SUPPORTTICKETSEARCHGRIDDISPLAYSTATUS");
                d.SupportTicketId = LRTrim($("#SupportTicketId").val());
                d.Subject = LRTrim($("#Subject").val());
                d.Contact = LRTrim($("#Contact").val());
                d.Status = LRTrim($("#Status").val());
                d.Agent = LRTrim($("#Agent").val());
                d.CreateDate = ValidateDate($("#Created").val());
                d.CompleteDate = ValidateDate($("#Completed").val());
                d.SearchText = LRTrim($(document).find('#txtColumnSearch').val());
                d.Order = order;
                //d.orderDir = orderDir;
            },
            "dataSrc": function (result) {
                searchcount = result.recordsTotal;
                if (result.data.length < 1) {
                    $(document).find('#btnSTExport').prop('disabled', true);
                }
                else {
                    $(document).find('#btnSTExport').prop('disabled', false);
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
                    "data": "SupportTicketId",
                    "autoWidth": false,
                    "bSearchable": true,
                    "bSortable": true,
                    className: 'text-left',
                    "name": "0",
                    "mRender": function (data, type, row) {
                        return '<a class=link_SupportTicket_detail href="javascript:void(0)">' + data + '</a>';
                    }
                },
                {
                    "data": "Subject", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1",
                    mRender: function (data, type, full, meta) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                {
                    "data": "Contact", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2",
                    mRender: function (data, type, full, meta) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                { "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3" },
                { "data": "Agent", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "CreateDate", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "CompleteDate", "autoWidth": true, "bSearchable": true, "bSortable": true }



            ],
        columnDefs: [
            {
                targets: [0],
                className: 'noVis'
            }
        ],
        initComplete: function () {
            SetPageLengthMenu();
            $("#ClientGridAction :input").removeAttr("disabled");
            $("#ClientGridAction :button").removeClass("disabled");
            DisableExportButton($("#SupportTicketSearch"), $(document).find('#btnSTExport'));
        }
    });
}
$(document).on('click', '#SupportTicketSearch_paginate .paginate_button', function () {
    run = true;
});
$(document).on('change', '#SupportTicketSearch_length .searchdt-menu', function () {
    run = true;
});
$('#SupportTicketSearch').find('th').click(function () {
    if ($(this).data('col') !== undefined && $(this).data('col') !== '') {
        run = true;
        order = $(this).data('col');
    }
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
            dtClientTable = $("#SupportTicketSearch").DataTable();
            var info = dtClientTable.page.info();
            var start = info.start;
            var lengthMenuSetting = info.length;
            var length = $('#SupportTicketSearch').dataTable().length;
            var searchText = LRTrim($(document).find('#txtColumnSearch').val());
            var colname = order;
            var coldir = orderDir;
            var jsonResult = $.ajax({
                "url": "/Admin/SupportTicket/GetSupportTicketPrintData",
                "type": "get",
                "datatype": "json",
                data: {
                    CustomQueryDisplayId: localStorage.getItem("SUPPORTTICKETSEARCHGRIDDISPLAYSTATUS"),
                    SupportTicketId: LRTrim($("#SupportTicketId").val()),
                    Subject: LRTrim($("#Subject").val()),
                    Status: LRTrim($("#Status").val()),
                    Contact: LRTrim($("#Contact").val()),
                    Agent: LRTrim($("#Agent").val()),
                    CreateDate: ValidateDate($("#Created").val()),
                    CompleteDate: ValidateDate($("#Completed").val()),
                    SearchText: LRTrim($(document).find('#txtColumnSearch').val()),
                    Order: order,
                    coldir: coldir
                },
                success: function (result) {
                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data;
            var visiblecolumnsIndex = $("#SupportTicketSearch thead tr th").map(function (key) {
                return this.getAttribute('data-th-index');
            }).get();
            var d = [];
            $.each(thisdata, function (index, item) {
                if (item.SupportTicketId != null) {
                    item.SupportTicketId = item.SupportTicketId;
                }
                else {
                    item.SupportTicketId = 0;
                }
                if (item.Subject != null) {
                    item.Subject = item.Subject;
                }
                else {
                    item.Subject = "";
                }
                if (item.Contact != null) {
                    item.Contact = item.Contact;
                }
                else {
                    item.Contact = "";
                }
                if (item.Status != null) {
                    item.Status = item.Status;
                }
                else {
                    item.Status = "";
                }
                if (item.Agent != null) {
                    item.Agent = item.Agent;
                }
                else {
                    item.Agent = "";
                }
                if (item.CreateDate != null) {
                    item.CreateDate = item.CreateDate;
                }
                else {
                    item.CreateDate = "";
                }
                if (item.CompleteDate != null) {
                    item.CompleteDate = item.CompleteDate;
                }
                else {
                    item.CompleteDate = "";
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
                header: $("#SupportTicketSearch thead tr th").find('div').map(function (key) {
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
        if ($(this).hasClass('dtpicker')) {
            $(this).val(ValidateDate($(this).val()));
        }
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

function RedirectToTicketDetail(SupportTicketId, ClientId) {
    $.ajax({
        url: "/Admin/SupportTicket/TicketDetails",
        type: "POST",
        dataType: 'html',
        data: { SupportTicketId: SupportTicketId, ClientId: ClientId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#supportticketmaincontainer').html(data);
            $(document).find('#spnlinkToSearch').text(titleText);
        },
        complete: function () {
            LoadTicketResponses(SupportTicketId);
            SetDetailEnvironment();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function SetDetailEnvironment() {
    CloseLoader();
    SetFixedHeadStyle();
}
function SupportTicketAddOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("SupportTicketAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("SupportTicketUpdateAlert");
        }
        if (data.Command == "save") {
            swal(SuccessAlertSetting, function () {
                ClientId = data.ClientId;
                RedirectToTicketDetail(data.SupportTicketId, ClientId);
            });
        }
        else {
            ResetErrorDiv();
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
function SetControls() {
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
$(document).on('keyup', '#stsearctxtbox', function (e) {
    var tagElems = $(document).find('#stsearchListul').children();
    $(tagElems).hide();
    for (var i = 0; i < tagElems.length; i++) {
        var tag = $(tagElems).eq(i);
        if ($(tag).text().toLowerCase().includes($(this).val().toLowerCase()) == true || $(this).val().toLowerCase().includes($(tag).text().toLowerCase()) == true) {
            $(tag).show();
        }
    }
});
$(document).on('click', '.stsearchdrpbox', function (e) {
    run = true;
    if ($(document).find('#txtColumnSearch').val() !== '')
        $("#dvFilterSearchSelect2").html('');
    $(document).find('#txtColumnSearch').val('');
    run = true;
    if ($(this).attr('id') != '0') {
        $('#stsearchtitle').text($(this).text());
    }
    else {
        $('#stsearchtitle').text("Client");
    }
    $(".searchList li").removeClass("activeState");
    $(this).addClass('activeState');
    $(document).find('#searcharea').hide("slide");
    var optionval = $(this).attr('id');
    localStorage.setItem("SUPPORTTICKETSEARCHGRIDDISPLAYSTATUS", optionval);
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
        generateSupportTicketDataTable();
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
    window.location.href = "../SupportTicket/Index?page=SupportTicket";
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

//#region Editor
let theEditor="";
function LoadCkEditorDesc() {

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
                    $('#SupportTicketModel_Description').val(getDataFromTheEditor());
                    $('#STNotesModel_Content').val(getDataFromTheEditor());
                    $(".content-container").removeClass("input-validation-error");

                    var areaddescribedby = $(this).attr('aria-describedby');
                    if ($(this).parents().find('.sidebar-content').attr('id') != "advsearchsidebar") {
                        $('#' + areaddescribedby).show();

                    }
                }
                else {
                    $('#SupportTicketModel_Description').val('');
                    $('#STNotesModel_Content').val('');
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
function checkformvalid() {
    $("#ticketpaddform").submit(function () {
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
    $("#ticketpaddform").submit(function () {
        var messageLength = "";
        if (getDataFromTheEditor() != '') {
            $('#SupportTicketModel_Description').val(getDataFromTheEditor());
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
//#endregion
//#region return only html
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

//#region Comment
var colorarray = [];
function colorobject(string, color) {
    this.string = string;
    this.color = color;
}
function getRandomColor() {
    var letters = '0123456789ABCDEF';
    var color = '#';
    for (var i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
}
function LoadTicketResponses(SupportTicketId) {
    var ClientId = $(document).find('#SupportTicketModel_ClientId').val();
    var SiteId = $(document).find('#SupportTicketModel_SiteId').val();
    $.ajax({
        "url": "/Admin/SupportTicket/LoadTicketResponses",
        data: { SupportTicketId: SupportTicketId },
        type: "POST",
        datatype: "json",
        success: function (data) {
            var getTexttoHtml = textToHTML(data);
            $(document).find('#commentstems').html(getTexttoHtml);
            $(document).find("#commentsList").mCustomScrollbar({
                theme: "minimal"
            });
        },
        complete: function () {
            var ftext = '';
            var bgcolor = '';
            $(document).find('#commentsdataloader').hide();
            $(document).find('#commentstems').find('.comment-header-item').each(function () {
                var thistext = LRTrim($(this).text());
                if (ftext == '' || ftext != thistext) {
                    var bgcolorarr = colorarray.filter(function (a) {
                        return a.string == thistext;
                    });
                    if (bgcolorarr.length == 0) {
                        bgcolor = getRandomColor();
                        var thisval = new colorobject(thistext, bgcolor);
                        colorarray.push(thisval);
                    }
                    else {
                        bgcolor = bgcolorarr[0].color;
                    }
                }
                $(this).attr('style', 'color:' + bgcolor + '!important;border:1px solid' + bgcolor + '!important;');
                ftext = LRTrim($(this).text());
            });
            var loggedinuserinitial = LRTrim($('#hdr-comments-add').text());
            var avlcolor = colorarray.filter(function (a) {
                return a.string == loggedinuserinitial;
            });
            if (avlcolor.length == 0) {
                $('#hdr-comments-add').attr('style', 'border:1px solid #264a7c !important;').show();
            }
            else {
                $('#hdr-comments-add').attr('style', 'color:' + avlcolor[0].color + ' !important;border:1px solid ' + avlcolor[0].color + '!important;').show();
            }
            $('.kt-notes__body a').attr('target', '_blank');
        }
    });
}
$(document).on('click', '.editcomments', function () {
    $(document).find(".ckeditorarea").each(function () {
        $(this).html('');
    });
    //$("#msparttxtcommentsnew").show();
    $(".ckeditorfield").hide();
    var notesitem = $(this).parents('.kt-notes__item').eq(0);
    notesitem.find('.ckeditorarea').html(CreateEditorHTML('supportticketresponseEdit'));
    var noteitem = $(document).find('.editcomments').parents('.kt-notes__item');
    noteitem.find('.kt-notes__body').show();
    noteitem.find('.commenteditdelearea').show();

    var rawHTML = $.parseHTML($(this).parents('.kt-notes__item').find('.kt-notes__body').find('.originalContent').html());
    LoadCkEditorEdit('supportticketresponseEdit', rawHTML);
    $(document).find('.ckeditorarea').hide();
    notesitem.find('.ckeditorarea').show();
    notesitem.find('.kt-notes__body').hide();
    notesitem.find('.commenteditdelearea').hide();
});

$(document).on('click', '#btnAddreplySave', function () {
    const data = getDataFromTheEditor();
    if (!data) {
        return false;
    }
    if (LRTrim(data) == "") {
        return false;
    }
    var ClientId = $(document).find('#SupportTicketModel_ClientId').val();
    var SiteId = $(document).find('#SupportTicketModel_SiteId').val();
    var SupportTicketId = $('#SupportTicketModel_SupportTicketId').val();
    var Type = $('#STNotesModel_Type').val();
    var noteId = 0;
    $.ajax({
        url: '/Admin/SupportTicket/AddUpdateResponse',
        type: 'POST',
        beforeSend: function () {
            ShowLoader();
        },
        data: { SupportTicketId: SupportTicketId, content: LRTrim(data), noteId: noteId, Type: Type },
        success: function (data) {
            if (data.Result == "success") {
                var message;
                $(document).find('#NotesModalpopup').modal("hide");
                if (Type == "Note") {
                    SuccessAlertSetting.text = getResourceValue("AddNoteAlert");
                }
                else {
                    SuccessAlertSetting.text = getResourceValue("ReplyAddAlert");
                }
                swal(SuccessAlertSetting, function () {
                    RedirectToTicketDetail(SupportTicketId, ClientId);
                });
            }
            else {
                ShowGenericErrorOnAddUpdate(data);
                CloseLoader();
            }
        },
        complete: function () {
            //ClearEditorEdit();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });

});
$(document).on('click', '.btneditcomments', function () {
    const data = getDataFromTheEditor();
    if (!data) {
        return false;
    }
    if (LRTrim(data) == "") {
        return false;
    }
    var ClientId = $(document).find('#SupportTicketModel_ClientId').val();
    var SiteId = $(document).find('#SupportTicketModel_SiteId').val();
    var SupportTicketId = $('#SupportTicketModel_SupportTicketId').val();
    var noteId = $(this).parents('.kt-notes__item').find('.editcomments').attr('id');
    $.ajax({
        url: '/Admin/SupportTicket/AddUpdateResponse',
        type: 'POST',
        beforeSend: function () {
            ShowLoader();
        },
        data: { SupportTicketId: SupportTicketId, content: LRTrim(data), noteId: noteId, ClientId: ClientId, SiteId: SiteId },
        success: function (data) {
            if (data.Result == "success") {
                var message;                
                SuccessAlertSetting.text = getResourceValue("ResponseUpdateAlert");
                swal(SuccessAlertSetting, function () {
                    RedirectToTicketDetail(SupportTicketId, ClientId);
                });
            }
            else {
                ShowGenericErrorOnAddUpdate(data);
                CloseLoader();
            }
        },
        complete: function () {
            //ClearEditorEdit();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });

});

$(document).on('click', '.deletecomments', function () {
    DeleteSTNote($(this).attr('id'));
});
function DeleteSTNote(notesId) {
    var ClientId = $(document).find('#SupportTicketModel_ClientId').val();
    var SupportTicketId = $('#SupportTicketModel_SupportTicketId').val();
    swal(CancelAlertSettingForCallback, function () {
        $.ajax({
            url: '/Admin/SupportTicket/DeleteResponse',
            data: {
                _notesId: notesId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    SuccessAlertSetting.text = getResourceValue("ResponseDeleteAlert");
                    swal(SuccessAlertSetting, function () {
                        RedirectToTicketDetail(SupportTicketId, ClientId);
                    });
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}
$(document).on('click', '.btncommandCancel', function () {
    ClearEditorEdit();
    $(document).find('.ckeditorarea').hide();
    $(this).parents('.kt-notes__item').find('.kt-notes__body').show();
    $(this).parents('.kt-notes__item').find('.commenteditdelearea').show();
});
//#region ckeditor 5
//let theEditor = "";
function LoadCkEditor(equtxtcomments) {
    $(".toolbar-container").html('');
    ClearEditor();
    DecoupledDocumentEditor
        .create(document.querySelector('#' + equtxtcomments), {
            toolbar: ['heading', '|', 'bold', 'italic', 'alignment', 'link', 'numberedList', 'bulletedList', '|', 'fontFamily', 'fontSize', 'fontColor', 'fontBackgroundColor', '|', 'removeFormat'],
            extraPlugins: [MentionCustomization],
            mediaEmbed: { previewsInData: true },
            fontSize: {
                options: [8, 9, 10, 11, 12, 13, 14, 16, 18, 24, 30, 36, 48, 60, 72, 96]
            },
            mention: {
                feeds: [
                    {
                        marker: '@',
                        feed: getFeedItems,
                    }
                ]
            }

        })
        .then(editor => {
            //editor.destroy();
            const toolbarContainer = document.querySelector('main .toolbar-container');
            toolbarContainer.prepend(editor.ui.view.toolbar.element);
            theEditor = editor;
            editor.execute('listStyle', { type: 'decimal' });
            editor.model.document.on('change:data', () => {
                if (getDataFromTheEditor() != '') {
                    //$('#SupportTicketModel_Description').val(getDataFromTheEditor());
                    $('#STNotesModel_Content').val(getDataFromTheEditor());
                    $(".content-container").removeClass("input-validation-error");

                    var areaddescribedby = $(this).attr('aria-describedby');
                    if ($(this).parents().find('.sidebar-content').attr('id') != "advsearchsidebar") {
                        $('#' + areaddescribedby).show();

                    }
                }
                else {
                    //$('#SupportTicketModel_Description').val('');
                    $('#STNotesModel_Content').val('');
                    $(".content-container").addClass("input-validation-error");
                }
            });
        })
        .catch(err => {
            console.error(err.stack);
        });
}

function LoadCkEditorEdit(equtxtcomments, data) {
    $(".toolbar-containerEdit").html('');
    ClearEditorEdit();
    DecoupledDocumentEditor
        .create(document.querySelector('#' + equtxtcomments), {
            toolbar: ['heading', '|', 'bold', 'italic', 'alignment', 'link', 'numberedList', 'bulletedList', '|', 'fontFamily', 'fontSize', 'fontColor', 'fontBackgroundColor', '|', 'removeFormat'],
            extraPlugins: [MyCustomUploadAdapterPlugin],
            mediaEmbed: { previewsInData: true },
            fontSize: {
                options: [8, 9, 10, 11, 12, 13, 14, 16, 18, 24, 30, 36, 48, 60, 72, 96]
            }
            //mention: {
            //    feeds: [
            //        {
            //            marker: '@',
            //            feed: getFeedItems,
            //        }
            //    ]

            //}
        })
        .then(editor => {
            //editor.destroy();
            var getParseHtml = GetParseHtml(data);
            editor.setData(getParseHtml);
            const toolbarContainer = document.querySelector('main .toolbar-containerEdit');
            toolbarContainer.prepend(editor.ui.view.toolbar.element);
            theEditor = editor;
            editor.execute('listStyle', { type: 'decimal' });
            editor.model.document.on('change:data', () => {

            });
        })
        .catch(err => {
            console.error(err.stack);
        });
}

//function MentionCustomization(editor) {
//    // The upcast converter will convert <a class="mention" href="" data-user-id="">
//    // elements to the model 'mention' attribute.
//    editor.conversion.for('upcast').elementToAttribute({
//        view: {
//            name: 'span',
//            key: 'data-mention',
//            classes: 'mention',
//            attributes: {
//                // href: ,
//                'data-user-id': true
//            }
//        },
//        model: {
//            key: 'mention',
//            value: viewItem => {
//                // The mention feature expects that the mention attribute value
//                // in the model is a plain object with a set of additional attributes.
//                // In order to create a proper object, use the toMentionAttribute helper method:
//                const mentionAttribute = editor.plugins.get('Mention').toMentionAttribute(viewItem, {
//                    // Add any other properties that you need.
//                    //link: viewItem.getAttribute('href'),
//                    userId: viewItem.getAttribute('data-user-id')
//                });

//                return mentionAttribute;
//            }
//        },
//        converterPriority: 'high'
//    });

//    // Downcast the model 'mention' text attribute to a view <a> element.
//    editor.conversion.for('downcast').attributeToElement({
//        model: 'mention',
//        view: (modelAttributeValue, { writer }) => {
//            // Do not convert empty attributes (lack of value means no mention).
//            if (!modelAttributeValue) {
//                return;
//            }

//            return writer.createAttributeElement('span', {
//                class: 'mention',
//                'data-mention': modelAttributeValue.id,
//                'data-user-id': modelAttributeValue.name,
//            }, {
//                // Make mention attribute to be wrapped by other attribute elements.
//                priority: 20,
//                // Prevent merging mentions together.
//                id: modelAttributeValue.uid
//            });
//        },
//        converterPriority: 'high'
//    });
//}


//var items = [];
//function getFeedItems(queryText) {
//    var ClientId = $(document).find('#SupportTicketModel_ClientId').val();
//    var SiteId = $(document).find('#SupportTicketModel_SiteId').val();
//    // As an example of an asynchronous action, return a promise
//    // that resolves after a 100ms timeout.
//    // This can be a server request or any sort of delayed action.
//    return new Promise(resolve => {
//        setTimeout(() => {
//            $.ajax({
//                url: '/Admin/SupportTicket/GetMentionList',
//                type: 'GET',
//                data: { searchText: queryText, ClientId: ClientId, SiteId: SiteId },
//                success: function (data) {
//                    var i;
//                    items = [];
//                    for (i = 0; i < data.length; i++) {
//                        items.push(
//                            {
//                                id: '@' + data[i].name,
//                                name: data[i].id
//                            });
//                    }
//                },
//                complete: function () {
//                    //CloseLoader();
//                }
//            });
//            items = uniqueArray(items);
//            const itemsToDisplay = items
//                // Filter out the full list of all items to only those matching the query text.
//                .filter(isItemMatching)
//                // Return 10 items max - needed for generic queries when the list may contain hundreds of elements.
//                .slice(0, 10);

//            resolve(itemsToDisplay);
//        }, 100);
//    });

//    // Filtering function - it uses the `name` and `username` properties of an item to find a match.
//    function isItemMatching(item) {
//        // Make the search case-insensitive.
//        const searchString = queryText.toLowerCase();

//        // Include an item in the search results if the name or username includes the current user input.
//        return (
//            item.name.toLowerCase().includes(searchString) ||
//            item.id.toLowerCase().includes(searchString)
//        );
//    }
//}

//function uniqueArray(myArray) {
//    var newArray = [];
//    $.each(myArray, function (key, value) {
//        var exists = false;
//        $.each(newArray, function (k, val2) {
//            if (value.id == val2.id) { exists = true };
//        });
//        if (exists == false && value.id != "") { newArray.push(value); }
//    });
//    return newArray;
//}
function ClearEditor() {
    if (theEditor != "") {
        theEditor.setData('');
        $(".toolbar-container").html('');
        theEditor.destroy(true);
        theEditor = "";
    }
}
function ClearEditorEdit() {
    if (theEditor != "") {
        theEditor.setData('');
        theEditor.destroy(true);
        $(".toolbar-containerEdit").html('');
        theEditor = "";
    }
}
//#endregion

function CreateEditorHTML(id) {
    return '<main style="margin-bottom:10px !important;">' +
        '<div class="document-editor ckeditorfield" >' +
        '<div class="toolbar-containerEdit"></div>' +
        '<div class="content-container form-control">' +
        '<div id="' + id + '"></div>' +
        '</div>' +
        '</div>' +
        '</main>' +
        '<button type="submit" class="btn btn-blue mobBttn btneditcomments" value="save">Save</button>' +
        '<button type="button" class="btn btn-blue mobBttn btncommandCancel">Cancel</button>';
}


//#region New Methods
let editorIntances = {};
function getDataFromEditorById(id) {
    return editorIntances[id] ? editorIntances[id].getData() : '';
}
function ClearEditorById(id) {
    if (editorIntances[id]) {
        editorIntances[id].setData('');
        //$(".mytoolbar-container").html('');
        $(document).find('#' + id).parent().siblings('.mytoolbar-container').html('');
        editorIntances[id].destroy(true);
        delete editorIntances[id];
    }
}
function LoadCkEditorById(id, succesCallback) {
    //$('.mytoolbar-container').html('');
    $(document).find('#' + id).parent().siblings('.mytoolbar-container').html('');
    ClearEditorById(id);
    DecoupledDocumentEditor
        .create(document.querySelector('#' + id), {
            toolbar: ['heading', '|', 'bold', 'italic', 'alignment', 'link', 'numberedList', 'bulletedList', '|', 'fontFamily', 'fontSize', 'fontColor', 'fontBackgroundColor', '|', 'removeFormat'],
            extraPlugins: [MentionCustomization],
            mediaEmbed: { previewsInData: true },
            fontSize: {
                options: [8, 9, 10, 11, 12, 13, 14, 16, 18, 24, 30, 36, 48, 60, 72, 96]
            },
            mention: {
                feeds: [
                    {
                        marker: '@',
                        feed: getFeedItems,
                    }
                ]
            }
        })
        .then(editor => {
            //var toolbarContainer = document.querySelector('main .mytoolbar-container');
            var toolbarContainer = $(document).find('#' + id).parent().siblings('.mytoolbar-container');
            toolbarContainer.prepend(editor.ui.view.toolbar.element);
            editor.execute('listStyle', { type: 'decimal' });
            editor.model.document.on('change:data', () => {
            });
            editorIntances[id] = editor;
            if ($.isFunction(succesCallback)) {
                succesCallback(editor);
            }
        })
        .catch(err => {
            console.error(err.stack);
        });
}
function SetDataById(id, content) {
    if (editorIntances[id]) {
        var getTexttoHtml = textToHTML(content);
        editorIntances[id].setData(getTexttoHtml);
    }
}
//#endregion

//#region Get parse html for comment
function GetParseHtml(data) {
    var Fullhtml = "";
    for (var i = 0; i < data.length; i++) {
        var inntext = data[i].innerText;
        if (typeof (inntext) != "undefined") {
            if (inntext.indexOf('</html>') > -1) {
                var endodedhtml = HtmlEncode(inntext);
                var outHtml = data[i].outerHTML;
                outHtml = outHtml.replace(endodedhtml, inntext);
                if (outHtml.indexOf('<html><body>') > 1) {
                    outHtml = outHtml.replace('<html><body>', '');
                    outHtml = outHtml.replace('</body></html>', '');
                }
                Fullhtml += outHtml;
            }
            else {
                Fullhtml += data[i].outerHTML;
            }
        }
    }
    return Fullhtml;
}
function HtmlEncode(s) {
    var el = document.createElement("div");
    el.innerText = el.textContent = s;
    s = el.innerHTML;
    return s;
}

//return only html
function supportstr(str) {
    if (!window.DOMParser) return false;
    var parser = new DOMParser();
    try {
        parser.parseFromString(str, 'text/html');
    } catch (err) {
        return false;
    }
    return true;
}

var textToHTML = function (str) {
    // check for DOMParser support
    str = checkTags(str);
    if (supportstr(str)) {
        var parser = new DOMParser();
        var doc = parser.parseFromString(str, 'text/html');
        return doc.body.innerHTML;
    }

    // Otherwise, create div and append HTML
    var dom = document.createElement('div');
    dom.innerHTML = str;
    return dom;

};
function checkTags(str) {
    var DOMHolderArray = new Array();
    var tagsArray = new Array();
    var lines = str.split('\n');
    for (var x = 0; x < lines.length; x++) {
        var tagsArray = lines[x].match(/<(\/{1})?\w+((\s+\w+(\s*=\s*(?:".*?"|'.*?'|[^'">\s]+))?)+\s*|\s*)>/g);
        if (tagsArray) {
            for (var i = 0; i < tagsArray.length; i++) {
                if (tagsArray[i].indexOf('</') >= 0) {
                    elementToPop = tagsArray[i].substr(2, tagsArray[i].length - 3);
                    elementToPop = elementToPop.replace(/ /g, '');
                    for (var j = DOMHolderArray.length - 1; j >= 0; j--) {
                        if (DOMHolderArray[j].element == elementToPop) {
                            DOMHolderArray.splice(j, 1);
                            if (elementToPop != 'html') {
                                break;
                            }
                        }
                    }
                } else {
                    var tag = new Object();
                    tag.full = tagsArray[i];
                    tag.line = x + 1;
                    if (tag.full.indexOf(' ') > 0) {
                        tag.element = tag.full.substr(1, tag.full.indexOf(' ') - 1);
                    } else {
                        tag.element = tag.full.substr(1, tag.full.length - 2);
                    }
                    var selfClosingTags = new Array('area', 'base', 'br', 'col', 'command', 'embed', 'hr', 'img', 'input', 'keygen', 'link', 'meta', 'param', 'source', 'track', 'wbr');
                    var isSelfClosing = false;
                    for (var y = 0; y < selfClosingTags.length; y++) {
                        if (selfClosingTags[y].localeCompare(tag.element) == 0) {
                            isSelfClosing = true;
                        }
                    }
                    if (isSelfClosing == false) {
                        DOMHolderArray.push(tag);
                    }
                }

            }
        }
    }
    var uniqueArr = getUniqueValues(DOMHolderArray, 'element')

    if (uniqueArr.length > 0) {
        for (var i = 0; i < uniqueArr.length; i++) {
            if (str.indexOf('</' + uniqueArr[i]) > -1) {
                str = str.replaceAll('</' + uniqueArr[i], '</' + uniqueArr[i] + '>');
                str = str.replaceAll('</' + uniqueArr[i] + '>>', '</' + uniqueArr[i] + '>');
            }
        }
    }
    return str;

}

function getUniqueValues(array, key) {
    var result = [];
    array.forEach(function (item) {
        if (item.hasOwnProperty(key)) {
            result.push(item[key]);
        }
    });
    var unique = result.filter(function (itm, i, result) {
        return i == result.indexOf(itm);
    });
    return unique;
}
//#endregion
//#endregion

//#region Add
$(document).on('change', '#ddlClient', function () {
    var ClientId = $('#ddlClient').val();
    $("#ddlSite").empty();
    $("#ddlContact").empty();
    $("#ddlAgent").empty();
    $(document).find('#SupportTicketModel_SiteId').val('');
    if (ClientId != "") {
        GetSites(ClientId);
    }
});
function GetSites(ClientId) {
    $.ajax({
        url: '/Admin/SupportTicket/GetSiteByClientId',
        data: {
            ClientId: ClientId
        },
        type: "POST",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.data == "success") {
                var SiteId = $('#SupportTicketModel_SiteId').val();
                $("#ddlSite").empty();
                $("#ddlSite").append("<option value=''>" + "--Select--" + "</option>");
                $.each(data.SiteList, function (index, item) {
                    $("#ddlSite").append("<option value='" + item.value + "'>" + item.text + "</option>");
                });
                if (SiteId!=0 && $("#ddlSite option[value='" + SiteId + "']").length > 0) {
                    $("#ddlSite").val(SiteId).trigger("change.select2");
                }
                else {
                    $("#ddlSite").val('').trigger("change.select2");
                }
            }
            else {
                GenericSweetAlertMethod(data.data);
            }
        },
        complete: function () {
            CloseLoader();
            SetControls();
            $.validator.unobtrusive.parse(document);
        }
    });
}
$(document).on('change', '#ddlSite', function () {
    var ClientId = $('#ddlClient').val();
    var SiteId = $('#ddlSite').val();
    $("#ddlContact").empty();
    $("#ddlAgent").empty();
    $(document).find('#SupportTicketModel_Contact_PersonnelId').val('');
    $(document).find('#SupportTicketModel_Agent_PersonnelId').val('');
    if (SiteId != "") {
        GetPersonnelList(ClientId, SiteId);
    }
});
function GetPersonnelList(ClientId, SiteId) {
    $(document).find('#SupportTicketModel_SiteId').val(SiteId);
    var SupportTicketId = $(document).find('#SupportTicketModel_SupportTicketId').val();
    var ContactId = $(document).find('#SupportTicketModel_Contact_PersonnelId').val();
    $.ajax({
        url: '/Admin/SupportTicket/GetAllActiveList_Personnel',
        data: {
            ClientId: ClientId, SiteId: SiteId
        },
        type: "POST",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.data == "success") {

                $("#ddlContact").empty();
                $("#ddlContact").append("<option value=''>" + "--Select--" + "</option>");
                $.each(data.ContactList, function (index, item) {
                    $("#ddlContact").append("<option value='" + item.value + "'>" + item.text + "</option>");
                });
                if (ContactId!=0 && $("#ddlContact option[value='" + ContactId + "']").length > 0) {
                    $("#ddlContact").val(ContactId).trigger("change.select2");
                }
                else {
                    $("#ddlContact").val('').trigger("change.select2");
                }
                if (SupportTicketId > 0) {
                    var AgentId = $(document).find('#SupportTicketModel_Agent_PersonnelId').val();
                    $("#ddlAgent").empty();
                    $("#ddlAgent").append("<option value=''>" + "--Select--" + "</option>");
                    $.each(data.ContactList, function (index, item) {
                        $("#ddlAgent").append("<option value='" + item.value + "'>" + item.text + "</option>");
                    });
                    if (AgentId!=0 && $("#ddlAgent option[value='" + AgentId + "']").length > 0) {
                        $("#ddlAgent").val(AgentId).trigger("change.select2");
                    }
                    else {
                        $(document).find('#SupportTicketModel_Agent_PersonnelId').val('');
                        $("#ddlAgent").val('').trigger("change.select2");
                    }
                }
                $(document).find('.select2picker').select2({});
            }
            else {
                GenericSweetAlertMethod(data.data);
            }
        },
        complete: function () {
            CloseLoader();
            SetControls();
            $.validator.unobtrusive.parse(document);
        }
    });
}
$(document).on('change', '#ddlContact', function () {
    var ContactId = $('#ddlContact').val();
    $(document).find('#SupportTicketModel_Contact_PersonnelId').val(ContactId);
    if ($(this).val()) {
        $(this).removeClass('input-validation-error');
    }
    else {
        $(this).addClass('input-validation-error');
    }
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
$(document).on('change', '#ddlAgent', function () {
    var ContactId = $('#ddlAgent').val();
    $(document).find('#SupportTicketModel_Agent_PersonnelId').val(ContactId);
    if ($(this).val()) {
        $(this).removeClass('input-validation-error');
    }
    else {
        $(this).addClass('input-validation-error');
    }
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
//#endregion

//#region taginput
function TagInput() {
    var intvalue = '';
    if ($(document).find('#ddlTag').val()) {
        intvalue = $(document).find('#ddlTag').val().trim().split(/\s*,\s*/);
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
            url: "/Admin/SupportTicket/RetrieveTagName",
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

//#region toolbar
$(document).on('click', '#btnAddReply', function () {
    var Type = "Reply";
    AddSTNote(Type);
});
function AddSTNote(Type) {
    var ClientId = $(document).find('#SupportTicketModel_ClientId').val();
    var SiteId = $(document).find('#SupportTicketModel_SiteId').val();
    var SupportTicketId = $('#SupportTicketModel_SupportTicketId').val();
    $.ajax({
        url: '/Admin/SupportTicket/AddSTNotes',
        data: {
            SupportTicketId: SupportTicketId, ClientId: ClientId, SiteId: SiteId, Type: Type
        },
        type: "GET",
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#NotesPopUp').html(data);
            $('#NotesModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
            LoadCkEditorDesc();
        },
        complete: function () {
            checkformvalid();
            CloseLoader();
            SetControls();
        }
    });
}
function AddSTNotesOnSuccess(data) {
    CloseLoader();
    var ClientId = $(document).find('#SupportTicketModel_ClientId').val();
    var SupportTicketId = data.SupportTicketId;
    if (data.Result == "success") {
        $(document).find('#NotesModalpopup').modal("hide");
        if (data.type == "Note") {
            SuccessAlertSetting.text = getResourceValue("AddNoteAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("ReplyAddAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToTicketDetail(SupportTicketId, ClientId);
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', '#btnAddNote', function () {
    var Type = "Note";
    AddSTNote(Type);
});

$(document).on('click', '#btnHold', function () {
    var Status = "Hold";
    var message = getResourceValue("STHoldAlertMessage");
    ChangeSTStatus(Status, message)
});
$(document).on('click', '#btnComplete', function () {
    var Status = "Complete";
    var message = getResourceValue("STCompleteAlertMessage");
    ChangeSTStatus(Status, message)
});
$(document).on('click', '#btnSTCancel', function () {
    var Status = "Cancel";
    var message = getResourceValue("STCancelAlertMessage");
    ChangeSTStatus(Status, message)
});
$(document).on('click', '#btnReopen', function () {
    var Status = "Reopen";
    var message = getResourceValue("STReopenAlertMessage");
    ChangeSTStatus(Status, message)
});
function ChangeSTStatus(Status,Message) {
    var ClientId = $(document).find('#SupportTicketModel_ClientId').val();
    var SupportTicketId = $('#SupportTicketModel_SupportTicketId').val();
    CancelAlertSetting.text = Message;
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: "/Admin/SupportTicket/ChangeStatus",
            type: "GET",
            dataType: 'json',
            data: { ClientId: ClientId, SupportTicketId: SupportTicketId, Status: Status },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                var message = "";
                if (data.Result == "success") {
                    if (Status == "Hold") {
                        SuccessAlertSetting.text = getResourceValue("STHoldAlert");
                    }
                    else if (Status == "Complete") {
                        SuccessAlertSetting.text = getResourceValue("STCompleteAlert");
                    }
                    else if (Status == "Cancel") {
                        SuccessAlertSetting.text = getResourceValue("STCancelAlert");
                    }
                    else if (Status == "Reopen") {
                        SuccessAlertSetting.text = getResourceValue("STReopenAlert");
                    }
                    swal(SuccessAlertSetting, function () {
                        RedirectToTicketDetail(SupportTicketId, ClientId);
                    });
                }
                else {
                    message = "";
                    swal({
                        title: getResourceValue("CommonErrorAlert"),
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
//#endregion