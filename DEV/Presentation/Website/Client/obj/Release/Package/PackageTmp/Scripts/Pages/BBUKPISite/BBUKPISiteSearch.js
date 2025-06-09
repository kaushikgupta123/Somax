var dtTable;
var run = false;
var GridName = "BBUKPISite_Search";
var order = '0';
var orderDir = 'asc';
var selectCount = 0;
var CustomQueryDisplayId = '2';
var today = $.datepicker.formatDate('mm/dd/yy', new Date());
var CreateStartDateVw = '';
var CreateEndDateVw = '';
var SubmitStartDateVw = '';
var SubmitEndDateVw = '';
var yearweekfilters = "";
var dtTableForExport;

$(document).ready(function () {
    //$('#sampleDatepicker').datepicker({
    //    changeMonth: true,
    //    changeYear: true,
    //    dateFormat: "dd/mm/yy",
    //    yearRange: "-90:+00"
    //});
    ShowbtnLoaderclass("LoaderDrop");
    $(".updateArea").hide();
    $(".actionBar").fadeIn();
    $("#SiteGridAction :input").attr("disabled", "disabled");
    //ShowbtnLoader("btnsortmenu");
    //$("#action").click(function () {
    //    $(".actionDrop").slideToggle();
    //});
    //$(".actionDrop ul li a").click(function () {
    //    $(".actionDrop").fadeOut();
    //});
    //$("#action").focusout(function () {
    //    $(".actionDrop").fadeOut();
    //});
    $("#sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $('#dismiss, .overlay').on('click', function () {
        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });
    //$("#btnupdateequip").click(function () {
    //    $(".actionDrop2").slideToggle();
    //});
    //$(".actionDrop2 ul li a").click(function () {
    //    $(".actionDrop2").fadeOut();
    //});
    //$("#btnupdateequip").focusout(function () {
    //    $(".actionDrop2").fadeOut();
    //});
    $("#btnSiteDataAdvSrch").on('click', function (e) {
        run = true;
        $(document).find('#txtColumnSearch').val('');
        searchresult = [];
        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();
        BBUKPISiteAdvSearch();
        dtTable.page('first').draw('page');
    });
    
    $(document).on('click', "ul.vtabs li", function () {
        $(document).find("ul.vtabs li").removeClass("active");
        $(document).find(this).addClass("active");
        $(document).find(".tabsArea").hide();
        var activeTab = $(this).find("a").attr("href");
        $(document).find(activeTab).fadeIn();
        return false;
    });

    $(document).find('.select2picker').select2({});
    $(document).find('.dtpicker:not(.readonly)').datepicker({
        changeMonth: true,
        changeYear: true,
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy');
    $('.select2picker, form').change(function () {
        var areaddescribedby = $(this).attr('aria-describedby');
        //if ($(this).attr('id') != 'EquipDeptId' && $(this).valid()) {
        if ($(this).closest('form').length > 0) {
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
        }
    });
    LoadPreviousValuesForCustomQuery();
    //#region Load Grid With Year Week Filter
    var yearweekfilterstatus = localStorage.getItem("yearweektext");
    if (yearweekfilterstatus != "" && yearweekfilterstatus != undefined) {
        yearweekfilters = yearweekfilterstatus;
    }
    else {
        yearweekfilters = "";
    }
    //#endregion
    generateBBUKPISiteDataTable();
    var yearweekfilterstatusval = localStorage.getItem("yearweekvalue");
    if (yearweekfilterstatusval != "" && yearweekfilterstatusval != undefined) {
        var stringToArray = yearweekfilterstatusval.split(',');
        $('#ddlYearWeeks').val(stringToArray).trigger('change.select2');
    }
});
function LoadPreviousValuesForCustomQuery() {
    let OldCustomQueryId = localStorage.getItem("bbukpisitestatus");
    let text = '';
    if (OldCustomQueryId != 'undefined' && OldCustomQueryId != null && OldCustomQueryId != "") {
        CustomQueryDisplayId = OldCustomQueryId;
        if (OldCustomQueryId == '4' || OldCustomQueryId == '5' || OldCustomQueryId == '6' || OldCustomQueryId == '7' || OldCustomQueryId == '1' || OldCustomQueryId == '12') {
            $('#cmbcreateview').val(OldCustomQueryId).trigger('change');
            text = $('#BBUKPISitesearchListul').find('li').eq(0).text();
            $('#sitesearchtitle').text(text);
            $("#BBUKPISitesearchListul li").removeClass("activeState");
            $("#BBUKPISitesearchListul li").eq(0).addClass('activeState');
        }
        else if (OldCustomQueryId == '8' || OldCustomQueryId == '9' || OldCustomQueryId == '10' || OldCustomQueryId == '11' || OldCustomQueryId == '3' || OldCustomQueryId == '13') {
            $('#cmbsubmitview').val(OldCustomQueryId).trigger('change');
            text = $('#BBUKPISitesearchListul').find('li').eq(2).text();
            $('#sitesearchtitle').text(text);
            $("#BBUKPISitesearchListul li").removeClass("activeState");
            $("#BBUKPISitesearchListul li").eq(2).addClass('activeState');
        }
        else {
            $('#BBUKPISitesearchListul li').each(function (index, value) {
                if ($(this).attr('id') == OldCustomQueryId && $(this).attr('id') != '0') {
                    $('#sitesearchtitle').text($(this).text());
                    $(".searchList li").removeClass("activeState");
                    $(this).addClass('activeState');
                }
            });
        }
        if ($('#BBUKPISitesearchListul').find('.activeState').attr('id') == '1') {
            if ($(document).find('#cmbcreateview').val() != '12')
                text = text + " - " + $(document).find('#cmbcreateview option[value=' + OldCustomQueryId + ']').text();
            else
                text = text + " - " + $('#createdaterange').val();
            $('#sitesearchtitle').text(text);
        }
        if ($('#BBUKPISitesearchListul').find('.activeState').attr('id') == '3') {
            if ($(document).find('#cmbsubmitview').val() != '13')
                text = text + " - " + $(document).find('#cmbsubmitview option[value=' + OldCustomQueryId + ']').text();
            else
                text = text + " - " + $('#submitdaterange').val();
            $('#sitesearchtitle').text(text);
        }
    }
    else {
        CustomQueryDisplayId = "2";
        $('#sitesearchtitle').text($('#BBUKPISitesearchListul').find('li').eq(1).text());
        $("#BBUKPISitesearchListul li").eq(1).addClass("activeState");
    }
}
function generateBBUKPISiteDataTable() {
    if ($(document).find('#BBUKPISiteSearch').hasClass('dataTable')) {
        dtTable.destroy();
    }
    dtTable = $("#BBUKPISiteSearch").DataTable({
        colReorder: {
            fixedColumnsLeft:1
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
                    "url": "/Base/CreateUpdateState",
                    "data": {
                        GridName: GridName,
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
                    GridName: GridName
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
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'BBU KPI Site List'
            },
            {
                extend: 'print',
                title: 'BBU KPI Site List'
            },

            {
                text: 'Export CSV',
                extend: 'csv',
                title: 'BBU KPI Site List',
                extension: '.csv'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                exportOptions: {
                    columns: ':visible'
                },
                css: 'display:none',
                title: 'BBU KPI Site List',
                orientation: 'landscape',
                pageSize: 'A3'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/BBUKPISite/GetBBUKPISiteGridData",
            "type": "post",
            "datatype": "json",
            cache: false,
            data: function (d) {
                d.customQueryDisplayId = CustomQueryDisplayId;
                d.Order = order;
                d.Week = LRTrim($("#Week").val());
                d.Year = LRTrim($("#Year").val());
                d.Status = LRTrim($("#Status").val());
                d.Created = LRTrim($("#Created").val());
                d.PMPercentCompleted = LRTrim($("#PMPercentCompleted").val());
                d.WOBacklogCount = LRTrim($("#WOBacklogCount").val());
                d.SubmitDate = LRTrim($("#SubmitDate").val());
                d.SearchText = LRTrim($(document).find('#txtColumnSearch').val());
                d.CreateStartDateVw = CreateStartDateVw;
                d.CreateEndDateVw = CreateEndDateVw;
                d.SubmitStartDateVw = SubmitStartDateVw;
                d.SubmitEndDateVw = SubmitEndDateVw;
                d.YearWeekfilters = yearweekfilters;
            },
            "dataSrc": function (result) {
                let colOrder = dtTable.order();
                //order = colOrder[0][0];
                orderDir = colOrder[0][1];

                if (result.data.length < 1) {
                    $(document).find('#btnExport').prop('disabled', true);
                }
                else {
                    $(document).find('#btnExport').prop('disabled', false);
                }

                //HidebtnLoader("btnsortmenu");
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
                
                { "data": "Year", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "0"},
                {
                    "data": "Week",
                    "bSortable": true,
                    "autoWidth": true,
                    "bSearchable": true,
                    "name": "1",
                    "mRender": function (data, type, row) {
                        return '<a class=link_bbukpi_detail href="javascript:void(0)">' + data + '</a>';
                    }
                },
                { "data": "weekStart", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2" },
                { "data": "weekEnd", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3" },
                /*{ "data": "Created", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "4", "className": "text-right" },*/
                { "data": "Created", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "4" },
                { "data": "PMWOCompleted", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right", "name": "5" },
                { "data": "WOBacklogCount", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right", "name": "6" },
                { "data": "phyInvAccuracy", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right", "name": "7" },

                /*hiden*/
                { "data": "pMFollowUpComp", "autoWidth": true, "bSearchable": false, "bSortable": false, "name": "8" },
                { "data": "activeMechUsers", "autoWidth": true, "bSearchable": false, "bSortable": false, "name": "9" },
                { "data": "rCACount", "autoWidth": true, "bSearchable": false, "bSortable": false, "name": "10" },
                { "data": "tTRCount", "autoWidth": true, "bSearchable": false, "bSortable": false, "name": "11" },
                { "data": "invValueOverMax", "autoWidth": true, "bSearchable": false, "bSortable": false, "name": "12" },
                { "data": "cycleCountProgress", "autoWidth": true, "bSearchable": false, "bSortable": false, "name": "13" },
                { "data": "eVTrainingHrs", "autoWidth": true, "bSearchable": false, "bSortable": false, "name": "14" },
                { "data": "siteName", "autoWidth": true, "bSearchable": false, "bSortable": false, "name": "15" },
            ],
        columnDefs: [
            {
                targets: [0],
                className: 'noVis'
            }
        ],
        initComplete: function () {
            SetPageLengthMenu();
            $("#SiteGridAction :input").removeAttr("disabled");
            $("#SiteGridAction :button").removeClass("disabled");
            DisableExportButton($("#BBUKPISiteSearch"), $(document).find('#btnExport'));
            $(document).find('.multiselect-container li').each(function (index, value) {
                var a = $(this).children().first().children().first().attr('title');
                if (a == "Count PM Follow up" || a == "# Active Mechanic Users" || a == "RCPS Events on Equip breakdown" || a == "TTR Events" || a == "Inventory Dollar value Over Max" || a == "Cycle Count Progress %" || a == "Weekly hours of EV Training" || a == "Site") {
                    $(this).removeClass('active');
                    $(this).css('display', 'none');
                }
            });
            $(document).find('#PresenterList li').each(function (index, value) {
                var a = $(this).attr('data-val');
                if (a == "8" || a == "9" || a == "10" || a == "11" || a == "12" || a == "13" || a == "14" || a == "15") {
                    $(this).css('display', 'none');
                }
            });
        }
    });
}
$(document).on('click', '#BBUKPISiteSearch_paginate .paginate_button', function () {
    run = true;
});
$(document).on('change', '#BBUKPISiteSearch_length .searchdt-menu', function () {
    run = true;
});
$('#BBUKPISiteSearch').find('th').click(function () {
    if ($(this).data('col') !== undefined && $(this).data('col') !== '') {
        run = true;
        order = $(this).data('col');
    }
});
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
function filterinfo(id, value) {
    this.key = id;
    this.value = value;
}
function BBUKPISiteAdvSearch() {
    var InactiveFlag = false;
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
    var optionVal = $(document).find("#equipDropdown").val();
    if (optionVal == "1") {
        InactiveFlag = true;
    }
    $("#dvFilterSearchSelect2").html(searchitemhtml);
    $("#spnControlCounter").text(selectCount);
    $('#liSelectCount').text(selectCount + ' filters applied');
}
$(document).on('click', '#liClearAdvSearchFilter', function () {
    run = true;
    $('#txtColumnSearch').val('');
    clearAdvanceSearch();
    dtTable.page('first').draw('page');
});
function clearAdvanceSearch() {
    $('.adv-item').val("");
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
    BBUKPISiteAdvSearch();
    dtTable.page('first').draw('page');
});

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
            dtTableForExport = $("#BBUKPISiteSearchForExport").DataTable();
            var info = dtTableForExport.page.info();
            var lengthMenuSetting = info.length;

            var jsonResult = $.ajax({
                "url": "/BBUKPISite/GetBBUKPISitePrintData",
                "type": "get",
                "datatype": "json",
                data: {
                    customQueryDisplayId: CustomQueryDisplayId,
                    Order: order,
                    OrderDir: orderDir,
                    length: lengthMenuSetting,
                    Week: LRTrim($("#Week").val()),
                    Year: LRTrim($("#Year").val()),
                    Status: LRTrim($("#Status").val()),
                    Created: LRTrim($("#Created").val()),
                    PMPercentCompleted: LRTrim($("#PMPercentCompleted").val()),
                    WOBacklogCount: LRTrim($("#WOBacklogCount").val()),
                    SubmitDate: LRTrim($("#SubmitDate").val()),
                    SearchText: LRTrim($(document).find('#txtColumnSearch').val()),
                    CreateStartDateVw: CreateStartDateVw,
                    CreateEndDateVw: CreateEndDateVw,
                    SubmitStartDateVw: SubmitStartDateVw,
                    SubmitEndDateVw: SubmitEndDateVw,
                    YearWeekfilters: yearweekfilters,
                },
                success: function (result) {
                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data;
            var visiblecolumnsIndex = $("#BBUKPISiteSearchForExport thead tr th").map(function (key) {
                return this.getAttribute('data-th-index');
            }).get();
            var d = [];
            $.each(thisdata, function (index, item) {
                if (item.Week != null) {
                    item.Week = item.Week;
                }
                else {
                    item.Week = "";
                }

                if (item.Year != null) {
                    item.Year = item.Year;
                }
                else {
                    item.Year = "";
                }

                if (item.siteName != null) {
                    item.siteName = item.siteName;
                }
                else {
                    item.siteName = "";
                }

                if (item.Created != null) {
                    item.Created = item.Created;
                }
                else {
                    item.Created = "";
                }

                if (item.PMWOCompleted != null) {
                    item.PMWOCompleted = item.PMWOCompleted;
                }
                else {
                    item.PMWOCompleted = "";
                }

                if (item.WOBacklogCount != null) {
                    item.WOBacklogCount = item.WOBacklogCount;
                }
                else {
                    item.WOBacklogCount = "";
                }

                if (item.pMFollowUpComp != null) {
                    item.pMFollowUpComp = item.pMFollowUpComp;
                }
                else {
                    item.pMFollowUpComp = "";
                }

                if (item.activeMechUsers != null) {
                    item.activeMechUsers = item.activeMechUsers;
                }
                else {
                    item.activeMechUsers = "";
                }
                if (item.rCACount != null) {
                    item.rCACount = item.rCACount;
                }
                else {
                    item.rCACount = "";
                }
                if (item.tTRCount != null) {
                    item.tTRCount = item.tTRCount;
                }
                else {
                    item.tTRCount = "";
                }
                if (item.invValueOverMax != null) {
                    item.invValueOverMax = item.invValueOverMax;
                }
                else {
                    item.invValueOverMax = "";
                }
                if (item.phyInvAccuracy != null) {
                    item.phyInvAccuracy = item.phyInvAccuracy;
                }
                else {
                    item.phyInvAccuracy = "";
                }
                if (item.cycleCountProgress != null) {
                    item.cycleCountProgress = item.cycleCountProgress;
                }
                else {
                    item.cycleCountProgress = "";
                }
                if (item.eVTrainingHrs != null) {
                    item.eVTrainingHrs = item.eVTrainingHrs;
                }
                else {
                    item.eVTrainingHrs = "";
                }
                if (item.weekStart != null) {
                    item.weekStart = item.weekStart;
                }
                else {
                    item.weekStart = "";
                }
                if (item.weekEnd != null) {
                    item.weekEnd = item.weekEnd;
                }
                else {
                    item.weekEnd = "";
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
                header: $("#BBUKPISiteSearchForExport thead tr th").map(function (key) {
                    return this.innerHTML;
                }).get()
            };
        }
    });
});

//#region ColumnVisibility
$(document).on('click', '#liCustomize', function () {
    funCustomizeBtnClick(dtTable, true);
    var SelectedColCounter = $(document).find('#lblCounter').text();
    $(document).find('.multiselect-container li').each(function (index, value) {
        var a = $(this).children().first().children().first().attr('title');
        if (a == "Count PM Follow up" || a == "# Active Mechanic Users" || a == "RCPS Events on Equip breakdown" || a == "TTR Events" || a == "Inventory Dollar value Over Max" || a == "Cycle Count Progress %" || a == "Weekly hours of EV Training" || a == "Site") {
            $(this).removeClass('active');
            $(this).css('display', 'none');
            SelectedColCounter--;
        }
    });
    $(document).find('#PresenterList li').each(function (index, value) {
        var a = $(this).attr('data-val');
        if (a == "8" || a == "9" || a == "10" || a == "11" || a == "12" || a == "13" || a == "14" || a == "15") {
            $(this).css('display', 'none');
        }
    });
    $(document).find('#lblCounter').text(SelectedColCounter);
});
$(document).on('click', '.saveConfig', function () {
    var colOrder = [0];
    funCustozeSaveBtn(dtTable, colOrder);
    run = true;
    dtTable.state.save(run);
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
$(document).on('keyup', '#BBUKPISitesearctxtbox', function (e) {
    var tagElems = $(document).find('#BBUKPISitesearchListul').children();
    $(tagElems).hide();
    for (var i = 0; i < tagElems.length; i++) {
        var tag = $(tagElems).eq(i);
        if ($(tag).text().toLowerCase().includes($(this).val().toLowerCase()) == true || $(this).val().toLowerCase().includes($(tag).text().toLowerCase()) == true) {
            $(tag).show();
        }
    }
});
$(document).on('click', '.BBUKPISitesearchdrpbox', function (e) {
    $(document).find('#txtColumnSearch').val('');
    $(".updateArea").hide();
    $(".actionBar").fadeIn();
    $('.itemcount').text(0);
    //SelectPRDetails = [];
    //SelectPRId = [];
    run = true;
    if ($(this).attr('id') == '1') {
        $(document).find('#searcharea').hide("slide");
        var val = localStorage.getItem("bbukpisitestatus");
        if (val == '1' || val == '4' || val == '5' || val == '6' || val == '7' || val == '12') {
            $('#cmbcreateview').val(val).trigger('change');
        }
        $(document).find('#SiteDateRangeModalForCreateDate').modal('show');
        return;
    }
    else {
        CreateStartDateVw = '';
        CreateEndDateVw = '';
        localStorage.removeItem('BBUKPISiteCreateStartDateVw');
        localStorage.removeItem('BBUKPISiteCreateEndDateVw');
        $(document).find('#cmbcreateview').val('').trigger('change');
    }
    if ($(this).attr('id') == '3') {
        $(document).find('#searcharea').hide("slide");
        var val = localStorage.getItem("bbukpisitestatus");
        if (val == '3' || val == '8' || val == '9' || val == '10' || val == '11' || val == '13') {
            $('#cmbsubmitview').val(val).trigger('change');
        }
        $(document).find('#SiteDateRangeModalForSubmitDate').modal('show');
        return;
    }
    else {
        SubmitStartDateVw = '';
        SubmitEndDateVw = '';
        localStorage.removeItem('BBUKPISiteSubmitStartDateVw');
        localStorage.removeItem('BBUKPISiteSubmitEndDateVw');
        $(document).find('#cmbsubmitview').val('').trigger('change');
    }
    //var val = localStorage.getItem("MATERIALREQUESTSTATUS");

    $(".searchList li").removeClass("activeState");
    $(this).addClass('activeState');
    $(document).find('#searcharea').hide("slide");
    var optionval = $(this).attr('id');
    //localStorage.setItem("MATERIALREQUESTSTATUS", optionval);
    localStorage.setItem("bbukpisitestatus", optionval);
    CustomQueryDisplayId = optionval;
    $(document).find('#sitesearchtitle').text($(this).text());
    ShowbtnLoaderclass("LoaderDrop");
    BBUKPISiteAdvSearch();
    dtTable.page('first').draw('page');
});
$(document).on('change', '#cmbcreateview', function (e) {
    var thielement = $(this);
    CustomQueryDisplayId = thielement.val();
    if (thielement.val() == '12') {
        var strtlocal = localStorage.getItem('BBUKPISiteCreateStartDateVw');
        if (strtlocal) {
            CreateStartDateVw = strtlocal;
        }
        else {
            CreateStartDateVw = today;
        }
        var endlocal = localStorage.getItem('BBUKPISiteCreateEndDateVw');
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
        localStorage.removeItem('BBUKPISiteCreateStartDateVw');
        localStorage.removeItem('BBUKPISiteCreateEndDateVw');
        $(document).find('#timeperiodcontainerForCreateDate').hide();
    }
})

$(document).on('click', '#btntimeperiodForCreateDate', function (e) {
    run = true;
    var daterangeval = $(document).find('#cmbcreateview').val();
    if (daterangeval == '') {
        return;
    }
    CustomQueryDisplayId = daterangeval;
    if (daterangeval != '12') {
        CreateStartDateVw = '';
        CreateEndDateVw = '';
        localStorage.removeItem('BBUKPISiteCreateStartDateVw');
        localStorage.removeItem('BBUKPISiteCreateEndDateVw');
    }
    else {
        localStorage.setItem('BBUKPISiteCreateStartDateVw', CreateStartDateVw);
        localStorage.setItem('BBUKPISiteCreateEndDateVw', CreateEndDateVw);
    }
    $(document).find('#SiteDateRangeModalForCreateDate').modal('hide');
    var text = $('#BBUKPISitesearchListul').find('li').eq(0).text();

    if (daterangeval != '12')
        text = text + " - " + $(document).find('#cmbcreateview option[value=' + daterangeval + ']').text();
    else
        text = text + " - " + $('#createdaterange').val();

    $('#sitesearchtitle').text(text);
    $("#BBUKPISitesearchListul li").removeClass("activeState");
    $("#BBUKPISitesearchListul li").eq(0).addClass('activeState');

    localStorage.setItem("bbukpisitestatus", daterangeval);
    if ($(document).find('#txtColumnSearch').val() !== '') {
        $('#advsearchfilteritems').find('span').html('');
        $('#advsearchfilteritems').find('span').removeClass('tagTo');
    }
    $(document).find('#txtColumnSearch').val('');

    CustomQueryDisplayId = daterangeval;
    //if (layoutType == 1) {
    //    cardviewstartvalue = 0;
    //    grdcardcurrentpage = 1;

    //    LayoutFilterinfoUpdate();
    //    ShowCardView();
    //}
    if (daterangeval.length !== 0) {
        ShowbtnLoaderclass("LoaderDrop");
        dtTable.page('first').draw('page');
    }
});

$(document).on('change', '#cmbsubmitview', function (e) {
    var thielement = $(this);
    CustomQueryDisplayId = thielement.val();
    if (thielement.val() == '13') {
        var strtlocal = localStorage.getItem('BBUKPISiteSubmitStartDateVw');
        if (strtlocal) {
            SubmitStartDateVw = strtlocal;
        }
        else {
            SubmitStartDateVw = today;
        }
        var endlocal = localStorage.getItem('BBUKPISiteSubmitEndDateVw');
        if (endlocal) {
            SubmitEndDateVw = endlocal;
        }
        else {
            SubmitEndDateVw = today;
        }
        $(document).find('#timeperiodcontainerForSubmitDate').show();
        $(document).find('#submitdaterange').daterangepicker({
            format: 'MM/DD/YYYY',
            startDate: SubmitStartDateVw,
            endDate: SubmitEndDateVw,
            "locale": {
                "applyLabel": getResourceValue("JsApply"),
                "cancelLabel": getResourceValue("CancelAlert")
            }
        }, function (start, end, label) {
            SubmitStartDateVw = start.format('MM/DD/YYYY');
            SubmitEndDateVw = end.format('MM/DD/YYYY');
        });
    }
    else {
        SubmitStartDateVw = '';
        SubmitEndDateVw = '';
        localStorage.removeItem('BBUKPISiteSubmitStartDateVw');
        localStorage.removeItem('BBUKPISiteSubmitEndDateVw');
        $(document).find('#timeperiodcontainerForSubmitDate').hide();
    }
})

$(document).on('click', '#btntimeperiodForSubmitDate', function (e) {
    run = true;
    var daterangeval = $(document).find('#cmbsubmitview').val();
    if (daterangeval == '') {
        return;
    }
    CustomQueryDisplayId = daterangeval;
    if (daterangeval != '13') {
        SubmitStartDateVw = '';
        SubmitEndDateVw = '';
        localStorage.removeItem('BBUKPISiteSubmitStartDateVw');
        localStorage.removeItem('BBUKPISiteSubmitEndDateVw');
    }
    else {
        localStorage.setItem('BBUKPISiteSubmitStartDateVw', SubmitStartDateVw);
        localStorage.setItem('BBUKPISiteSubmitEndDateVw', SubmitEndDateVw);
    }
    $(document).find('#SiteDateRangeModalForSubmitDate').modal('hide');
    var text = $('#BBUKPISitesearchListul').find('li').eq(2).text();

    if (daterangeval != '13')
        text = text + " - " + $(document).find('#cmbsubmitview option[value=' + daterangeval + ']').text();
    else
        text = text + " - " + $('#submitdaterange').val();

    $('#sitesearchtitle').text(text);
    $("#BBUKPISitesearchListul li").removeClass("activeState");
    $("#BBUKPISitesearchListul li").eq(2).addClass('activeState');
    localStorage.setItem("bbukpisitestatus", daterangeval);
    if ($(document).find('#txtColumnSearch').val() !== '') {
        $('#advsearchfilteritems').find('span').html('');
        $('#advsearchfilteritems').find('span').removeClass('tagTo');
    }
    $(document).find('#txtColumnSearch').val('');

    CustomQueryDisplayId = daterangeval;
    //if (layoutType == 1) {
    //    cardviewstartvalue = 0;
    //    grdcardcurrentpage = 1;

    //    LayoutFilterinfoUpdate();
    //    ShowCardView();
    //}
    if (daterangeval.length !== 0) {
        ShowbtnLoaderclass("LoaderDrop");
        dtTable.page('first').draw('page');
    }
});
$(document).on('click', "#SrchBttnNew", function () {
    $.ajax({
        url: '/Base/PopulateNewSearchList',
        type: 'GET',
        data: { tableName: 'BBUKPISite' },
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
        data: { tableName: 'BBUKPISite', searchText: txtSearchval, isClear: isClear },
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
        generateEquipmentDataTable();
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
//$(document).on('click', '#linkToSearch', function () {
//    window.location.href = "../Equipment/Index?page=Maintenance_Assets";
//});
//#endregion


$("#ddlYearWeeks").change(function () {

    run = true;
    var id = $('#ddlYearWeeks option:selected').toArray().map(item => item.text).join();
    var ids = $(this).val();
    if (id == "") {
        yearweekfilters = "";
    }
    else {
        yearweekfilters = id;
    }
    localStorage.setItem("yearweekvalue", ids);
    localStorage.setItem("yearweektext", id);
    dtTable.page('first').draw('page');

});