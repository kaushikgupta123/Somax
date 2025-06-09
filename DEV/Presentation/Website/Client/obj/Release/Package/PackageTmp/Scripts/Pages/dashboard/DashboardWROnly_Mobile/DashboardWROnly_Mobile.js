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


    mobiscroll.settings = {
        lang: 'en',                                       // Specify language like: lang: 'pl' or omit setting to use default
        theme: 'ios',                                     // Specify theme like: theme: 'ios' or omit setting to use default
        themeVariant: 'light'                             // More info about themeVariant: https://docs.mobiscroll.com/4-10-9/javascript/popup#opt-themeVariant
    };

    CustomQueryDropdown = $('#SanCustomQueryDropdownDiv').mobiscroll().popup({
        display: 'bubble',
        anchor: '#ShowCustomQueryDropdown',
        buttons: [],
        cssClass: 'mbsc-no-padding md-vertical-list'
    }).mobiscroll('getInst');
    SortByDropdown = $('#SortByDropdownDiv').mobiscroll().popup({
        display: 'bubble',
        anchor: '#ShowSortByDropdown',
        buttons: [],
        cssClass: 'mbsc-no-padding md-vertical-list'
    }).mobiscroll('getInst');

    $('#SanCustomQueryDropdown').mobiscroll().listview({
        enhance: true,
        swipe: false,
    });
    $('#SortByDropdown').mobiscroll().listview({
        enhance: true,
        swipe: false
    });
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
       initComplete: function () {
            SetPageLengthMenu();
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

//#region V2-1100 Add Work Request
$(document).on('click', "#AddWorkRequestMbl", function (e) {
    $.ajax({
        url: "/Dashboard/AddWoRequestDynamicOnlyDashboard_Mobile",
        type: "GET",
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#AddWorkRequestDiv').html(data);
        },
        complete: function () {
            SetControls();
            $('.dtpicker').mobiscroll().calendar({
                display: 'bottom',
                touchUi: true,
                weekDays: 'min',
                yearChange: false,
                min: new Date(),
                months: 1
            }).inputmask('mm/dd/yyyy');
            $('.decimal').mobiscroll().numpad({
                touchUi: true,
                //min: 0.01,
                //max: 99999999.99,
                scale: 2,
                preset: 'decimal',
                thousandsSeparator: ''
            });
            $('#AddWorkRequestModalDialog').addClass('slide-active').trigger('mbsc-enhance');
        },
        error: function () {
            CloseLoader();
        }
    });
});

