//#region Global variables
var gridnamePTL = "TaskList_Search";
var dtTablePTL;
var run = false;
var selectedcountPTL = 0;
var selectCountPTL = 0;
var PTListorder = '1';
var PTListorderDir = 'asc';
var StartDateEditArray = [];
var EndDateEditArray = [];
var ProjectId = 0;
var ProjecttaskIdsArray = [];
var ProjecttaskWOidArray = [];
var totalCountTaskGrid = 0;
var ProgressEditArray = [];
//#endregion

function LoadProjectTab()
{
    var ProjectId = $(document).find("#projectModel_ProjectId").val();
    LoadProjectHeaderData(ProjectId);
}
function LoadProjectHeaderData(ProjectId) {
    $.ajax({
        url: '/Project/Details',
        type: 'POST',
        dataType: 'html',
        data: {
            ProjectId: ProjectId
        },
        beforeSend: function () {
            ShowLoader();
        },
        //contentType: 'html',
        success: function (data) {
            if (data) {
                $(document).find('#projectmaincontainer').html("");
                $(document).find('#projectmaincontainer').html(data);
            }
           
        },
        complete: function () {
            generateProjectTaskDataTable();
            LoadPTLAdvancedSearchComponents();
            LoadActivity(ProjectId)
            CloseLoader();
           
        },
        error: function (err) {
            CloseLoader();
        }
    });
}


//#region Project Task list datatable

