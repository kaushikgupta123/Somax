var dtTable;
var selectCount = 0;
var selectedcount = 0;
var totalcount = 0;
var searchcount = 0;
var run = false;
var titleText = '';
var DefectsFleetIssue;
var StartRecordDate = '';
var EndRecordDate = '';
var CreateStartDateVw = '';
var CreateEndDateVw = '';
var today = $.datepicker.formatDate('mm/dd/yy', new Date());
var dtFuel = '<div class="symbol"><img src="#val0" /></div><div class="dtlsBox"><h2>#val1</h2><p>#val2</p></div>';
var colstr = '<div class="dtlsBox"><h2>#val1</h2><p>#val2</p></div>';
//Search Retention
var gridname = "FleetIssue_Search";
var orderbycol = 0;
var orderDir = 'asc';
//var FItempCustomdisplayId = '';
var fitempsearchtitle = '';
var CustomQueryDisplayId = 1;
var activeStatus;
function filterinfo(id, value) {
    this.key = id;
    this.value = value;
}
$.validator.setDefaults({ ignore: null });
$(document).ready(function () {
    $('#sampleDatepicker').datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: "dd/mm/yy",
        yearRange: "-90:+00"
    });
    ShowbtnLoaderclass("LoaderDrop");
    $(document).find('.select2picker').select2({});
    $("#fleetissueGridAction :input").attr("disabled", "disabled");
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
    $("#btnupdateequip").click(function () {
        $(".actionDrop2").slideToggle();
    });
    $(".actionDrop2 ul li a").click(function () {
        $(".actionDrop2").fadeOut();
    });
    $("#btnupdateequip").focusout(function () {
        $(".actionDrop2").fadeOut();
    });
    $("#btnFleetIssueDataAdvSrch").on('click', function (e) {
        run = true;
        $(document).find('#txtColumnSearch').val('');
        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();
        DefectsFleetIssue = $("#Defects").val();      
        FleetIssueAdvSearch();
        dtTable.page('first').draw('page');
    });
    var fleetissuestatus = localStorage.getItem("FLEETISSUESEARCHGRIDDISPLAYSTATUS");
    if (fleetissuestatus) {
        CustomQueryDisplayId = fleetissuestatus;
        if (fleetissuestatus === '3' || fleetissuestatus === '4' || fleetissuestatus === '5' ||
            fleetissuestatus === '6' || fleetissuestatus === '7' || fleetissuestatus === '8') {
            $('#cmbFIview').val(fleetissuestatus).trigger('change');
            $("#fleetissuesearchListul li").removeClass("activeState");
            $("#fleetissuesearchListul li[id='3']").addClass("activeState");
            text = $("#fleetissuesearchListul li[id='3']").text();
            $('#fleetissuesearchtitle').text(text);
        }
        $('#fleetissuesearchListul li').each(function (index, value) {
            if ($(this).attr('id') == CustomQueryDisplayId) {
                text = $(this).text();
                $('#fleetissuesearchtitle').text($(this).text());
                $(".searchList li").removeClass("activeState");
                $(this).addClass('activeState');
            }
        });
        if (fleetissuestatus === '3' || fleetissuestatus === '4' || fleetissuestatus === '5' ||
            fleetissuestatus === '6' || fleetissuestatus === '7' || fleetissuestatus === '8') {
            if (fleetissuestatus === '8')
                text = text + " - " + $('#FIdaterange').val();
            else
                text = text + " - " + $(document).find('#cmbFIview option[value=' + fleetissuestatus + ']').text();
            $('#fleetissuesearchtitle').text(text);
        }
    }
    else {
        localStorage.setItem("FLEETISSUESEARCHGRIDDISPLAYSTATUS", "1");
        $('#fleetissuesearchListul li').first().addClass('activeState');
        $('#fleetissuesearchtitle').text(getResourceValue("OpenIssuesAlert"));
    }
    generateFleetIssueDataTable();
});
$(document).on('click', '#liPdf', function () {
    var params = {
        customQueryDisplayId: localStorage.getItem("FLEETISSUESEARCHGRIDDISPLAYSTATUS"),
        colname: orderbycol,
        coldir: orderDir,
        ClientLookupId: LRTrim($("#EquipmentID").val()),
        Name: LRTrim($("#Name").val()),
        Make: LRTrim($("#Make").val()),
        Model: LRTrim($("#ModelNumber").val()),
        VIN: LRTrim($("#VIN").val()),
        StartRecordDate: ValidateDate(StartRecordDate),
        EndRecordDate: ValidateDate(EndRecordDate),
        StartCreateDateVw: ValidateDate(CreateStartDateVw),
        EndCreateDateVw: ValidateDate(CreateEndDateVw),
        Defects: (DefectsFleetIssue != null) ? DefectsFleetIssue : $('#Defects').val(),
        SearchText: LRTrim($(document).find('#txtColumnSearch').val())
    };
    fleetFuelPrintParams = JSON.stringify({ 'fleetIssuePrintParams': params });
    $.ajax({
        "url": "/FleetIssue/SetPrintData",
        "data": fleetFuelPrintParams,
        contentType: 'application/json; charset=utf-8',
        "dataType": "json",
        "type": "POST",
        "success": function () {
            window.open('/FleetIssue/PrintASPDF', '_self');
            return;
        }
    });
    funcCloseExportbtn();
});

