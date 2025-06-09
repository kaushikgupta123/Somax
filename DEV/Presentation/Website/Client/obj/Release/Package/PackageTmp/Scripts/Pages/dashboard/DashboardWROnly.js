var workOrdersSearchdt;
var run = false;
var woStatus;
var woStatusVal;
var _isLoggedInFromMobile = false;
//#region Common 
$(function () {
    $(document).find('.select2picker').select2({});
    $(".dtpicker").keypress(function (event) { event.preventDefault(); });
    ShowbtnLoaderclass("LoaderDrop");
});
function ChangeTab(evt, gridtab) {
    var i, tabcontent, tablinks;

    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }

    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    
    document.getElementById(gridtab).style.display = "block";
    if (gridtab == 'Sanitation') {
        $(document).find('#Sanitationtab').addClass('active');
        generateSJDataTable();
    }
    else if (gridtab == 'Maintenance') {
        $(document).find('#Maintenancetab').addClass('active');
        generateWorkordersDataTable();
    }
}
//#endregion

//#region Work Request
$(function () {
    ShowbtnLoader("wrbtnsortmenu");
    $(document).find("#wradvancesearchsidebar").mCustomScrollbar({
        theme: "minimal"
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
    $(document).on('click', '#dismiss, .overlay', function () {
        $('.sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });
    $(document).on('click', "#wrsidebarCollapse", function () {
        $('#wradvancesearchsidebar').addClass('active');
        $('.overlay').fadeIn();
        $('.collapse.in').toggleClass('in');
        $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    });

    if ($(document).find('#tblworkorders').length > 0 && localStorage.getItem("TabType") != "SaitationRequest") {
        var workorderstatus = localStorage.getItem("workorderWROnlySearchstatus");
        if (workorderstatus) {
            woStatus = workorderstatus;
            generateWorkordersDataTable();
            $('#ScheduleWorkList').val(workorderstatus).trigger('change.select2');
        }
        else {
            woStatus = 0;
            generateWorkordersDataTable();
        }
    }
});
$("#ScheduleWorkList").change(function () {
    run = true;
    var optionval = $('#ScheduleWorkList option:selected').val();
    localStorage.setItem("workorderWROnlySearchstatus", optionval);
    woStatus = optionval;
    if (optionval.length !== 0) {
        ShowbtnLoaderclass("LoaderDrop");
        workOrdersSearchdt.page('first').draw('page');
    }
});
var order = '0';
var orderDir = 'asc';
function generateWorkordersDataTable() {
    if ($(document).find('#tblworkorders').hasClass('dataTable')) {
        workOrdersSearchdt.destroy();
    }
    workOrdersSearchdt = $("#tblworkorders").DataTable({
        colReorder: {
            fixedColumnsLeft: 1
        },
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "desc"]],
        stateSave: true,
        "stateSaveCallback": function (settings, data) {
            if (run == true) {
                if (data.order) {
                    data.order[0][0] = order;
                    data.order[0][1] = orderDir;
                }
                $.ajax({
                    "url": gridStateSaveUrl,
                    "data": {
                        GridName: "WorkRequest_DashBoard_Search",
                        LayOutInfo: JSON.stringify(data)
                    },
                    "dataType": "json",
                    "type": "POST",
                    "success": function () { return; }
                });
            }
            run = false;
        },
        "stateLoadCallback": function (settings, callback) {
            if (localStorage.getItem("TabType")) {
                callback("");
            }
            else {
                $.ajax({
                    "url": gridStateLoadUrl,
                    "data": {
                        GridName: "WorkRequest_DashBoard_Search"
                    },
                    "async": false,
                    "dataType": "json",
                    "success": function (json) {
                        if (json) {
                            callback(JSON.parse(json));
                        }
                        else {
                            callback(json);
                        }
                    }
                });
            }
            localStorage.removeItem("TabType");
        },
        scrollX: true,
        //fixedColumns: {
        //    leftColumns: 1
        //},
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true
        },
        sDom: 'Btlipr',
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'Data Export'
            },
            {
                extend: 'print',
                title: 'Data Export',
            },
            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Data Export',
                extension: '.csv'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                exportOptions: {
                    columns: ':visible'
                },
                orientation: 'landscape',
                pageSize: 'A2',
                title: 'Data Export',
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/Dashboard/GetWorkOrderMaintGrid",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.CustomQueryDisplayId = woStatus;
                d.workorder = LRTrim($("#gridadvsearchworkorder").val());
                d.description = LRTrim($("#gridadvsearchdescription").val());
                d.Chargeto = LRTrim($("#gridadvsearchChargeto").val());
                d.Chargetoname = LRTrim($("#gridadvsearchChargetoname").val());
                d.status = $("#gridadvsearchstatus").val();
                d.Created = ValidateDate($("#dtgridadvsearchCreated").val());
                d.assigned = LRTrim($("#gridadvsearchassigned").val());
                d.Scheduled = ValidateDate($("#dtgridadvsearchScheduled").val());
                d.Complete = ValidateDate($("#gridadvsearcomplete").val());
            },
            "dataSrc": function (result) {
                let colOrder = workOrdersSearchdt.order();
                orderDir = colOrder[0][1];
                //#region advancesrachdropdown
                $("#gridadvsearchstatus").empty();
                $("#gridadvsearchstatus").append("<option value=''>" + "--Select--" + "</option>");
                for (var key in result.lookupLists.status) {
                    if (result.lookupLists.status.hasOwnProperty(key)) {
                        
                        var id = key;
                        var name = result.lookupLists.status[key];
                        var ResourceId = getStatusValue(name);
                        if (ResourceId == undefined) {
                            continue;
                        }
                        $("#gridadvsearchstatus").append("<option value='" + id + "'>" + getStatusValue(name) + "</option>");
                    }
                }
                if (woStatusVal && $("#gridadvsearchstatus option[value='" + woStatusVal + "']").length > 0) {
                    $("#gridadvsearchstatus").val(woStatusVal).trigger("change.select2");
                }
                else {
                    $("#gridadvsearchstatus").val("").trigger("change.select2");
                }
                //#endregion

               /* HidebtnLoader("wrbtnsortmenu");*/
                HidebtnLoaderclass("LoaderDrop");
                return result.data;
            },
            global: true
        },
        "columns":
            [
                {
                    "data": "ClientLookupId",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": true,
                    "className": "text-left",
                    "name": "0"

                },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1",
                    mRender: function (data, type, full, meta) {
                        if (data.length > 50) {
                            return "<div class='text-wrap width-300'>" + data.substring(0, 50) + "..." + "</div>";
                        }
                        else {
                            return "<div class='text-wrap width-300'>" + data + "</div>";
                        }
                    }
                },
                { "data": "ChargeToClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2" },
                { "data": "ChargeTo_Name", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3" },
                {
                    "data": "Status", "autoWidth": false, "bSearchable": true, "bSortable": true, "name": "4",
                    render: function (data, type, row, meta) {
                        
                        if (data == statusCode.Approved) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--yellow m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Canceled) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--orange m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Complete) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--green m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Denied) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--purple m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Scheduled) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--blue m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.WorkRequest) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--red m-badge--wide' style='width:95px;' >" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Planning) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--grey m-badge--wide' style='width:95px;' >" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.AwaitApproval) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--teal' style='width:95px;' >" + getStatusValue(data) + "</span >";
                        }
                        else {
                            return getStatusValue(data);
                        }
                    }

                },
                {
                    "data": "CreateDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "type": "date", "name": "5",
                    render: function (data, type, row, meta) {
                        if (data == null) {
                            return '';
                        } else {
                            return data;
                        }
                    }
                },
                { "data": "Assigned", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "6" },
                {
                    "data": "ScheduledFinishDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "type": "date", "name": "7",
                    render: function (data, type, row, meta) {
                        if (data == null) {
                            return '';
                        } else {
                            return data;
                        }
                    }
                },
                {
                    "data": "CompleteDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "type": "date", "name": "8",
                    render: function (data, type, row, meta) {
                        if (data == null) {
                            return '';
                        } else {
                            return data;
                        }
                    }
                }
            ],
        //"columnDefs": [
        //    {
        //        targets: [0],
        //        className: 'noVis'
        //    }
        //],
        initComplete: function () {
            SetPageLengthMenu();
            /*commented for V2-834*/
            //var currestsortedcolumn = $('#tblworkorders').dataTable().fnSettings().aaSorting[0][0];
            //var column = this.api().column(currestsortedcolumn);
            //var columnId = $(column.header()).attr('id');
            //switch (columnId) {
            //    case "thWoId":
            //        EnableIdColumnSorting();
            //        break;
            //    case "thWoDesc":
            //        EnableDescColumnSorting();
            //        break;
            //    case "thWoCahrgeTo":
            //        EnableCahrgeToColumnSorting();
            //        break;
            //    case "thWoStatus":
            //        EnableStatusColumnSorting();
            //        break;
            //}

           /* $('#wrbtnsortmenu').text(getResourceValue("spnSorting") + " : " + column.header().innerHTML);*/
            $("#woGridAction :input").removeAttr("disabled");
            $("#woGridAction :button").removeClass("disabled");
            //V2-834
            _isLoggedInFromMobile = CheckLoggedInFromMob();
            if (_isLoggedInFromMobile === true) {
                $(".import-export").remove();
                $("#AddWorkRequestMbl").css('display', '');
            }
        }

    });
}
/*commented for V2-834*/
//function EnableIdColumnSorting() {
//    $('.DTFC_LeftWrapper').find('#thWoId').css('pointer-events', 'auto');
//    document.getElementById('thWoDesc').style.pointerEvents = 'none';
//    document.getElementById('thWoCahrgeTo').style.pointerEvents = 'none';
//    document.getElementById('thWoStatus').style.pointerEvents = 'none';
//}
//function EnableDescColumnSorting() {
//    $(document).find('.th-WoId').css('pointer-events', 'none');
//    document.getElementById('thWoDesc').style.pointerEvents = 'auto';
//    document.getElementById('thWoCahrgeTo').style.pointerEvents = 'none';
//    document.getElementById('thWoStatus').style.pointerEvents = 'none';
//}
//function EnableCahrgeToColumnSorting() {
//    $(document).find('.th-WoId').css('pointer-events', 'none');
//    document.getElementById('thWoDesc').style.pointerEvents = 'none';
//    document.getElementById('thWoCahrgeTo').style.pointerEvents = 'auto';
//    document.getElementById('thWoStatus').style.pointerEvents = 'none';
//}
//function EnableStatusColumnSorting() {
//    $(document).find('.th-WoId').css('pointer-events', 'none');
//    document.getElementById('thWoDesc').style.pointerEvents = 'none';
//    document.getElementById('thWoCahrgeTo').style.pointerEvents = 'none';
//    document.getElementById('thWoStatus').style.pointerEvents = 'auto';
//}
//$(document).find('.wrsrtpartcolumn').click(function () {
//    ShowbtnLoader("wrbtnsortmenu");
//    var col = $(this).data('col');
//    switch (col) {
//        case 0:
//            EnableIdColumnSorting();
//            $('.DTFC_LeftBodyWrapper').find('#thWoId').trigger('click');
//            break;
//        case 1:
//            EnableDescColumnSorting();
//            $('#thWoDesc').trigger('click');
//            break;
//        case 2:
//            EnableCahrgeToColumnSorting();
//            $('#thWoCahrgeTo').trigger('click');
//            break;
//        case 4:
//            EnableStatusColumnSorting();
//            $('#thWoStatus').trigger('click');
//            break;
//    }
//    $('#wrbtnsortmenu').text(getResourceValue("spnSorting") + " : " + $(this).text());
//    $(document).find('.wrsrtpartcolumn').removeClass('sort-active');
//    $(this).addClass('sort-active');
//    run = true;
//});
$(document).on('click', '#tblworkorders_paginate .paginate_button', function () {
    run = true;
});
$(document).on('change', '#tblworkorders_length .searchdt-menu', function () {
    run = true;
});

