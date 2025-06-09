var dtClientTable;
var selectCount = 0;
var selectedcount = 0;
var totalcount = 0;
var searchcount = 0;
var activeStatus;
var run = false;
var titleText = '';
var order = '1';
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
    
    $(document).find('.select2picker').select2({});
    var clientstatus = localStorage.getItem("CLIENTSEARCHGRIDDISPLAYSTATUS");
    if (clientstatus) {
        activeStatus = clientstatus;
        $('#clientsearchListul li').each(function (index, value) {
            if ($(this).attr('id') == activeStatus) {
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
        $('#clientsearchListul li:nth-child(2)').addClass('activeState');
        $('#clientsearchtitle').text(getResourceValue("AlertActive"));
    }
    $(document).on('change', '#colorselector', function (evt) {
        $(document).find('.tabsArea').hide();
        openCity(evt, $(this).val());
        $('#' + $(this).val()).show();
    });
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
        rowGrouping: true,
        searching: true,
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
        //fixedColumns: {
        //    leftColumns: 1
        //},
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
            "url": "/Admin/Clients/GetClientGridData",
            "type": "post",
            "datatype": "json",
            //cache: false,
            data: function (d) {
                d.customQueryDisplayId = localStorage.getItem("CLIENTSEARCHGRIDDISPLAYSTATUS");
                d.ClientId = LRTrim($("#ddlClientID").val());
                d.SiteId = LRTrim($("#ddlSiteId").val());
                d.Name = LRTrim($("#Name").val());
                d.Contact = LRTrim($("#Contact").val());
                d.Mail = LRTrim($("#Email").val());
                d.SearchText = LRTrim($(document).find('#txtColumnSearch').val());
                d.Order = order;
                
            },
            "dataSrc": function (result) {
                let colOrder = dtClientTable.order();
                orderDir = colOrder[0][1];
                searchcount = result.recordsTotal;
                if (result.data.length < 1) {
                    $(document).find('#btnClientExport').prop('disabled', true);
                }
                else {
                    $(document).find('#btnClientExport').prop('disabled', false);
                }
                HidebtnLoaderclass("LoaderDrop");
                return result.data;
            },
            global: true
        },
        "columns":
            [
                {
                    "data": "ClientId",
                    "bVisible": true,
                    "bSortable": false,
                    "autoWidth": false,
                    "bSearchable": false,
                    "mRender": function (data, type, row) {
                        if (row.ChildCount > 0) {
                            return '<img id="' + data + '" src="../Images/details_open.png" alt="expand/collapse" rel="' + data + '" style="cursor: pointer;"/>';
                        }
                        else {
                            return '';
                        }
                    }
                },
                {
                    "data": "ClientId",
                    "autoWidth": false,
                    "bSearchable": true,
                    "bSortable": true,
                    className: 'text-left',
                    "name": "1",
                    "mRender": function (data, type, row) {
                        return '<a class=link_client_detail href="javascript:void(0)">' + data + '</a>';
                    }
                },
                {
                    "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2",
                    mRender: function (data, type, full, meta) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                {
                    "data": "Contact", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3",
                    mRender: function (data, type, full, meta) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                { "data": "BusinessType", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "4" },
                { "data": "PackageLevel", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "5" },
                { "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "6" },
                { "data": "CreateDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "7" }
            ],

        columnDefs: [
            {
                targets: [1, 2, 3, 4],
                className: 'noVis'
            }
        ],
        initComplete: function () {
            SetPageLengthMenu();
            
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
$('#clientSearch').find('th').click(function () {
    if ($(this).data('col')) {
        run = true;
        order = $(this).data('col');
    }
});

$(document).find('#clientSearch').on('click', 'tbody td img', function (e) {
    var tr = $(this).closest('tr');
    var row = dtClientTable.row(tr);
    if (this.src.match('details_close')) {
        this.src = "../Images/details_open.png";
        row.child.hide();
        tr.removeClass('shown');
    }
    else {
        this.src = "../Images/details_close.png";
        var ClientId = $(this).attr("rel");
        $.ajax({
            url: "/Admin/Clients/GetClientInnerGrid",
            data: {
                ClientId: ClientId
            },
            beforeSend: function () {
                ShowLoader();
            },
            dataType: 'html',
            success: function (json) {
                row.child(json).show();
                dtinnerGrid = row.child().find('.clientsinnerDataTable').DataTable(
                    {
                        "order": [[0, "asc"]],
                        paging: false,
                        searching: false,
                        "bProcessing": true,
                        responsive: true,
                        scrollY: 300,
                        "scrollCollapse": true,
                        sDom: 'Btlipr',
                        language: {
                            url: "/Admin/base/GetDataTableLanguageJson?nGrid=" + true
                        },
                        buttons: [],
                        "columnDefs": [
                            { className: 'text-center', targets: [0, 2, 3, 4, 5] },
                            {
                                "render": function (data, type, row) {
                                    return "<div class='text-wrap'>" + data + "</div>";
                                }
                                , targets: [1]
                            }
                        ],
                        
                        initComplete: function () {
                            tr.addClass('shown');
                            row.child().find('.dataTables_scroll').addClass('tblchild-scroll');
                            CloseLoader();
                        }
                    });
            }
        });

    }
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
        if ($(this).attr('id') == "ddlClientID") {
            if ($(this).val() == null) {
                selectCount++;
                searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
            }
        }
        if ($(this).attr('id') == "ddlSiteId") {
            if ($(this).val() == null) {
                selectCount++;
                searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
            }
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
    $(document).find('.dtpicker:not(.readonly)').datepicker({
        changeMonth: true,
        changeYear: true,
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy');
    SetFixedHeadStyle();
}

$(document).on('change', '#CreateClientModel_PackageLevel', function (e) {
    $(document).find('#CreateClientModel_MaxSites').val('');
    var PackageLevel = $(this).val();
    if (PackageLevel == "Enterprise") {
        $(document).find('#CreateClientModel_MaxSites').val('1');
        $('#CreateClientModel_MaxSites').prop("readonly", false).removeClass('readonly');
    }
    else {
        $(document).find('#CreateClientModel_MaxSites').val('1');
        $('#CreateClientModel_MaxSites').prop("readonly", true).addClass('readonly').removeClass('input-validation-error');
    }
});
//#region Add 
$(document).on('click', '.AddClient', function () {
    $.ajax({
        url: "/Admin/Clients/AddClient",
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
                RedirectToClientDetail(data.ClientId, "client", 0);
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
$(document).on('click', "#btnCancelAddClient", function () {
    swal(CancelAlertSetting, function () {
        window.location.href = "/Admin/Clients/Index?page=Clients";
    });
});
//#endregion
//#region For Details Page
$(document).on('click', '.link_client_detail', function (e) {
    titleText = $('#clientsearchtitle').text();
    var row = $(this).parents('tr');
    var data = dtClientTable.row(row).data();
    var clientId = data.ClientId;
    $.ajax({
        url: "/Admin/Clients/ClientDetailsById",
        type: "POST",
        data: { clientId: clientId },
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#clientmaincontainer').html(data);
            $(document).find('#spnlinkToSearch').text(titleText);
        },
        complete: function () {
            CloseLoader();
            GenerateCEEventLogGrid();
            GenerateActivityTableGrid();
        },
        error: function () {
            CloseLoader();
        }
    });
});

function RedirectToClientDetail(ClientId, mode, SiteId) {
    $.ajax({
        url: "/Admin/Clients/ClientDetailsById",
        type: "POST",
        dataType: 'html',
        data: { ClientId: ClientId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#clientmaincontainer').html(data);
            $(document).find('#spnlinkToSearch').text(titleText);
        },
        complete: function () {
            CloseLoader();
            SetFixedHeadStyle();
            GenerateCEEventLogGrid();
            GenerateActivityTableGrid();
            if (mode === "client") {
                $(document).find('#clientSidebar').trigger('click');
                $('#colorselector').val('ClientOverview');
                $(document).find('#clientSidebar').addClass("active");
            }
            if (mode === "site") {
                $(document).find('#clientsites').trigger('click');
                $('#colorselector').val('ClientSites');
                $(document).find('#clientsites').addClass("active");
                if (SiteId > 0) {
                    GetSiteDetailsByClientIdSiteId(ClientId, SiteId);
                    GetSiteBillingDetailsByClientIdSiteId(ClientId, SiteId);
                }
            }
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', '#linkToSearch', function () {
    window.location.href = "../Clients/Index?page=Clients";
});
//#endregion
//#region Edit 
$(document).on('click', "#btnclientedit", function () {
    var clientid = LRTrim($(document).find('#ClientId').val());
    $.ajax({
        url: '/Admin/Clients/EditClient',
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
            SetClientControls();
        }
    });
});
function ClientEditOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("ClientUpdateAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToClientDetail(data.ClientId, "client", 0);
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', "#btnCancelEditClient", function () {
    var clientid = $(document).find('#CreateClientModel_ClientId').val();
    swal(CancelAlertSetting, function () {
        RedirectToClientDetail(clientid, "client", 0);
    });
});
$(document).on('click', '.lithisclient', function () {
    var ClientId = $(this).attr('data-val');
    RedirectToClientDetail(ClientId, "client", 0);
});
//#endregion
//#region TestConnectionStrings
$(document).on('click', '#btnTestConnection', function () {
    if ($("#CreateClientModel_ConnectionString").val() !== "") {
        $.ajax({
            url: "/Admin/Clients/TestConnectionString",
            type: "POST",
            dataType: 'json',
            data: { conStr: $("#CreateClientModel_ConnectionString").val() },
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
//#endregion
