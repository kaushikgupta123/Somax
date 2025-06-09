var gridname = "FleetService_Search";
//var orderbycol = 0;
var orderDir = 'asc';
var run = false;
var selectCount = 0;
var totalcount = 0;
var enterhit = 0; 
var SelectSOId = [];
var CustomQueryDisplayId = 0;
var assignedUsers = [];
var titleArray = [];
var classNameArray = [];
var order = '0';
var CreateStartDateVw = '';
var CreateEndDateVw = '';
var CompleteStartDateVw = '';
var CompleteEndDateVw = '';
var today = $.datepicker.formatDate('mm/dd/yy', new Date());
var personnelList = "";
var ServiceOrderType;
var ServiceOrderShift;

$(document).ready(function () {
    var strCreateStartDateVw = localStorage.getItem('SOCreateStartDateVw');
    if (strCreateStartDateVw) {
        CreateStartDateVw = strCreateStartDateVw;
    }
    var endCreateEndDateVw = localStorage.getItem('SOCreateEndDateVw');
    if (endCreateEndDateVw) {
        CreateEndDateVw = endCreateEndDateVw;
    }
    var strCompleteStartDateVw = localStorage.getItem('SOCompleteStartDateVw');
    if (strCompleteStartDateVw) {
        CompleteStartDateVw = strCompleteStartDateVw;
    }
    var endCompleteEndDateVw = localStorage.getItem('SOCompleteEndDateVw');
    if (endCompleteEndDateVw) {
        CompleteEndDateVw = endCompleteEndDateVw;
    }
    var ServiceOrderstatus = localStorage.getItem("ServiceOrderstatus");
    if (ServiceOrderstatus) {
        var text = "";
        CustomQueryDisplayId = ServiceOrderstatus;

        if (ServiceOrderstatus === '11' || ServiceOrderstatus === '12' || ServiceOrderstatus === '13' ||
            ServiceOrderstatus === '14' || ServiceOrderstatus === '15' || ServiceOrderstatus === '16') {
            $('#cmbcreateview').val(ServiceOrderstatus).trigger('change');
            $("#fleetservicesearchListul li").removeClass("activeState");
            $("#fleetservicesearchListul li[id='0']").addClass("activeState");
            text = $("#fleetservicesearchListul li[id='0']").text();
            $('#fleetservicesearchtitle').text(text);
        }
        else if (ServiceOrderstatus === '5' || ServiceOrderstatus === '6' || ServiceOrderstatus === '7' ||
            ServiceOrderstatus === '8' || ServiceOrderstatus === '9' || ServiceOrderstatus === '10') {
            $('#cmbcompleteview').val(ServiceOrderstatus).trigger('change');
            $("#fleetservicesearchListul li").removeClass("activeState");
            $("#fleetservicesearchListul li[id='4']").addClass("activeState");
            text = $("#fleetservicesearchListul li[id='4']").text();
            $('#fleetservicesearchtitle').text(text);
        }
        $('#fleetservicesearchListul li').each(function (index, value) {
            if ($(this).attr('id') == CustomQueryDisplayId) {
                text = $(this).text();
                $('#fleetservicesearchtitle').text($(this).text());
                $(".searchList li").removeClass("activeState");
                $(this).addClass('activeState');
            }
        });

        if (ServiceOrderstatus === '11' || ServiceOrderstatus === '12' || ServiceOrderstatus === '13' ||
            ServiceOrderstatus === '14' || ServiceOrderstatus === '15' || ServiceOrderstatus === '16') {
            if (ServiceOrderstatus === '16')
                text = text + " - " + $('#createdaterange').val();
            else
                text = text + " - " + $(document).find('#cmbcreateview option[value=' + ServiceOrderstatus + ']').text();
            $('#fleetservicesearchtitle').text(text);
        }
        else if (ServiceOrderstatus === '5' || ServiceOrderstatus === '6' || ServiceOrderstatus === '7' ||
            ServiceOrderstatus === '8' || ServiceOrderstatus === '9' || ServiceOrderstatus === '10') {
            if (ServiceOrderstatus === '10')
                text = text + " - " + $('#completedaterange').val();
            else
                text = text + " - " + $(document).find('#cmbcompleteview option[value=' + ServiceOrderstatus + ']').text();
            $('#fleetservicesearchtitle').text(text);
        }       
        
    }
    else {
        ServiceOrderstatus = "3";
        localStorage.setItem("ServiceOrderstatus", "3");
        $('#fleetservicesearchListul li#3').addClass('activeState');
        $('#fleetservicesearchtitle').text(getResourceValue("OpenStatusAlert"));
    }
   
    $(".dtpicker").keypress(function (event) { event.preventDefault(); });
    $(document).find('.dtpicker').datepicker({
        changeMonth: true,
        changeYear: true,
        beforeShow: function (i) { if ($(i).attr('readonly')) { return false; } },
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy');
    // for redirect from fleet asset
    var IsRedirectFromAsset = $("#IsRedirectFromAsset").val();
    var SoId = $("#SOId").val(); 
    if (IsRedirectFromAsset == "True") {
        GetSoLayout("FleetService_Search");
        
        localStorage.setItem("IsRedirectFromAssetDetails", "Redirect");
        RedirectToDetail(SoId, "overview");
        
    }
    else {
        localStorage.removeItem('IsRedirectFromAssetDetails');
        generateSODataTable();
    }
    
   
    
});
$(document).on('click', "ul.vtabs li", function () {
    $(document).find("ul.vtabs li").removeClass("active");
    $(document).find(this).addClass("active");
    $(document).find(".tabsArea").hide();
    var activeTab = $(this).find("a").attr("href");
    $(document).find(activeTab).fadeIn();
    return false;
});
$(document).on('click', '.dismiss, .overlay', function () {
    $(document).find('.sidebar').removeClass('active');
    $('.overlay').fadeOut();
});
$(document).on('click', '#pinvsidebarCollapse,#sidebarCollapse', function () {
    $('.sidebar').addClass('active');
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
//#region Search Service Task
function generateSODataTable() {
    var printCounter = 0;
    if ($(document).find('#fleetServiceSearch').hasClass('dataTable')) {
        dtTable.destroy();
    }
    dtTable = $("#fleetServiceSearch").DataTable({
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
                }
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
                    if (json.LayoutInfo) {
                        var LayoutInfo = JSON.parse(json.LayoutInfo);
                        order = LayoutInfo.order[0][0];
                        orderDir = LayoutInfo.order[0][1];
                        callback(JSON.parse(json.LayoutInfo));
                        if (json.FilterInfo) {
                            setsearchui(JSON.parse(json.FilterInfo), $("#txtColumnSearch"), $(".filteritemcount"), $("#advsearchfilteritems"));
                        }
                    }
                    else {
                        callback(json);
                    }

                }
            });
            //return o;
        },
        scrollX: true,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'Service Orders'
            },
            {
                extend: 'print',
                title: 'Service Orders'
            },
            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Service Orders',
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
                title: 'Service Orders'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/FleetService/GetFleetServiceGridData",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.CustomQueryDisplayId = localStorage.getItem("ServiceOrderstatus");
                d.CreateStartDateVw = ValidateDate(CreateStartDateVw);
                d.CreateEndDateVw = ValidateDate(CreateEndDateVw);
                d.CompleteStartDateVw = ValidateDate(CompleteStartDateVw);
                d.CompleteEndDateVw = ValidateDate(CompleteEndDateVw);
                d.personnelList = personnelList;
                d.AssetID = LRTrim($("#AssetId").val());
                d.Name = LRTrim($("#Name").val());
                d.Description = LRTrim($("#Description").val());
                d.Shift = (ServiceOrderShift != null) ? ServiceOrderShift : LRTrim($("#Shift").val());
                d.Type = (ServiceOrderType != null) ? ServiceOrderType : LRTrim($("#Type").val());
                d.VIN = LRTrim($("#VIN").val());
                d.searchText = LRTrim($(document).find('#txtColumnSearch').val());
                d.orderDir = Array.from(d.order).length > 0 ? Array.from(d.order).shift().dir : 'asc';
                d.order = order;
                 
            },
            "dataSrc": function (result) {
                var i = 0;
                let colOrder = dtTable.order();
                orderDir = colOrder[0][1];
                totalcount = result.recordsTotal;

                HidebtnLoader("btnsortmenu");
                HidebtnLoaderclass("LoaderDrop");
                if (result.data.length == "0") {
                    $(document).find('.import-export,#serbviceorder-select-all').attr('disabled', 'disabled');
                }
                else {
                    $(document).find('.import-export,#serbviceorder-select-all').removeAttr('disabled');
                }

                return result.data;
            },
            global: true
        },
        "columns":
            [
                {
                    "data": "ServiceOrderId",
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
                    "name": "0",//
                    "mRender": function (data, type, row) {
                        return '<a class=link_fleetservice_detail href="javascript:void(0)">' + data + '</a>';
                    }
                },
                { "data": "EquipmentClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1" },
                { "data": "AssetName", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2" },
                {
                    "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3",
                    render: function (data, type, row, meta) {

                        if (data == statusCode.Canceled) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--orange m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Complete) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--green m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Open) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--purple m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Scheduled) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--blue m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        
                        else {
                            return getStatusValue(data);
                        }
                    }},
                { "data": "Type", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "CreateDate", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "Assigned", "autoWidth": true, "bSearchable": true, "bSortable": true, "sClass": "ghover",
                    mRender: function (data, type, full, meta) {
                        if (full.Assign_PersonnelId == -1) {
                            return "<span>" + data + "</span><span class='tooltipgrid' id='ServiceOrder" + full.ServiceOrderId+"'></span><span class='loadingImg' style='display:none !important;'><img src='/Images/lineLoader.gif' style='width:55px;height:auto;position:absolute;left:6px;top:30px;'></span>";

                        }
                        else {
                            return "<span>" + data + "</span>";
                        }
                    }

                },
                { "data": "ScheduleDate", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "CompleteDate", "autoWidth": true, "bSearchable": true, "bSortable": true }
            ],
        columnDefs: [
            {
                targets: [0, 1],
                className: 'noVis'
            }
        ],
        initComplete: function (settings, json) {
            SetPageLengthMenu();          
            $("#fleetserviceGridAction :input").removeAttr("disabled");
            $("#fleetserviceGridAction :button").removeClass("disabled");
            DisableExportButton($("#fleetServiceSearch"), $(document).find('.import-export'));
        }
    });
}
$(document).on('click', '#fleetServiceSearch_paginate .paginate_button', function () {
    run = true;
});
$(document).on('change', '#fleetServiceSearch_length .searchdt-menu', function () {
    run = true;
});
$('#fleetServiceSearch').find('th').click(function () {
    run = true;
    order = $(this).data('col');    
});

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
        if ($(this).parent('div').find('div').hasClass('range-timeperiod')) {
            if ($(this).parent('div').find('input').val() !== '' && $(this).val() == '10') {
                f = new filterinfo('this-' + $(this).attr('id'), $(this).parent('div').find('input').val());
                filterinfoarray.push(f);
            }
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
                if ($('#' + item.key).hasClass('has-dtrangepicker') && item.value !== '') {
                    $('#' + item.key).val(item.value).trigger('change');
                    var datarangeval = data.filter(function (val) { return val.key === 'this-' + item.key; });
                    if (datarangeval.length > 0) {
                        if (datarangeval[0].value) {
                            var rangeid = $('#' + item.key).parent('div').find('input').attr('id');
                            $('#' + rangeid).css('display', 'block');
                            $('#' + rangeid).val(datarangeval[0].value);
                            if (item.key === 'dtgridadvsearchReadingDate') {
                                StartReadingDate = datarangeval[0].value.split(' - ')[0];
                                EndReadingDate = datarangeval[0].value.split(' - ')[1];
                                $(document).find('#advreadingDatedaterange').daterangepicker(daterangepickersetting, function (start, end, label) {
                                    StartReadingDate = start.format('MM/DD/YYYY');
                                    EndReadingDate = end.format('MM/DD/YYYY');
                                });
                            }
                        }
                    }
                }
                else {
                    $('#' + item.key).val(item.value);
                }
            }
            advcountercontainer.text(selectCount);
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    searchstringcontainer.html(searchitemhtml);
}
//#endregion