var order = '1';
var orderDir = 'asc';
function generateProjectTaskDataTable() {
    selectCountPTL = 0;
    var visibility = $(document).find('#securityProjectEdit').val();
    $(document).find('.dt-body-center').find('#ptlsearch-select-all').prop('checked', false);
    $("#DivPTLSearchitemcount").hide();
    EndDateEditArray = [];
    StartDateEditArray = [];
    ProjecttaskIdsArray = [];
    ProjecttaskWOidArray = [];
    ProgressEditArray = [];
    if ($(document).find('#ProjectTaskListSearchTable').hasClass('dataTable')) {
        dtTablePTL.destroy();
    }
    dtTablePTL = $("#ProjectTaskListSearchTable").DataTable({
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
                    data.order[0][0] = PTListorder;
                    data.order[0][1] = PTListorderDir;
                }
                var filterinfoarray = getfilterinfoarray($("#txtColumnPTLSearch"), $('#advsearchsidebar'));
                $.ajax({
                    "url": "/Base/CreateUpdateState",
                    "data": {
                        GridName: gridnamePTL,
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
                    GridName: gridnamePTL
                },
                "async": false,
                "dataType": "json",
                "success": function (json) {
                    if (json.LayoutInfo) {
                        var LayoutInfo = JSON.parse(json.LayoutInfo);
                        PTListorder = LayoutInfo.order[0][0];
                        PTListorderDir = LayoutInfo.order[0][1];
                        callback(JSON.parse(json.LayoutInfo));
                        if (json.FilterInfo) {
                            setsearchui(JSON.parse(json.FilterInfo), $("#txtColumnPTLSearch"), $(".filteritemcountPTL"), $("#advsearchfilteritemsPTL"));
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
        "orderMulti": true,
        "ajax": {
            "url": "/Project/GetProjectTaskByProjectIdGridDataForChunkSearch",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.txtSearchval = LRTrim($(document).find('#txtColumnPTLSearch').val());
                d.ProjectId = $(document).find('#projectModel_ProjectId').val();
                d.WorkOrderClientLookupId = LRTrim($("#WorkOrderClientLookupId").val());
                d.WorkOrderDescription = LRTrim($("#Description").val());
                d.StartDate = LRTrim($("#StartDate").val());
                d.EndDate = LRTrim($("#EndDate").val());
                d.Order = order;

            },
            "dataSrc": function (result) {
                let colOrder = dtTablePTL.order();
                PTListorderDir = colOrder[0][1];
                totalCountTaskGrid = result.recordsTotal;                
                HidebtnLoader("btnsortmenu");
                HidebtnLoaderclass("LoaderDrop");
                if ($('#ptlsearch-select-all').is(':checked')) {
                    $(document).find('.dt-body-center').find('#ptlsearch-select-all').prop('checked', false);
                }
                return result.data;
            },
            global: true
        },
        select: {
            style: 'os',
            selector: 'td:first-child'
        },
        "columnDefs": [
            {
                targets: [6],
                className: "hide_column"
            }
        ],

        "columns":
            [
                {
                    "data": "ProjectTaskId",
                    orderable: false,
                    "bSortable": false,
                    className: 'select-checkbox dt-body-center',
                    'render': function (data, type, full, meta) {
                        if ($('#ptlsearch-select-all').is(':checked') && totalCountTaskGrid == ProjecttaskIdsArray.length) {
                            return '<input type="checkbox" checked="checked" name="id[]" data-eqid="' + data + '" class="chkProjectTask_GridSearch ' + data + '"  value="'
                                + $('<div/>').text(data).html() + '">';
                        } else {
                            if (ProjecttaskIdsArray.indexOf(data) != -1) {
                                return '<input type="checkbox" checked="checked" name="id[]" data-eqid="' + data + '" class="chkProjectTask_GridSearch ' + data + '"  value="'
                                    + $('<div/>').text(data).html() + '">';
                            }
                            else {
                                return '<input type="checkbox" name="id[]" data-eqid="' + data + '" class="chkProjectTask_GridSearch ' + data + '"  value="'
                                    + $('<div/>').text(data).html() + '">';
                            }
                        }
                    }
                },
                {
                    "data": "WorkOrderClientLookupId",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": true,
                    "className": "text-left",
                    "name": "1"

                },
                {
                    "data": "WorkOrderDescription", "autoWidth": false, "bSearchable": true, "bSortable": true, "name": "2",
                },
                {
                    "data": "StartDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "type": "date", "className": "text-left Personnel", "name": "3",
                    'render': function (data, type, row) {
                        if (visibility == 'False') { return data; }
                        else if (StartDateEditArray.indexOf(row.ProjectTaskId) != -1) {
                            return "<div style='width:110px !important; position:relative;'><span class='field-validation-error' data-valmsg-replace='true' style='display: none'><span class='ValidationErr'></span></span><input type='text'  class='grid-dtpicker startdatedtpicker dt-inline-text' readonly='readonly' autocomplete='off' value='" + data + "'><i class='fa fa-check-circle is-saved-check' style='float: right; position: absolute; top: 8px; right:-22px; color:green;display:block;' title='success'></i><i class='fa fa-times-circle is-saved-times' style='float: right; position: absolute; top: 8px; right:-22px; color:red;display:none;'></i><img src='../Content/Images/grid-cell-loader.gif' class='is-saved-loader' style='float:right;position:absolute;top: 8px; right:-22px;display:none;' /></div>";
                        } else {
                            return "<div style='width:110px !important; position:relative;'><span class='field-validation-error' data-valmsg-replace='true' style='display: none'><span class='ValidationErr'></span></span><input type='text'  class='grid-dtpicker startdatedtpicker dt-inline-text' readonly='readonly' autocomplete='off' value='" + data + "'><i class='fa fa-check-circle is-saved-check' style='float: right; position: absolute; top: 8px; right:-22px; color:green;display:none;' title='success'></i><i class='fa fa-times-circle is-saved-times' style='float: right; position: absolute; top: 8px; right:-22px; color:red;display:none;'></i><img src='../Content/Images/grid-cell-loader.gif' class='is-saved-loader' style='float:right;position:absolute;top: 8px; right:-22px;display:none;' /></div>";
                        }
                    }
                },
                {
                    "data": "EndDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "type": "date", "className": "text-left Personnel", "name": "4",
                    'render': function (data, type, row) {
                        if (visibility == 'False') { return data; }
                        else if (EndDateEditArray.indexOf(row.ProjectTaskId) != -1) {
                            return "<div style='width:110px !important; position:relative;'><span class='field-validation-error' data-valmsg-replace='true' style='display: none'><span class='ValidationErr'></span></span><input type='text'  class='grid-dtpicker enddatedtpicker dt-inline-text' readonly='readonly' autocomplete='off' value='" + data + "'><i class='fa fa-check-circle is-saved-check' style='float: right; position: absolute; top: 8px; right:-22px; color:green;display:block;' title='success'></i><i class='fa fa-times-circle is-saved-times' style='float: right; position: absolute; top: 8px; right:-22px; color:red;display:none;'></i><img src='../Content/Images/grid-cell-loader.gif' class='is-saved-loader' style='float:right;position:absolute;top: 8px; right:-22px;display:none;' /></div>";
                        } else {
                            return "<div style='width:110px !important; position:relative;'><span class='field-validation-error' data-valmsg-replace='true' style='display: none'><span class='ValidationErr'></span></span><input type='text'  class='grid-dtpicker enddatedtpicker dt-inline-text' readonly='readonly' autocomplete='off' value='" + data + "'><i class='fa fa-check-circle is-saved-check' style='float: right; position: absolute; top: 8px; right:-22px; color:green;display:none;' title='success'></i><i class='fa fa-times-circle is-saved-times' style='float: right; position: absolute; top: 8px; right:-22px; color:red;display:none;'></i><img src='../Content/Images/grid-cell-loader.gif' class='is-saved-loader' style='float:right;position:absolute;top: 8px; right:-22px;display:none;' /></div>";
                        }
                    }
                },
                {
                    "data": "Progress", "autoWidth": true, "bSearchable": true, "bSortable": true, "type": "date", "className": "text-left Personnel", "name": "5",
                    "mRender": function (data, type, row) {
                        if (visibility == 'False') { return data; }
                        else if (ProgressEditArray.indexOf(row.ProjectTaskId) != -1 ) {
                            return "<div style='width:110px !important;'><span class='field-validation-error' data-valmsg-replace='true' style='display: none'><span class='ValidationErr'></span></span><input type='text' style='width:90px !important; float:left;text-align:right;' class='duration  dt-inline-text decimalinputupto2places grd-Progress' autocomplete='off' value='" + data + "' maskedFormat='3'><i class='fa fa-check-circle is-saved-check' style='float: right; position: relative; top: 8px; right:-3px; color:green;display:block;' title='success'></i><i class='fa fa-times-circle is-saved-times' style='float: right; position: relative; top: 8px; right:-3px; color:red;display:none;'></i><img src='../Content/Images/grid-cell-loader.gif' class='is-saved-loader' style='float:right;position:relative;top: 8px; right:-3px;display:none;' /></div>";
                        } else {
                            return "<div style='width:110px !important;'><span class='field-validation-error' data-valmsg-replace='true' style='display: none'><span class='ValidationErr'></span></span><input type='text' style='width:90px !important; float:left;text-align:right;' class='duration  dt-inline-text decimalinputupto2places grd-Progress' autocomplete='off' value='" + data + "' maskedFormat='3'><i class='fa fa-check-circle is-saved-check' style='float: right; position: relative; top: 8px; right:-3px; color:green;display:none;' title='success'></i><i class='fa fa-times-circle is-saved-times' style='float: right; position: relative; top: 8px; right:-3px; color:red;display:none;' ></i><img src='../Content/Images/grid-cell-loader.gif' class='is-saved-loader' style='float:right;position:relative;top: 8px; right:-3px;display:none;' /></div>";
                        }
                    }
                }

                ,
                {
                    "data": "WorkOrderId", "autoWidth": true, "className": " hide_column", "name": "6",

                }
            ],
        //'rowCallback': function (row, data, dataIndex) {
        //    var found = WoCheckedItem.some(function (el) {
        //        return el.WorkOrderSchedId == data.WorkOrderScheduleId;
        //    });
        //    if (found) {
        //        $(row).find('input[type="checkbox"]').prop('checked', true);
        //    }

        //},
        initComplete: function () {
            $(document).find('.adv-dtpicker').datepicker({
                //minDate: 0,
                "dateFormat": "mm/dd/yy",
                autoclose: true,
                changeMonth: true,
                changeYear: true
            });
            if ($(document).find('#securityProjectEdit').val() == 'False') {
                $("#ProjectTaskListSearchTable").DataTable().column(0).visible(false);
            }
        },
        "drawCallback": function (settings, data) {
            $(document).find('.adv-dtpicker').datepicker({
                // minDate: 0,
                changeMonth: true,
                changeYear: true,
                "dateFormat": "mm/dd/yy",
                autoclose: true
            });
        }

    });

    $('#ProjectTaskListSearchTable').find('th').click(function () {
        if ($(this).data('col')) {
            run = true;
            PTListorder = $(this).data('col');
        }

    });

    $(document).on('change', '#ProjectTaskListSearchTable_length .searchdt-menu', function () {
        run = true;
    });

    $(document).on('click', '#ProjectTaskListSearchTable .paginate_button', function () {
        run = true;
    });
}
function setsearchui(data, txtsearchelement, advcountercontainer, searchstringcontainer) {
    var searchitemhtml = '';
    $.each(data, function (index, item) {
        if (item.key == 'searchstring' && item.value) {
            var txtSearchval = item.value;
            if (item.value) {
                txtsearchelement.val(txtSearchval);
                searchitemhtml = "";
                searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + txtsearchelement.attr('id') + '">Search: ' + txtSearchval + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossPTL" aria-hidden="true"></a></span>';
            }
            return false;
        }
        else {
            if ($('#' + item.key).parent('div').is(":visible")) {
                $('#' + item.key).val(item.value);
                if (item.value) {
                    selectCountPTL++;
                    searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + item.key + '">' + $("label[for='" + item.key + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossPTL" aria-hidden="true"></a></span>';
                }
            }
            else if (item.key == 'advrequiredDatedaterange' && item.value !== '') {
                $('#' + item.key).val(item.value);
                if (item.value) {
                    searchitemhtml = searchitemhtml + '<span style="display:none;" class="label label-primary tagTo" id="_' + item.key + '">' + $("label[for='" + item.key + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossPTL" aria-hidden="true"></a></span>';
                }
            }
            if (item.key == 'RequiredDate') {
                $("#RequiredDate").trigger('change.select2');
            }
            advcountercontainer.text(selectCountPTL);
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    searchstringcontainer.html(searchitemhtml);
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
//#endregion


//#region New Search button
$(document).on('click', "#PTLSrchBttnNew", function () {
    $.ajax({
        url: '/Base/PopulateNewSearchList',
        type: 'GET',
        data: { tableName: 'ProjectTask' },
        beforeSend: function () {
            ShowbtnLoader("PTLSrchBttnNew");
        },
        success: function (data) {
            var i; var str = '';
            for (i = 0; i < data.searchOptionList.length; i++) {
                str += '<li><a href="javascript:void(0)" id= "mem_' + i + '"' + '><i class="fa fa-search" style="font-size: 1rem;position: relative;top: -1px;left: 0px;"></i> &nbsp;' + data.searchOptionList[i] + '</a></li>';
            }
            UlSearchListPTL.innerHTML = str;
            $(document).find('#PTLsearchBttnNewDrop').show("slideToggle");
        },
        complete: function () {
            HidebtnLoader("PTLSrchBttnNew");
        },
        error: function () {
            HidebtnLoader("PTLSrchBttnNew");
        }
    });
});
function GenerateSearchPTList(txtSearchval, isClear) {
    $.ajax({
        url: '/Base/ModifyNewSearchList',
        type: 'POST',
        data: { tableName: 'ProjectTask', searchText: txtSearchval, isClear: isClear },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            var i; var str = '';
            for (i = 0; i < data.searchOptionList.length; i++) {
                str += '<li><a href="javascript:void(0)"><i class="fa fa-search" style="font-size: 1rem;position: relative;top: -1px;left: 0px;"></i> &nbsp;' + data.searchOptionList[i] + '</a></li>';
            }
            UlSearchListPTL.innerHTML = str;
        },
        complete: function () {
            if (isClear == false) {
                dtTablePTL.page('first').draw('page');
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
$(document).on('keyup', '#txtColumnPTLSearch', function (event) {
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if (keycode == 13) {
        PTLTextSearch();
    }
    else {
        event.preventDefault();
    }
});
$(document).on('click', '.txtSearchPTLClick', function () {
    PTLTextSearch();
});
function PTLTextSearch() {
    run = true;
    clearPTLAdvanceSearch();
    $("#gridadvsearchstatus").val('');
    var txtSearchval = LRTrim($(document).find('#txtColumnPTLSearch').val());
    if (txtSearchval) {
        GenerateSearchPTList(txtSearchval, false);
        var searchitemhtml = "";
        searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $('#txtColumnPTLSearch').attr('id') + '">Search: ' + txtSearchval + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossPTL" aria-hidden="true"></a></span>';
        searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
        $("#advsearchfilteritemsPTL").html(searchitemhtml);
    }
    else {
        generateProjectTaskDataTable();
    }
    var container = $(document).find('#PTLsearchBttnNewDrop');
    container.hide("slideToggle");
}
$(document).on('click', '#UlSearchListPTL li', function () {
    var v = LRTrim($(this).text());
    $(document).find('#txtColumnPTLSearch').val(v);
    PTLTextSearch();
});
$(document).on('click', '#cancelText', function () {
    $(document).find('#txtColumnPTLSearch').val('');
});
$(document).on('click', '#clearTextPTL', function () {
    GenerateSearchPTList('', true);
});

function clearPTLAdvanceSearch() {
    selectCountPTL = 0;
    $("#WorkOrderClientLookupId").val("");
    $("#Description").val("");
    $("#StartDate").val("").trigger('change');
    $("#EndDate").val("").trigger('change');
    $("#advsearchfilteritemsPTL").html('');
    $(".filteritemcountPTL").text(selectCountPTL);

}
//#endregion

//#region Dropdown toggle   
$(document).mouseup(function (e) {
    var container = $(document).find('#PTLsearchBttnNewDrop');
    if (!container.is(e.target) && container.has(e.target).length === 0) {
        container.hide("slideToggle");
    }
});
//#endregion
//#region Advance Search
function LoadPTLAdvancedSearchComponents() {
    $("#sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $('#dismiss, .overlay').on('click', function () {
        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();

    });
    $("#sidebarPTLCollapse").on('click', function () {

        $('#sidebar').addClass('active');
        $('.overlay').fadeIn();
        $('.collapse.in').toggleClass('in');
        $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    });
    $("#btnPTLDataAdvSrch").on('click', function (e) {
        run = true;
        $(document).find('#txtColumnPTLSearch').val('');
        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();
        PTLAdvSearch();
        dtTablePTL.page('first').draw('page');
    });
    function PTLAdvSearch() {
        $('#txtColumnPTLSearch').val('');
        var searchitemhtml = "";
        selectCountPTL = 0;
        $('#advsearchsidebar').find('.adv-item').each(function (index, item) {
            if ($(this).hasClass('dtpicker')) {
                $(this).val(ValidateDate($(this).val()));
            }
            if ($(this).attr('id') != 'advrequiredDatedaterange') {
                if ($(this).val()) {
                    selectCountPTL++;
                    searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossPTL" aria-hidden="true"></a></span>';
                }
            }
            else {
                if ($(this).val()) {
                    searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossPTL" aria-hidden="true"></a></span>';
                }
            }

        });
        searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
        $("#advsearchfilteritemsPTL").html(searchitemhtml);
        // $('#_advrequiredDatedaterange').hide();
        $(".filteritemcountPTL").text(selectCountPTL);
    }


    $(document).on('click', '.btnCrossPTL', function () {
        run = true;
        var btnCrossed = $(this).parent().attr('id');
        var searchtxtId = btnCrossed.split('_')[1];
        $('#' + searchtxtId).val('').trigger('change');
        $(this).parent().remove();
        selectCountPTL--;
        PTLAdvSearch();
        dtTablePTL.page('first').draw('page');
    });
}
//#endregion

//#region start date edit
var oldStartDate = '';
$(document).on('focus', '.startdatedtpicker', function () {
    $(document).find('.grid-dtpicker').datepicker("destroy");
    oldStartDate = $(this).val();
    $(document).find('.grid-dtpicker').datepicker({
        minDate: 0,
        "dateFormat": "mm/dd/yy",
        autoclose: true,
        changeMonth: true,
        changeYear: true
    });
});
$(document).on('change', '.startdatedtpicker', function (event) {
    var row = $(this).parents('tr');
    var data = dtTablePTL.row(row).data();
    var thstextbox = $(this);
    thstextbox.siblings('.is-saved-times').hide();
    thstextbox.removeClass('input-validation-error');
    thstextbox.siblings('.field-validation-error').find('.ValidationErr').text('');

    var currentStartDate = new Date($(this).val());
    var endDate = new Date(data.EndDate)
    endDate = endDate.setDate(endDate.getDate() - 1);
    if (currentStartDate > endDate) {
        thstextbox.addClass('input-validation-error');
        thstextbox.siblings('.field-validation-error').find('.ValidationErr').text(getResourceValue("StartDateMustBeLesserThanEndDateAlert"));
        return;
    }

    if (oldStartDate != $(this).val()) {
        $.ajax({
            url: '/Project/UpdateProjectTaskStartDateAndEndDate',
            data: {
                ProjectTaskId: data.ProjectTaskId,
                StartDate: $(this).val(),
                EndDate: data.EndDate
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
                    StartDateEditArray.push(dtTablePTL.row(row).data().ProjectTaskId);
                    pageno = dtTablePTL.page.info().page;
                    dtTablePTL.page(pageno).draw('page');
                }
                else {
                    thstextbox.siblings('.is-saved-times').show();
                }
            },
            complete: function () {
                oldStartDate = '';
            }
        });
    }
    else {
        oldStartDate = '';
    }
});

//#endregion

//#region end date edit
var oldEndDate = '';
$(document).on('focus', '.enddatedtpicker', function () {
    $(document).find('.grid-dtpicker').datepicker("destroy");
    var row = $(this).parents('tr');
    var data = dtTablePTL.row(row).data()
    if (data.StartDate == '') {
        row.find('.startdatedtpicker').addClass('input-validation-error');
        row.find('.startdatedtpicker').siblings('.field-validation-error').find('.ValidationErr').text(getResourceValue("SelectStartDateForEndDateAlert"));
        return;
    }
    oldEndDate = $(this).val();
    $(document).find('.grid-dtpicker').datepicker({
        minDate: 0,
        "dateFormat": "mm/dd/yy",
        autoclose: true,
        changeMonth: true,
        changeYear: true
    });
});
$(document).on('change', '.enddatedtpicker', function (event) {
    var row = $(this).parents('tr');
    var data = dtTablePTL.row(row).data();
    var thstextbox = $(this);
    thstextbox.siblings('.is-saved-times').hide();
    thstextbox.removeClass('input-validation-error');
    thstextbox.siblings('.field-validation-error').find('.ValidationErr').text('');
    var currentEndDate = new Date($(this).val());
    var startDate = new Date(data.StartDate)
    startDate = startDate.setDate(startDate.getDate() + 1);
    if (currentEndDate < startDate) {
        thstextbox.addClass('input-validation-error');
        thstextbox.siblings('.field-validation-error').find('.ValidationErr').text(getResourceValue("EndDateMustBeGreaterThanStartDateAlert"));
        return;
    }

    if (oldEndDate != $(this).val()) {
        $.ajax({
            url: '/Project/UpdateProjectTaskStartDateAndEndDate',
            data: {
                ProjectTaskId: data.ProjectTaskId,
                StartDate: data.StartDate,
                EndDate: $(this).val()
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
                    EndDateEditArray.push(dtTablePTL.row(row).data().ProjectTaskId);
                    pageno = dtTablePTL.page.info().page;
                    dtTablePTL.page(pageno).draw('page');
                }
                else {
                    thstextbox.siblings('.is-saved-times').show();
                }
            },
            complete: function () {
                oldEndDate = '';
            }
        });
    }
    else {
        oldEndDate = '';
    }
});
//#endregion


//Activity & Comments
var selectedUsers = [];
var selectedUnames = [];
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
function LoadActivity(ProjectId) {
    $.ajax({
        "url": "/Project/LoadActivity",
        data: { ProjectId: ProjectId },
        type: "POST",
        datatype: "json",
        success: function (data) {
            $(document).find('#activityitems').html(data);
            $(document).find("#activityList").mCustomScrollbar({
                theme: "minimal"
            });
        },
        complete: function () {
            $(document).find('#activitydataloader').hide();
            var ftext = '';
            var bgcolor = '';
            $(document).find('#activityitems').find('.activity-header-item').each(function () {
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
                $(this).attr('style', 'color:' + bgcolor + ' !important;border:1px solid ' + bgcolor + ' !important;');
                ftext = thistext;
            });
            LoadComments(ProjectId);
        }
    });
}
function LoadComments(ProjectId) {
    $.ajax({
        "url": "/Project/LoadComments",
        data: { ProjectId: ProjectId },
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


//#region CKEditor
$(document).on("focus", "#wotxtcommentsnew", function () {
    $(document).find('.ckeditorarea').show();
    var noteitem = $(document).find('.editcomments').parents('.kt-notes__item');
    noteitem.find('.ckeditorarea').hide();
    noteitem.find('.kt-notes__body').show();
    noteitem.find('.commenteditdelearea').show();
    //ClearEditor();
    LoadCkEditor('wotxtcomments');
    $("#wotxtcommentsnew").hide();
    $(".ckeditorfield").show();
});
$(document).on('click', '#btnsavecommand', function () {
    var selectedUsers = [];
    const data = getDataFromTheEditor();
    if (!data) {
        return false;
    }
    var ProjectId = $(document).find("#projectModel_ProjectId").val();
    var ClientLookupId = $(document).find("#projectHeaderSummaryModel_ClientlookupId").val();
    var noteId = 0;
    if (LRTrim(data) == "") {
        return false;
    }
    else {
        /*  alert("ajax");*/
        $(document).find('.ck-content').find('.mention').each(function (item, index) {
            selectedUsers.push($(index).data('user-id'));
        });
        $.ajax({
            url: '/Project/AddComments',
            type: 'POST',
            beforeSend: function () {
                ShowLoader();
            },
            data: {
                ProjectId: ProjectId,
                content: data,
                ClientLookupId: ClientLookupId,
                userList: selectedUsers,
                noteId: noteId,
            },
            success: function (data) {
                if (data.Result == "success") {
                    var message;
                    if (data.mode == "add") {
                        SuccessAlertSetting.text = getResourceValue("CommentAddAlert");
                    }
                    else {
                        SuccessAlertSetting.text = getResourceValue("CommentUpdateAlert");
                    }
                    swal(SuccessAlertSetting, function () {

                        LoadProjectTab();
                    });
                }
                else {
                    ShowGenericErrorOnAddUpdate(data);
                    CloseLoader();
                }
            },
            complete: function () {
                ClearEditor();
                $("#wotxtcommentsnew").show();
                $(".ckeditorfield").hide();
                selectedUsers = [];
                selectedUnames = [];
                CloseLoader();
            },
            error: function () {
                CloseLoader();
            }
        });
    }
});

$(document).on('click', '#commandCancel', function () {
    var noteitem = $(document).find('.editcomments').parents('.kt-notes__item');
    noteitem.find('.kt-notes__body').show();
    noteitem.find('.commenteditdelearea').show();
    ClearEditor();
    $("#wotxtcommentsnew").show();
    $(".ckeditorfield").hide();
});
$(document).on('click', '.editcomments', function () {
    $(document).find(".ckeditorarea").each(function () {
        $(this).html('');
    });
    $("#wotxtcommentsnew").show();
    $(".ckeditorfield").hide();
    var notesitem = $(this).parents('.kt-notes__item').eq(0);
    notesitem.find('.ckeditorarea').html(CreateEditorHTML('wotxtcommentsEdit'));
    var noteitem = $(document).find('.editcomments').parents('.kt-notes__item');
    noteitem.find('.kt-notes__body').show();
    noteitem.find('.commenteditdelearea').show();
    var rawHTML = $.parseHTML($(this).parents('.kt-notes__item').find('.kt-notes__body').find('.originalContent').html());
    LoadCkEditorEdit('wotxtcommentsEdit', rawHTML);
    $(document).find('.ckeditorarea').hide();
    notesitem.find('.ckeditorarea').show();
    notesitem.find('.kt-notes__body').hide();
    notesitem.find('.commenteditdelearea').hide();
});

$(document).on('click', '.deletecomments', function () {
    DeleteWoNote($(this).attr('id'));
});
$(document).on('click', '.btneditcomments', function () {
    var data = getDataFromTheEditor();
    var ProjectId = $(document).find("#projectModel_ProjectId").val();
    var ClientLookupId = $(document).find("#projectHeaderSummaryModel_ClientlookupId").val();
    var noteId = $(this).parents('.kt-notes__item').find('.editcomments').attr('id');
    var updatedindex = $(this).parents('.kt-notes__item').find('.hdnupdatedindex').val();
    $.ajax({
        url: '/Project/AddComments',
        type: 'POST',
        beforeSend: function () {
            ShowLoader();
        },
        data: { ProjectId: ProjectId, content: LRTrim(data), ClientLookupId: ClientLookupId, noteId: noteId, updatedindex: updatedindex },
        success: function (data) {
            if (data.Result == "success") {
                var message;
                if (data.mode == "add") {
                    SuccessAlertSetting.text = getResourceValue("CommentAddAlert");
                }
                else {
                    SuccessAlertSetting.text = getResourceValue("CommentUpdateAlert");
                }
                swal(SuccessAlertSetting, function () {
                    LoadProjectTab();

                });
            }
            else {
                ShowGenericErrorOnAddUpdate(data);
                CloseLoader();
            }
        },
        complete: function () {
            // ClearEditorEdit();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });

});
$(document).on('click', '.btncommandCancel', function () {
    ClearEditorEdit();
    $(document).find('.ckeditorarea').hide();
    $(this).parents('.kt-notes__item').find('.kt-notes__body').show();
    $(this).parents('.kt-notes__item').find('.commenteditdelearea').show();
});
function DeleteWoNote(notesId) {
    swal(CancelAlertSettingForCallback, function () {
        $.ajax({
            url: '/Base/DeleteComment',
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
                    SuccessAlertSetting.text = getResourceValue("CommentDeleteAlert");
                    swal(SuccessAlertSetting, function () {

                        LoadProjectTab();
                    });
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}
//#endregion

//#region  Look up List WO
var ProjectWOPopUpTable;
var WoClientLookupId = "";
var WoChargeTo = "";
var WoChargeTo_Name = "";
var WoDescription = "";
var WoStatus = "";
var WoPriority = "";
var WoType = "";
var woLookuptotalcount = 0;
var SelectWO_WorkOrderId = [];
var SelectWO_WorkOrderIdDetails = [];
var woLookuporder = '1';
var woLookuporderDir = 'asc';
function generateProjectWOLookupGrid() {
    SelectWO_WorkOrderId = [];
    SelectWO_WorkOrderIdDetails = [];
    /* $(document).find('#ProjectWOLookupModal').modal("show");*/
    var rCount = 0;
    var visibility;
    if ($(document).find('#ProjectWOLookupTable').hasClass('dataTable')) {
        ProjectWOPopUpTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    ProjectWOPopUpTable = $("#ProjectWOLookupTable").DataTable({
        colReorder: {
            fixedColumnsLeft: 2
        },
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[1, "asc"]],
        "pageLength": 10,
        stateSave: true,
        language: {
            url: "/base/GetDataTableLanguageJson?InnerGrid=" + true,
        },

        sDom: 'Btlipr',
        "orderMulti": true,
        "ajax": {
            "url": "/Project/GetProjectWOLookupListchunksearch",

            data: function (d) {
                d.clientLookupId = WoClientLookupId;
                d.ChargeTo = WoChargeTo;
                d.ChargeTo_Name = WoChargeTo_Name;
                d.Description = WoDescription;
                d.Status = WoStatus;
                d.Priority = WoPriority;
                d.Type = WoType;
            },
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (json) {
                rCount = json.data.length;
                woLookuptotalcount = json.recordsTotal;
                let woLookuporder = ProjectWOPopUpTable.order();
                woLookuporderDir = woLookuporder[0][1];
                if ($('#ProjectWOLookupsearch-select-all').is(':checked')) {
                    $(document).find('.dt-body-center').find('#ProjectWOLookupsearch-select-all').prop('checked', false);
                }
                return json.data;
            }
        },
        //fixedColumns: {
        //    leftColumns: 2
        //},
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
                    'render': function (data, type, full, meta) {
                        if ($('#ProjectWOLookupsearch-select-all').is(':checked') && woLookuptotalcount == SelectWO_WorkOrderId.length) {
                            return '<input type="checkbox" checked="checked"  data-prid="' + data + '" class="chkWOProject_WorkOrder ' + data + '"  value="'
                                + $('<div/>').text(data).html() + '">';
                        } else {
                            if (SelectWO_WorkOrderId.indexOf(data) != -1) {
                                return '<input type="checkbox" checked="checked" data-prid="' + data + '" class="chkWOProject_WorkOrder ' + data + '"  value="'
                                    + $('<div/>').text(data).html() + '">';
                            }
                            else {
                                return '<input type="checkbox"  data-prid="' + data + '" class="chkWOProject_WorkOrder ' + data + '"  value="'
                                    + $('<div/>').text(data).html() + '">';
                            }
                        }
                    }
                },
                {
                    "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true

                },
                { "data": "ChargeTo", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "ChargeTo_Name", "autoWidth": true, "bSearchable": true, "bSortable": true
                },
                { "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Priority", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Type", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "RequiredDate", "autoWidth": true, "bSearchable": false, "bSortable": false }
            ],
        "rowCallback": function (row, data, index, full) {
            var colType = this.api().column(3).index('visible');
        },
        columnDefs: [
            {
                targets: [0, 1],
                className: 'noVis'
            }
        ],
        initComplete: function () {
            $(document).find('#tbleqpfooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#ProjectWOLookupModal').hasClass('show')) {
                $(document).find('#ProjectWOLookupModal').modal("show");
            }

            $('#ProjectWOLookupTable tfoot th').each(function (i, v) {
                var colIndex = i;
                if (colIndex > 0 && colIndex < 8) {
                    var title = $('#ProjectWOLookupTable thead th').eq($(this).index()).text();
                    $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="colindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                    if (WoClientLookupId) { $('#colindex_1').val(WoClientLookupId); }
                    if (WoChargeTo) { $('#colindex_2').val(WoChargeTo); }
                    if (WoChargeTo_Name) { $('#colindex_3').val(WoChargeTo_Name); }
                    if (WoDescription) { $('#colindex_4').val(WoDescription); }
                    if (WoStatus) { $('#colindex_5').val(WoStatus); }
                    if (WoPriority) { $('#colindex_6').val(WoPriority); }
                    if (WoType) { $('#colindex_7').val(WoType); }
                }


            });

            $('#ProjectWOLookupTable tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    WoClientLookupId = $('#colindex_1').val();
                    WoChargeTo = $('#colindex_2').val();
                    WoChargeTo_Name = $('#colindex_3').val();
                    WoDescription = $('#colindex_4').val();
                    WoStatus = $('#colindex_5').val();
                    WoPriority = $('#colindex_6').val();
                    WoType = $('#colindex_7').val();
                    ProjectWOPopUpTable.page('first').draw('page');
                }
            });

        }
    });
}

$(document).on('click', '#ProjectWOLookupsearch-select-all', function (e) {
    SelectWO_WorkOrderIdDetails = [];
    SelectWO_WorkOrderId = [];

    ProjectWOPopUpTable = $("#ProjectWOLookupTable").DataTable();
    var colname = woLookuporder;
    var coldir = woLookuporderDir;
    var checked = this.checked;
    $.ajax({
        "url": "/Project/GetProjectWOLookupListSelectAllData",
        data:
        {
            colname: colname,
            coldir: coldir,
            clientLookupId: WoClientLookupId,
            ChargeTo: WoChargeTo,
            ChargeTo_Name: WoChargeTo_Name,
            Description: WoDescription,
            Status: WoStatus,
            Priority: WoPriority,
            Type: WoType
        },
        async: true,
        type: "GET",
        beforeSend: function () {
            ShowLoader();
        },
        datatype: "json",
        success: function (data) {
            if (data) {
                $.each(data, function (index, item) {
                    if (checked) {
                        var exist = $.grep(SelectWO_WorkOrderId, function (obj) {
                            return obj.WorkOrderId === item.WorkOrderId;
                        });
                        if (exist.length == 0) {
                            SelectWO_WorkOrderId.push(item.WorkOrderId);
                        }
                    }
                    else {
                        SelectWO_WorkOrderId = $.grep(SelectWO_WorkOrderId, function (obj) {
                            return obj.WorkOrderId !== item.WorkOrderId;
                        });

                        var i = SelectWO_WorkOrderId.indexOf(item.WorkOrderId);
                        SelectWO_WorkOrderId.splice(i, 1);
                    }

                });
            }
        },
        complete: function () {
            $('.WoLookupGriditemcount').text(SelectWO_WorkOrderId.length);

            if (checked) {

                $(document).find('.dt-body-center').find('.chkWOProject_WorkOrder').prop('checked', 'checked');

            }
            else {
                $(document).find('.dt-body-center').find('.chkWOProject_WorkOrder').prop('checked', false);


            }
            CloseLoader();
        }
    });
});

$(document).on('change', '.chkWOProject_WorkOrder', function () {
    var data = ProjectWOPopUpTable.row($(this).parents('tr')).data();
    if (!this.checked) {
        var el = $('#ProjectWOLookupsearch-select-all').get(0);
        if (el && el.checked && ('indeterminate' in el)) {
            el.indeterminate = true;
        }

        var index = SelectWO_WorkOrderId.indexOf(data.WorkOrderId);
        SelectWO_WorkOrderId.splice(index, 1);
    }
    else {
        var item = data.WorkOrderId;
        var found = SelectWO_WorkOrderId.some(function (el) {
            return el.WorkOrderId === data.WorkOrderId;
        });
        if (!found) {

            SelectWO_WorkOrderId.push(data.WorkOrderId);
        }

    }
    if (SelectWO_WorkOrderId.length == woLookuptotalcount) {
        $(document).find('.dt-body-center').find('#ProjectWOLookupsearch-select-all').prop('checked', 'checked');
    }
    else {
        $(document).find('.dt-body-center').find('#ProjectWOLookupsearch-select-all').prop('checked', false);
    }

    $('.WoLookupGriditemcount').text(SelectWO_WorkOrderId.length);
});

$(document).on('click', '#AddPopupWo', function () {
    if (SelectWO_WorkOrderId.length < 1) {
        ShowGridItemSelectionAlert();
        return false;
    }
    else {
        var ProjectId = $("#projectModel_ProjectId").val();
        var jsonResult = {
            "WorkorderIds": SelectWO_WorkOrderId, "ProjectId": ProjectId
        }
        {
            $.ajax({
                url: '/Project/AddProjectLineItem',
                data: jsonResult,
                type: "POST",
                datatype: "json",
                beforeSend: function () {
                    ShowLoader();
                },
                success: function (result) {
                    if (result.success == true) {
                        $(document).find('#ProjectWOLookupModal').modal("hide");
                    }
                    else {
                        CloseLoader();
                        return false;
                    }

                },
                complete: function () {
                    CloseLoader();
                    $(document).find('.dt-body-center').find('#ProjectWOLookupsearch-select-all').prop('checked', false);
                    $(document).find('.chkWOProject_WorkOrder').prop('checked', false);
                    $('.WoLookupGriditemcount').text(0);
                    SelectWO_WorkOrderIdDetails = [];
                    SelectWO_WorkOrderId = [];
                    LookupGridResetTextboxValue();
                    generateProjectTaskDataTable();

                }
            });
        }
    }
});

function PreventEventForWoLookupGridSearch() {
    $(window).keydown(function (event) {
        if (event.keyCode == 13) {
            event.preventDefault();
            return false;
        }
    });
}
function LookupGridResetTextboxValue() {

    WoClientLookupId = "";
    WoChargeTo = "";
    WoChargeTo_Name = "";
    WoDescription = "";
    WoStatus = "";
    WoPriority = "";
    WoType = "";
}

$(document).on('click', '#btnAddWorkOrders', function () {
    generateProjectWOLookupGrid();
});
//#endregion


$(document).on('click', '#ptlsearch-select-all', function (e) {
    var ProjectId = $("#projectModel_ProjectId").val();
    ProjecttaskIdsArray = [];
    ProjecttaskWOidArray = [];
    dtTablePTL = $("#ProjectTaskListSearchTable").DataTable();
    var colname = order;
    var coldir = orderDir;
    var checked = this.checked;
    $.ajax({
        "url": "/Project/GetProjectTaskByProjectIdGridSelectAllData",
        data:
        {
            colname: colname,
            coldir: coldir,
            ProjectId: $(document).find('#projectModel_ProjectId').val(),
            WorkOrderClientLookupId: LRTrim($("#WorkOrderClientLookupId").val()),
            WorkOrderDescription: LRTrim($("#Description").val()),
            StartDate: LRTrim($("#StartDate").val()),
            EndDate: LRTrim($("#EndDate").val()),
            Order: order,
            txtSearchval: LRTrim($(document).find('#txtColumnPTLSearch').val())


        },
        async: true,
        type: "POST",
        beforeSend: function () {
            ShowLoader();
        },
        datatype: "json",
        success: function (data) {
            if (data) {
                $.each(data, function (index, item) {
                    if (checked) {

                        if (ProjecttaskWOidArray.indexOf(item.WorkOrderId) == -1) {
                            ProjecttaskWOidArray.push(item.WorkOrderId);
                        }
                        if (ProjecttaskIdsArray.indexOf(item.ProjectTaskId) == -1) {
                            ProjecttaskIdsArray.push(item.ProjectTaskId);
                        }
                    }
                    else {

                        var i = ProjecttaskIdsArray.indexOf(item.ProjectTaskId);
                        ProjecttaskIdsArray.splice(i, 1);

                        var j = ProjecttaskWOidArray.indexOf(item.WorkOrderId);
                        ProjecttaskWOidArray.splice(j, 1);
                    }

                });
            }
        },
        complete: function () {
            $('.PTLSearchitemcount').text(ProjecttaskWOidArray.length);
            if (ProjecttaskWOidArray.length == 0) {
                $("#DivPTLSearchitemcount").hide();
            } else {
                $("#DivPTLSearchitemcount").show();
            }
            if (checked) {

                $(document).find('.dt-body-center').find('.chkProjectTask_GridSearch').prop('checked', 'checked');

            }
            else {
                $(document).find('.dt-body-center').find('.chkProjectTask_GridSearch').prop('checked', false);


            }
            CloseLoader();
        }
    });
});

$(document).on('change', '.chkProjectTask_GridSearch', function () {
    var data = dtTablePTL.row($(this).parents('tr')).data();
    if (!this.checked) {
        var el = $('#ptlsearch-select-all').get(0);
        if (el && el.checked && ('indeterminate' in el)) {
            el.indeterminate = true;
        }


        var i = ProjecttaskIdsArray.indexOf(data.ProjectTaskId);
        ProjecttaskIdsArray.splice(i, 1);

        var j = ProjecttaskWOidArray.indexOf(data.WorkOrderId);
        ProjecttaskWOidArray.splice(j, 1);
    }
    else {

        if (ProjecttaskWOidArray.indexOf(data.WorkOrderId) == -1) {
            ProjecttaskWOidArray.push(data.WorkOrderId);

        }
        if (ProjecttaskIdsArray.indexOf(data.ProjectTaskId) == -1) {
            ProjecttaskIdsArray.push(data.ProjectTaskId);
        }

    }
    if (ProjecttaskWOidArray.length == totalCountTaskGrid) {
        $(document).find('.dt-body-center').find('#ptlsearch-select-all').prop('checked', 'checked');
    }
    else {
        $(document).find('.dt-body-center').find('#ptlsearch-select-all').prop('checked', false);
    }

    $('.PTLSearchitemcount').text(ProjecttaskIdsArray.length);
    if (ProjecttaskIdsArray.length == 0) {
        $("#DivPTLSearchitemcount").hide();
    } else {
        $("#DivPTLSearchitemcount").show();
    }
});
$(document).on('click', '#RemoveWorkOder', function () {
    if (ProjecttaskIdsArray.length < 1 || ProjecttaskWOidArray.length < 1) {
        ShowGridItemSelectionAlert();
        return false;
    } else {
        swal({
            title: getResourceValue("spnAreyousure"),
            text: getResourceValue("ConfirmRemoveWorkOrderAlert"),
            type: "warning",
            showCancelButton: true,
            closeOnConfirm: false,
            confirmButtonClass: "btn-sm btn-primary",
            cancelButtonClass: "btn-sm",
            confirmButtonText: getResourceValue("CancelAlertYes"),
            cancelButtonText: getResourceValue("CancelAlertNo")
        }, function () {

            var jsonResult = {
                "SelectedProjectIds": ProjecttaskIdsArray, "SelectedWorkOrderIds": ProjecttaskWOidArray
            };
            {
                $.ajax({
                    url: '/Project/RemoveProjectLineItem',
                    data: jsonResult,
                    type: "POST",
                    datatype: "json",
                    beforeSend: function () {
                        ShowLoader();
                    },
                    success: function (result) {
                        if (result.success == true) {

                            SuccessAlertSetting.text = getResourceValue("WorkOrderRemoveAlert");
                            swal(SuccessAlertSetting, function () {
                                generateProjectTaskDataTable();
                            });
                        }
                        else {
                            CloseLoader();
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

});


function ProjectStatusUpdate(status) {

    var confrimtext = status == "Complete" ? getResourceValue("ConfirmToCompleteProjectAlert") : status == "Canceled" ? getResourceValue("ConfirmToCancelProjectAlert") : status == "Open" ? getResourceValue("ConfirmToReopenProjectAlert") : "";

    if (status != "") {

        var ProjectId = $(document).find("#projectModel_ProjectId").val();
        swal({
            title: getResourceValue("spnAreyousure"),
            text: confrimtext,
            type: "warning",
            showCancelButton: true,
            closeOnConfirm: false,
            confirmButtonClass: "btn-sm btn-primary",
            cancelButtonClass: "btn-sm",
            confirmButtonText: getResourceValue("CancelAlertYes"),
            cancelButtonText: getResourceValue("CancelAlertNo")
        }, function () {


            {
                $.ajax({
                    url: "/Project/UpdatingProjectStatus",
                    type: "POST",
                    data: { ProjectId: ProjectId, Status: status },
                    datatype: "json",
                    beforeSend: function () {
                        ShowLoader();
                    },
                    success: function (data) {

                        var Successtext = status == "Complete" ? getResourceValue("ProjectCompletedSuccessfullyAlert") : status == "Canceled" ? getResourceValue("ProjectCanceledSuccessfullyAlert") : status == "Open" ? getResourceValue("ProjectReopenedSuccessfullyAlert") : "";

                        SuccessAlertSetting.text = Successtext;
                        CloseLoader();
                        swal(SuccessAlertSetting, function () {
                            //LoadProjectTab();
                            RedirectToProjectDetail(ProjectId, "");
                        });
                    },
                    complete: function () {

                    },
                    error: function () {
                        CloseLoader();
                    }
                });
            }
        });





    }


}


$(document).on('click', '.editProject', function () {
    var ProjectId = $(document).find("#projectModel_ProjectId").val();
    $.ajax({
        url: "/Project/AddorEditProject",
        type: "GET",
        dataType: 'html',
        data: { ProjectId: ProjectId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#projectmaincontainer').html(data);
        },
        complete: function () {
            SetProjControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', "#btnCancelEditProject", function () {
    var ProjectId = $(document).find("#projectAddorEdirModel_ProjectId").val();
    var ClientLookupId = $(document).find("#projectAddorEdirModel_ClientLookupId").val();
    swal(CancelAlertSetting, function () {
        RedirectToProjectDetail(ProjectId, ClientLookupId);
    });
});


//#region  Progress Value Update on Enter key and blur
var enterhit = 0;
var oldProgressvalue = '';
$(document).on('focus', '.grd-Progress', function () {
    oldProgressvalue = $(this).val();
});
$(document).on('blur', '.grd-Progress', function (event) {
    if (enterhit == 1)
        return;

    var row = $(this).parents('tr');
    var data = dtTablePTL.row(row).data();
    var keycode = (event.keyCode ? event.keyCode : event.which);
    var thstextbox = $(this);
    thstextbox.removeClass('input-validation-error');
    thstextbox.siblings('.field-validation-error').find('.ValidationErr').text('');
    thstextbox.siblings('.is-saved-times').hide();


    if (thstextbox.val() == "") {
        thstextbox.val('0');
        thstextbox.addClass('input-validation-error');
        thstextbox.siblings('.field-validation-error').find('.ValidationErr').text(getResourceValue("ProgressValidationReqErrMsgAlert"));
        return;
    }
    else if ($.isNumeric(thstextbox.val()) === false) {
        thstextbox.addClass('input-validation-error');
        thstextbox.siblings('.field-validation-error').find('.ValidationErr').text(getResourceValue("ProgressValidationRegErrMsgAlert"));
        return;
    }
    else if (thstextbox.val() > 100) {
        thstextbox.addClass('input-validation-error');
        thstextbox.siblings('.field-validation-error').find('.ValidationErr').text(getResourceValue("MaximumProgressValueHundredAlert"));
        return;
    }

    if (oldProgressvalue != $(this).val()) {
        $.ajax({
            url: '/Project/UpdateProjectTaskProgressVlaue',
            data: {
                ProjectTaskId: data.ProjectTaskId,
                Progress: $(this).val()
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
                    ProgressEditArray.push(dtTablePTL.row(row).data().ProjectTaskId);
                    pageno = dtTablePTL.page.info().page;
                    dtTablePTL.page(pageno).draw('page');
                }
                else {
                    thstextbox.siblings('.is-saved-times').show();
                }
            },
            complete: function () {
                oldProgressvalue = '';
            }
        });
    }
    else {
        oldProgressvalue = '';
    }
});
$(document).on('keypress', '.grd-Progress', function (event) {

    var row = $(this).parents('tr');
    var data = dtTablePTL.row(row).data();
    var keycode = (event.keyCode ? event.keyCode : event.which);
    var thstextbox = $(this);
    thstextbox.removeClass('input-validation-error');
    thstextbox.siblings('.field-validation-error').find('.ValidationErr').text('');
    thstextbox.siblings('.is-saved-times').hide();

    if (keycode == '13') {
        enterhit = 1;
        if (thstextbox.val() == "") {
            thstextbox.val('0');
            thstextbox.addClass('input-validation-error');
            thstextbox.siblings('.field-validation-error').find('.ValidationErr').text(getResourceValue("ProgressValidationReqErrMsgAlert"));
            return;
        }
        else if ($.isNumeric(thstextbox.val()) === false) {
            thstextbox.addClass('input-validation-error');
            thstextbox.siblings('.field-validation-error').find('.ValidationErr').text(getResourceValue("ProgressValidationRegErrMsgAlert"));
            return;
        }
        else if (thstextbox.val() > 100) {
            thstextbox.addClass('input-validation-error');
            thstextbox.siblings('.field-validation-error').find('.ValidationErr').text(getResourceValue("MaximumProgressValueHundredAlert"));
            return;
        }
        thstextbox.blur();
        if (oldProgressvalue != $(this).val()) {
            $.ajax({
                url: '/Project/UpdateProjectTaskProgressVlaue',
                data: {
                    ProjectTaskId: data.ProjectTaskId,
                    Progress: $(this).val()
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
                        ProgressEditArray.push(dtTablePTL.row(row).data().ProjectTaskId);
                        pageno = dtTablePTL.page.info().page;
                        dtTablePTL.page(pageno).draw('page');
                    }
                    else {
                        thstextbox.siblings('.is-saved-times').show();
                    }
                },
                complete: function () {
                    oldProgressvalue = '';
                    enterhit = 0;
                }
            });
        } else {
            oldProgressvalue = '';
        }
    }
    event.stopPropagation();
});
//#endregion
$(document).on('click', '#Projectreaddescription', function () {
    $(document).find('#Projectdetaildesmodaltext').text($(this).data("des"));
    $(document).find('#Projectdetaildesmodal').modal('show');
});

$(document).on('click', '#brdProject', function () {
    var ProjectId = $(document).find("#projectAddorEdirModel_ProjectId").val();
    var ClientLookupId = $(document).find("#projectAddorEdirModel_ClientLookupId").val();
    RedirectToProjectDetail(ProjectId, ClientLookupId);
});