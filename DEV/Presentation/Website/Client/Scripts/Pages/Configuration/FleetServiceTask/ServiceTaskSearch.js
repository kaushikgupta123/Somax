//Search Retention
var gridname = "ServiceTask_Search";
var orderbycol = 0;
var orderDir = 'asc';
var run = false;
var selectCount = 0;
var totalcount = 0;
var enterhit = 0; 

$(document).ready(function () {
    generateFleetServiceTaskDataTable();
});

//#region Search Service Task
function generateFleetServiceTaskDataTable() {
    if ($(document).find('#fleetServiceTaskSearch').hasClass('dataTable')) {
        dtTable.destroy();
    }
    dtTable = $("#fleetServiceTaskSearch").DataTable({
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
                if (data.order)
                    data.order[0][0] = $(document).find('.srtfleetservicetaskcolumn.sort-active').data('col');
                //Search Retention
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
                //Search Retention
            }
            run = false;
        },
        "stateLoadCallback": function (settings, callback) {

            //Search Retention
            $.ajax({
                "url": "/Base/GetLayout",
                "data": {
                    GridName: gridname
                },
                "async": false,
                "dataType": "json",
                "success": function (json) {
                    if (json.LayoutInfo) {
                        callback(JSON.parse(json.LayoutInfo));
                        if (json.FilterInfo) {
                            setsearchui(JSON.parse(json.FilterInfo), $("#txtColumnSearch"), $("#spnControlCounter"), $("#dvFilterSearchSelect2"));

                        }
                    }
                    else {
                        callback(json);
                    }
                }
            });
            //Search Retention
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
                title: 'Service Tasks'
            },
            {
                extend: 'print',
                title: 'Service Tasks'
            },

            {
                text: 'Export CSV',
                extend: 'csv',
                title: 'Service Tasks',
                filename: 'Fleet Service Task List',
                extension: '.csv'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                exportOptions: {
                    columns: ':visible'
                },
                css: 'display:none',
                title: 'Service Tasks',
                orientation: 'potrait',
                pageSize: 'A4'
            }

        ],
        "orderMulti": true,
        "ajax": {
            "url": "/FleetServiceTask/GetFleetServiceTaskGridData",
            "type": "post",
            "datatype": "json",
            cache: false,
            data: function (d) {
                d.ClientLookupId = "";
                d.Description = "";
                d.SearchText = LRTrim($(document).find('#txtColumnSearch').val());
            },
            "dataSrc": function (result) {
                searchcount = result.recordsTotal;
                IsFleetServiceTaskAccessSecurity = result.IsFleetServiceTaskAccessSecurity;
                if (result.data.length < 1) {
                    $(document).find('#btnServiceTaskExport').prop('disabled', true);
                }
                else {
                    $(document).find('#btnServiceTaskExport').prop('disabled', false);
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
        columnDefs: [
            {
                targets: [2], render: function (a, b, data, d) {
                    if (data.InactiveFlag == false) {
                        if (IsFleetServiceTaskAccessSecurity) {
                            return '<a class="btn btn-outline-primary ActiveInactiveServiceTask gridinnerbutton" data-clntlkpid="' + data.ClientLookupId + '" title="Inactivate"><i class="fa fa-toggle-on"></i></a>';
                        }
                        else {
                            return '<a class="btn btn-outline-primary ActiveInactiveServiceTask gridinnerbutton" data-clntlkpid="' + data.ClientLookupId + '" title="Inactivate"><i class="fa fa-toggle-on"></i></a>';
                        }
                    }
                    else {
                        if (IsFleetServiceTaskAccessSecurity) {
                            return '<a class="btn btn-outline-primary ActiveInactiveServiceTask gridinnerbutton" data-clntlkpid="' + data.ClientLookupId + '" title="Activate"><i class="fa fa-toggle-off"></i></a>';
                        }
                        else {
                            return '<a class="btn btn-outline-primary ActiveInactiveServiceTask gridinnerbutton" data-clntlkpid="' + data.ClientLookupId + '" title="Activate"><i class="fa fa-toggle-off"></i></a>';
                        }
                    }

                }
            },
            { "width": "70%", "targets": 1 }
        ],
        "columns":
            [
                {
                    "data": "ClientLookupId",
                    "autoWidth": false,
                    "bSearchable": true,
                    "bSortable": false,
                    className: 'text-left',
                    "mRender": function (data, type, full) {
                        if (IsFleetServiceTaskAccessSecurity) {
                            return "<span class='field-validation-error' data-valmsg-replace='true' style='display: none'><span class='ValidationErr'></span></span><input type='text' style='width:55% !important; float:left;' maxlength = '31' data-val='" + data + "' class='clientLookupId  dt-inline-text  grd-servicetask' autocomplete='off' value='" + data + "' ><i class='fa fa-check-circle is-saved-check' style='float: left; position: relative; top: 8px; right:-3px; color:green;display:none;' title='success'></i><i class='fa fa-times-circle is-saved-times' style='float: left; position: relative; top: 8px; right:-3px; color:red;display:none;' title='test djhgjdfd kjhfhdksh'></i><img src='../Content/Images/grid-cell-loader.gif' class='is-saved-loader' style='float:left;position:relative;top: 8px; right:-3px;display:none;' />";
                        }
                        else {
                            return "<span class='field-validation-error' data-valmsg-replace='true' style='display: none'><span class='ValidationErr'></span></span><input type='text' style='width:55% !important; float:left;' maxlength = '31' disabled data-val='" + data + "' class='clientLookupId  dt-inline-text  grd-servicetask' autocomplete='off' value='" + data + "' ><i class='fa fa-check-circle is-saved-check' style='float: left; position: relative; top: 8px; right:-3px; color:green;display:none;' title='success'></i><i class='fa fa-times-circle is-saved-times' style='float: left; position: relative; top: 8px; right:-3px; color:red;display:none;' title='test djhgjdfd kjhfhdksh'></i><img src='../Content/Images/grid-cell-loader.gif' class='is-saved-loader' style='float:left;position:relative;top: 8px; right:-3px;display:none;' />";
                        }
                    }

                },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": false, className: 'text-center',
                    "mRender": function (data, type, full) {
                        if (IsFleetServiceTaskAccessSecurity) {
                            return "<span class='field-validation-error' data-valmsg-replace='true' style='display: none'><span class='ValidationErr'></span></span><input type='text' style='width:95% !important; float:left;', maxlength = '255'  data-val='" + data + "'  class='Description  dt-inline-text grd-servicetask' autocomplete='off' value='" + data + "' ><i class='fa fa-check-circle is-saved-check' style='float: left; position: relative; top: 8px; right:-3px; color:green;display:none;' title='success'></i><i class='fa fa-times-circle is-saved-times' style='float: left; position: relative; top: 8px; right:-3px; color:red;display:none;' title='test djhgjdfd kjhfhdksh'></i><img src='../Content/Images/grid-cell-loader.gif' class='is-saved-loader' style='float:left;position:relative;top: 8px; right:-3px;display:none;' />";
                        }
                        else {
                            return "<span class='field-validation-error' data-valmsg-replace='true' style='display: none'><span class='ValidationErr'></span><input type='text' style='width:95% !important; float:left;' , maxlength = '255' disabled  data-val='" + data + "'  class='Description  dt-inline-text grd-servicetask' autocomplete='off' value='" + data + "' ><i class='fa fa-check-circle is-saved-check' style='float: left; position: relative; top: 8px; right:-3px; color:green;display:none;' title='success'></i><i class='fa fa-times-circle is-saved-times' style='float: left; position: relative; top: 8px; right:-3px; color:red;display:none;' title='test djhgjdfd kjhfhdksh'></i><img src='../Content/Images/grid-cell-loader.gif' class='is-saved-loader' style='float:left;position:relative;top: 8px; right:-3px;display:none;' />";
                        }
                    }
                }


            ],
        initComplete: function () {
            SetPageLengthMenu();
            orderbycol = $('#fleetServiceTaskSearch').dataTable().fnSettings().aaSorting[0][0];
            orderDir = $('#fleetServiceTaskSearch').dataTable().fnSettings().aaSorting[0][1];
            switch (orderbycol) {
                case 0:
                    $(document).find('.srtfleetservicetaskcolumn').eq(0).addClass('sort-active');
                    sorttext = $(document).find('.srtfleetservicetaskcolumn').eq(0).text();
                    break;
                case 1:
                    $(document).find('.srtfleetservicetaskcolumn').eq(1).addClass('sort-active');
                    sorttext = $(document).find('.srtfleetservicetaskcolumn').eq(1).text();
                    break;
                default:
                    $(document).find('.srtfleetservicetaskcolumn').eq(0).addClass('sort-active');
                    sorttext = $(document).find('.srtfleetservicetaskcolumn').eq(0).text();
                    break;
            }
            switch (orderDir) {
                case "asc":
                    $(document).find('.srtfleetservicetaskcolumnorder').eq(0).addClass('sort-active');
                    break;
                case "desc":
                    $(document).find('.srtfleetservicetaskcolumnorder').eq(1).addClass('sort-active');
                    break;
            }
            $('#btnsortmenu').text(getResourceValue("spnSorting") + " : " + sorttext);
            $("#fleetservicetaskGridAction :input").removeAttr("disabled");
            $("#fleetservicetaskGridAction :button").removeClass("disabled");
            DisableExportButton($("#fleetServiceTaskSearch"), $(document).find('.import-export'));
        }
    });
}
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