//#region Line Item Grid
$(document).find('#fleetServiceSearch').on('click', 'tbody td img', function (e) {
    var tr = $(this).closest('tr');
    var row = dtTable.row(tr);
    if (this.src.match('details_close')) {
        this.src = "../../Images/details_open.png";
        row.child.hide();
        tr.removeClass('shown');
    }
    else {
        this.src = "../../Images/details_close.png";
        var ServiceOrderID = $(this).attr("rel");
        $.ajax({
            url: "/FleetService/GetServiceOrderInnerGrid",
            data: {
                ServiceOrderID: ServiceOrderID
            },
            beforeSend: function () {
                ShowLoader();
            },
            dataType: 'html',
            success: function (json) {
                row.child(json).show();
                dtinnerGrid = row.child().find('.ServiceOrderinnerDataTable').DataTable(
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
                        "columnDefs": [
                            { className: 'text-right', targets: [3] }
                        ],
                        "footerCallback": function (row, data, start, end, display) {
                            var api = this.api(),
                                // Total over all pages
                                total = api.column(3).data().reduce(function (a, b) {
                                    return parseFloat(a) + parseFloat(b);
                                }, 0);
                            // Update footer
                            $(api.column(3).footer()).html(total.toFixed(2));
                        },
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

//#region Details 
$(document).on('click', '.link_fleetservice_detail', function (e) {
    e.preventDefault();
    titleText = $('#fleetservicesearchtitle').text();
    localStorage.setItem("ServiceOrderstatustext", titleText);
    var index_row = $('#fleetserviceSearch tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var ServiceOrderId = dtTable.row(row).data().ServiceOrderId; 
    $.ajax({
        url: "/FleetService/FleetServiceDetails",
        type: "POST",
        beforeSend: function () {
            ShowLoader();
        },
        data: { 'ServiceOrderId': ServiceOrderId },
        success: function (data) {
            $('#fleetservicemaincontainer').html(data);
            $('.tabcontent').show();
            $(document).find('#spnlinkToSearch').text(titleText);
            $('#Request').show();
            $('#Completion').hide();
            CloseLoader();
        },
        complete: function () {
            PopulateLineItems(ServiceOrderId);
            SetFleetServiceDetailEnvironment();
            Activity(ServiceOrderId);
        }
    });
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

//#region  Search Functionality
$(document).on('click', "#SrchBttnNew", function () {
    $.ajax({
        url: '/Base/PopulateNewSearchList',
        type: 'GET',
        data: { tableName: 'ServiceOrder' },
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
        data: { tableName: 'ServiceOrder', searchText: txtSearchval, isClear: isClear },
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
    var txtSearchval = LRTrim($(document).find('#txtColumnSearch').val());
    if (txtSearchval) {
        GenerateSearchList(txtSearchval, false);
        var searchitemhtml = "";
        searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $('#txtColumnSearch').attr('id') + '">Search: ' + txtSearchval + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
        searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
        $("#advsearchfilteritems").html(searchitemhtml);
    }
    else {
        dtTable.page('first').draw('page');
    }
    var container = $(document).find('#searchBttnNewDrop');
    container.hide("slideToggle");
}
$(document).on('click', '.btnCross', function () {
    run = true;
    var btnCrossed = $(this).parent().attr('id');
    var searchtxtId = btnCrossed.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    selectCount--;
    if (searchtxtId == "Shift") {
        $(document).find("#Shift").val("").trigger('change.select2');
        ServiceOrderShift = "";
    }
    if (searchtxtId == "Type") {
        $(document).find("#Type").val("").trigger('change.select2');
        ServiceOrderType = "";
    }
    SOAdvSearch(dtTable.length);
    dtTable.page('first').draw('page');
});
//#endregion

//#region Advance Search
function SOAdvSearch(status) {
    var InactiveFlag = false;
    var searchitemhtml = "";
    $(document).find('#txtColumnSearch').val('');
    selectCount = 0;
    $('#advsearchsidebar').find('.adv-item').each(function (index, item) {
        if ($(this).hasClass('dtpicker')) {
            $(this).val(ValidateDate($(this).val()));
        }
        if ($(this).val()) {
            selectCount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
        }
        if ($(this).attr('id') == "Shift") {
            if ($(this).val() == null && ServiceOrderShift != null) {
                selectCount++;
                searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
            }
        }
         if ($(this).attr('id') == "Type") {
            if ($(this).val() == null && ServiceOrderType != null) {
                selectCount++;
                searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
            }
        }
        
    });



    if (status != 0) {
        searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
        $("#advsearchfilteritems").html(searchitemhtml);
        $('#sp_advprocessdedbydaterange').hide();
        $('#sp_advcreatedaterange').hide();
    }
   
    $(".filteritemcount").text(selectCount);
    $('#liSelectCount').text(selectCount + ' filters applied');
}

$("#btnSODataAdvSrch").on('click', function (e) {
    run = true;
    searchresult = [];
    SOAdvSearch();
    $('.sidebar').removeClass('active');
    $('.overlay').fadeOut();
    ServiceOrderType = $("#Type").val();
    ServiceOrderShift = $("#Shift").val();
    dtTable.page('first').draw('page');
});

function clearAdvanceSearch() {
    selectCount = 0;
    $("#AssetId").val("");
    $("#Name").val("");
    $("#Description").val("");
    $("#Shift").val("").trigger('change');
    $('#Type').val("").trigger('change');
    $("#VIN").val("");
    ServiceOrderType = $('#Type').val();
    ServiceOrderShift = $("#Shift").val();

}
//#endregion
//#region Export Functionality
$(document).on('click', '#liCsv', function () {
    $(".buttons-csv")[0].click();
    $('#mask').trigger('click');
});
$(document).on('click', "#liExcel", function () {
    $(".buttons-excel")[0].click();
    $('#mask').trigger('click');
});
$(document).on('click', '#liPdf,#liPrint', function () {
    var thisid = $(this).attr('id');
    var TableHaederProp = [];
    function table(property, title) {
        this.property = property;
        this.title = title;
    }
    $("#fleetServiceSearch thead tr th").map(function (key) {
        var thisdiv = $(this).find('div');
        if ($(this).parents('.ServiceOrderinnerDataTable').length == 0 && thisdiv.html()) {
            if (this.getAttribute('data-th-prop')) {
                var tablearr = new table(this.getAttribute('data-th-prop'), thisdiv.html());
                TableHaederProp.push(tablearr);
            }
        }
    });
    var params = {
        tableHaederProps: TableHaederProp,
        colname: order,
        coldir: orderDir,
        CustomQueryDisplayId: localStorage.getItem("ServiceOrderstatus"),
        AssetID: LRTrim($("#AssetId").val()),
        AssetName: LRTrim($("#Name").val()),
        Description: LRTrim($("#Description").val()),
        Shift: (ServiceOrderShift != null) ? ServiceOrderShift : LRTrim($("#Shift").val()),
        Type: (ServiceOrderType != null) ? ServiceOrderType : LRTrim($("#Type").val()),
        VIN: LRTrim($("#VIN").val()),
        CreateStartDateVw: ValidateDate(CreateStartDateVw),
        CreateEndDateVw: ValidateDate(CreateEndDateVw),
        CompleteStartDateVw: ValidateDate(CompleteStartDateVw),
        CompleteEndDateVw: ValidateDate(CompleteEndDateVw),
        personnelList: personnelList,
        searchText: LRTrim($("#txtColumnSearch").val())
        
    };
    sOPrintParams = JSON.stringify({ 'sOPrintParams': params });
    $.ajax({
        "url": "/FleetService/SetPrintData",
        "data": sOPrintParams,
        contentType: 'application/json; charset=utf-8',
        "dataType": "json",
        "type": "POST",
        "success": function () {
            if (thisid == 'liPdf') {
                window.open('/FleetService/ExportASPDF?d=d', '_self');
            }
            else if (thisid == 'liPrint') {
                window.open('/FleetService/ExportASPDF', '_blank');
            }

            return;
        }
    });
    $('#mask').trigger('click');
});

$(function () {
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            var Asset_ID = LRTrim($("#AssetId").val());
            var Asset_Name = LRTrim($("#Name").val());
            var Description_ = LRTrim($("#Description").val());
            var Shift_ = (ServiceOrderShift != null) ? ServiceOrderShift : LRTrim($("#Shift").val());
            var Type_ = (ServiceOrderType != null) ? ServiceOrderType : LRTrim($("#Type").val());
            var VIN_ = LRTrim($("#VIN").val());
            var colname_ = order;
            var coldir_ = orderDir;
            var CreateStartDateVw_ = ValidateDate(CreateStartDateVw);
            var CreateEndDateVw_ = ValidateDate(CreateEndDateVw);
            var CompleteStartDateVw_ = ValidateDate(CompleteStartDateVw);
            var CompleteEndDateVw_ = ValidateDate(CompleteEndDateVw);
            var searchText_ = LRTrim($("#txtColumnSearch").val());
            var personnelList_ = personnelList;

            var jsonResult = $.ajax({
                url: '/FleetService/GetFSPrintData?page=all',
                data: {
                    CustomQueryDisplayId: localStorage.getItem("ServiceOrderstatus"),
                    CreateStartDateVw: CreateStartDateVw_,
                    CreateEndDateVw: CreateEndDateVw_,
                    CompleteStartDateVw: CompleteStartDateVw_,
                    CompleteEndDateVw: CompleteEndDateVw_,
                    personnelList: personnelList_,
                    AssetID: Asset_ID,
                    AssetName: Asset_Name,
                    Description: Description_,
                    Shift: Shift_,
                    Type: Type_,
                    VIN: VIN_,
                    colname: colname_,
                    coldir: coldir_,
                    searchText: searchText_
                },
                success: function (result) {
                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data;
            var visiblecolumnsIndex = $("#fleetServiceSearch thead tr th").not(":eq(0)").map(function (key) {
                return this.getAttribute('data-th-index');
            }).get();
            var d = [];
            $.each(thisdata, function (index, item) {
                if (item.CreateDate != null) {
                    item.CreateDate = item.CreateDate;
                }
                else {
                    item.CreateDate = "";
                }
                if (item.ProcessedDate != null) {
                    item.ProcessedDate = item.ProcessedDate;
                }
                else {
                    item.ProcessedDate = "";
                }
                var fData = [];
                $.each(visiblecolumnsIndex, function (index, inneritem) {
                    var key = Object.keys(item)[inneritem];
                    var value = item[key];
                    fData.push(value);
                });
                d.push(fData);
            })
            return {

                body: d,
                header: $("#fleetServiceSearch thead tr th").not(":eq(0)").find('div').map(function (key) {
                    if ($(this).parents('.ServiceOrderinnerDataTable').length == 0 && this.innerHTML) {
                        return this.innerHTML;
                    }
                }).get()
            };
        }
    });
});
//#endregion

//#region Customize
$(document).on('click', '#liCustomize', function () {
    funCustomizeBtnClick(dtTable, null, titleArray);
});
//#endregion
//#region Add
$(document).on('click', '.AddFleetService', function () {    
    $.ajax({
        url: "/FleetService/FleetServiceAddOrEdit",
        type: "GET",
        dataType: 'html',
        data: { FleetServiceId: 0 },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#fleetservicepopup').html(data);
            $('#FleetServiceModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            SetFleetServiceControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
function SetFleetServiceControls() {
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
function FleetServiceAddOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        if (data.Command== "save")
        { 
        if (data.Mode == "Add") {

            SuccessAlertSetting.text = getResourceValue("FleetServiceAddAlert");
            }
        else {
            SuccessAlertSetting.text = getResourceValue("FleetServiceUpdateAlert");       

            }
        swal(SuccessAlertSetting, function () {
            localStorage.setItem("ServiceOrderstatus", '3');
            localStorage.setItem("ServiceOrderstatustext", 'Open Service Orders');
            $("#FleetServiceModalpopup").modal('hide');
            RedirectToDetail(data.ServiceOrderId, "overview");
        });
        }       
        else {
            SuccessAlertSetting.text = getResourceValue("FleetServiceAddAlert");
            ResetErrorDiv();
            swal(SuccessAlertSetting, function () {                
                $(document).find('form').trigger("reset"); 
                SetFleetServiceControls();                              
            });
        }  
       
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}

//#endregion

//#region Edit
$(document).on('click', '#editserviceorder', function () {
    var FleetServiceId = LRTrim($(document).find('#ServiceOrderId').val());
    
    $.ajax({
        url: "/FleetService/FleetServiceAddOrEdit",
        type: "GET",
        dataType: 'html',
        data: { FleetServiceId: FleetServiceId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            
            $('#fleetservicepopup').html(data);
            $('#FleetServiceModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });           
            $(document).find("#FleetServiceModel_Pagetype").val("Edit");
            $(document).find('.select2picker').select2({});            
        },
        complete: function () {
            SetFleetServiceControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
//#endregion

//#region Search View
$(document).on('click', '.fssearchdrpbox', function (e) {
    $(".updateArea").hide();
    $(".actionBar").fadeIn();
    $(document).find('.chksearch').prop('checked', false);
    $('.itemcount').text(0);
    SelectedWoIdToCancel = [];
    run = true;
    if ($(this).attr('id') == '2') {
        personnelList = localStorage.getItem('ASSIGNEDUSERSLIST');
        if (personnelList != null && personnelList.length > 0) {
            assignedUsers = localStorage.getItem('ASSIGNEDUSERSLIST').split(',');
        }
        if (assignedUsers != null && assignedUsers.length > 0) {
            var selectedValues = new Array();
            for (var i = 0; i < assignedUsers.length; i++) {
                selectedValues[i] = assignedUsers[i];
            }
            $("#ddlUserSelect").val(selectedValues).trigger('change.select2');
        }
        $(document).find('#searcharea').hide("slide");
        $(document).find('#AssignedUsersModal').modal('show');
        return;
    }
    else {
        assignedUsers = [];
        localStorage.removeItem('ASSIGNEDUSERSLIST');
        personnelList = "";
        $("#ddlUserSelect").val('').trigger("change.select2");
    }

    
    if ($(this).attr('id') == '0') {
        $(document).find('#searcharea').hide("slide");
        var val = localStorage.getItem("ServiceOrderstatus");
        if (val == '11' || val == '12' || val == '13' || val == '14' || val == '15' || val == '16') {
            $('#cmbcreateview').val(val).trigger('change');
        }
        $(document).find('#FSDateRangeModalForCreateDate').modal('show');
        return;
    }

    else {
        CreateStartDateVw = '';
        CreateEndDateVw = '';
        localStorage.removeItem('SOCreateStartDateVw');
        localStorage.removeItem('SOCreateEndDateVw');
        $(document).find('#cmbcreateview').val('').trigger('change');
    }
 
    if ($(this).attr('id') == '4') {
        $(document).find('#searcharea').hide("slide");
        var val = localStorage.getItem("ServiceOrderstatus");
        if (val == '5' || val == '6' || val == '7' || val == '8' || val == '9' || val == '10') {
            $('#cmbcompleteview').val(val).trigger('change');
        }
        $(document).find('#SODateRangeModal').modal('show');
        return;
    }
    else {
        CompleteStartDateVw = '';
        CompleteEndDateVw = '';
        localStorage.removeItem('SOCompleteStartDateVw');
        localStorage.removeItem('SOCompleteEndDateVw');
        $(document).find('#cmbcompleteview').val('').trigger('change');
    }

    if ($(this).attr('id') != '0') {
        $('#fleetservicesearchtitle').text($(this).text());
        localStorage.setItem("ServiceOrderstatustext", $(this).text());
    }
    else {
        $('#fleetservicesearchtitle').text(getResourceValue("spnServiceOrder"));
        localStorage.setItem("ServiceOrderstatustext", getResourceValue("spnServiceOrder"));
    }
    $(".searchList li").removeClass("activeState");
    $(this).addClass('activeState');
    $(document).find('#searcharea').hide("slide");
    var optionval = $(this).attr('id');
    localStorage.setItem("ServiceOrderstatus", optionval);
    woStatus = optionval;
    if ($(document).find('#txtColumnSearch').val() !== '') {
        $('#advsearchfilteritems').find('span').html('');
        $('#advsearchfilteritems').find('span').removeClass('tagTo');
    }
    $(document).find('#txtColumnSearch').val('');

        if (optionval.length !== 0) {
            ShowbtnLoaderclass("LoaderDrop");
            dtTable.page('first').draw('page');
        }
    
});

$(function () {
    personnelList = localStorage.getItem('ASSIGNEDUSERSLIST');
    if (personnelList != null && personnelList.length > 0) {
        assignedUsers = localStorage.getItem('ASSIGNEDUSERSLIST').split(',');
    }
    if (assignedUsers != null && assignedUsers.length > 0) {
        var selectedValues = new Array();
        for (var i = 0; i < assignedUsers.length; i++) {
            selectedValues[i] = assignedUsers[i];
        }
        $("#ddlUserSelect").val(selectedValues).trigger('change');
    }
});
$(document).on('change', '#ddlUserSelect', function () {
    var val = $(this).val();
    if (val && val.length > 0) {
        $('#btnAssignedUsers').removeAttr('disabled');
    }
    else {
        $('#btnAssignedUsers').attr('disabled', 'disabled');
    }
});
//Seect Assigned User
$(document).on('click', '#btnAssignedUsers', function () {
    run = true;
    personnelList = ''; var i = 0; assignedUsers = [];
    assignedUsers = $(document).find('#ddlUserSelect').val();
    if (assignedUsers.length == 0) {
        return false;
    }
    localStorage.setItem('ASSIGNEDUSERSLIST', assignedUsers);
    for (var i = 0; i < assignedUsers.length; i++) {
        personnelList = personnelList + assignedUsers[i] + ',';
    }
    personnelList = personnelList.slice(0, -1);
    $(document).find('#AssignedUsersModal').modal('hide');
    var text = $('#fleetservicesearchListul').find('li').eq(2).text();
    $('#fleetservicesearchtitle').text(text);
    $("#fleetservicesearchListul li").removeClass("activeState");
    $("#fleetservicesearchListul li").eq(2).addClass('activeState');
    var optionval = $("#fleetservicesearchListul li").eq(2).attr('id');
    localStorage.setItem("ServiceOrderstatus", optionval);
   
    CustomQueryDisplayId = optionval;
    localStorage.setItem("ServiceOrderstatus", CustomQueryDisplayId);
    
        if (assignedUsers.length !== 0) {
            ShowbtnLoaderclass("LoaderDrop");
            dtTable.page('first').draw('page');
        }
    
    if ($(document).find('#txtColumnSearch').val() !== '') {
        $('#advsearchfilteritems').find('span').html('');
        $('#advsearchfilteritems').find('span').removeClass('tagTo');
    }
    $(document).find('#txtColumnSearch').val('');
});
//all status
$(document).on('change', '#cmbcreateview', function (e) {
    var thielement = $(this);
    CustomQueryDisplayId = thielement.val();
    if (thielement.val() == '16') {
        var strtlocal = localStorage.getItem('SOCreateStartDateVw');
        if (strtlocal) {
            CreateStartDateVw = strtlocal;
        }
        else {
            CreateStartDateVw = today;
        }
        var endlocal = localStorage.getItem('SOCreateEndDateVw');
        if (endlocal) {
            CreateEndDateVw = endlocal;
        }
        else {
            CreateEndDateVw = today;
        }
        $(document).find('#timeperiodcontainerForCreateDate').show();
        $(document).find('#createdaterange').daterangepicker({
            format: 'MM/DD/YYYY',
            startDate: CreateStartDateVw,
            endDate: CreateEndDateVw,
            "locale": {
                "applyLabel": getResourceValue("JsApply"),
                "cancelLabel": getResourceValue("CancelAlert")
            }
        }, function (start, end, label) {
            CreateStartDateVw = start.format('MM/DD/YYYY');
            CreateEndDateVw = end.format('MM/DD/YYYY');
        });
    }
    else {
        CreateStartDateVw = '';
        CreateEndDateVw = '';
        localStorage.removeItem('SOCreateStartDateVw');
        localStorage.removeItem('SOCreateEndDateVw');
        $(document).find('#timeperiodcontainerForCreateDate').hide();
    }
});

$(document).on('click', '#btntimeperiodForCreateDate', function (e) {
    run = true;
    var daterangeval = $(document).find('#cmbcreateview').val();
    if (daterangeval == '') {
        return;
    }
    CustomQueryDisplayId = daterangeval;
    if (daterangeval != '16') {
        CreateStartDateVw = '';
        CreateEndDateVw = '';
        localStorage.removeItem('SOCreateStartDateVw');
        localStorage.removeItem('SOCreateEndDateVw');
    }
    else {
        localStorage.setItem('SOCreateStartDateVw', CreateStartDateVw);
        localStorage.setItem('SOCreateEndDateVw', CreateEndDateVw);
    }
    $(document).find('#FSDateRangeModalForCreateDate').modal('hide');
    var text = $('#fleetservicesearchListul').find('li').eq(0).text();

    if (daterangeval != '16')

        text = text + " - " + $(document).find('#cmbcreateview option[value=' + daterangeval + ']').text();
    else
        text = text + " - " + $('#createdaterange').val();

    $('#fleetservicesearchtitle').text(text);
    $("#fleetservicesearchListul li").removeClass("activeState");
    $("#fleetservicesearchListul li").eq(0).addClass('activeState');
    localStorage.setItem("ServiceOrderstatus", daterangeval);
    if ($(document).find('#txtColumnSearch').val() !== '') {
        $('#advsearchfilteritems').find('span').html('');
        $('#advsearchfilteritems').find('span').removeClass('tagTo');
    }
    $(document).find('#txtColumnSearch').val('');

    CustomQueryDisplayId = daterangeval;
    localStorage.setItem("ServiceOrderstatus", CustomQueryDisplayId);
    if (daterangeval.length !== 0) {
        ShowbtnLoaderclass("LoaderDrop");
        dtTable.page('first').draw('page');
    }

});
//Complete view
$(document).on('change', '#cmbcompleteview', function (e) {
    var thielement = $(this);
    woStatus = thielement.val();

    if (thielement.val() == '10') {
        CompleteStartDateVw = today;
        CompleteEndDateVw = today;
        var strtlocal = localStorage.getItem('SOCompleteStartDateVw');
        if (strtlocal) {
            CompleteStartDateVw = strtlocal;
        }
        else {
            CompleteStartDateVw = today;
        }
        var endlocal = localStorage.getItem('SOCompleteEndDateVw');
        if (endlocal) {
            CompleteEndDateVw = endlocal;
        }
        else {
            CompleteEndDateVw = today;
        }
        $(document).find('#timeperiodcontainer').show();
        $(document).find('#completedaterange').daterangepicker({
            format: 'MM/DD/YYYY',
            startDate: CompleteStartDateVw,
            endDate: CompleteEndDateVw,
            "locale": {
                "applyLabel": getResourceValue("JsApply"),
                "cancelLabel": getResourceValue("CancelAlert")
            }
        }, function (start, end, label) {
            CompleteStartDateVw = start.format('MM/DD/YYYY');
            CompleteEndDateVw = end.format('MM/DD/YYYY');
        });
    }
    else {
        $(document).find('#timeperiodcontainer').hide();
    }
});
$(document).on('click', '#btntimeperiod', function (e) {
    run = true;
    var daterangeval = $(document).find('#cmbcompleteview').val();
    if (daterangeval == '') {
        return;
    }
    CustomQueryDisplayId = daterangeval;
    if (daterangeval != '10') {
        CompleteStartDateVw = '';
        CompleteEndDateVw = '';
        localStorage.removeItem('SOCompleteStartDateVw');
        localStorage.removeItem('SOCompleteEndDateVw');
    }
    else {
        localStorage.setItem('SOCompleteStartDateVw', CompleteStartDateVw);
        localStorage.setItem('SOCompleteEndDateVw', CompleteEndDateVw);
    }
    $(document).find('#SODateRangeModal').modal('hide');
    var text = $('#fleetservicesearchListul').find('li').eq(4).text();

    //-------------------------------------------------------
    if (daterangeval != '10')
        text = text + " - " + $(document).find('#cmbcompleteview option[value=' + daterangeval + ']').text();
    else
        text = text + " - " + $('#completedaterange').val();
    //-------------------------------------------------------

    $('#fleetservicesearchtitle').text(text);
    $("#fleetservicesearchListul li").removeClass("activeState");
    $("#fleetservicesearchListul li").eq(4).addClass('activeState');
    localStorage.setItem("ServiceOrderstatus", daterangeval);
    if ($(document).find('#txtColumnSearch').val() !== '') {
        $('#advsearchfilteritems').find('span').html('');
        $('#advsearchfilteritems').find('span').removeClass('tagTo');
    }
    $(document).find('#txtColumnSearch').val('');
    CustomQueryDisplayId = daterangeval;
    localStorage.setItem("ServiceOrderstatus", CustomQueryDisplayId);
    if (daterangeval.length !== 0) {
        ShowbtnLoaderclass("LoaderDrop");
        dtTable.page('first').draw('page');
    }
});
//#endregion

//#region ColumnVisibility
$(document).on('click', '#liCustomize', function () {
    funCustomizeBtnClick(dtTable,true);
});
$(document).on('click', '.saveConfig', function () {
    var colOrder = [0, 1];
    funCustozeSaveBtn(dtTable, colOrder);
    run = true;
    dtTable.state.save(run);
});
//#endregion

$(document).on("hidden.bs.modal", "#FleetServiceModal", function (e) {
    $("body").addClass("modal-open");
});
//#endregion

//#region
$(document).on('click', '#anchPhoto', function () {
    hideAudit();
});
$(document).on('click', '#photot', function () {
    $('#Overview').hide();
    $('.tabcontent').hide();
});
$(document).on('click', '#fleetServiceOverview', function () {
    $('#Overview').show();
    RemovedTabsActive();
    $('#tabRequest').addClass('active');
    $('.tabcontent').show();
    $('#Request').show();
    $('#Completion').hide();
    $('.lineitemdetail').trigger('click');
    ShowAudit();
});

function SetFleetServiceDetailEnvironment() {
    CloseLoader();
    ZoomImage($(document).find('#EquipZoom'));
    Dropzone.autoDiscover = false;
    if ($(document).find('#dropzoneForm').length > 0) {
        var myDropzone = new Dropzone("div#dropzoneForm", {
            url: "../Base/SaveUploadedFile",
            addRemoveLinks: true,
            paramName: 'file',
            maxFilesize: 10, // MB
            maxFiles: 4,
            dictDefaultMessage: 'Drag files here to upload, or click to select one',
            acceptedFiles: ".jpeg,.jpg,.png",
            init: function () {
                this.on("removedfile", function (file) {
                    if (file.type != 'image/jpeg' && file.type != 'image/jpg' && file.type != 'image/png') {
                        ShowErrorAlert(getResourceValue("spnValidImage"));
                    }
                });
            },
            autoProcessQueue: true,
            dictRemoveFile: "X",
            uploadMultiple: true,
            dictRemoveFileConfirmation: "Do you want to remove this file ?",
            dictCancelUpload: "X",
            parallelUploads: 1,
            dictMaxFilesExceeded: "You can not upload any more files.",
            success: function (file, response) {
                var imgName = response;
                file.previewElement.classList.add("dz-success");
                var radImage = '<a class="lnkContainer setImage" data-toggle="tooltip" title="Upload a selective Image!" data-image="' + file.name + '" style="cursor:pointer;"><i class="fa fa-upload" style="cursor:pointer;"></i></a>';
                $(file.previewElement).append(radImage);
            },
            error: function (file, response) {
                if (file.size > (1024 * 1024 * 10)) // not more than 10mb
                {
                    ShowImageSizeExceedAlert();
                }
                file.previewElement.classList.add("dz-error");
                var _this = this;
                _this.removeFile(file);

            }
        });
    }
    SetFixedHeadStyle();
}
function ZoomImage(element) {
    element.elevateZoom(
        {
            zoomType: "window",
            lensShape: "round",
            lensSize: 1000,
            zoomWindowFadeIn: 500,
            zoomWindowFadeOut: 500,
            lensFadeIn: 100,
            lensFadeOut: 100,
            easing: true,
            scrollZoom: true,
            zoomWindowWidth: 450,
            zoomWindowHeight: 450
        });
}
$(document).on('click', '#linkToSearch', function () {
    window.location.href = "../FleetService/Index?page=Fleet_Service";
});
//#endregion

//#region Hovor on search grid
$('#fleetServiceSearch').on('mouseenter', '.ghover', function (e) {
    var rowData = dtTable.row(this).data();
    var ServiceOrderId = rowData.ServiceOrderId;
    var thise = $(this);
    if (rowData.Assign_PersonnelId == -1) {
        if (LRTrim(thise.find('.tooltipgrid').text()).length > 0) {
            thise.find('.tooltipgrid').attr('style', 'display :block !important;');
            return;
        }
    }

    if (rowData.Assign_PersonnelId == -1) {
        $.ajax({
            "url": "/FleetService/PopulateHover",
            "data": {
                ServiceOrderId: ServiceOrderId
            },
            "dataType": "json",
            "type": "POST",
            "beforeSend": function (data) {
                thise.find('.loadingImg').show();
            },
            "success": function (data) {
                if (data.personnelList != null) {
                    $('#ServiceOrder' + ServiceOrderId).text(data.personnelList);
                }
            },
            "complete": function () {
                thise.find('.loadingImg').hide();
                thise.find('.tooltipgrid').attr('style', 'display :block !important;');
            }
        });
    }
});
$('#fleetServiceSearch').on('mouseleave', '.ghover', function (e) {
    $(this).find('.tooltipgrid').attr('style', 'display :none !important;');
});
//#endregion

$(document).on('keyup', '#Servicesearctxtbox', function (e) {
    var tagElems = $(document).find('#fleetservicesearchListul').children();
    $(tagElems).hide();
    for (var i = 0; i < tagElems.length; i++) {
        var tag = $(tagElems).eq(i);
        if ($(tag).text().toLowerCase().includes($(this).val().toLowerCase()) == true || $(this).val().toLowerCase().includes($(tag).text().toLowerCase()) == true) {
            $(tag).show();
        }
    }
});

//#region Get layout for service order
function GetSoLayout(GridName) {
    $.ajax({
        "url": "/Base/GetLayout",
        "data": {
            GridName: GridName
        },
        "async": false,
        "dataType": "json",
        "success": function (json) {
            if (json.LayoutInfo) {
                var LayoutInfo = JSON.parse(json.LayoutInfo);
                order = LayoutInfo.order[0][0];
                orderDir = LayoutInfo.order[0][1];
                //callback(JSON.parse(json.LayoutInfo));
                if (json.FilterInfo) {
                    setsearchui(JSON.parse(json.FilterInfo), $("#txtColumnSearch"), $(".filteritemcount"), $("#advsearchfilteritems"));
                }
            }
            else {
                //callback(json);
            }
        }
    });
}
//#endregion