//#region Work Request Grid Search
$(document).on('click', "#btnDataAdvSrchworkorder", function (e) {
    var searchitemhtml = "";
    hGridfilteritemcount = 0;
    $('#advsearchsidebarWorkorder').find('.adv-item').each(function (index, item) {
        if ($(this).hasClass('dtpicker')) {
            $(this).val(ValidateDate($(this).val()));
        }
        if ($(this).val()) {
            hGridfilteritemcount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossWorkorder" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $('#wradvsearchfilteritems').html(searchitemhtml);
    $('#wradvancesearchsidebar').removeClass('active');
    $('.overlay').fadeOut();
    woStatusVal = $("#gridadvsearchstatus").val();
    hGridAdvanceSearch();
    workOrdersSearchdt.page('first').draw('page');
});
function hGridAdvanceSearch() {
    $('.wrfilteritemcount').text(hGridfilteritemcount);
}
$(document).on('click', '#wrliClearAdvSearchFilterWorkOrder', function () {
    run = true;
    $('#ScheduleWorkList').val('0').trigger('change.select2');
    woStatus = 0;
    localStorage.removeItem("workorderWROnlySearchstatus");
    hGridclearAdvanceSearch();
    workOrdersSearchdt.page('first').draw('page');
});
function hGridclearAdvanceSearch() {
    $('#advsearchsidebarWorkorder').find('input:text').val('');
    $('#advsearchsidebarWorkorder').find("select").val("").trigger('change.select2');
    hGridfilteritemcount = 0;
    $(".wrfilteritemcount").text(hGridfilteritemcount);
    $('#wradvsearchfilteritems').find('span').html('');
    $('#wradvsearchfilteritems').find('span').removeClass('tagTo');
    woStatusVal = $("#gridadvsearchstatus").val();
}
$(document).on('click', '.btnCrossWorkorder', function () {
    var btnCrossedId = $(this).parent().attr('id');
    var searchtxtId = btnCrossedId.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    hGridfilteritemcount--;
    if (searchtxtId == "gridadvsearchstatus") {
        woStatusVal = null;
    }
    hGridAdvanceSearch();
    workOrdersSearchdt.page('first').draw('page');
});
//#endregion

$(function () {

    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if ($('#Maintenance').is(':visible') == true) {
            if (this.context.length) {
               
                var CustomQueryDisplayId = woStatus;
                var workorder = LRTrim($("#gridadvsearchworkorder").val());
                var description = LRTrim($("#gridadvsearchdescription").val());
                var Chargeto = LRTrim($("#gridadvsearchChargeto").val());
                var Chargetoname = LRTrim($("#gridadvsearchChargetoname").val());
                var status = $("#gridadvsearchstatus").val();
                var Created = ValidateDate($("#dtgridadvsearchCreated").val());
                var assigned = LRTrim($("#gridadvsearchassigned").val());
                var Scheduled = ValidateDate($("#dtgridadvsearchScheduled").val());
                var Complete = ValidateDate($("#gridadvsearcomplete").val());
                var currestsortedcolumn = $('#tblworkorders').dataTable().fnSettings().aaSorting[0][0];
                var coldir = $('#tblworkorders').dataTable().fnSettings().aaSorting[0][1];
                var colname = $('#tblworkorders').dataTable().fnSettings().aoColumns[currestsortedcolumn].name;
                var jsonResult = $.ajax({
                    "url": "/Dashboard/GetWorkOrderPrintData",
                    "type": "get",
                    "datatype": "json",
                    data: {
                        _CustomQueryDisplayId: CustomQueryDisplayId,
                        _ClientLookupId: workorder,
                        _Description: description,
                        _ChargeToClientLookupId: Chargeto,
                        _ChargeToName: Chargetoname,
                        _Status: status,
                        _Created: Created,
                        _Assigned: assigned,
                        _Scheduled: Scheduled,
                        _Complete: Complete,
                        _colname: colname,
                        _coldir: coldir
                    },
                    success: function (result) {
                    },
                    async: false
                });
                var thisdata = JSON.parse(jsonResult.responseText).data;
                var visiblecolumnsIndex = $("#tblworkorders thead tr th").map(function (key) {
                    return this.getAttribute('data-th-index');
                }).get();
                var d = [];
                $.each(thisdata, function (index, item) {
                    if (item.ClientLookupId != null) {
                        item.ClientLookupId = item.ClientLookupId;
                    }
                    else {
                        item.ClientLookupId = "";
                    }
                    if (item.Description != null) {
                        item.Description = item.Description;
                    }
                    else {
                        item.Description = "";
                    }

                    if (item.ChargeToClientLookupId != null) {
                        item.ChargeToClientLookupId = item.ChargeToClientLookupId;
                    }
                    else {
                        item.ChargeToClientLookupId = "";
                    }
                    if (item.ChargeTo_Name != null) {
                        item.ChargeTo_Name = item.ChargeTo_Name;
                    }
                    else {
                        item.ChargeTo_Name = "";
                    }
                    if (item.Type != null) {
                        item.Type = item.Type;
                    }
                    else {
                        item.Type = "";
                    }
                    if (item.Status != null) {
                        item.Status = item.Status;
                    }
                    else {
                        item.Status = "";
                    }
                    if (item.Shift != null) {
                        item.Shift = item.Shift;
                    }
                    else {
                        item.Shift = "";
                    }
                    if (item.Priority != null) {
                        item.Priority = item.Priority;
                    }
                    else {
                        item.Priority = "";
                    }
                    if (item.CreateDate != null) {
                        item.CreateDate = item.CreateDate;
                    }
                    else {
                        item.CreateDate = "";
                    }
                    if (item.Creator != null) {
                        item.Creator = item.Creator;
                    }
                    else {
                        item.Creator = "";
                    }
                    if (item.Assigned != null) {
                        item.Assigned = item.Assigned;
                    }
                    else {
                        item.Assigned = "";
                    }
                    if (item.ScheduledFinishDate != null) {
                        item.ScheduledFinishDate = item.ScheduledFinishDate;
                    }
                    else {
                        item.ScheduledFinishDate = "";
                    }

                    if (item.CompleteDate != null) {
                        item.CompleteDate = item.CompleteDate;
                    }
                    else {
                        item.CompleteDate = "";
                    }


                    if (item.ScheduledStartDate != null) {
                        item.ScheduledStartDate = item.ScheduledStartDate;
                    }
                    else {
                        item.ScheduledStartDate = "";
                    }
                    if (item.FailureCode != null) {
                        item.FailureCode = item.FailureCode;
                    }
                    else {
                        item.FailureCode = "";
                    }
                    if (item.ActualFinishDate != null) {
                        item.ActualFinishDate = item.ActualFinishDate;
                    }
                    else {
                        item.ActualFinishDate = "";
                    }
                    if (item.CompleteComments != null) {
                        item.CompleteComments = item.CompleteComments;
                    }
                    else {
                        item.CompleteComments = "";
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
                    header: $("#tblworkorders thead tr th div").map(function (key) {
                        return this.innerHTML;
                    }).get()
                };
            }
        }
        else if ($('#Sanitation').is(':visible') == true) {
            if (this.context.length) {
                var CustomQueryDisplayId = parseInt($('#SanitationList option:selected').val());
                var JobId = LRTrim($("#JobId").val());
                var Description = LRTrim($("#Description").val());
                var ChargeTo = LRTrim($("#ChargeTo").val());
                var ChargeToName = LRTrim($('#ChargeToName').val());
                var Status = LRTrim($("#Status").val());
                var Created = LRTrim($("#Created").val());
                var Assigned = LRTrim($("#Assigned").val());
                var CompleteDate = LRTrim($("#CompleteDate").val());
                sanitationJobSearchdt = $("#sanitationJobSearchTable").DataTable();
                var currestsortedcolumn = $('#sanitationJobSearchTable').dataTable().fnSettings().aaSorting[0][0];
                var coldir = $('#sanitationJobSearchTable').dataTable().fnSettings().aaSorting[0][1];
                var colname = $('#sanitationJobSearchTable').dataTable().fnSettings().aoColumns[currestsortedcolumn].name;
                var jsonResult = $.ajax({
                    "url": "/Dashboard/GetSantPrintData",
                    "type": "get",
                    "datatype": "json",
                    data: {
                        CustomQueryDisplayId: CustomQueryDisplayId,
                        ClientLookupId: JobId,
                        Description: Description,
                        ChargeTo_ClientLookupId: ChargeTo,
                        ChargeTo_Name: ChargeToName,
                        Status: Status,
                        CreateDate: Created,
                        Assigned: Assigned,
                        CompleteDate: CompleteDate,
                        colname: colname,
                        coldir: coldir
                    },
                    success: function (result) {
                    },
                    async: false
                });
                var thisdata = JSON.parse(jsonResult.responseText).data;
                var visiblecolumnsIndex = $("#sanitationJobSearchTable thead tr th").map(function (key) {
                    return this.getAttribute('data-th-index');
                }).get();
                var d = [];
                $.each(thisdata, function (index, item) {
                    if (item.ClientLookupId != null) {
                        item.ClientLookupId = item.ClientLookupId;
                    }
                    else {
                        item.ClientLookupId = "";
                    }
                    if (item.Description != null) {
                        item.Description = item.Description;
                    }
                    else {
                        item.Description = "";
                    }

                    if (item.ChargeTo_ClientLookupId != null) {
                        item.ChargeTo_ClientLookupId = item.ChargeTo_ClientLookupId;
                    }
                    else {
                        item.ChargeTo_ClientLookupId = "";
                    }
                    if (item.ChargeTo_Name != null) {
                        item.ChargeTo_Name = item.ChargeTo_Name;
                    }
                    else {
                        item.ChargeTo_Name = "";
                    }
                    if (item.Status != null) {
                        item.Status = item.Status;
                    }
                    else {
                        item.Status = "";
                    }
                    if (item.CreateDate != null) {
                        item.CreateDate = item.CreateDate;
                    }
                    else {
                        item.CreateDate = "";
                    }
                    if (item.Assigned != null) {
                        item.Assigned = item.Assigned;
                    }
                    else {
                        item.Assigned = "";
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
                    header: $("#sanitationJobSearchTable thead tr th").find('div').map(function (key) {
                       
                        return this.innerHTML;
                    }).get()
                };
            }
        }
    });
});

//#region ColumnVisibility
$(document).on('click', '#wrliCustomize', function () {
    funCustomizeBtnClick(workOrdersSearchdt);
});
$(document).on('click', '.saveConfig', function () {
    var colOrder = [0];
    run = true;
    if ($('#Maintenance').is(':visible') == true) {
        funCustozeSaveBtn(workOrdersSearchdt, colOrder);
        workOrdersSearchdt.state.save(run);
    }
    else if ($('#Sanitation').is(':visible') == true) {
        var colOrder = [0];
        funCustozeSaveBtn(sanitationJobSearchdt, colOrder);
        run = true;
        sanitationJobSearchdt.state.save(run);
    }
});

//#endregion



//#endregion