//#region Add Service Task
$(document).on('click', '#AddServiceTask', function () {

    $('#fleetServiceTaskModal').modal('show');
});
function FleetServiceTaskAddOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        $("#fleetServiceTaskModal").modal('hide');
        SuccessAlertSetting.text = getResourceValue("ServiceTaskAddAlert");
            swal(SuccessAlertSetting, function () {
                titleText = getResourceValue("AlertActive");
                window.location.href = '/FleetServiceTask/Index?page=Libraries_Service_Tasks';
            });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);

    }
}
//#endregion

//#region Search Service Task from List
$(document).on('click', "#SrchBttnNew", function () {
    $.ajax({
        url: '/Base/PopulateNewSearchList',
        type: 'GET',
        data: { tableName: 'ServiceTasks' },
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
$(document).mouseup(function (e) {
    var container = $(document).find('#searchBttnNewDrop');
    if (!container.is(e.target) && container.has(e.target).length === 0) {
        container.hide("slideToggle");
    }
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
$(document).on('click', '#UlSearchList li', function () {
    var v = LRTrim($(this).text());
    $(document).find('#txtColumnSearch').val(v);
    TextSearch();
});
$(document).on('click', '.btnCross', function () {
    run = true;
    var btnCrossed = $(this).parent().attr('id');
    var searchtxtId = btnCrossed.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    selectCount--;
    dtTable.page('first').draw('page');
});
function TextSearch() {
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
        generateFleetServiceTaskDataTable();
    }
    var container = $(document).find('#searchBttnNewDrop');
    container.hide("slideToggle");
}
function GenerateSearchList(txtSearchval, isClear) {
    run = true;
    $.ajax({
        url: '/Base/ModifyNewSearchList',
        type: 'POST',
        data: { tableName: 'ServiceTasks', searchText: txtSearchval, isClear: isClear },
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

//#region Cancel Search
$(document).on('click', '#cancelText', function () {
    $(document).find('#txtColumnSearch').val('');
});
$(document).on('click', '#clearText', function () {
    GenerateSearchList('', true);
});
//#endregion

//#endregion

//#region Sort and OrderBy Service Task 
$(document).find('.srtfleetservicetaskcolumn').click(function () {
    ShowbtnLoader("btnsortmenu");
    $(document).find('.srtfleetservicetaskcolumn').removeClass('sort-active');
    $(this).addClass('sort-active');
    orderbycol = $(this).data('col');
    orderDir = $('li.srtfleetservicetaskcolumnorder.sort-active').data('mode');
    $('#fleetServiceTaskSearch').DataTable().order([orderbycol, orderDir]).draw();
    $('#btnsortmenu').text(getResourceValue("spnSorting") + " : " + $(this).text());
    run = true;
});
$(document).find('.srtfleetservicetaskcolumnorder').click(function () {
    ShowbtnLoader("btnsortmenu");
    $(document).find('.srtfleetservicetaskcolumnorder').removeClass('sort-active');
    $(this).addClass('sort-active');
    orderbycol = $(this).parent('ul').find('li.srtfleetservicetaskcolumn.sort-active').data('col');
    orderDir = $(this).data('mode');
    $('#fleetServiceTaskSearch').DataTable().order([orderbycol, orderDir]).draw();
    run = true;
});
//#endregion

//#region Export Service Task 
$(function () {
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {

            dtTable = $("#fleetServiceTaskSearch").DataTable();
            var info = dtTable.page.info();
            var start = info.start;
            var lengthMenuSetting = info.length;
            var currestsortedcolumn = $('#fleetServiceTaskSearch').dataTable().fnSettings().aaSorting[0][0];
            var length = $('#fleetServiceTaskSearch').dataTable().length;
            var coldir = $('#fleetServiceTaskSearch').dataTable().fnSettings().aaSorting[0][1];

            var searchText = LRTrim($(document).find('#txtColumnSearch').val());
            var jsonResult = $.ajax({
                "url": "/FleetServiceTask/GetServiceTaskPrintData",
                "type": "get",
                "datatype": "json",
                data: {
                    start: start,
                    length: lengthMenuSetting,
                    _colname: currestsortedcolumn,
                    _coldir: coldir,
                    _searchText: searchText
                },
                success: function (result) {
                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data;
            var visiblecolumnsIndex = $("#fleetServiceTaskSearch thead tr th.forExport").map(function (key) {
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
                header: $("#fleetServiceTaskSearch thead tr th.forExport").find('div').map(function (key) {
                    return this.innerHTML;
                }).get()
            };
        }
    });
    $("#EqpBulksidebar").mCustomScrollbar({
        theme: "minimal"
    });
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
//#endregion

//#region Edit/Update Service Task 
$(document).on('blur', '.grd-servicetask', function (event) {
    var PrevVal = $(this).attr("data-val");
    var row = $(this).parents('tr');
    var data = dtTable.row(row).data();
    var thstextbox = $(this);
   
    if (enterhit == 1)
        return;
    GetUpdateServiceTask(thstextbox, data, PrevVal);
    event.stopPropagation();
});
$(document).on('keypress', '.grd-servicetask', function (event) { 
    var PrevVal = $(this).attr("data-val");
    var row = $(this).parents('tr');
    var data = dtTable.row(row).data();
    var keycode = (event.keyCode ? event.keyCode : event.which);
    var thstextbox = $(this);
    thstextbox.removeClass('input-validation-error');
    thstextbox.siblings('.field-validation-error').find('.ValidationErr').text('');
    thstextbox.siblings('.field-validation-error').find('.ValidationErr').hide();
    enterhit = 0;
    if (keycode == '13') {
        enterhit = 1;
        GetUpdateServiceTask(thstextbox, data, PrevVal);
    }
    event.stopPropagation();
});


function GetUpdateServiceTask(thstextbox, data, PrevVal) {
    thstextbox.removeClass('input-validation-error');
    thstextbox.siblings('.field-validation-error').find('.ValidationErr').text('');
    var $regexname = /^[A-Z0-9\\%\\-\\:\\/\\$\\*\\+\\.]+$/;
    var titletext = "";
    var dataval = "";
    var clId = data.ClientLookupId;
    var desc = data.Description;
    thstextbox.siblings('.is-saved-times').hide();
    thstextbox.siblings('.is-saved-check').hide();
    if (thstextbox.hasClass('clientLookupId')) {
        dataval = thstextbox.val();
        clId = thstextbox.val();
        titletext = getResourceValue("ServiceTaskIDAlert");
        desc = "";

        if (!clId.match($regexname) && clId != "") {
            thstextbox.addClass('input-validation-error');
            thstextbox.siblings('.field-validation-error').find('.ValidationErr').text(getResourceValue("ServiceTaskIdRegAlert"));
            return;
        }
    }
    if (thstextbox.hasClass('Description')) {
        dataval = thstextbox.val();
        desc = thstextbox.val();
        titletext = getResourceValue("DescriptionAlert");
        clId = "";
    }
    if (thstextbox.val() == "") {
        thstextbox.siblings('.field-validation-error').find('.ValidationErr').text(titletext);
        thstextbox.addClass('input-validation-error');
        return;
    }
    thstextbox.removeClass('input-validation-error');
    $.ajax({
        url: '/FleetServiceTask/UpdateServiceTask',
        data: {
            SurviceTaskId: data.ServiceTaskId,
            ClientLookupId: clId,
            Description: desc,
            InactiveFlag: data.InactiveFlag
        },
        type: "POST",
        "datatype": "json",
        beforeSend: function () {
            $('.clientLookupId').attr('disabled', 'disabled');
            $('.Description').attr('disabled', 'disabled');
            thstextbox.siblings('.is-saved-loader').show();
        },
        success: function (data) {
            thstextbox.siblings('.is-saved-loader').hide();
            if (data.validationStatus == true) {
                thstextbox.siblings('.is-saved-check').show();
                thstextbox.attr({ "data-val": data.ClientLookupId });
            }
            else {
                thstextbox.val(PrevVal);
                GenericSweetAlertMethod(data);
            }
        },
        complete: function () {
            enterhit = 0;
            $('.clientLookupId').removeAttr('disabled');
            $('.Description').removeAttr('disabled');
        }
    });
}
//#endregion

//#region Active/Inactive Service Task 
$(document).on('click', ".ActiveInactiveServiceTask", function () {
    var row = $(this).parents('tr');
    var data = dtTable.row(row).data();

    var activeInactive = data.InactiveFlag;
    var salerttext = "";
    var Act_Inact_alt = "";
    if (activeInactive == false) {
        salerttext = getResourceValue("ServiceTaskInActiveSuccessAlert");
        Act_Inact_alt = getResourceValue("ServiceTaskInActivateAlert");
        
    }
    else {
        salerttext = getResourceValue("ServiceTaskActiveSuccessAlert");
        Act_Inact_alt = getResourceValue("ServiceTaskActivateAlert");
    }
    CancelAlertSettingForCallback.text = Act_Inact_alt;
    swal(CancelAlertSettingForCallback, function () {
       

        $.ajax({
            url: '/FleetServiceTask/ActiveInactiveServiceTask',
            data: {
                SurviceTaskId: data.ServiceTaskId
            },
            type: "POST",
            "datatype": "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {

                if (data.validationStatus == true) {
                    SuccessAlertSetting.text = salerttext;
                    swal(SuccessAlertSetting, function () {
                        dtTable.destroy();
                        generateFleetServiceTaskDataTable();
                    });
                }
                else {
                    GenericSweetAlertMethod(data);
                   
                }
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
//#endregion

//#region Cancel Service Task 
$(document).on('click', '#btnServiceTaskcancel', function () {
    
    $("#fleetServiceTaskModal").modal('hide');
    $(document).find('form').trigger("reset");
    $('.errormessage').html('');
});
//#endregion

//#region AddNew Service Task 
$(document).on('click', '.AddServiceTask', function () {
    $(document).find('form').trigger("reset");
    $('.errormessage').html('');
});
//#endregion

