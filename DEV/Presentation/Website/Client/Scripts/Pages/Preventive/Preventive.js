var equipmentId;
var locationId;
var assignedId;
var activeStatus;
var run = false;
var procSelectTbl;
var gridname = "PrevMaint_Search";
var ClientLookupId = "";
var JobDuration = "";
var Description = "";
var FrequencyType = "";
var Frequency = "";
function filterinfo(id, value) {
    this.key = id;
    this.value = value;
}
$(function () {
    equipmentId = 0;
    locationId = 0;
    assignedId = 0;
    activeStatus = false;
    ShowbtnLoader("btnsortmenu");
    ShowbtnLoaderclass("LoaderDrop");
    $("#action").click(function () {
        $(".actionDrop").slideToggle();
    });
    $(".actionDrop ul li a").click(function () {
        $(".actionDrop").fadeOut();
    });
    $("#action").focusout(function () {
        $(".actionDrop").fadeOut();
    });
    $(document).find("#sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $(document).on('click', '#dismiss, .overlay', function () {
        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });   
    $(document).on('click', '#drpDwnLink', function (e) {
        e.preventDefault();
        $(document).find("#drpDwn").slideToggle();
    });
    $(document).on('change', '#colorselector', function (evt) {
        $(document).find('.tabsArea').hide();
        openCity(evt, $(this).val());
        $('#' + $(this).val()).show();
    });
    $(".actionBar").fadeIn();
    $("#PMGridAction :input").attr("disabled", "disabled");

    $('.select2picker, form').change(function () {
        var areaddescribedby = $(this).attr('aria-describedby');
        var validity = false;
        if ($(this).closest('form').length > 0) {
            validity = $(this).valid();
        }
        if (validity == true) {
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
});
$(document).on('click', "#sidebarCollapse", function () {
    $('#renderpreventive').find('#sidebar').addClass('active');
    $('.overlay').fadeIn();
    $('.collapse.in').toggleClass('in');
    $('a[aria-expanded=true]').attr('aria-expanded', 'false');
});
function openCity(evt, cityName) {
    evt.preventDefault();
    switch (cityName) {
        case "PMTasks":
            generateTasksGrid();
            break;
        case "PMNotes":
            generateNotesGrid();
            break;
        case "PMAttachments":
            generateAttachmentsGrid();
            break;
        case "PMEstimatesPart":
            generateEstimatesPartGrid(0);
            break;
        case "PMEstimatesLabor":
            generateEstimatesLaborGrid();
            break;
        case "PMEstimatesOther":
            generateEstimatesOtherGrid();
            break;
        case "PMEstimatesSummery":
            generateEstimatesSummeryGrid();
            break;
    }
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(cityName).style.display = "block";
    if (typeof evt.currentTarget !== "undefined") {
        evt.currentTarget.className += " active";
    }
    else {
        evt.target.className += " active";
    }
}
$(document).on('click', "ul.vtabs li", function () {
    if ($(this).find('#drpDwnLink').length > 0) {
        $("ul.vtabs li").removeClass("active");
        $(this).addClass("active");
        return false;
    }
    else {
        $("ul.vtabs li").removeClass("active");
        $(this).addClass("active");
        $(".tabsArea").hide();
        var activeTab = $(this).find("a").attr("href");
        $(activeTab).fadeIn();
        return false;
    }
});
//#region Search
$(document).on('click', '#liPdf', function () {
    var thisid = $(this).attr('id');
    var TableHaederProp = [];
    function table(property, title) {
        this.property = property;
        this.title = title;
    }
    $("#prevSearchTable thead tr th").map(function (key) {
        var thisdiv = $(this).find('div');
        if ($(this).parents('.prevmentinnerDataTable').length == 0 && thisdiv.html()) {
            var tablearr = new table(this.getAttribute('data-th-prop'), thisdiv.html());
            TableHaederProp.push(tablearr);
        }
    });
    var params = {
        tableHaederProps: TableHaederProp,
        colname: order, 
        coldir: orderDir,
        equipmentId: equipmentId,
        locationId: locationId,
        assignedId: assignedId,
        InactiveFlag: activeStatus,
        MasterjobId: LRTrim($("#masterjobid").val()),
        Description: LRTrim($("#Description").val()),
        Type: $('#Type').val(),
        ScheduleType: $('#scheduletype').val(),
        ChargeTo: LRTrim($("#ChargeTo").val()),
        ChargeToName: LRTrim($("#ChargeToName").val()),
        SearchText: $('#txtColumnSearch').val()
    };
    pMPrintParams = JSON.stringify({ 'pMPrintParams': params });
    $.ajax({
        "url": "/PreventiveMaintenance/SetPrintData",
        "data": pMPrintParams,
        contentType: 'application/json; charset=utf-8',
        "dataType": "json",
        "type": "POST",
        "success": function () {
            window.open('/PreventiveMaintenance/PrintASPDF', '_self');
            return;
        }
    });
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
    var thisid = $(this).attr('id');
    var TableHaederProp = [];
    function table(property, title) {
        this.property = property;
        this.title = title;
    }
    $("#prevSearchTable thead tr th").map(function (key) {
        var thisdiv = $(this).find('div');
        if ($(this).parents('.prevmentinnerDataTable').length == 0 && thisdiv.html()) {
            var tablearr = new table(this.getAttribute('data-th-prop'), thisdiv.html());
            TableHaederProp.push(tablearr);
        }
    });
    var params = {
        tableHaederProps: TableHaederProp,
        colname: order, 
        coldir: orderDir, 
        equipmentId: equipmentId,
        locationId: locationId,
        assignedId: assignedId,
        InactiveFlag: activeStatus,
        MasterjobId: LRTrim($("#masterjobid").val()),
        Description: LRTrim($("#Description").val()),
        Type: $('#Type').val(),
        ScheduleType: $('#scheduletype').val(),
        ChargeTo: LRTrim($("#ChargeTo").val()),
        ChargeToName: LRTrim($("#ChargeToName").val()),
        SearchText: $('#txtColumnSearch').val()
    };
    pMPrintParams = JSON.stringify({ 'pMPrintParams': params });
    $.ajax({
        "url": "/PreventiveMaintenance/SetPrintData",
        "data": pMPrintParams,
        contentType: 'application/json; charset=utf-8',
        "dataType": "json",
        "type": "POST",
        "success": function () {
            window.open('/PreventiveMaintenance/ExportASPDF', '_blank');
            return;
        }
    });
    funcCloseExportbtn();
});
var prevSearchTable;
var dtinnerGrid;
var filteritemcount = 0;
var order = '1';//Preventive Sorting
var orderDir = 'asc';//Preventive Sorting
function generatePrevDataTable() {
    if ($(document).find('#prevSearchTable').hasClass('dataTable')) {
        prevSearchTable.destroy();
    }
    prevSearchTable = $("#prevSearchTable").DataTable({
        colReorder: {
            fixedColumnsLeft: 2
        },
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[1, "asc"]],
        stateSave: true,
        "stateSaveCallback": function (settings, data) {
            if (run == true) {
                if (data.order) {
                    data.order[0][0] = order;
                    data.order[0][1] = orderDir;
                }//Preventive Sorting
                var filterinfoarray = getfilterinfoarray($("#txtColumnSearch"), $('#advsearchsidebar'));
                $.ajax({
                    "url": gridStateSaveUrl,
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
                        var LayoutInfo = JSON.parse(json.LayoutInfo);//Preventive Sorting
                        order = LayoutInfo.order[0][0];//Preventive Sorting
                        orderDir = LayoutInfo.order[0][1]; //Preventive Sorting
                        callback(JSON.parse(json.LayoutInfo));
                        if (json.FilterInfo) {
                            setsearchui(JSON.parse(json.FilterInfo), $("#txtColumnSearch"), $('.filteritemcount'), $("#advsearchfilteritems"));
                        }
                    }
                    else {
                        callback(json.LayoutInfo);
                    }
                }
            });
        },
        scrollX: true,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'Preventive Maintenance List'
            },
            {
                extend: 'print',
                title: 'Preventive Maintenance List'
            },

            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Preventive Maintenance List',
                extension: '.csv'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                exportOptions: {
                    columns: ':visible'
                },
                css: 'display:none',
                title: 'Preventive Maintenance List',
                orientation: 'landscape',
                pageSize: 'A3'
            }

        ],
        "orderMulti": true,
        "ajax": {
            "url": "/PreventiveMaintenance/GetPrevMaintGrid",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.equipmentId = equipmentId;
                d.locationId = locationId;
                d.assignedId = assignedId;               
                d.InactiveFlag = activeStatus;
                d.MasterjobId = LRTrim($("#masterjobid").val());
                d.Description = LRTrim($("#Description").val());
                d.Type = $('#Type').val();
                d.ScheduleType = $('#scheduletype').val();
                d.ChargeTo = LRTrim($("#ChargeTo").val());
                d.ChargeToName = LRTrim($("#ChargeToName").val());
                d.SearchText = $('#txtColumnSearch').val();
                d.Order = order;//Preventive Sorting               
            },
            "dataSrc": function (result) {
                let colOrder = prevSearchTable.order();
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
                    "data": "PrevMaintMasterId",
                    "bVisible": true,
                    "bSortable": false,
                    "autoWidth": false,
                    "bSearchable": false,
                    "mRender": function (data, type, row) {
                        if (row.ChildCount > 0) {
                            return '<img id="' + data + '" src="../../Images/details_open.png" alt="expand/collapse" rel="' + data + '" style="cursor: pointer;"/>';
                        }
                        else {
                            return '';
                        }
                    }
                },
                {
                    "data": "ClientLookupId",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": true,
                    "className": "text-left",
                    "name": "0",
                    "mRender": function (data, type, row) {
                        return '<a class=lnk_prevmaintanance href="javascript:void(0)">' + data + '</a>';
                    }
                },
                {
                    "data": "ScheduleType", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-left",
                    render: function (data, type, row, meta) {
                        return getStatusValue(data);
                    }
                },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-left", "name": "2",
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-400'>" + data + "</div>";
                    }
                },
                { "data": "Type", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-left", "name": "3" }
            ],
        "columnDefs": [
            {
                targets: [0, 1],
                className: 'noVis'
            }
        ],
        initComplete: function () {
            SetPageLengthMenu();            
            $(document).find('.prevsearchview').removeClass('sort-active');           
            switch (activeStatus) {
                case false:
                    $(document).find('.prevsearchview').eq(0).addClass('sort-active');
                    $(document).find('#prevsearchtitle').text($(document).find('.prevsearchview').eq(0).text());
                    break;
                case true:
                    $(document).find('.prevsearchview').eq(1).addClass('sort-active');
                    $(document).find('#prevsearchtitle').text($(document).find('.prevsearchview').eq(1).text());
                    break;
            }           
            $("#PMGridAction :input").removeAttr("disabled");
            $("#PMGridAction :button").removeClass("disabled");
            DisableExportButton($("#prevSearchTable"), $(document).find('.import-export'));
        }
    });
};
$(document).find('#prevSearchTable').on('click', 'tbody td img', function (e) {
    var tr = $(this).closest('tr');
    var row = prevSearchTable.row(tr);
    if (this.src.match('details_close')) {
        this.src = "../../Images/details_open.png";
        row.child.hide();
        tr.removeClass('shown');
    }
    else {
        this.src = "../../Images/details_close.png";
        var PrevMaintMasterId = $(this).attr("rel");
        $.ajax({
            url: "/PreventiveMaintenance/GetPoInnerGrid",
            data: {
                PrevMasterID: PrevMaintMasterId
            },
            beforeSend: function () {
                ShowLoader();
            },
            dataType: 'html',
            success: function (json) {
                row.child(json).show();
                dtinnerGrid = row.child().find('.prevmentinnerDataTable').DataTable(
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
                            url: "/base/GetDataTableLanguageJson?nGrid=" + true
                        },
                        buttons: [],
                        initComplete: function () { row.child().find('.dataTables_scroll').addClass('tblchild-scroll'); CloseLoader(); },
                        select: {
                            style: 'os',
                            selector: 'td:nth-child(6)'
                        },

                    });
                tr.addClass('shown');
            }
        });
    }
});

