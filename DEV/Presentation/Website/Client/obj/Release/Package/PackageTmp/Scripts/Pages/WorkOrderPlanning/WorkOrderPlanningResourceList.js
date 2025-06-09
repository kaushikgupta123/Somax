//#region Global variables
var PersonnelListRL = '';
var gridnameRL = "ResourceList_Search";
var WoHoursEditArray = [];
var WoScheduleEditArray = [];
var dtTableRL;
var WoCheckedItem = [];
var woToupdate  = [];
var run = false;
var filterinfoarray = [];
var selectedcountRL = 0;
var selectCountRL = 0;
var RListorder = '0';
var RListorderDir = 'asc';
//#endregion
function LoadResourceListTab() {
    $.ajax({
        url: '/WorkOrderPlanning/ResourceList',
        type: 'POST',
        dataType: 'html',
        data: {
            LockPlan: LockPlan, PlanStatus: PlanStatus
        },
        beforeSend: function () {
            ShowLoader();
        },
        //contentType: 'html',
        success: function (data) {
            if (data) {
                $(document).find('#ResourceList').html(data);               
            }
        },
        complete: function () {
            $(document).find(".assignedBlockRL").css("display", "inline-block");
            $(document).find("#sidebarRLCollapse").css("display", "inline-block");
            $(document).find(".groupBlock").css("display", "inline-block");
            $(document).find("#SrchBttnNewRL").css("display", "inline-block");
            $(document).find("#btnWoExport").css("display", "inline-block");
            if(LockPlan=='True' && PlanStatus=='Locked')
            {
                $(document).find("#btnAvailableWorkRL").css("display", "inline-block");
               // $('#popupExport').css({ 'right': '172px !important;' });
            }
            else
            {
                $(document).find("#btnAvailableWorkRL").css("display", "none"); 
               // $('#popupExport').css({ 'right': '40px !important;'});
            }        
            selectCountRL = 0;
            selectedcountRL = 0;
            LoadAdvancedSearchComponents();
            PersonnelListRL = [];
            TotalcheckedRL = 0;
             WoCheckedItem = [];
             woToupdate = [];
            CloseLoader();
            $(document).find('.select2picker').select2({});
            GetUserAssignedDropdownRL();
            generateResourceListDataTable();
            $(document).find("#ResourceListid").css("display", "block");
        },
        error: function (err) {
            CloseLoader();
        }
    });
}
//#region Export Button Show Hide
function funcShowExportRLbtn() {
    if (LockPlan == 'True' && PlanStatus == 'Locked') {
        $(document).find(".AvailableClass").show();
    }
    else
    {
        $(document).find(".NotAvailableClass").show();
    }
    
    $(document).find("#mask").show();
}

//#endregion
//#region Dropdown toggle   
$(document).mouseup(function (e) {
    var container = $(document).find('#searchBttnNewDropRL');
    if (!container.is(e.target) && container.has(e.target).length === 0) {
        container.hide("slideToggle");
    }
});