var Equipment_textFieldID_Mobile_WR = '', Equipment_valueFieldID_Mobile_WR = '';
$(document).on('click', '.openOJobAssetgrid', function () {
    Equipment_textFieldID_Mobile_WR = $(this).data('textfield');
    Equipment_valueFieldID_Mobile_WR = $(this).data('valuefield');
    $('#SJEquipmentModal').addClass('slide-active');
    $(document).find("#AssetListViewForSearch").html("");
    generateEquipmentDataTable_Mobile();
});
function generateEquipmentDataTable_Mobile() {
    //var Search = $(document).find('#txtEquipmentWRSSearch_Mobile').val();
    AssetListlength = 0;
    $.ajax({
        "url": "/Equipment/EquipmentLookupListView_Mobile",
        type: "POST",
        dataType: "html",
        data: {
            //Search: Search,
            //Name: Search
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#DivEquipmentSearchScrollViewModal').html(data);

        },
        complete: function () {

            InitializeAssetListView_Mobile();

            $('#SJEquipmentModal').addClass('slide-active');
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
//#region Common
$(document).on('click', '.clearTextBoxValue', function () {
    var id = $(this).data('txtboxid');
    $(document).find('#' + id).val('');
    if (id == 'txtPersonnelPlannerSearch_Mobile') {
        generatePersonnelPlannerDataTable_Mobile();
    }
    else if (id == 'txtEquipmentSearch_Mobile') {
        generateEquipmentDataTable_Mobile();
    }
    else if (id == 'txtAccountSearch_Mobile') {
        generateAccountDataTable_Mobile();
    }
});
//#endregion
function WorksRequestAddOnSuccess(data) {
    if (data.data === "success") {
        localStorage.setItem("workorderstatus", '2');
        localStorage.setItem("workorderstatustext", getResourceValue("spnWorkRequest"));
        if (data.Command === "save") {
            if (fileExtAddProccess != "") {
                var imgname = data.workOrderID + "_" + Math.floor((new Date()).getTime() / 1000);
                CompressImageAddProccess(FilesAddProccess[0], imgname + "." + fileExtAddProccess, data.workOrderID);
                fileExtAddProccess = "";
            }

            SuccessAlertSetting.text = getResourceValue("spnWoRequestAddSuccessfully");
            swal(SuccessAlertSetting, function () {
                localStorage.setItem("TabType", 'WorkRequest')
                window.location.href = '/Dashboard/Dashboard';
            });
        }
    }
    else {
        CloseLoader();
        ShowGenericErrorOnAddUpdate(data);
    }
}

$(document).on('click', '.btnCancelAddWorkRequest', function () {
    $('#AddWorkRequestModalDialog').removeClass('slide-active');
    fileExtAddProccess = "";
    $('#AddWorkRequestDiv').html('');
});
//#endregion

// if we are overriding highlight and unhighlight methods then
// adding an error class and valid class should be done manually
function SetControls() {
    var errClass = 'mobile-validation-error';
    CloseLoader();
    $.validator.setDefaults({
        ignore: null,
        //errorClass: "mobile-validation-error", // default values is input-validation-error
        //validClass: "valid", // default values is valid
        highlight: function (element, errorClass, validClass) { //for the elements having error
            $(element).addClass(errClass).removeClass(validClass);
            $(element.form).find("#" + element.id).parent().parent().addClass("mbsc-err");
            var elemName = $(element.form).find("#" + element.id).attr('name');
            $(element.form).find('[data-valmsg-for="' + elemName + '"]').addClass("mbsc-err-msg");
        },
        unhighlight: function (element, errorClass, validClass) { //for the elements having not any error
            $(element).removeClass(errClass).addClass(validClass);
            $(element.form).find("#" + element.id).parent().parent().removeClass("mbsc-err");
            var elemName = $(element.form).find("#" + element.id).attr('name');
            $(element.form).find('[data-valmsg-for="' + elemName + '"]').removeClass("mbsc-err-msg");
        },
    });
    $.validator.unobtrusive.parse(document);
    $('input, form').blur(function () {
        if ($(this).closest('form').length > 0) {
            $(this).valid();
        }
    });

    $(document).find('.select2picker').select2({});
    $(document).find('.mobiscrollselect:not(:disabled)').mobiscroll().select({
        display: 'bubble',
        filter: true,
        group: {
            groupWheel: false,
            header: false
        },
    });
    $(document).find('.mobiscrollselect:disabled').mobiscroll().select({
        disabled: true
    });
    $(document).find('.mobiscrollselect:not(:disabled)').mobiscroll().select({
        display: 'bubble',
    //touchUi: false,
    filter: true,
        group: {
        groupWheel: false,
        header: false
    },
    //width: 400,
    //placeholder: 'Please Select...'

});
$(document).find('.mobiscrollselect:disabled').mobiscroll().select({
    disabled: true
});
    SetFixedHeadStyle();
}

//#region V2-1100 immage process

var FilesAddProccess;
var fileExtAddProccess = "";
$(document).on('change', '.addphotoWorkorder', function () {

    var id = $(this).attr('id');
    var val = $(this).val();
    var previewid = $(this).closest(".takePic").find("img").attr('id');
    var imageName = val.replace(/^.*[\\\/]/, '');
    var fileUpload = $("#" + id).get(0);
    var fileExt = imageName.substr(imageName.lastIndexOf('.') + 1);

    if (fileExt != 'jpeg' && fileExt != 'jpg' && fileExt != 'png' && fileExt != 'JPEG' && fileExt != 'JPG' && fileExt != 'PNG') {
        ShowErrorAlert(getResourceValue("spnValidImage"));
        $("#" + id).val('');
        //e.preventDefault();
        return false;
    }
    else if (this.files[0].size > (1024 * 1024 * 10)) {
        ShowImageSizeExceedAlert();
        $("#" + id).val('');
        //e.preventDefault();
        return false;
    }

    else {

        if (window.FormData !== undefined) {
            var url = window.URL.createObjectURL(this.files[0]);
            $('#' + previewid).attr('src', url);
            FilesAddProccess = fileUpload.files;
            fileExtAddProccess = fileExt;
        }
        else {

        }

    }
});
function CompressImageAddProccess(files, imageName, WorkOrderId) {
    new Compressor(files, {
        quality: 0.6,
        convertTypes: ['image/png'],
        convertSize: 100000,
        // The compression process is asynchronous,
        // which means you have to access the `result` in the `success` hook function.
        success(result) {
            if (result.size < files.size) {
                SaveCompressedImageAddProccess(result, imageName, WorkOrderId);
            }
            else {
                SaveCompressedImageAddProccess(files, imageName, WorkOrderId);
            }
            console.log('file name ' + result.name + ' after compress ' + result.size);
        },
        error(err) {
            console.log(err.message);
        },
    });
}
function SaveCompressedImageAddProccess(data, imageName, WorkOrderId) {
    var AddPhotoFileData = new FormData();
    AddPhotoFileData.append('file', data, imageName);

    $.ajax({
        url: '../base/SaveUploadedFile',
        type: "POST",
        contentType: false, // Not to set any content header  
        processData: false, // Not to process data  
        data: AddPhotoFileData,
        success: function (result) {
            SaveUploadedFileToServerAddProccess(WorkOrderId, imageName);
            $('#add_photosWR').val('');
        }
    });
}
function SaveUploadedFileToServerAddProccess(WorkOrderId, imageName) {
    $.ajax({
        url: '../base/SaveMultipleUploadedFileToServer',
        type: 'POST',

        data: { 'fileName': imageName, objectId: WorkOrderId, TableName: "WorkOrder", AttachObjectName: "WorkOrder" },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.result == "0") {

                CloseLoader();
                /*ShowImageSaveSuccessAlert();*/
            }
            else if (data.result == "1") {
                CloseLoader();
                var errorMessage = getResourceValue("ImageExistAlert");
                ShowErrorAlert(errorMessage);

            }
            else {
                CloseLoader();
                var errorMessage = getResourceValue("NotAuthorisedUploadFileAlert");
                ShowErrorAlert(errorMessage);

            }
        },
        complete: function () {

        },
        error: function () {
            CloseLoader();
        }
    });
}

//#region Add QR scanner

//#region WorkOrder Request Only QR Reader
$(document).on('click', '#btnQrScanner', function () {
    $(document).find('#txtpartid').val('');
    $(document).find('#PartIssueAddModel_PartId').val('');
    if (!$(document).find('#QrCodeReaderModal_Mobile').hasClass('slide-active')) {
        $(document).find('#QrCodeReaderModal_Mobile').addClass("slide-active");
        StartQRReader_Mobile('');
    }
});
var Equipment_textFieldID_Mobile = '', Equipment_valueFieldID_Mobile = '';
function QrScannerEquipment_Mobile(txtID, ValID) {
    Equipment_textFieldID_Mobile = '#' + txtID;
    Equipment_valueFieldID_Mobile = '';
    if (ValID != '') {
        Equipment_valueFieldID_Mobile = '#' + ValID;
    }
    if (!$(document).find('#QrCodeReaderModal_Mobile').hasClass('slide-active')) {
        $(document).find('#QrCodeReaderModal_Mobile').addClass("slide-active");
        StartQRReader_Mobile('Equipment');
    }
}
$(document).on('click', '#closeQrScanner_Mobile', function () {
    if ($(document).find('#QrCodeReaderModal_Mobile').hasClass('slide-active')) {
        $(document).find('#QrCodeReaderModal_Mobile').removeClass('slide-active');
    }
    if (!$(document).find('#QrCodeReaderModal_Mobile').hasClass('slide-active')) {
        $(document).find('#QrCodeReaderModal_Mobile').removeClass('slide-active');
        StopCamera(); // using same method from somax_main.js
    }
});
function StartQRReader_Mobile(Module) {
    Html5Qrcode.getCameras().then(devices => {
        if (devices && devices.length) {
            scanner = new Html5Qrcode('reader', false);
            scanner.start({ facingMode: "environment" }, {
                fps: 10,
                qrbox: 150,
                //aspectRatio: aspectratio //1.7777778
            }, success => {
                if (Module == 'Equipment') {
                    onScanSuccessEquipment_Mobile(success);
                }
            }, error => {
            });
        } else {
            if ($(document).find('#QrCodeReaderModal_Mobile').hasClass('slide-active')) {
                $(document).find('#QrCodeReaderModal_Mobile').removeClass("slide-active");
            }
        }
    }).catch(e => {
        if ($(document).find('#QrCodeReaderModal_Mobile').hasClass('slide-active')) {
            $(document).find('#QrCodeReaderModal_Mobile').removeClass("slide-active");
        }
        if (e && e.startsWith('NotReadableError')) {
            ShowErrorAlert(getResourceValue("cameraIsBeingUsedByAnotherAppAlert"));
        }
        else if (e && e.startsWith('NotFoundError')) {
            ShowErrorAlert(getResourceValue("cameraDeviceNotFoundAlert"));
        }

    });
}

function detectMob() {
    var isMobile = false;
    if (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|ipad|iris|kindle|Android|Silk|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino/i.test(navigator.userAgent)
        || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(navigator.userAgent.substr(0, 4))) {
        return true;
    }
    return isMobile;
}

//#endregion

//#region Equipment qr scanner
function onScanSuccessEquipment_Mobile(decodedText) {
    $.ajax({
        url: "/WorkOrder/GetEquipmentIdByClientLookUpId?clientLookUpId=" + decodedText,
        type: "GET",
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if ($(document).find('#QrCodeReaderModal_Mobile').hasClass('slide-active')) {
                $(document).find('#QrCodeReaderModal_Mobile').removeClass("slide-active");
            }
            if (data.EquipmentId > 0) {
                $(document).find(Equipment_textFieldID_Mobile).val('');
                $(document).find(Equipment_textFieldID_Mobile).val(decodedText).trigger('mbsc-enhance');//.removeClass('input-validation-error');
                $(document).find(Equipment_textFieldID_Mobile).closest('form').valid();
                $(document).find(Equipment_textFieldID_Mobile).parents('div').eq(0).css('display', 'block');
                if (Equipment_valueFieldID_Mobile != '') {
                    $(document).find(Equipment_valueFieldID_Mobile).val('');
                    $(document).find(Equipment_valueFieldID_Mobile).val(data.EquipmentId).trigger('change');
                    if ($(document).find(Equipment_valueFieldID_Mobile).css('display') == 'block') {
                        $(document).find(Equipment_valueFieldID_Mobile).parents('div').eq(0).css('display', 'none');
                    }
                    if ($(document).find(Equipment_valueFieldID_Mobile).parent().find('div > button.ClearAssetModalPopupGridData').length > 0) {
                        //Dynamic Pages Cross button to clear the textbox Value
                        $(document).find(Equipment_valueFieldID_Mobile).parent().find('div > button.ClearAssetModalPopupGridData').css('display', 'block');
                    }
                }
            }
            else {
                //Show Error Swal
                ShowErrorAlert(getResourceValue('spnInvalidQrCodeMsgforEquipment').replace('${decodedText}', decodedText));
            }
        },
        complete: function () {
            StopCamera();
            CloseLoader();
        },
        error: function (xhr) {
            ShowErrorAlert(getResourceValue("somethingWentWrongAlert"));
            CloseLoader();
        }
    });
}
//#endregion


//#region Equipment Hierarchy ModalTree in Add Work Request Modal
var TextFieldId_ChargeTo = "";
var HdnfieldId_ChargeTo = "";
$(document).on('click', '#imgChargeToTreeGridOverWorkReqModal', function (e) {
    TextFieldId_ChargeTo = $(this).data('textfield');
    HdnfieldId_ChargeTo = $(this).data('valuefield');
    $(this).blur();
    generateWREquipmentTreeDynamic(-1);
});
function generateWREquipmentTreeDynamic(paramVal) {
    $.ajax({
        url: '/PlantLocationTree/WorkOrderEquipmentHierarchyTreeDynamic',
        datatype: "json",
        type: "post",
        contenttype: 'application/json; charset=utf-8',
        async: true,
        cache: false,
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find(".cntTreeWRM").html('');
            $(document).find(".cntTreeWRM").html(data);
        },
        complete: function () {
            CloseLoader();
            $('#wrEquipTreeModal').addClass('slide-active');
            treeTable($(document).find(".cntTreeWRM").find('#tblTree'));
            $(document).find('.radSelectWoDynamic').each(function () {
                if ($(document).find('#' + HdnfieldId_ChargeTo).val() == '0' || $(document).find('#' + HdnfieldId_ChargeTo) == '') {
                    if ($(this).data('equipmentid') === equipmentid) {
                        $(this).attr('checked', true);
                    }
                }
                else {
                    if ($(this).data('equipmentid') == $(document).find('#' + HdnfieldId_ChargeTo).val()) {
                        $(this).attr('checked', true);
                    }

                }

            });
            //-- V2-518 collapse all element
            // looking for the collapse icon and triggered click to collapse
            $.each($(document).find(".cntTreeWRM").find('#tblTree > tbody > tr').find('img[src="data:image/png;base64,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"]'), function (i, elem) {
                var parentId = elem.parentNode.parentNode.getAttribute('data-tt-id');
                $(document).find(".cntTreeWRM").find('#tblTree > tbody > tr[data-tt-id=' + parentId + ']').trigger('click');
            });
            //-- collapse all element

            $('#tblTree tr td').removeAttr('style');// code to remove the style applied from treetable.js -- white-space: nowrap;
        },
        error: function (xhr) {
            alert('error');
        }
    });
}
$(document).on('change', ".radSelectWoDynamic", function () {
    var s = $(this).data;
    $(document).find('#' + HdnfieldId_ChargeTo).val('0');
    equipmentid = $(this).data('equipmentid');
    var clientlookupid = $(this).data('clientlookupid');
    var chargetoname = $(this).data('itemname');
    chargetoname = chargetoname.substring(0, chargetoname.length - 1);

    $(document).find('#' + TextFieldId_ChargeTo).val(clientlookupid).trigger('mbsc-enhance');
    $(document).find('#' + TextFieldId_ChargeTo).parents('div').eq(0).css("display", "block");
    $(document).find('#' + HdnfieldId_ChargeTo).val(equipmentid);//.removeClass('input-validation-error').trigger('change');
    $(document).find('#' + HdnfieldId_ChargeTo).closest('form').valid();
    $(document).find('#' + HdnfieldId_ChargeTo).parents('div').eq(0).css("display", "none");
    $(document).find('#' + HdnfieldId_ChargeTo).parent().find('div > button.ClearAssetModalPopupGridData').css('display', 'block');
    $('#wrEquipTreeModalHide').trigger('click');
});
$(document).on('click', '#wrEquipTreeModalHide', function () {
    $('#wrEquipTreeModal').removeClass('slide-active');
    TextFieldId_ChargeTo = "";
    HdnfieldId_ChargeTo = "";
});
//#endregion
$(document).on('click', '.txtSearchClickComp', function () {
    generateAccountDataTable_Mobile();
});
$(document).on('click', '.txtSearchClickCompPlanner', function () {
    generatePersonnelPlannerDataTable_Mobile(); 
});
$(document).on('click', '.txtSearchClickCompEquipment', function () {
    generateEquipmentDataTable_Mobile();
});

//#endregion