$(document).on('click', '#liPrint', function () {
    var params = {
        customQueryDisplayId: localStorage.getItem("FLEETISSUESEARCHGRIDDISPLAYSTATUS"),
        colname: orderbycol,
        coldir: orderDir,
        ClientLookupId: LRTrim($("#EquipmentID").val()),
        Name: LRTrim($("#Name").val()),
        Make: LRTrim($("#Make").val()),
        Model: LRTrim($("#ModelNumber").val()),
        VIN: LRTrim($("#VIN").val()),
        StartRecordDate: ValidateDate(StartRecordDate),
        EndRecordDate: ValidateDate(EndRecordDate),
        StartCreateDateVw: ValidateDate(CreateStartDateVw),
        EndCreateDateVw: ValidateDate(CreateEndDateVw),
        Defects: (DefectsFleetIssue != null) ? DefectsFleetIssue : $('#Defects').val(),
        SearchText: LRTrim($(document).find('#txtColumnSearch').val())

    };
    fleetFuelPrintParams = JSON.stringify({ 'fleetIssuePrintParams': params });
    $.ajax({
        "url": "/FleetIssue/SetPrintData",
        "data": fleetFuelPrintParams,
        contentType: 'application/json; charset=utf-8',
        "dataType": "json",
        "type": "POST",
        "success": function () {
            window.open('/FleetIssue/ExportASPDF', '_blank');
            return;
        }
    });
    funcCloseExportbtn();
});
function generateFleetIssueDataTable() {
    var printCounter = 0;
    var IsFleetIssueEditSecurity = false;
    var IsFleetIssueDeleteSecurity = false;
    var OpenItemCount = 0;
    var LocalizedFuelUnit = "";
    var column;
    if ($(document).find('#fleetissueSearch').hasClass('dataTable')) {
        dtTable.destroy();
    }
    dtTable = $("#fleetissueSearch").DataTable({
        colReorder: {
            fixedColumnsLeft: 1
        },
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[1, "asc"]],
        "stateSave": true,
        "stateSaveCallback": function (settings, data) {
            if (run == true) {
                //V2-557
                if (data.order) {
                    data.order[0][0] = orderbycol;
                    data.order[0][1] = orderDir;
                }
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
                    
                        if (json.LayoutInfo !== '') {
                            var LayoutInfo = JSON.parse(json.LayoutInfo);
                            orderbycol = LayoutInfo.order[0][0];
                            orderDir = LayoutInfo.order[0][1];
                            callback(JSON.parse(json.LayoutInfo));
                        if (json.FilterInfo) {
                            setsearchui(JSON.parse(json.FilterInfo), $("#txtColumnSearch"), $("#spnControlCounter"), $("#dvFilterSearchSelect2"));

                        }
                    }
                    else {
                            callback(json.LayoutInfo);
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
                extend: 'print',
                title: 'Fleet Issue List'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                exportOptions: {
                    columns: ':visible'
                },
                css: 'display:none',
                title: 'Fleet Issue List',
                orientation: 'landscape',
                pageSize: 'A3'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/FleetIssue/GetFleetIssueGridData",
            "type": "post",
            "datatype": "json",
            cache: false,
            data: function (d) {
                d.customQueryDisplayId = localStorage.getItem("FLEETISSUESEARCHGRIDDISPLAYSTATUS");
                d.CreateStartDateVw = ValidateDate(CreateStartDateVw);
                d.CreateEndDateVw = ValidateDate(CreateEndDateVw);
                d.ClientLookupId = LRTrim($("#EquipmentID").val());
                d.Name = LRTrim($("#Name").val());
                d.Make = LRTrim($("#Make").val());
                d.Model = LRTrim($("#ModelNumber").val());
                d.VIN = LRTrim($("#VIN").val());
                d.StartRecordDate = ValidateDate(StartRecordDate);
                d.EndRecordDate = ValidateDate(EndRecordDate);              
                d.Defects = (DefectsFleetIssue != null) ? DefectsFleetIssue : $('#Defects').val(); 
                d.SearchText = LRTrim($(document).find('#txtColumnSearch').val());
            },
            "dataSrc": function (result) {
                let colOrder = dtTable.order();
                orderbycol = colOrder[0][0];
                orderDir = colOrder[0][1];
                searchcount = result.recordsTotal;
                IsFleetIssueEditSecurity = result.IsFleetIssueEditSecurity;
                IsFleetIssueDeleteSecurity = result.IsFleetIssueDeleteSecurity;
                OpenItemCount = result.openItemCount;
                if (column) {
                    if (IsFleetIssueEditSecurity == false && IsFleetIssueDeleteSecurity == false) {
                        column.visible(false);
                    }
                    else {
                        if (OpenItemCount > 0) {
                            column.visible(true);
                        }
                        else {
                            column.visible(false);
                        }
                    }
                }
                if (result.data.length < 1) {
                    $(document).find('#btnFleetIssueExport').prop('disabled', true);
                }
                else {
                    $(document).find('#btnFleetIssueExport').prop('disabled', false);
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
                targets: [6], render: function (a, b, data, d) {
                    if (data.Status.toLowerCase() == statusCode.Open.toLowerCase()) {
                        if (IsFleetIssueEditSecurity && IsFleetIssueDeleteSecurity) {
                            return '<a class="btn btn-outline-success editFleetIssueBttn gridinnerbutton" data-defectsfi="' + data.Defects + '"  data-fiid="' + data.FleetIssuesId + '" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                                '<a class="btn btn-outline-danger delFleetIssueBttn gridinnerbutton" data-fiid="' + data.FleetIssuesId + '"  title="Delete"> <i class="fa fa-trash"></i></a>';
                        }
                        else if (IsFleetIssueEditSecurity) {
                            return '<a class="btn btn-outline-success editFleetIssueBttn gridinnerbutton" data-defectsfi="' + data.Defects + '"  data-fiid="' + data.FleetIssuesId + '" title= "Edit"> <i class="fa fa-pencil"></i></a>';

                        }
                        else if (IsFleetIssueDeleteSecurity) {
                            return '<a class="btn btn-outline-danger delFleetIssueBttn gridinnerbutton" data-fiid="' + data.FleetIssuesId + '"  title="Delete"> <i class="fa fa-trash"></i></a>';
                        }
                        else { return ""; }
                    }
                    else {
                        return "";
                    }
                }
            }
        ],
        "columns":
        [
            {
                "data": "ClientLookupId",
                "autoWidth": false,
                "bSearchable": true,
                "bSortable": true,               
                "mRender": function (data, type, full) {
                    return dtFuel.replace('#val0', full.ImageUrl).replace('#val1', full.ClientLookupId).replace('#val2',  full.Name );
                }
               
            },
            { "data": "RecordDate", "autoWidth": true, "bSearchable": true, "bSortable": true },
            {
                "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                mRender: function (data, type, full, meta) {
                    return "<div class='text-wrap width-300'>" + data + "</div>";
                }
            },            
            { "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true },
            {
                "data": "Defects", "autoWidth": true, "bSearchable": true, "bSortable": true,
                mRender: function (data, type, full, meta) {
                    return "<div class='text-wrap width-100'>" + data + "</div>";
                }
            },
            { "data": "CompleteDate", "autoWidth": true, "bSearchable": true, "bSortable": true},
            { "data": "FleetIssuesId", "autoWidth": true, "bSearchable": false, "bSortable": false, className: 'text-center' }
        ],
        initComplete: function () {
            SetPageLengthMenu();
            $("#fleetissueGridAction :input").removeAttr("disabled");
            $("#fleetissueGridAction :button").removeClass("disabled");
            DisableExportButton($("#fleetissueSearch"), $(document).find('.import-export'));
        }
    });
}
$(document).on('click', '#fleetissueSearch_paginate .paginate_button', function () {
    run = true;
});
$(document).on('change', '#fleetissueSearch_length .searchdt-menu', function () {
    run = true;
});
function FleetIssueAddOnSuccess(data) {   
    CloseLoader();
    if (data.Result == "success") {
        if (data.Mode == "Add") {
            SuccessAlertSetting.text = getResourceValue("FleetIssueAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("FleetIssueUpdateAlert");
        }       
        swal(SuccessAlertSetting, function () {
            $("#fleetIssueModalpopup").modal('hide');
            window.location.href = '/FleetIssue/Index?page=Fleet_Issue';            
            });       
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}

function FleetIssueAdvSearch() {
    var InactiveFlag = false;
    $('#txtColumnSearch').val('');
    var searchitemhtml = "";
    selectCount = 0;
    $('#advsearchsidebar').find('.adv-item').each(function (index, item) {
        if ($(this).val() && $(this).val().length>0) {
            selectCount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
        }
      
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
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
    $('#advsearchsidebar').find('input:text').val('');
    $("#Defects").val("").trigger('change'); 
    $('.adv-item').val("");
    selectCount = 0;
    $("#spnControlCounter").text(selectCount);
    $('#liSelectCount').text(selectCount + ' filters applied');
    newEle = "";
    $('#dvFilterSearchSelect2').find('span').html('');
    $('#dvFilterSearchSelect2').find('span').removeClass('tagTo');
    DefectsFleetIssue = $("#Defects").val();  
    StartRecordDate = '';
    EndRecordDate = '';
}

$(document).on('change', '#dtgridadvsearchRecordDate', function () {
    var thisval = $(this).val();
    switch (thisval) {
        case '2':
            $('#RecordDatetimeperiodcontainer').hide();
            StartRecordDate = today;
            EndRecordDate = today;
            break;
        case '3':
            $('#RecordDatetimeperiodcontainer').hide();
            StartRecordDate = PreviousDateByDay(7);
            EndRecordDate = today;
            break;
        case '4':
            $('#RecordDatetimeperiodcontainer').hide();
            StartRecordDate = PreviousDateByDay(30);
            EndRecordDate = today;
            break;
        case '5':
            $('#RecordDatetimeperiodcontainer').hide();
            StartRecordDate = PreviousDateByDay(60);
            EndRecordDate = today;
            break;
        case '6':
            $('#RecordDatetimeperiodcontainer').hide();
            StartRecordDate = PreviousDateByDay(90);
            EndRecordDate = today;
            break;
        case '10':
            $('#RecordDatetimeperiodcontainer').show();
            StartRecordDate = today;
            EndRecordDate = today;
            $('#advrecordDatedaterange').val(StartRecordDate + ' - ' + EndRecordDate);
            $(document).find('#advrecordDatedaterange').daterangepicker(daterangepickersetting, function (start, end, label) {
                StartRecordDate = start.format('MM/DD/YYYY');
                EndRecordDate = end.format('MM/DD/YYYY');
            });
            break;
        default:
            $('#RecordDatetimeperiodcontainer').hide();
            $(document).find('#advrecordDatedaterange').daterangepicker({
                format: 'MM/DD/YYYY'
            });
            StartRecordDate = '';
            EndRecordDate = '';
            break;
    }
});
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
    if (searchtxtId == "Defects") {        
        DefectsFleetIssue = null;
    }  
    FleetIssueAdvSearch();
    dtTable.page('first').draw('page');
});
function SetFleetIssueControls() {
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
    $(document).find('.dtpicker').datepicker({
        changeMonth: true,
        changeYear: true,
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy');
    SetFixedHeadStyle();
}

//#region Dropdown toggle   
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
$(document).on('keyup', '#fleetissuesearctxtbox', function (e) {
    var tagElems = $(document).find('#fleetissuesearchListul').children();
    $(tagElems).hide();
    for (var i = 0; i < tagElems.length; i++) {
        var tag = $(tagElems).eq(i);
        if ($(tag).text().toLowerCase().includes($(this).val().toLowerCase()) == true || $(this).val().toLowerCase().includes($(tag).text().toLowerCase()) == true) {
            $(tag).show();
        }
    }
});
$(document).on('click', '.fleetissuesearchdrpbox', function (e) {
    run = true;
    if ($(document).find('#txtColumnSearch').val() !== '')
        $("#dvFilterSearchSelect2").html('');
    $(document).find('#txtColumnSearch').val('');
    run = true;
    $(".searchList li").removeClass("activeState");
    $(this).addClass('activeState');
    $(document).find('#searcharea').hide("slide");
    var optionval = $(this).attr('id');
    var val = localStorage.getItem("FLEETISSUESEARCHGRIDDISPLAYSTATUS");
    if (optionval == '3') {
        if (val == '3' || val == '4' || val == '5' || val == '6' || val == '7' || val == '8') {
            $('#cmbFIview').val(val).trigger('change');
        }
        else {
            $('#cmbFIview').val('').trigger('change');
        }
        fitempsearchtitle = $(this).text();
        $(document).find('#FIDateRangeModal').modal('show');
        return;
    }
    $('#fleetissuesearchtitle').text($(this).text());
    localStorage.setItem("FLEETISSUESEARCHGRIDDISPLAYSTATUS", optionval);
    if (optionval.length !== 0) {
        ShowbtnLoaderclass("LoaderDrop");
        dtTable.page('first').draw('page');
    }

});
$(document).on('click', "#SrchBttnNew", function () {
    $.ajax({
        url: '/Base/PopulateNewSearchList',
        type: 'GET',
        data: { tableName: 'FleetIssues' },
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
        url: '/Base/ModifyNewSearchList',
        type: 'POST',
        data: { tableName: 'FleetIssues', searchText: txtSearchval, isClear: isClear },
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
        generateFleetIssueDataTable();
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
    window.location.href = "../FleetIssue/Index?page=Fleet_Issue";
});

$(document).on('change', '#cmbFIview', function (e) {
    var thielement = $(this);
    if (thielement.val() == '8') {
        CreateStartDateVw = today;
        CreateEndDateVw = today;
        var strtlocal = localStorage.getItem('CreateStartDateVw');
        if (strtlocal) {
            CreateStartDateVw = strtlocal;
        }
        else {
            CreateStartDateVw = today;
        }
        var endlocal = localStorage.getItem('CreateEndDateVw');
        if (endlocal) {
            CreateEndDateVw = endlocal;
        }
        else {
            CreateEndDateVw = today;
        }
        $(document).find('#timeperiodcontainer').show();
        $(document).find('#FIdaterange').daterangepicker({
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
        localStorage.removeItem('CreateStartDateVw');
        localStorage.removeItem('CreateEndDateVw');
        $(document).find('#timeperiodcontainer').hide();
    }
});
$(document).on('click', '#btntimeperiod', function (e) {
    run = true;
    var daterangeval = $(document).find('#cmbFIview').val();
    if (daterangeval == '') {
        return;
    }
    CustomQueryDisplayId = daterangeval;
    if (daterangeval != '8') {
        CreateStartDateVw = '';
        CreateEndDateVw = '';
        localStorage.removeItem('CreateStartDateVw');
        localStorage.removeItem('CreateEndDateVw');
    }
    else {
        localStorage.setItem('CreateStartDateVw', CreateStartDateVw);
        localStorage.setItem('CreateEndDateVw', CreateEndDateVw);

    }
    $(document).find('#FIDateRangeModal').modal('hide');
    var text = fitempsearchtitle;
    if (daterangeval === '8')
        text = text + " - " + $('#FIdaterange').val();
    else
        text = text + " - " + $(document).find('#cmbFIview option[value=' + daterangeval + ']').text();
    $('#fleetissuesearchtitle').text(text);
    if (daterangeval.length !== 0) {
        $(".searchList li").removeClass("activeState");
        $(".searchList li").eq(2).addClass("activeState");     
        CustomQueryDisplayId = daterangeval;
        $(document).find('#searcharea').hide("slide");
        localStorage.setItem("FLEETISSUESEARCHGRIDDISPLAYSTATUS", CustomQueryDisplayId);
        ShowbtnLoaderclass("LoaderDrop");
        FleetIssueAdvSearch();
        dtTable.page('first').draw('page');

    }
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
            if (item.value  && item.value.length>0) {
                txtsearchelement.val(txtSearchval);
                searchitemhtml = "";
                searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + txtsearchelement.attr('id') + '">Search: ' + txtSearchval + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
            }
            return false;
        }
        else {
            if ($('#' + item.key).parent('div').is(":visible")) {               
                $('#' + item.key).val(item.value).trigger('change');
                if (item.value && item.value.length > 0) {
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
                            if (item.key === 'dtgridadvsearchRecordDate') {
                                StartRecordDate = datarangeval[0].value.split(' - ')[0];
                                EndRecordDate = datarangeval[0].value.split(' - ')[1];
                                $(document).find('#advrecordDatedaterange').daterangepicker(daterangepickersetting, function (start, end, label) {
                                    StartRecordDate = start.format('MM/DD/YYYY');
                                    EndRecordDate = end.format('MM/DD/YYYY');
                                });
                            }
                        }
                    }
                }
                else {
                    $('#' + item.key).val(item.value).trigger('change');
                }
            }
            advcountercontainer.text(selectCount);
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    searchstringcontainer.html(searchitemhtml);
}
//#endregion
//#region add,edit opr delete fleetfuel

$(document).on('click', '.AddIssue', function () {
    $.ajax({
        url: "/FleetIssue/FleetIssueAddOrEdit",
        type: "GET",
        dataType: 'html',
        data: { FleetIssuesId: 0 },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#FleetIssuePopup').html(data);
            $('#fleetIssueModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            SetFleetIssueControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', '.editFleetIssueBttn', function () {
    var FleetIssuesId = $(this).data("fiid");
    var DefectsList = $(this).data("defectsfi");
    $.ajax({
        url: "/FleetIssue/FleetIssueAddOrEdit",
        type: "GET",
        dataType: 'html',
        data: { FleetIssuesId: FleetIssuesId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            var DefectsArrayList = new Array();
            $('#FleetIssuePopup').html(data);
            $('#fleetIssueModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
            $(document).find("#FleetIssueModel_Pagetype").val("Edit");
            $(document).find("#divSearch").css('display', 'none');
            var separatedArray = DefectsList.split(',');
            for (var i = 0; i < separatedArray.length; i++) {
                DefectsArrayList[i] = separatedArray[i];
            }
            $('#FleetIssueModel_DefectsIds').val(DefectsArrayList).trigger("change.select2");
        },
        complete: function () {
            SetFleetIssueControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', '.delFleetIssueBttn', function () {
    var fleetIssuesId = $(this).data("fiid");    
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: "/FleetIssue/DeleteFleetIssue",
            type: "GET",
            data: { fleetIssuesId: fleetIssuesId },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                var message = "";
                if (data == "success") {
                    SuccessAlertSetting.text = getResourceValue("FleetIssueDeleteAlert");
                    swal(SuccessAlertSetting, function () {
                        if (data == "success") {
                            dtTable.destroy();
                            generateFleetIssueDataTable();
                        }
                    });
                }
                else if (data=="ServiceOrderExist")
                {
                    message = getResourceValue("SOFailDelFleetIssueAlert");
                    ShowErrorAlert(message);
                }
                    
                else {
                    message = getResourceValue("FailDelFleetIssueAlert");
                    ShowErrorAlert(message);
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

$(document).on('click', "#btnFleetIssuecancel", function () {
    swal(CancelAlertSetting, function () {
        $("#fleetIssueModalpopup").modal('hide');
        $(document).find('.errormessage').css("display", "none");
        $('body').removeClass('modal-open');
    });
   
});


$(document).on('click', ".clearerrdiv", function () {
    $(document).find('.errormessage').css("display", "none");
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
$(document).on("hidden.bs.modal", "#FleetIssueModal", function (e) {
    $("body").addClass("modal-open");
});
//#endregion