$(document).on('click', '#prevSearchTable_paginate .paginate_button', function () {
    run = true;
});
$(document).on('change', '#prevSearchTable_length .searchdt-menu', function () {
    run = true;
});
$('#prevSearchTable').find('th').click(function () {
    run = true;
    order = $(this).data('col');
});
$(function () {
    if ($(document).find('#PrevDetails').length == 1) {
        var text = '';
        var state = '';
        GenerateScheduleGrid();
        var scheduleType = $(document).find('#preventiveMaintenanceModel_ScheduleType').val();
        if (scheduleType) {
            $(document).find("#ScheduleTypelocval").text(getStatusValue(scheduleType));
        }
        var IsInactive = $(document).find('#preventiveMaintenanceModel_InactiveFlag').is(':checked');
        if (IsInactive == false) {
            text = getResourceValue('AlertActive');
            state = '1';
        }
        else {
            text = getResourceValue('AlertInactive');
            state = '2';
        }
        $(document).find('#spnlinkToSearch').text(text);
        localStorage.setItem("PREVACTIVESTATUS", text);
        localStorage.setItem("PREVENTIVESEARCHGRIDDISPLAYSTATUS", state);
        $(document).find("#addPreventive").removeAttr('disabled');
    }
    else {
        //#region Load Grid With Status
        var displayState = localStorage.getItem("PREVENTIVESEARCHGRIDDISPLAYSTATUS");
        if (displayState) {
            if (displayState == "1") {
                activeStatus = false;
            }
            else {
                activeStatus = true;
            }
        }
        generatePrevDataTable();
        $(document).find('.select2picker').select2({});
        //#endregion
    }

    $(function () {
        jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
            if (this.context.length) {
                var MasterjobId = LRTrim($("#masterjobid").val());
                var Description = LRTrim($("#Description").val());
                var Duration = LRTrim($("#Duration").val());
                var Type = LRTrim($("#Type").val());
                var ScheduleType = LRTrim($("#scheduletype").val());
                dtTable = $("#prevSearchTable").DataTable();
                var currestsortedcolumn = $('#prevSearchTable').dataTable().fnSettings().aaSorting[0][0];
                var colname = order;//Preventive Sorting
                var coldir = orderDir;//Preventive Sorting
                var column = dtTable.column(currestsortedcolumn);
                var columnId = $(column.header()).attr('data-th-index');
                var SearchText = $('#txtColumnSearch').val();
                var ChargeTo = $("#ChargeTo").val();
                var ChargeToName = $("#ChargeToName").val();
                var jsonResult = $.ajax({
                    url: '/PreventiveMaintenance/GetPreventiveMaintenancePrintData?page=all',
                    data: {
                        equipmentId: equipmentId,
                        locationId: locationId,
                        assignedId: assignedId,
                        InactiveFlag: activeStatus,
                        MasterjobId: MasterjobId,
                        Description: Description,
                        Duration: Duration,
                        Type: Type,
                        ScheduleType: ScheduleType,
                        SearchText: SearchText,
                        ChargeTo: ChargeTo,
                        ChargeToName: ChargeToName,
                        colname: colname,
                        coldir: coldir
                    },
                    success: function (result) {
                    },
                    async: false
                });
                var thisdata = JSON.parse(jsonResult.responseText).data;
                var visiblecolumnsIndex = $("#prevSearchTable thead tr th").map(function (key) {
                    return this.getAttribute('data-th-index');
                }).get();
                var d = [];
                $.each(thisdata, function (index, item) {
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
                    header: $("#prevSearchTable thead tr th").find('div').map(function (key) {
                        if ($(this).parents('.prevmentinnerDataTable').length == 0 && this.innerHTML) {
                            return this.innerHTML;
                        }
                    }).get()
                };
            }
        });
    });
    function AdvanceSearch() {
        $('.filteritemcount').text(filteritemcount);
    }
    $("#btnPreventiveAdvSrch").on('click', function (e) {
        run = true;
        $(document).find('#txtColumnSearch').val('');
        var searchitemhtml = "";
        filteritemcount = 0;
        $("#txtsearchbox").val("");
        $('#advsearchsidebar').find('.adv-item').each(function (index, item) {
            if ($(this).val()) {
                filteritemcount++;
                searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
            }
        });
        searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
        $('#advsearchfilteritems').html(searchitemhtml);
        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();
        AdvanceSearch();
        prevSearchTable.page('first').draw('page');
    });
    $(document).on('click', '.btnCross', function () {
        run = true;
        var btnCrossedId = $(this).parent().attr('id');
        var searchtxtId = btnCrossedId.split('_')[1];
        $('#' + searchtxtId).val('').trigger('change');
        $(this).parent().remove();
        if (filteritemcount > 0) filteritemcount--;
        AdvanceSearch();
        prevSearchTable.page('first').draw('page');
    });
    $(document).on('click', '.lnk_prevmaintanance', function (e) {
        var row = $(this).parents('tr');
        var data = prevSearchTable.row(row).data();
        var titletext = $('#prevsearchtitle').text();
        localStorage.setItem("PREVACTIVESTATUS", titletext);
        $.ajax({
            url: "/PreventiveMaintenance/PrevDetails",
            type: "POST",
            data: { PrevMaintMasterId: data.PrevMaintMasterId },
            dataType: 'html',
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                $('#renderpreventive').html(data);
                $(document).find('#spnlinkToSearch').text(titletext);
                GenerateScheduleGrid();
            },
            complete: function () {
                CloseLoader();
            },
            error: function () {
                CloseLoader();
            }
        });
    });
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
$(document).on('click', '.prevsearchview', function (e) {
    if ($(document).find('#txtColumnSearch').val() !== '')
        $("#advsearchfilteritems").html('');
    $(document).find('#txtColumnSearch').val('');
    $(".searchList li").removeClass("sort-active");
    $(this).addClass('sort-active');
    $(document).find('#searcharea').hide("slide");
    var optionval = $(this).attr('id');
    localStorage.setItem("PREVENTIVESEARCHGRIDDISPLAYSTATUS", optionval);
    if (optionval == '1') {
        activeStatus = false;
    }
    else {
        activeStatus = true;
    }
    $('#prevsearchtitle').text($(this).text());
    ShowbtnLoaderclass("LoaderDrop");
    run = true;
    prevSearchTable.page('first').draw('page');
});
$(document).on('keyup', '#prevsearctxtbox', function (e) {
    var tagElems = $(document).find('#prevsearchListul').children();
    $(tagElems).hide();
    for (var i = 0; i < tagElems.length; i++) {
        var tag = $(tagElems).eq(i);
        if ($(tag).text().toLowerCase().includes($(this).val().toLowerCase()) == true || $(this).val().toLowerCase().includes($(tag).text().toLowerCase()) == true) {
            $(tag).show();
        }
    }
});
//#endregion
//#region New Search button
$(document).on('click', "#SrchBttnNew", function () {
    $.ajax({
        url: '/Base/PopulateNewSearchList',
        type: 'GET',
        data: { tableName: 'PrevMaintMaster' },
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
        data: { tableName: 'PrevMaintMaster', searchText: txtSearchval, isClear: isClear },
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
                prevSearchTable.page('first').draw('page');

            }
            CloseLoader();
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
function clearAdvanceSearch() {
    var filteritemcount = 0;
    $('#advsearchsidebar').find('input:text').val('');
    $(document).find("#scheduletype").val("").trigger('change');
    $(document).find("#Type").val("").trigger('change');
    $('.filteritemcount').text(filteritemcount);
    $('#advsearchfilteritems').find('span').html('');
    $('#advsearchfilteritems').find('span').removeClass('tagTo');
    equipmentId = 0;
    locationId = 0;
    assignedId = 0;
}
function TextSearch() {
    run = true;
    clearAdvanceSearch();
    switch (activeStatus) {
        case false:
            $(document).find('.prevsearchview').eq(0).addClass('sort-active');
            $(document).find('#prevsearchtitle').text($(document).find('.prevsearchview').eq(0).text());
            break;
        case true:
            $(document).find('.prevsearchview').eq(1).addClass('sort-active');
            $(document).find('#prevsearchtitle').text($(document).find('.prevsearchview').eq(1).text());
            break;
    }
    var txtSearchval = LRTrim($(document).find('#txtColumnSearch').val());
    if (txtSearchval) {
        GenerateSearchList(txtSearchval, false);
        var searchitemhtml = "";
        searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $('#txtColumnSearch').attr('id') + '">Search: ' + txtSearchval + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
        searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
        $("#advsearchfilteritems").html(searchitemhtml);
    }
    else {
        prevSearchTable.page('first').draw('page');
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
//#region Preventive Add/Edit
$(document).on('click', '#btnSelectChildren', function (e) {

    $('#searchTable3 tfoot th').each(function (i, v) {
        if (i > 0) {
            var colIndex = i;
            var title = $('#searchTable3 thead th').eq($(this).index()).text();
            $(this).html('<input type="text" class="popupSearch" id="colindex_' + colIndex + '"  /><i class="fa fa-search dropSearchIcon"></i>');
        }
    });
    if ($(document).find('#searchTable3').hasClass('dataTable')) {
        SelectChildrenTable.destroy();
    }
    generateSelectChildrenTable();
    $('#searchTable3').find('.popupSearch').on('keyup change', function () {
        var thisId = $(this).attr('id');
        var colIdx = thisId.split('_')[1];
        var searchText = LRTrim($(this).val());
        SelectChildrenTable.column(colIdx)
            .search(searchText)
            .draw();
    });
});
function generateSelectPmProcTable() {
    if ($(document).find('#procSelectTable').hasClass('dataTable')) {
        procSelectTbl.destroy();
    }
    var rCount = 0;
    mcxDialog.loading({ src: "../content/Images" });
    procSelectTbl = $("#procSelectTable").DataTable({
        columnDefs: [{
            "data": "PrevMaintLibraryId",
            orderable: false,
            className: 'select-checkbox dt-body-center',
            targets: 0,
            'render': function (data, type, full, meta) {
                return '<input type="radio" name="id[]" data-pmlid="' + data + '" class="isSelect" value="'
                    + $('<div/>').text(data).html() + '">';
            }
        }],
        select: {
            style: 'os',
            selector: 'td:first-child'
        },
        serverSide: true,
        "pagingType": "full_numbers",
        stateSave: true,
        order: [[1, 'asc']],
        colReorder: true,
        rowGrouping: true,
        searching: true,
        'bPaginate': true,
        "bProcessing": true,
        dom: 'rtip',
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        buttons: [],
        "filter": true,
        "orderMulti": true,
        "ajax": {           
            //***V2-694
            "url": "/PreventiveMaintenance/PmProcGridByInactiveFlag",
            "type": "POST",
            "datatype": "json",
            data: function (d) {
                d.ClientLookupId = ClientLookupId;
                d.JobDuration = JobDuration;
                d.Description = Description;
                d.FrequencyType = FrequencyType;
                d.Frequency = Frequency;
            },
            "dataSrc": function (json) {
                rCount = json.data.length;
                return json.data;
            }
        },
        "columns":
            [
                {},
                { "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "JobDuration", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "FrequencyType", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Frequency", "autoWidth": true, "bSearchable": true, "bSortable": true },
            ],
        initComplete: function () {
            $("#selectPmProcedureModal").modal('show');
            $("#procSelectTablefooter").show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#selectPmProcedureModal').hasClass('show')) {
                $(document).find('#selectPmProcedureModal').modal("show");
            }
            $('#procSelectTable tfoot th').each(function (i, v) {
                if (i > 0) {
                    var colIndex = i;
                    var title = $('#procSelectTable thead th').eq($(this).index()).text();
                    $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="proc_select_colindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                    if (ClientLookupId) { $('#proc_select_colindex_1').val(ClientLookupId); }
                    if (JobDuration) { $('#proc_select_colindex_3').val(JobDuration); }
                    if (Description) { $('#proc_select_colindex_2').val(Description); }
                    if (FrequencyType) { $('#proc_select_colindex_4').val(FrequencyType); }
                    if (Frequency) { $('#chkwolocationcolindex_5').val(Frequency); }
                }
            });
            $(document).ready(function () {
                $("#proc_select_colindex_3,#proc_select_colindex_5").attr("placeholder", "Enter numeric value only");
                $("#proc_select_colindex_3").keypress(function (e) {
                    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                        return false;
                    }
                });
                $("#proc_select_colindex_5").keypress(function (e) {
                    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                        return false;
                    }
                });
                $('#procSelectTable tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                    if (e.keyCode === 13) {
                        var thisId = $(this).attr('id');
                        var colIdx = thisId.split('_')[1];
                        var searchText = LRTrim($(this).val());
                        ClientLookupId = $('#proc_select_colindex_1').val();
                        Description = $('#proc_select_colindex_2').val();
                        JobDuration = $('#proc_select_colindex_3').val();
                        FrequencyType = $('#proc_select_colindex_4').val();
                        Frequency = $('#proc_select_colindex_5').val();
                        procSelectTbl.page('first').draw('page');
                    }
                });
            });
            return;
        }
    });
}
$(document).on('click', '#btnPmLibrary', function () {
    $('#createPmPopup').modal('hide');
    $('.modal-backdrop').remove();
    if ($(document).find('#procSelectTable').hasClass('dataTable')) {
        procSelectTbl.destroy();
    }
    generateSelectPmProcTable();
    $('#procSelectTable').find('.popupSearch').on('keyup change', function () {
        var thisId = $(this).attr('id');
        var colIdx = thisId.split('_')[1];
        var searchText = LRTrim($(this).val());
        SelectChildrenTable.column(colIdx)
            .search(searchText)
            .draw();
    });
    $(document).find('#procSelectTable').on('change', 'input[type="radio"]', function () {
        if (!this.checked) {
            var el = $(document).find('#example-select-all').get(0);
            if (el && el.checked && ('indeterminate' in el)) {
                el.indeterminate = true;
            }
        }
    });
});
$(document).on('click', '#btnDescribeNeed', function () {
    AddPreventive();
});
$(document).on('click', '.addPreventive', function (e) {
    $('#createPmPopup').modal('show');
});
$(document).on('click', '.addPreventiveDesc', function () {
    AddPreventive();
});
function AddPreventive() {
    $.ajax({
        url: "/PreventiveMaintenance/AddPreventive",
        type: "GET",
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpreventive').html(data);
        },
        complete: function () {
            CloseLoader();
            $.validator.setDefaults({ ignore: null });
            $.validator.unobtrusive.parse(document);
            $(document).find('.select2picker').select2({});
            $('input, form').blur(function () {
                $(this).valid();
            });
            $('#createPmPopup').modal('hide');
            $('.modal-backdrop').remove();
            SetPreventiveControls();
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', '#btnAddPmMaster', function () {
    var PrevMaintMasterId = 0;
    var pmlID = procSelectTbl.column(0).nodes().to$().map(function () {
        if ($(this).find('.isSelect').is(':checked')) {
            return $(this).find('.isSelect').data('pmlid');
        }
    }).get();
    if (pmlID && pmlID.length > 0) {
        $.ajax({
            url: '/PreventiveMaintenance/AddPmLibrary',
            data: { PrevMaintLibraryId: pmlID },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.ErrorMessages && data.ErrorMessages.length > 0) {
                    ShowErrorAlert(data.ErrorMessages);
                    return false;
                }
                else {
                    if (data.LibraryActivationStatus == true) {
                        localStorage.setItem("PREVACTIVESTATUS", getResourceValue('AlertInactive'));
                        activeStatus = true;
                        localStorage.setItem("PREVENTIVESEARCHGRIDDISPLAYSTATUS", 2);
                    }
                    else {
                        localStorage.setItem("PREVACTIVESTATUS", getResourceValue('AlertActive'));
                        localStorage.setItem("PREVENTIVESEARCHGRIDDISPLAYSTATUS", 1);
                        activeStatus = false;
                    }


                    SuccessAlertSetting.text = getResourceValue("PMLibraryProcedureAddAlert");
                    swal(SuccessAlertSetting, function () {
                        RedirectToPmDetail(PrevMaintMasterId, "overview");
                    });
                    PrevMaintMasterId = data.PrevMaintMasterId;
                    procSelectTbl.destroy();
                    $('#selectPmProcedureModal').modal('hide');
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
        swal({
            title: getResourceValue("NoRowsSelectAlert"),
            text: getResourceValue("SelectRowAlert"),
            type: "error",
            confirmButtonClass: "btn-sm btn-primary",
            confirmButtonText: getResourceValue("SaveAlertOk"),
        });
        return false;
    }
});
function PrevMaintAddOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        if (data.Command === "save") {
            var message;
            if (data.mode === "add") {
                SuccessAlertSetting.text = getResourceValue("AddPMAlerts");
            }
            else {
                SuccessAlertSetting.text = getResourceValue("UpdatePMAlerts");
            }
            swal(SuccessAlertSetting, function () {
                RedirectToPmDetail(data.PrevMaintMasterId, "overview");
            });
        }
        else {
            ResetErrorDiv();
            $('#identificationtab').addClass('active').trigger('click');
            SuccessAlertSetting.text = getResourceValue("AddPMAlerts");
            swal(SuccessAlertSetting, function () {
                $(document).find('form').trigger("reset");
                $(document).find('form').find("select").val("").trigger('change');
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
$(document).on('click', "#editprevmaintence", function (e) {
    e.preventDefault();
    var pmId = $('#preventiveMaintenanceModel_PrevMaintMasterId').val();
    $.ajax({
        url: "/PreventiveMaintenance/EditPreventive",
        type: "GET",
        dataType: 'html',
        data: { PrevMaintMasterId: pmId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpreventive').html(data);
        },
        complete: function () {
            SetPreventiveControls();

        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', "#btnSaveAnotherOpenPM,#btnSavePM", function () {
    if ($(document).find("form").valid()) {
        if ($('#preventiveMaintenanceModel_InactiveFlag').is(":checked")) {
            localStorage.setItem("PREVACTIVESTATUS", getResourceValue('AlertInactive'));
            activeStatus = true;
            localStorage.setItem("PREVENTIVESEARCHGRIDDISPLAYSTATUS", 2);
        }
        else {
            localStorage.setItem("PREVACTIVESTATUS", getResourceValue('AlertActive'));
            activeStatus = false;
            localStorage.setItem("PREVENTIVESEARCHGRIDDISPLAYSTATUS", 1);
        }
        return;
    }
    else {
        var activetagid = $('.vtabs li.active').attr('id');
        if (activetagid !== 'identificationtab') {
            $('#identificationtab').trigger('click');
        }
    }
});
$(document).on('click', "#btnCancelAddPM", function () {
    var prevMasterID = $('#preventiveMaintenanceModel_PrevMaintMasterId').val();
    if (typeof prevMasterID != "undefined" && prevMasterID != 0) {
        RedirectToDetailOncancel(prevMasterID);
    }
    else {
        swal(CancelAlertSetting, function () {
            window.location.href = "../PreventiveMaintenance/Index?page=Maintenance_Preventive_Maintenance_Search";
        });
    }
});
$(document).on('click', "#brdpreventive", function () {
    var prevMasterID = $(this).attr('data-val');
    RedirectToPmDetail(prevMasterID);
});
$(document).on('click', "#liFltEq", function (e) {
    $.ajax({
        url: "/PreventiveMaintenance/GetEquipmentLookUpIds",
        type: "GET",
        dataType: 'json',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            var equipmentIdDropDown = $(document).find('#prevMaintReassignModel_EquipmentId');
            equipmentIdDropDown.empty();
            equipmentIdDropDown.append("<option value=''>" + "--Select--" + "</option>");
            if (data.data.length > 0) {
                for (var i = 0; i < data.data.length; i++) {
                    equipmentIdDropDown.append("<option value='" + data.data[i].Value + "'>" + data.data[i].Text + "</option>");
                }
            }
        },
        complete: function () {
            $(document).find('#EquipmentPMModal').modal('show');
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', "#liFltAssign", function (e) {
    $.ajax({
        url: "/PreventiveMaintenance/GetAssignmentLookUpIds",
        type: "GET",
        dataType: 'json',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            var personnelIdDropDown = $(document).find('#prevMaintReassignModel_PersonnelId');
            personnelIdDropDown.empty();
            personnelIdDropDown.append("<option value=''>" + "--Select--" + "</option>");
            if (data.length > 0) {
                for (var i = 0; i < data.length; i++) {
                    personnelIdDropDown.append("<option value='" + data[i].Value + "'>" + data[i].Text + "</option>");
                }
            }
        },
        complete: function () {
            $(document).find('#AssignedPMModal').modal('show');
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });

});
$(document).on('click', "#liFltLoc", function (e) {
    $.ajax({
        url: "/PreventiveMaintenance/GetLocationLookUpIds",
        type: "GET",
        dataType: 'json',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            var locationDropDown = $(document).find('#prevMaintReassignModel_LocationId');
            locationDropDown.empty();
            locationDropDown.append("<option value=''>" + "--Select--" + "</option>");
            if (data.length > 0) {
                for (var i = 0; i < data.length; i++) {
                    locationDropDown.append("<option value='" + data[i].Value + "'>" + data[i].Text + "</option>");
                }
            }
        },
        complete: function () {
            $(document).find('#LocationPMModal').modal('show');
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });

});
//#endregion
$(document).on('click', '#linkToSearch', function () {
    window.location.href = "../PreventiveMaintenance/Index?page=Maintenance_Preventive_Maintenance_Search";
});
function RedirectToPmDetail(pmId, mode) {
    $.ajax({
        url: "/PreventiveMaintenance/PrevDetails",
        type: "POST",
        dataType: 'html',
        data: { PrevMaintMasterId: pmId, IsFromEquipment: IsFromEquipment },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpreventive').html(data);
            if (IsFromEquipment == true) {
                $(document).find("#linkToEquipment").text(EquipmentClientLookupId);
                $(document).find("#EquipmentId").val(RedirectEquipmentId);
            }
            $(document).find('#spnlinkToSearch').text(localStorage.getItem("PREVACTIVESTATUS"));
            $(document).find('#colorselector').prop('selectedIndex', si);
            GenerateScheduleGrid();
        },
        complete: function () {
            CloseLoader();
            if (mode === "overview") {
                $('#lioverview').trigger('click');
                $('#colorselector').val('Preventive');
            }
            if (mode === "schedule") {
                $('#pmschedulet').trigger('click');
                $('#colorselector').val('PMSchedule');
            }
            if (mode === "tasks") {
                $('#pmtaskt').trigger('click');
                $('#colorselector').val('PMTasks');
            }
            if (mode === "notes") {
                $('#pmnotest').trigger('click');
                $('#colorselector').val('PMNotes');
            }
            if (mode === "attachments") {
                $('#pmattachmentt').trigger('click');
                $('#colorselector').val('PMAttachments');
            }
            if (mode === "estimatesPart") {
                $(document).find("#drpDwn").slideToggle();
                $('#pmestpartt').trigger('click');
                $('#colorselector').val('PMEstimatesPart');
            }
            if (mode === "estimatesLabor") {
                $(document).find("#drpDwn").slideToggle();
                $('#pmestlabort').trigger('click');
                $('#colorselector').val('PMEstimatesLabor');
            }
            if (mode === "estimatesOther") {
                $(document).find("#drpDwn").slideToggle();
                $('#pmestothert').trigger('click');
                $('#colorselector').val('PMEstimatesOther');
            }
            if (mode === "EstimatesSummary") {
                $(document).find("#drpDwn").slideToggle();
                $('#EstimatesSummary').trigger('click');
                $('#colorselector').val('PMEstimatesSummery');
            }
        },
        error: function () {
            CloseLoader();
        }
    });
}
function SetPreventiveControls() {
    CloseLoader();
    $(document).find('.select2picker').select2({});
    $.validator.setDefaults({ ignore: null });
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
    $(document).find('.dtpicker').datepicker({
        changeMonth: true,
        changeYear: true,
        beforeShow: function (i) { if ($(i).attr('readonly')) { return false; } },
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy');
}
//#region WoGeneration
$(document).on('click', '#liGenWo', function () {
    $(document).find('.dtpicker1').datepicker({
        changeMonth: true,
        changeYear: true,
        "dateFormat": "mm/dd/yy",
        autoclose: true,
        minDate: 1
    }).inputmask('mm/dd/yyyy');
    $(document).find('form').trigger("reset");
    $(document).find('form').find("select").val("").trigger('change');
    $(document).find('form').find("select").removeClass("input-validation-error");
    $(document).find('form').find("input").removeClass("input-validation-error");
    ResetErrorDiv();
    $('#WorkOrderPMModal').modal('show');
});
$(document).on('change', '#ddlscheduletype', function () {
    var type = $(this).val();
    if (type == "OnDemand") {
        $(document).find('#divOnDemand').show();
    }
    else {
        $(document).find('#divOnDemand').hide();
    }
    if (type)
    {
        $(this).removeClass("input-validation-error");
    }
    
});
$(document).on('change', '#dtgeneratedthrough', function () {
    var generatedthroughid = $(this).val();   
    if (generatedthroughid) {
        $(this).removeClass("input-validation-error");
    }

});
function CloseWOModal() {
    var option = '<option value="">--Select--</option>';
    $(document).find('#ddlscheduletype').val("").trigger('change');
    $(document).find('#ondemandgroup').val("").trigger('change');
    $(document).find('#dtgeneratedthrough').val("");
    $(document).find('#preventiveMaitenanceWOModel_chkPrintWorkOrder').prop('checked', false);
    $('#WorkOrderPMModal').modal('hide');
}
function CreatePMWorkOrderOnSuccess(data) {
    CloseLoader();
    if (data.Msg > 0) {
        CloseWOModal();
        var rowCount = data.Msg;
        SuccessAlertSetting.text = getResourceValue("woGenAlert") + ' ' + rowCount;
        swal(SuccessAlertSetting, function () {
            if (data.WoList.length > 0) {
                $.ajax({
                    url: '/Workorder/PrintWoList',
                    data: {
                        listwo: data.WoList
                    },
                    type: "POST",
                    datatype: "json",
                    responseType: 'arraybuffer',
                    beforeSend: function () {
                        ShowLoader();
                    },
                    success: function (result) {
                        if (result.success && result.jsonStringExceed == false) {
                            PdfPrintAllWoList(result.pdf);
                        }
                        else {
                            CloseLoader();
                            var errorMessage = getResourceValue("PdfFileSizeExceedAlert");
                            ShowErrorAlert(errorMessage);
                            return false;
                        }
                    },
                    complete: function () {
                        CloseLoader();
                    }
                });
            }
        });
    }
    else {
        ResetErrorDiv();
        ShowGenericErrorOnAddUpdate(data, '#WorkOrderPMModal');
    }
}
function PdfPrintAllWoList(pdf) {
    var blob = b64StrtoBlob(pdf, 'application/pdf');
    var blobUrl = URL.createObjectURL(blob);
    window.open(blobUrl, "_blank");
}
function b64StrtoBlob(b64Data, contentType, sliceSize) {
    contentType = contentType || '';
    sliceSize = sliceSize || 512;
    var byteCharacters = atob(b64Data);
    var byteArrays = [];
    for (var offset = 0; offset < byteCharacters.length; offset += sliceSize) {
        var slice = byteCharacters.slice(offset, offset + sliceSize);
        var byteNumbers = new Array(slice.length);
        for (var i = 0; i < slice.length; i++) {
            byteNumbers[i] = slice.charCodeAt(i);
        }
        var byteArray = new Uint8Array(byteNumbers);
        byteArrays.push(byteArray);
    }
    var blob = new Blob(byteArrays, { type: contentType });
    return blob;
}
//#endregion

//#region ColumnVisibility
$(document).on('click', '#liCustomize', function () {
    funCustomizeBtnClick(prevSearchTable, true);
});
$(document).on('click', '.saveConfig', function () {
    var colOrder = [0, 1];
    funCustozeSaveBtn(prevSearchTable, colOrder);
    run = true;
    prevSearchTable.state.save(run);
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

//#region V2-1132 Reset Grid
$('#liResetGridClearBtn').click(function (e) {
    CancelAlertSetting.text = getResourceValue("ResetGridAlertMessage");
    swal(CancelAlertSetting, function () {
        var localstorageKeys = [];
        localstorageKeys.push("PREVENTIVESEARCHGRIDDISPLAYSTATUS"); 
        localstorageKeys.push("PREVACTIVESTATUS");
        DeleteGridLayout('PrevMaint_Search', prevSearchTable, localstorageKeys);
        GenerateSearchList('', true);
        window.location.href = "../PreventiveMaintenance/Index?page=Maintenance_Preventive_Maintenance_Search";
    });
});
//#endregion