//#endregion
function generateResourceListDataTable() {
    WoHoursEditArray = [];
    WoScheduleEditArray = [];
    if ($(document).find('#ResourceListSearchTable').hasClass('dataTable')) {
        dtTableRL.destroy();
    }
    dtTableRL = $("#ResourceListSearchTable").DataTable({       
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
                    data.order[0][0] = RListorder;
                    data.order[0][1] = RListorderDir;
                }
                var filterinfoarray = getfilterinfoarray($("#txtColumnRLSearch"), $('#advsearchsidebar'));
                $.ajax({
                    "url": "/Base/CreateUpdateState",
                    "data": {
                        GridName: gridnameRL,
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
                    GridName: gridnameRL
                },
                "async": false,
                "dataType": "json",
                "success": function (json) {
                    if (json.LayoutInfo) {
                        var LayoutInfo = JSON.parse(json.LayoutInfo);
                        RListorder = LayoutInfo.order[0][0];
                        RListorderDir = LayoutInfo.order[0][1];
                        callback(JSON.parse(json.LayoutInfo));
                        if (json.FilterInfo) {
                            setsearchui(JSON.parse(json.FilterInfo), $("#txtColumnRLSearch"), $(".filteritemcount"), $("#advsearchfilteritemsRL"));
                        }
                    }
                    else {
                        callback(json);
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
                title: 'Resource List',
                exportOptions: {
                    columns: ':visible',
                    gridtoexport: 'RList-search'
                }
            },
            {
                extend: 'print',
                title: 'Resource List',
                exportOptions: {
                    columns: ':visible',
                    gridtoexport: 'RList-search'
                }
            },

            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Resource List',
                extension: '.csv',
                exportOptions: {
                    columns: ':visible',
                    gridtoexport: 'RList-search'                  
                }
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                exportOptions: {
                    columns: ':visible',
                    gridtoexport: 'RList-search'
                },
                css: 'display:none',
                title: 'Resource List',
                orientation: 'landscape',
                pageSize: 'A3'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/WorkOrderPlanning/GetResourceListGridData",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.SearchText = LRTrim($(document).find('#txtColumnRLSearch').val());
                d.WorkOrderPlanId = $(document).find('#workorderPlanningModel_WorkOrderPlanId').val();
                d.ClientLookupId = LRTrim($("#EquipmentId").val());
                d.Name = LRTrim($("#Name").val());
                d.Description = LRTrim($("#Description").val());
                d.RequiredDate = LRTrim($("#RequiredDate").val());             
                d.Type = LRTrim($("#Type").val());
                d.PersonnelList = PersonnelListRL;
                d.Order = $("#GroupingLabor").val();            

            },
            "dataSrc": function (result) {
                let colOrder = dtTableRL.order();
                RListorderDir = colOrder[0][1];
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
        select: {
            style: 'os',
            selector: 'td:first-child'
        },
        "columns":
        [
            {
                "data": "WorkOrderId",
                orderable: false,
                "bSortable": false,
                className: 'select-checkbox dt-body-center',
                targets: 0,

                'render': function (data, type, full, meta) {
                    return '<input type="checkbox" name="id[]" data-eqid="' + data + '" class="chksearch"  value="'
                        + $('<div/>').text(data).html() + '">';
                }
            },
            {
                "data": "PersonnelName",
                "autoWidth": true,
                "bSearchable": true,
                "bSortable": false,
                "className": "text-left",
                "name": "1"

            },
            {
                "data": "WorkOrderClientLookupId",
                "autoWidth": true,
                "bSearchable": true,
                "bSortable": false,
                "className": "text-left",
                "name": "2"

            },
            {
                "data": "Description", "autoWidth": false, "bSearchable": true, "bSortable": false, "name": "3",
                "mRender": function (data, type, row) {
                    return "<div class='text-wrap width-300'>" + data + "</div>";
                }
            },
            { "data": "Type", "autoWidth": true, "bSearchable": true, "bSortable": false, "name": "4", },
            {
                "data": "ScheduledStartDate", "autoWidth": true, "bSearchable": true, "bSortable": false, "type": "date", "className": "text-left Personnel", "name": "5",
                'render': function (data, type, row) {
                    if (WoScheduleEditArray.indexOf(row.WorkOrderScheduleId) != -1) {
                        return "<div style='width:110px !important; position:relative;'><input type='text'  class='dtpicker Scheduleddtpicker dt-inline-text' readonly='readonly' autocomplete='off' value='" + data + "'><i class='fa fa-check-circle is-saved-check' style='float: right; position: absolute; top: 8px; right:-22px; color:green;display:block;' title='success'></i><i class='fa fa-times-circle is-saved-times' style='float: right; position: absolute; top: 8px; right:-22px; color:red;display:none;'></i><img src='../Content/Images/grid-cell-loader.gif' class='is-saved-loader' style='float:right;position:absolute;top: 8px; right:-22px;display:none;' /></div>";
                    } else {
                        return "<div style='width:110px !important; position:relative;'><input type='text'  class='dtpicker Scheduleddtpicker dt-inline-text' readonly='readonly' autocomplete='off' value='" + data + "'><i class='fa fa-check-circle is-saved-check' style='float: right; position: absolute; top: 8px; right:-22px; color:green;display:none;' title='success'></i><i class='fa fa-times-circle is-saved-times' style='float: right; position: absolute; top: 8px; right:-22px; color:red;display:none;'></i><img src='../Content/Images/grid-cell-loader.gif' class='is-saved-loader' style='float:right;position:absolute;top: 8px; right:-22px;display:none;' /></div>";
                    }
                }
            },
            {
                "data": "ScheduledHours", "autoWidth": true, "bSearchable": true, "bSortable": false,
                'render': function (data, type, row) {
                    if (WoHoursEditArray.indexOf(row.WorkOrderScheduleId) != -1) {
                        return "<div style='width:110px !important; position:relative;'><input type='text' style='width:90px !important;text-align:right;' class='duration  dt-inline-text decimalinputupto2places grd-hours' autocomplete='off' value='" + data + "' maskedFormat='6,2'><i class='fa fa-check-circle is-saved-check' style='float: right; position: absolute; top: 8px; right:-3px; color:green;display:block;' title='success'></i><i class='fa fa-times-circle is-saved-times' style='float: right; position: absolute; top: 8px; right:-3px; color:red;display:none;'></i><img src='../Content/Images/grid-cell-loader.gif' class='is-saved-loader' style='float:right;position:absolute;top: 8px; right:-3px;display:none;' /></div>";
                    } else {
                        return "<div style='width:110px !important; position:relative;'><input type='text' style='width:90px !important;text-align:right;' class='duration  dt-inline-text decimalinputupto2places grd-hours' autocomplete='off' value='" + data + "' maskedFormat='6,2'><i class='fa fa-check-circle is-saved-check' style='float: right; position: absolute; top: 8px; right:-3px; color:green;display:none;' title='success'></i><i class='fa fa-times-circle is-saved-times' style='float: right; position: absolute; top: 8px; right:-3px; color:red;display:none;'></i><img src='../Content/Images/grid-cell-loader.gif' class='is-saved-loader' style='float:right;position:absolute;top: 8px; right:-3px;display:none;' /></div>";
                    }
                }
            },
            {
                "data": "RequiredDate", "autoWidth": true, "bSearchable": true, "bSortable": false, "type": "date", "name": "7"
            },
            {
                "data": "EquipmentClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": false, "name": "8"
            },
            {
                "data": "ChargeTo_Name", "autoWidth": true, "bSearchable": true, "bSortable": false, "name": "9"
            }

        ],
        'rowCallback': function (row, data, dataIndex) {
            var found = WoCheckedItem.some(function (el) {
                return el.WorkOrderSchedId == data.WorkOrderScheduleId;
            });
            if (found) {
                $(row).find('input[type="checkbox"]').prop('checked', true);
            }

        },
        initComplete: function () {
            //Hide column based on the grouping
            $(document).find('.dtpicker').datepicker({
                minDate: 0,
                "dateFormat": "mm/dd/yy",
                autoclose: true,
                changeMonth: true,
                changeYear: true
            });
            $(document).find('#RequiredDate').datepicker({
                //minDate: 0,
                "dateFormat": "mm/dd/yy",
                autoclose: true,
                changeMonth: true,
                changeYear: true
            });
            var GroupingId = $("#GroupingLabor").val();
            SetPageLengthMenu();
            $("#lsGridAction :input").removeAttr("disabled");
            DisableExportButton($("#ResourceListSearchTable"), $(document).find('.import-export'));
        },
        "drawCallback": function (settings, data) {

            var api = this.api();
            var rows = api.rows().nodes();
            var getData = api.rows({ page: 'current' }).data();
            var aData = new Array();
            var info = dtTableRL.page.info();
            var GroupingValue = $("#GroupingLabor").val();
            var thisgroupId = 0;
            if (GroupingValue == "0") {
                thisgroupId = 1;
                api.column(1).visible(true);
                api.column(5).visible(true);
            }
            else {
                thisgroupId = 5;
                api.column(1).visible(true);
                api.column(5).visible(true);
            }

            var thisgroup = $(this).find('td').eq(thisgroupId).text();
            var last = thisgroup;

            var start = info.start;
            var end = info.end;

            api.column(thisgroupId).data().each(function (group, i) {
                if ((last.toUpperCase() == group.toUpperCase() && i == 0)) {
                    $(rows).eq(i).before(
                        '<tr class="group"><td colspan=' + 10 + '>' + group + '</td></tr>'
                    );
                    last = group;
                }
                if (GroupingValue == "1") {
                    if ((last.toUpperCase() != group.toUpperCase())) {
                        $(rows).eq(i).before(
                            '<tr class="group"><td colspan=' + 10 + '>' + group + '</td></tr>'
                        );
                        last = group;
                    }
                }
                last = group;
                var TotalCount = getData[0].TotalCount;
                var tablelength = api.column(1).data().length;
                var PerIDNextValue = getData[i].PerIDNextValue;
                var PersonnelId = getData[i].PersonnelId;
                var PerNextValue = getData[i].PerNextValue;
                var pageLength = info.length;

                if (GroupingValue == "0") { //Print only at the time of assigned
                    if (last.toUpperCase() == group.toUpperCase() /*&& i != 0*/ && PerIDNextValue != PersonnelId) {
                        if (PerNextValue != "" && (pageLength - 1) != i)
                            $(rows).eq(i).after(
                                '<tr class="group"><td colspan=' + 10 + '>' + PerNextValue + '</td></tr>'
                            );
                    }
                }

                //Print Grand Total Hour at the end for both grouping

                if (end == TotalCount && tablelength == (i + 1)) {
                    var rowhtml_grabdtotal = rowCreateGrandTotal();
                    var sum_GrandTotal = getData[i].GrandTotalHour;
                    var thiscolumn_GrandTotal = "ScheduledHours";
                    rowhtml_grabdtotal = rowhtml_grabdtotal.replace("ScheduledHours", GetFormattednumber(thiscolumn_GrandTotal[0], sum_GrandTotal));
                    $(rows).eq(i).after(rowhtml_grabdtotal);
                }

                if (GroupingValue == "0") { //Print Total Hours when grouping by Assigned
                    if ((last.toUpperCase() !== PerNextValue.toUpperCase() || PerNextValue == "") || PerIDNextValue != PersonnelId) {
                        var rowhtml = rowCreate('');
                        var sum = getData[i].SumPersonnelHour;
                        var thiscolumn = "ScheduledHours";
                        rowhtml = rowhtml.replace("ScheduledHours", GetFormattednumber(thiscolumn[0], sum));
                        $(rows).eq(i).after(rowhtml);
                    }
                }
                else { ////Print Total Hours when grouping by Schedule date
                    var SDNextValue = getData[i].SDNextValue;
                    if (last !== SDNextValue || SDNextValue == "") {
                        var rowhtml_date = rowCreate('');
                        var sum_date = getData[i].SumScheduledateHour;
                        var thiscolumn_date = "ScheduledHours";
                        rowhtml_date = rowhtml_date.replace("ScheduledHours", GetFormattednumber(thiscolumn_date[0], sum_date));
                        $(rows).eq(i).after(rowhtml_date);
                    }
                }

            });
            // SetPageLengthMenu();
            CloseLoader();

            if (GroupingValue == "0") {
                api.column(1).visible(false);
                api.column(5).visible(true);
            }
            else {
                api.column(5).visible(false);
                api.column(1).visible(true);
            }
            $('#ResourceListSearchTable').dataTable({ "bSort": false });
            $(document).find('.dtpicker').datepicker({
                minDate: 0,
                changeMonth: true,
                changeYear: true,
                "dateFormat": "mm/dd/yy",
                autoclose: true
            });
        }

    });
}

function rowCreate(GrandTotal) {

    var changedColumns = dtTableRL.settings()[0].aoColumns;
    var rowstring = '<tr style="color:#575962 !important;">';
    for (var i = 0; i < changedColumns.length - 1; i++) {
        if (changedColumns[i].bVisible) {
            if (changedColumns[i + 1].data == "ScheduledHours")
                if (GrandTotal != "") {
                    rowstring = rowstring + '<td align = "left" style="font-weight:500;padding-left:1px">' + 'Grand Total : ' + changedColumns[i + 1].data + '</td>';
                }
                else {
                    rowstring = rowstring + '<td style="text-align:right !important;font-weight: 400 !important;color: #000 !important;position: relative;height: 18px;"><div style="position:absolute;width:88px;left:0;top:0;min-height:34px;line-height:34px;">' + changedColumns[i + 1].data + '</div></td>';
                }

            else
                rowstring = rowstring + "<td></td>";
        }
    }
    rowstring = rowstring + '</tr>';

    return rowstring;
};
function rowCreateGrandTotal() {
    var changedColumns = dtTableRL.settings()[0].aoColumns;
    var rowstring = '<tr style="color:#575962 !important;">';
    for (var i = 0; i < changedColumns.length - 2; i++) {
        if (changedColumns[i + 2].bVisible) {
            if (changedColumns[i + 2].data == "ScheduledHours") {
                rowstring = rowstring + '<td align="right" style="font-weight:500;padding-left:1px">Grand Total : </td><td style="text-align:right !important;font-weight: 400 !important;color: #000 !important;position: relative;height: 18px;"><div style="position:absolute;width:88px;left:0;top:0;min-height:34px;line-height:34px;">' + changedColumns[i + 2].data + '</div></td>';
            }
            else
                rowstring = rowstring + "<td></td>";
        }
    }
    rowstring = rowstring + '</tr>';
    return rowstring;
};
function GetFormattednumber(column, numberToFormat) {
    var result = numberToFormat;
    var format = column.NumericFormat;
    if (format) {
        format = format.toUpperCase();
        if (format == plain) {
            if (column.NumofDecPlaces > 0) {
                return getFlooredFixed(numberToFormat, column.NumofDecPlaces);
            }
            return result;
        }
        else if (format == number && column.NumofDecPlaces) {
            return parseFloat(numberToFormat).toLocaleString(undefined, { minimumFractionDigits: column.NumofDecPlaces, maximumFractionDigits: column.NumofDecPlaces });
        }
        else if (format == currency) {
            var decimelplaces = 0;
            if (column.NumofDecPlaces != 0) {
                decimelplaces = column.NumofDecPlaces;
            }
            if (column.CurrencyCode) {
                return parseFloat(numberToFormat).toLocaleString(column.SiteLocalization, { style: 'currency', currency: column.CurrencyCode, minimumFractionDigits: decimelplaces, maximumFractionDigits: decimelplaces });
            }

        }
        else if (format == percentage) {
            if (column.NumofDecPlaces) {
                return parseFloat(numberToFormat).toLocaleString(undefined, { style: 'percent', minimumFractionDigits: column.NumofDecPlaces, maximumFractionDigits: column.NumofDecPlaces });
            }

        }
    }
    return result;
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
function filterinfo(id, value) {
    this.key = id;
    this.value = value;
}
$(document).on('click', '#ResourceListSearchTable_paginate .paginate_button', function () {
    run = true;
});
$(document).on('change', '#ResourceListSearchTable_length .searchdt-menu', function () {
    run = true;
});
$(function () {    
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {

            if (options.gridtoexport == 'RList-search') {
                // RListExport();
                var exportRListHeader = $("#ResourceListSearchTable thead tr th").not(":eq(0)").find('div').map(function () { return this.innerHTML; }).get();
                var exportRListBody = GetRListDataToExport();
                return { body: exportRListBody, header: exportRListHeader };

            }
            else {
                //ExportMainGrid();
                var exportHeader = $("#WoPlanningSearch thead tr th").not(":eq(0)").find('div').map(function () { return this.innerHTML; }).get();
                var exportBody = GetDataToExport();
                return { body: exportBody, header: exportHeader };
            }

        }
    });

});

function GetRListDataToExport() {
    var PersonnelList = $(document).find('#ddlUserRL').val();
    var jsonResult = $.ajax({
        url: "/WorkOrderPlanning/GetResourceListPrintData",
        data: {
            WorkOrderPlanId: $(document).find('#workorderPlanningModel_WorkOrderPlanId').val(),
            ClientLookupId: LRTrim($("#EquipmentId").val()),
            Name: LRTrim($("#Name").val()),
            Description: LRTrim($("#Description").val()),
            RequiredDate: LRTrim($("#RequiredDate").val()),         
            Type: LRTrim($("#Type").val()),
            "PersonnelList": PersonnelListRL,
            SearchText: LRTrim($(document).find('#txtColumnRLSearch').val()),
            colname: RListorder,
            coldir: RListorderDir
        },
        success: function (result) { },
        async: false
    });
    var exportRListBody = JSON.parse(jsonResult.responseText).data;

    $.each(exportRListBody, function (index, item) {
        if (item.PersonnelName != null) {
            item.PersonnelName = item.PersonnelName;
        }
        else {
            item.PersonnelName = "";
        }
        if (item.WorkOrderClientLookupId != null) {
            item.WorkOrderClientLookupId = item.WorkOrderClientLookupId;
        }
        else {
            item.WorkOrderClientLookupId = "";
        }
        if (item.Description != null) {
            item.Description = item.Description;
        }
        else {
            item.Description = "";
        }

        if (item.Type != null) {
            item.Type = item.Type;
        }
        else {
            item.Type = "";
        }
        if (item.ScheduledStartDate != null) {
            item.ScheduledStartDate = item.ScheduledStartDate;
        }
        else {
            item.ScheduledStartDate = "";
        }
        if (item.ScheduledHours != null) {
            item.ScheduledHours = item.ScheduledHours;
        }
        else {
            item.ScheduledHours = "";
        }
        if (item.RequiredDate != null) {
            item.RequiredDate = item.RequiredDate;
        }
        else {
            item.RequiredDate = "";
        }

        if (item.EquipmentClientLookupId != null) {
            item.EquipmentClientLookupId = item.EquipmentClientLookupId;
        }
        else {
            item.EquipmentClientLookupId = "";
        }

        if (item.ChargeTo_Name != null) {
            item.ChargeTo_Name = item.ChargeTo_Name;
        }
        else {
            item.ChargeTo_Name = "";
        }

    })
    return exportRListBody.map(function (el) {
        return Object.keys(el).map(function (key) { return el[key] });
    });
}
$(document).on('click', '#liPrint,#liPdf,#liExcel,#liCsv', function () {
    var thisid = $(this).attr('id');
    var TableHaederProp = [];
    var PersonnelListRL = $(document).find('#ddlUserRL').val();
    function table(property, title) {
        this.property = property;
        this.title = title;
    }
    $("#ResourceListSearchTable thead tr th").map(function (key) {
        var thisdiv = $(this).find('div');
        var tablearr = new table(this.getAttribute('data-th-prop'), thisdiv.html());
        TableHaederProp.push(tablearr);
    });
    var params = {
        tableHaederProps: TableHaederProp,
        colname: $("#GroupingLabor").val(),
        coldir: RListorderDir,
        WorkOrderPlanId: $(document).find('#workorderPlanningModel_WorkOrderPlanId').val(),
        ClientLookupId: LRTrim($("#EquipmentId").val()),
        Name: LRTrim($("#Name").val()),
        Description: LRTrim($("#Description").val()),
        RequiredDate: LRTrim($("#RequiredDate").val()),     
        Type: LRTrim($("#Type").val()),
        "PersonnelList": PersonnelListRL,
        SearchText: LRTrim($(document).find('#txtColumnRLSearch').val()),
    };
    RSPrintParams = JSON.stringify({ 'RSPrintParams': params });
    $.ajax({
        "url": "/WorkOrderPlanning/SetPrintDataRS",
        "data": RSPrintParams,
        contentType: 'application/json; charset=utf-8',
        "dataType": "json",
        "type": "POST",
        "success": function () {
            if (thisid == 'liPdf') {
                window.open('/WorkOrderPlanning/ExportASPDFRS?d=d', '_self');
            }
            else if (thisid == 'liPrint') {
                window.open('/WorkOrderPlanning/ExportASPDFRS', '_blank');
            }
            else if (thisid == 'liExcel') {
                window.open('/WorkOrderPlanning/ExportASPDFRS?d=excel', '_self');
            }
            else if (thisid == 'liCsv') {
                window.open('/WorkOrderPlanning/ExportASPDFRS?d=csv', '_self');
            }
            return;
        }
    });
    $('#mask').trigger('click');
});
function setsearchui(data, txtsearchelement, advcountercontainer, searchstringcontainer) {
    var searchitemhtml = '';
    $.each(data, function (index, item) {
        if (item.key == 'searchstring' && item.value) {
            var txtSearchval = item.value;
            if (item.value) {
                txtsearchelement.val(txtSearchval);
                searchitemhtml = "";
                searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + txtsearchelement.attr('id') + '">Search: ' + txtSearchval + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossRL" aria-hidden="true"></a></span>';
            }
            return false;
        }
        else {
            if ($('#' + item.key).parent('div').is(":visible")) {
                $('#' + item.key).val(item.value);
                if (item.value) {
                    selectCountRL++;
                    searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + item.key + '">' + $("label[for='" + item.key + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossRL" aria-hidden="true"></a></span>';
                }
            }
            else if (item.key == 'advrequiredDatedaterange' && item.value !== '') {
                $('#' + item.key).val(item.value);
                if (item.value) {
                    searchitemhtml = searchitemhtml + '<span style="display:none;" class="label label-primary tagTo" id="_' + item.key + '">' + $("label[for='" + item.key + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossRL" aria-hidden="true"></a></span>';
                }
            }
            if (item.key == 'RequiredDate') {
                $("#RequiredDate").trigger('change.select2');
            }
            advcountercontainer.text(selectCountRL);
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    searchstringcontainer.html(searchitemhtml);
}
$(document).on('change', '#ddlUserRL', function (event) {
    PersonnelListRL = $(this).val();
    ChangeListRSHeaderInfo();
    event.stopPropagation();
});

function ChangeListRSHeaderInfo() {
    dtTableRL.page('first').draw('page');
}
//#region Multiple Select Drowpdown  With CheckBox 
var Select2MultiCheckBoxObjRL = [];
var id_selectElementRL = 'ddlUserRL';
var staticWordInIDRL = 'stateRL_';
var TotalcheckedRL = 0;
var AllselectedRL = false;
function GetUserAssignedDropdownRL() {
    $.map($('#' + id_selectElementRL + ' option'), function (option) {
        AddItemInSelect2MultiCheckBoxObjRL(option.value, false);
    });
    function formatResultRL(state) {
        if (Select2MultiCheckBoxObjRL.length > 0) {
            var stateIdRL = staticWordInIDRL + state.id;
            let indexRL = Select2MultiCheckBoxObjRL.findIndex(x => x.id == state.id);
            if (indexRL > -1) {
                var checkboxRL = "";
                if (indexRL == 0) {
                    checkboxRL = $('<div class="checkbox"><input class="select2Checkbox checkboxAssignedUserRL " id="stateRL_0" type="checkbox" ' + (Select2MultiCheckBoxObjRL[indexRL]["IsChecked"] ? 'checked' : '') +
                        '><label for="checkboxstateRL_0 "> ' + state.text + '</label></div>', { id: 'stateRL_0' });
                }
                else {
                    checkboxRL = $('<div class="checkbox"><input class="select2Checkbox checkboxAssignedUserRL" id="' + stateIdRL + '" type="checkbox" ' + (Select2MultiCheckBoxObjRL[indexRL]["IsChecked"] ? 'checked' : '') +
                        '><label for="checkbox' + stateIdRL + '"> ' + state.text + '</label></div>', { id: stateIdRL });
                }

                return checkboxRL;
            }
        }
    }

    let optionSelect2RL = {
        templateResult: formatResultRL,
        closeOnSelect: false,
        allowClear: true
        /* width: '100%'*/
    };

    let $select2 = $(document).find("#" + id_selectElementRL).select2(optionSelect2RL);
    $("#" + id_selectElementRL).find("option[value='']").attr("disabled", 'disabled');
    getCalMultiplecheckRL();
    $select2.on('select2:close', function () {

    });
    $select2.on("select2:select", function (event) {
        $("#" + staticWordInIDRL + event.params.data.id).prop("checked", true);
        AddItemInSelect2MultiCheckBoxObjRL(event.params.data.id, true);
        //If all options are slected then selectAll option would be also selected.

        if (Select2MultiCheckBoxObjRL.filter(x => x.IsChecked === false).length === 1) {
            AddItemInSelect2MultiCheckBoxObjRL(0, true);
            $("#" + staticWordInIDRL + "0").prop("checked", true);
        }
        TotalcheckedRL = TotalcheckedRL + 1;
        getCalMultiplecheckRL();
    });

    $select2.on("select2:unselect", function (event) {
        $("#" + staticWordInIDRL + "0").prop("checked", false);
        AddItemInSelect2MultiCheckBoxObjRL(0, false);
        $("#" + staticWordInIDRL + event.params.data.id).prop("checked", false);
        AddItemInSelect2MultiCheckBoxObjRL(event.params.data.id, false);
        TotalcheckedRL = TotalcheckedRL - 1;
        getCalMultiplecheckRL();
    });

    $(document).on("click", "#" + staticWordInIDRL + "0", function () {

        var b = $("#" + staticWordInIDRL + "0").is(':checked');
        if (TotalcheckedRL == Select2MultiCheckBoxObjRL.length - 1 && b == false && $('.checkboxAssignedUserRL:checked').length<1) {
            IsCheckedAllOptionRL(true);
            $("#" + staticWordInIDRL + "0").prop("checked", true);
            TotalcheckedRL = Select2MultiCheckBoxObjRL.length - 1;
           
        } else {
            IsCheckedAllOptionRL(b);
            if (b == true) {
                TotalcheckedRL = $('.checkboxAssignedUserRL:checked').length - 1;
            } else {
                TotalcheckedRL = $('.checkboxAssignedUserRL:checked').length;
            }
        }
       
        getCalMultiplecheckRL();
        $("#" + id_selectElementRL).select2("close");
    });
    $(document).on("click", ".checkboxAssignedUserRL", function (event) {
        let selectorRL = "#" + this.id;
        let isCheckedRL = false;
        if (this.id == "stateRL_0") {
            isCheckedRL = Select2MultiCheckBoxObjRL[Select2MultiCheckBoxObjRL.findIndex(x => x.id == "")]['IsChecked'] ? true : false;
        } else {
            isCheckedRL = Select2MultiCheckBoxObjRL[Select2MultiCheckBoxObjRL.findIndex(x => x.id == this.id.replaceAll(staticWordInIDRL, ''))]['IsChecked'] ? true : false;
        }
        $(selectorRL).prop("checked", isCheckedRL);
        getCalMultiplecheckRL();
    });
    $(document).on("click", ".assignedBlockRL span  .select2-selection__rendered", function (event) {
        $(".assignedBlockRL span ul li .select2-search__field").val("");
    });
    $(document).on("click", ".assignedBlockRL span  .select2-selection__rendered .select2-selection__clear", function (event) {
        IsCheckedAllOptionRL(false);
        TotalcheckedRL = $('.checkboxAssignedUserRL:checked').length;
        getCalMultiplecheckRL();
        $("#" + id_selectElementRL).select2("close");

    });
    $(document).on('focusout', '.assignedBlockRL span ul li .select2-search__field', function (e) {
        getCalMultiplecheckRL();
    });
    $(document).on('keypress', '.assignedBlockRL span ul li .select2-search__field', function (e) {
        var tcountRL = TotalcheckedRL;
        var ftcountRL = "";
        if (tcountRL == 0) {
            ftcountRL = "All Personnel";
        }
        else {
            ftcountRL = tcountRL.toString() + " People";
        }
        var text = $(".assignedBlockRL span ul li .select2-search__field").val();
        $(".assignedBlockRL span ul li .select2-search__field").val(text.replace(ftcount, ''));
        $('.assignedBlockRL span ul li .select2-search__field[placeholder=""]').attr('style', 'width:100%');
    });


}
function AddItemInSelect2MultiCheckBoxObjRL(id, IsChecked) {
    if (Select2MultiCheckBoxObjRL.length > 0) {
        let indexRL = Select2MultiCheckBoxObjRL.findIndex(x => x.id == id);
        if (indexRL > -1) {
            Select2MultiCheckBoxObjRL[indexRL]["IsChecked"] = IsChecked;
        }
        else {
            Select2MultiCheckBoxObjRL.push({ "id": id, "IsChecked": IsChecked });
        }
    }
    else {
        Select2MultiCheckBoxObjRL.push({ "id": id, "IsChecked": IsChecked });
    }
}
function IsCheckedAllOptionRL(trueOrFalse) {
    $.map($('#' + id_selectElementRL + ' option'), function (option) {
        AddItemInSelect2MultiCheckBoxObjRL(option.value, trueOrFalse);
    });
    $('#' + id_selectElementRL + " > option").not(':first').prop("selected", trueOrFalse);
    //This will select all options and adds in Select2
    $("#" + id_selectElementRL).trigger("change");//This will effect the changes
    /* $(".select2-results__option").not(':first').attr("aria-selected", trueOrFalse);*/
    //This will make grey color of selected options

    $("input[id^='" + staticWordInIDRL + "']").prop("checked", trueOrFalse);
}
function getCalMultiplecheckRL() {
    $('.assignedBlockRL span ul li .select2-search__field[placeholder=""]').attr('style', 'width:100%');
    $(".assignedBlockRL span  .select2-selection__rendered").find('.select2-selection__choice').remove();
    AllselectedRL = $("#" + staticWordInIDRL + "0").prop('checked') ? true : false;
    var tcountRL = TotalcheckedRL;
    var ftcountRL = "";
    if (tcountRL == 0) {
        ftcountRL = "All Personnel";
    }
    else {
        ftcountRL = tcountRL.toString() + " People";
    }
    $(".assignedBlockRL span ul li .select2-search__field").val(ftcountRL);

}
//#endregion Multiple Select Drowpdown  With CheckBox 

//#region Multi Select
$(document).on('change', '.chksearch', function () {
    var data = dtTableRL.row($(this).parents('tr')).data();

    if (!this.checked) {
        selectedcountRL--;
        woToupdate = woToupdate.filter(function (el) {
            return el.WorkOrderId !== data.WorkOrderId;
        });
        WoCheckedItem = WoCheckedItem.filter(function (wo) {
            return wo.WorkOrderSchedId !== data.WorkOrderScheduleId;
        });
    }
    else {
        var item = new SelectedItemData(data.WorkOrderId, data.WorkOrderClientLookupId, data.WoStatus, data.ScheduledHours, data.WorkOrderScheduleId);
        var found = woToupdate.some(function (el) {
            return el.WorkOrderId === data.WorkOrderId;
        });
        WoCheckedItem.push(item);
        if (!found) { woToupdate.push(item); }
        selectedcountRL = selectedcountRL + woToupdate.length;
    }

    if ((woToupdate.length > 0 || WoCheckedItem.length > 0) && (LockPlan == 'True' && PlanStatus == 'Locked')) {
        $(".actionBar").hide();
        $(".updateArea").fadeIn();
    }
    else {
        $(".updateArea").hide();
        $(".actionBar").fadeIn();
    }
    $('.itemcount').text(WoCheckedItem.length);
});

function SelectedItemData(WorkOrderId, ClientLookupId, Status,ScheduledHours, WorkOrderSchedId) {
    this.WorkOrderId = WorkOrderId;  
    this.ClientLookupId = ClientLookupId;
    this.Status = Status;
    this.ScheduledHours = ScheduledHours;
    this.WorkOrderSchedId = WorkOrderSchedId;
};
//#region Reschedule
var SelectedScheduledHoursToSchedule = [];
var SelectedLookupIdToSchedule = [];
var SelectedStatusSchedule = [];
var SelectedWoIdToSchedule = [];
$(document).on('click', '#btnReschedule', function () {
    $(document).find('#Schedulestartdate').val("").trigger('change');
    $('#ddlSchUser').val(null).trigger("change.select2");
    var found = false;

    for (var i = 0; i < woToupdate.length; i++) {
        SelectedWoIdToSchedule.push(woToupdate[i].WorkOrderId);
        SelectedLookupIdToSchedule.push(woToupdate[i].ClientLookupId);
        SelectedStatusSchedule.push(woToupdate[i].Status);
        SelectedScheduledHoursToSchedule.push(woToupdate[i].ScheduledHours);
    }
    ShowbtnLoader("schedulesortmenu");
    HidebtnLoader("schedulesortmenu");
    $(document).find('#Schedulestartdate').val("").trigger('change');
    $('#ddlSchUser').val("").trigger("change.select2");
    $(document).find('#woRescheduleModel_WorkOrderIds').val(SelectedWoIdToSchedule);
    $(document).find('#woRescheduleModel_ClientLookupIds').val(SelectedLookupIdToSchedule);
    $(document).find('#woRescheduleModel_Status').val(SelectedStatusSchedule);
    $(document).find('#woRescheduleModel_ScheduledDurations').val(SelectedScheduledHoursToSchedule);
    $(document).find('#WorkOrderPlanStartDate').val(PlanStartDate);
    $(document).find('#WorkOrderPlanEndDate').val(PlanEndDate);
    $(document).find('.select2picker').select2({});
    SetControlsRL();
    addControlsRL();
    $(document).find('#ScheduleModal').modal({ backdrop: 'static', keyboard: false, show: true });

    $(document).find('form').find("#Schedulestartdate").removeClass("input-validation-error");


});

$(document).on('click', '.btncancelmod', function () {

    var areaddescribedby = $(document).find("#ddlSchUser").attr('aria-describedby');
    if (typeof areaddescribedby !== 'undefined') {
        $('#' + areaddescribedby).hide();
    }
    $(document).find('form').find("#ddlSchUser").removeClass("input-validation-error");
    $(document).find('form').find("#Schedulestartdate").removeClass("input-validation-error");

});
function ReScheduleAddOnSuccess(data) {
    $('#ScheduleModal').modal('hide');
    if (data.data === "success") {
        SuccessAlertSetting.text = getResourceValue("RescheduleUpdateSuccessAlert");
        swal(SuccessAlertSetting, function () {
            CloseLoader();
            $(".updateArea").hide();
            $(".actionBar").fadeIn();
            pageno = dtTableRL.page.info().page;
            dtTableRL.page(pageno).draw('page');
            $(document).find('#Schedulestartdate').val("").trigger('change');
            $('#ddlSchUser').val("").trigger("change.select2");

        });
    }
    else {
        CloseLoader();
        GenericSweetAlertMethod(data.data);

    }
    $(document).find('.chksearch').prop('checked', false);
    $(document).find('.itemcount').text(0);
    woToupdate = [];
    WoCheckedItem = [];
    SelectedLookupIdToSchedule = [];
    SelectedWoIdToSchedule = [];
    $(document).find('#Schedulestartdate').val("").trigger('change');
    $('#ddlSchUser').val(null).trigger("change.select2");
    $(document).find('#ScheduleModal').modal('hide');
}
//#endregion
//#region Remove Schedule
$(document).on('click', '#btnRemove', function () {
    if (WoCheckedItem.length < 1) {
        ShowGridItemSelectionAlert();
        return false;
    }
    else {
        var found = false;
        for (var i = 0; i < WoCheckedItem.length; i++) {
            if (WoCheckedItem[i].Status === 'Scheduled') {
                found = true;
                break;
            }

        }
        if (found === false) {
            var errorMessage = getResourceValue("WorkOrderRemovescheduleScheduledAlert");
            ShowErrorAlert(errorMessage);
            return false;
        }
        else {
            for (var i = 0; i < WoCheckedItem.length; i++) {
                SelectedWoIdToSchedule.push(WoCheckedItem[i].WorkOrderId);
                SelectedLookupIdToSchedule.push(WoCheckedItem[i].WorkOrderId);
            }
            ShowbtnLoader("schedulesortmenu");
            HidebtnLoader("schedulesortmenu");
            $(document).find('#woRescheduleModel_WorkOrderIds').val(SelectedWoIdToSchedule);
            $(document).find('#woRescheduleModel_ClientLookupIds').val(SelectedLookupIdToSchedule);


            swal({
                title: getResourceValue("spnAreyousure"),
                text: getResourceValue("ConfirmRemoveScheduleAlert"),
                type: "warning",
                showCancelButton: true,
                closeOnConfirm: false,
                confirmButtonClass: "btn-sm btn-primary",
                cancelButtonClass: "btn-sm",
                confirmButtonText: getResourceValue("CancelAlertYes"),
                cancelButtonText: getResourceValue("CancelAlertNo")
            }, function () {
                ShowLoader();
                var jsonResult = {
                    "list": WoCheckedItem
                };
                $.ajax({
                    url: '/WorkOrderPlanning/RemoveScheduleList',
                    type: "POST",
                    datatype: "json",
                    data: JSON.stringify(jsonResult),
                    contentType: 'application/json; charset=utf-8',
                    beforeSend: function () {
                        ShowLoader();
                    },
                    success: function (data) {
                        if (data.data == "success") {
                            SuccessAlertSetting.text = getResourceValue("spnWorkorderSuccessfullyRemovedScheduled");
                            swal(SuccessAlertSetting, function () {
                                pageno = dtTableRL.page.info().page;
                                dtTableRL.page(pageno).draw('page');
                            });
                        }
                        else {
                            GenericSweetAlertMethod(data.data);

                        }
                        $(".updateArea").hide();
                        $(".actionBar").fadeIn();
                        $(document).find('.chksearch').prop('checked', false);
                        $(document).find('.itemcount').text(0);
                        woToupdate = [];
                        WoCheckedItem = [];
                        SelectedLookupIdToSchedule = [];
                        SelectedWoIdToSchedule = [];

                    },
                    complete: function () {
                        CloseLoader();
                    },
                    error: function () {
                        CloseLoader();
                    }
                });
            });
        }
    }
});
//#endregion
//#region Advance Search
function LoadAdvancedSearchComponents() {
    $("#sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $('#dismiss, .overlay').on('click', function () {
        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();

    });
    $('#sidebarRLCollapse').on('click', function () {
        $('#sidebar').addClass('active');
        $('.overlay').fadeIn();
        $('.collapse.in').toggleClass('in');
        $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    });
    $("#btnRLDataAdvSrch").on('click', function (e) {
        run = true;
        $(document).find('#txtColumnRLSearch').val('');
        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();
        RLAdvSearch();
        dtTableRL.page('first').draw('page');
    });
    function RLAdvSearch() {
        $('#txtColumnRLSearch').val('');
        var searchitemhtml = "";
        selectCountRL = 0;
        $('#advsearchsidebar').find('.adv-item').each(function (index, item) {
            if ($(this).hasClass('dtpicker')) {
                $(this).val(ValidateDate($(this).val()));
            }
            if ($(this).attr('id') != 'advrequiredDatedaterange') {
                if ($(this).val()) {
                    selectCountRL++;
                    searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossRL" aria-hidden="true"></a></span>';
                }
            }
            else {
                if ($(this).val()) {
                    searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossRL" aria-hidden="true"></a></span>';
                }
            }

        });
        searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
        $("#advsearchfilteritemsRL").html(searchitemhtml);       
        $(".filteritemcount").text(selectCountRL);
    }


    $(document).on('click', '.btnCrossRL', function () {
        run = true;
        var btnCrossed = $(this).parent().attr('id');
        var searchtxtId = btnCrossed.split('_')[1];
        $('#' + searchtxtId).val('').trigger('change');
        $(this).parent().remove();
        selectCountRL--;
        RLAdvSearch();
        dtTableRL.page('first').draw('page');
    });
}

//#endregion
//#region New Search button

$(document).on('click', "#SrchBttnNewRL", function () {
    $.ajax({
        url: '/Base/PopulateNewSearchList',
        type: 'GET',
        data: { tableName: 'ResourceList' },
        beforeSend: function () {
            ShowbtnLoader("SrchBttnNewRL");
        },
        success: function (data) {
            var i; var str = '';
            for (i = 0; i < data.searchOptionList.length; i++) {
                str += '<li><a href="javascript:void(0)" id= "mem_' + i + '"' + '><i class="fa fa-search" style="font-size: 1rem;position: relative;top: -1px;left: 0px;"></i> &nbsp;' + data.searchOptionList[i] + '</a></li>';
            }
            UlSearchListRL.innerHTML = str;
            $(document).find('#searchBttnNewDropRL').show("slideToggle");
        },
        complete: function () {
            HidebtnLoader("SrchBttnNewRL");
        },
        error: function () {
            HidebtnLoader("SrchBttnNewRL");
        }
    });
});
function GenerateSearchList(txtSearchval, isClear) {
    run = true;
    $.ajax({
        url: '/Base/ModifyNewSearchList',
        type: 'POST',
        data: { tableName: 'ResourceList', searchText: txtSearchval, isClear: isClear },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            var i; var str = '';
            for (i = 0; i < data.searchOptionList.length; i++) {
                str += '<li><a href="javascript:void(0)"><i class="fa fa-search" style="font-size: 1rem;position: relative;top: -1px;left: 0px;"></i> &nbsp;' + data.searchOptionList[i] + '</a></li>';
            }
            UlSearchListRL.innerHTML = str;
        }
        ,
        complete: function () {
            if (isClear == false) {
                dtTableRL.page('first').draw('page');
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
$(document).on('keyup', '#txtColumnRLSearch', function (event) {
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if (keycode == 13) {
        TextSearchRL();
    }
    else {
        event.preventDefault();
    }
});
$(document).on('click', '.txtSearchClickRL', function () {
    TextSearchRL();
});
function TextSearchRL() {
    clearAdvanceSearchRL();
    var txtSearchval = LRTrim($(document).find('#txtColumnRLSearch').val());
    if (txtSearchval) {
        GenerateSearchList(txtSearchval, false);
        var searchitemhtml = "";
        searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $('#txtColumnRLSearch').attr('id') + '">Search: ' + txtSearchval + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossRL" aria-hidden="true"></a></span>';
        searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
        $("#advsearchfilteritemsRL").html(searchitemhtml);
    }
    else {
        run = true;
        generateResourceListDataTable();
    }
    var container = $(document).find('#searchBttnNewDropRL');
    container.hide("slideToggle");
}
$(document).on('click', '#UlSearchListRL li', function () {
    var v = LRTrim($(this).text());
    $(document).find('#txtColumnRLSearch').val(v);
    TextSearchRL();
});
$(document).on('click', '#cancelText', function () {
    $(document).find('#txtColumnRLSearch').val('');
});
$(document).on('click', '#clearText', function () {
    GenerateSearchList('', true);
});
function clearAdvanceSearchRL() {
    selectCountRL = 0;
    $("#EquipmentId").val("");
    $('#Name').val("");
    $("#Description").val("");
    $("#RequiredDate").val("").trigger('change');
    $("#advsearchfilteritemsRL").html('');
    $(".filteritemcount").text(selectCountRL);

}
//#endregion
//#region Grouping Dropdown Change
$(document).on('change', '#GroupingLabor', function () {
    run = true;
    dtTableRL.page('first').draw('page');

});
//#endregion
//#region Estimated hour edit
var enterhit = 0;
var oldHour = 0;
var oldScheduledDate = '';
$(document).on('blur', '.grd-hours', function (event) {
    if (enterhit == 1)
        return;
    var row = $(this).parents('tr');
    var data = dtTableRL.row(row).data();
    var keycode = (event.keyCode ? event.keyCode : event.which);
    var thstextbox = $(this);
    thstextbox.siblings('.is-saved-times').hide();
    //thstextbox.siblings('.is-saved-check').hide();

    if (thstextbox.val() == "") {
        thstextbox.val('0');
        thstextbox.siblings('.is-saved-times').show().attr('title', getResourceValue("EnterValueHoursAlert"));
        return;
    }
    else if ($.isNumeric(thstextbox.val()) === false) {
        thstextbox.siblings('.is-saved-times').show().attr('title', getResourceValue("EnterValidValueAlert"));
        return;
    }
    else if (thstextbox.val() > 999999.99) {
        thstextbox.siblings('.is-saved-times').show().attr('title', getResourceValue("MaximumValue999999.99Alert"));
        return;
    }

    if (oldHour != $(this).val()) {
        $.ajax({
            url: '/LaborScheduling/UpdateWoHoursScheduleDate',
            data: {
                WorkOrderSchedId: data.WorkOrderScheduleId,
                hours: $(this).val(),
                ScheduleDate: data.ScheduledStartDate
            },
            type: "POST",
            "datatype": "json",
            beforeSend: function () {
                thstextbox.siblings('.is-saved-loader').show();
            },
            success: function (data) {
                thstextbox.siblings('.is-saved-loader').hide();
                if (data.Result == 'success') {
                    thstextbox.siblings('.is-saved-check').show();
                    WoHoursEditArray.push(dtTableRL.row(row).data().WorkOrderScheduleId);
                    pageno = dtTableRL.page.info().page;
                    dtTableRL.page(pageno).draw('page');
                }
                else {
                    thstextbox.siblings('.is-saved-times').show();
                }
            },
            complete: function () {
                oldHour = 0;
            }
        });
    }
    else {
        oldHour = 0;
    }
});
$(document).on('keypress', '.grd-hours', function (event) {
    var row = $(this).parents('tr');
    var data = dtTableRL.row(row).data();
    var keycode = (event.keyCode ? event.keyCode : event.which);
    var thstextbox = $(this);
    thstextbox.siblings('.is-saved-times').hide();
    //thstextbox.siblings('.is-saved-check').hide();
    if (keycode == '13') {
        enterhit = 1;
        if (thstextbox.val() == "") {
            thstextbox.val('0');
            thstextbox.siblings('.is-saved-times').show().attr('title', getResourceValue("EnterValueHoursAlert"));
            return;
        }
        else if ($.isNumeric(thstextbox.val()) === false) {
            thstextbox.siblings('.is-saved-times').show().attr('title', getResourceValue("EnterValidValueAlert"));
            return;
        }
        else if (thstextbox.val() > 999999.99) {
            thstextbox.siblings('.is-saved-times').show().attr('title', getResourceValue("MaximumValue999999.99Alert"));
            return;
        }
        thstextbox.blur();

        if (oldHour != $(this).val()) {
            $.ajax({
                url: '/LaborScheduling/UpdateWoHoursScheduleDate',
                data: {
                    WorkOrderSchedId: data.WorkOrderScheduleId,
                    hours: $(this).val(),
                    ScheduleDate: data.ScheduledStartDate
                },
                type: "POST",
                "datatype": "json",
                beforeSend: function () {
                    thstextbox.siblings('.is-saved-loader').show();
                },
                success: function (data) {
                    thstextbox.siblings('.is-saved-loader').hide();
                    if (data.Result == 'success') {
                        thstextbox.siblings('.is-saved-check').show();
                        WoHoursEditArray.push(dtTableRL.row(row).data().WorkOrderScheduleId);
                        pageno = dtTableRL.page.info().page;
                        dtTableRL.page(pageno).draw('page');
                    }
                    else {
                        thstextbox.siblings('.is-saved-times').show();
                    }
                },
                complete: function () {
                    enterhit = 0;
                    oldHour = 0;
                }
            });
        }
        else {
            oldHour = 0;
        }
    }
    event.stopPropagation();
});
$(document).on('focus', '.grd-hours', function (event) {
    oldHour = $(this).val();
});
//#endregion

//#region Scheduled Date edit

$(document).on('change', '.Scheduleddtpicker', function (event) {
    var row = $(this).parents('tr');
    var data = dtTableRL.row(row).data();
    var thstextbox = $(this);
    thstextbox.siblings('.is-saved-times').hide();  
    if (oldScheduledDate != $(this).val()) {
        $.ajax({
            url: '/LaborScheduling/UpdateWoHoursScheduleDate',
            data: {
                WorkOrderSchedId: data.WorkOrderScheduleId,
                hours: data.ScheduledHours,
                ScheduleDate: $(this).val()
            },
            type: "POST",
            "datatype": "json",
            beforeSend: function () {
                thstextbox.siblings('.is-saved-loader').show();
            },
            success: function (data) {
                thstextbox.siblings('.is-saved-loader').hide();
                if (data.Result == 'success') {
                    thstextbox.siblings('.is-saved-check').show();
                    WoScheduleEditArray.push(dtTableRL.row(row).data().WorkOrderScheduleId);
                    pageno = dtTableRL.page.info().page;
                    dtTableRL.page(pageno).draw('page');

                }
                else {
                    thstextbox.siblings('.is-saved-times').show();
                }
            },
            complete: function () {
                enterScheduleDatehit = 0;
                oldScheduledDate = '';
            }
        });
    }
    else {
        oldScheduledDate = '';
    }
});
$(document).on('focus', '.Scheduleddtpicker', function () {
    oldScheduledDate = $(this).val();
});
//#endregion
