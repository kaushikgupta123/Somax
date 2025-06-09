var dtTable;
var selectCount = 0;
var selectedcount = 0;
var totalcount = 0;
var searchcount = 0;
var run = false;
var titleText = '';
var dtSecheduleServ = '<div class="symbol"><img src="#val0" /></div><div class="dtlsBox"><h2>#val1</h2><p>#val2</p></div>';
var colstr = '<div class="dtlsBox"><h2>#val1</h2><p>#val2</p></div>';
var dtLastCompleted = '<div class="dtlsBox"><h2>#val1</h2><p>#val2</p><p></p></div>';
//Search Retention
var gridname = "ScheduledService_Search";
var order = 0;
var orderDir = 'asc';
function filterinfo(id, value) {
    this.key = id;
    this.value = value;
}
$.validator.setDefaults({ ignore: null });
$(document).ready(function () {
    $(".hiddenInitial").hide();
    ShowbtnLoaderclass("LoaderDrop");
    $(document).find('.select2picker').select2({});
    $("#scServGridAction :input").attr("disabled", "disabled");
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
    generateScheduledServiceDataTable();
    $(".actionDrop2 ul li a").click(function () {
        $(".actionDrop2").fadeOut();
    });
    $("#btnScheduledServiceDataAdvSrch").on('click', function (e) {
        run = true;
        $(document).find('#txtColumnSearch').val('');
        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();       
        ScheduledServiceAdvSearch();
        dtTable.page('first').draw('page');
    }); 
    var fleetscheduledstatus = localStorage.getItem("FLEETSCHEDULEDSEARCHGRIDDISPLAYSTATUS");
    if (fleetscheduledstatus) {
        activeStatus = fleetscheduledstatus;
        $('#fleetschduledsearchListul li').each(function

            (index, value) {
            if ($(this).attr('id') == activeStatus && $(this).attr('id') != '0') {
                $('#fleetscheduledsearchtitle').text($(this).text());
                $(".searchList li").removeClass("activeState");
                $(this).addClass('activeState');
            }
        });
    }
    else {
        localStorage.setItem("FLEETSCHEDULEDSEARCHGRIDDISPLAYSTATUS", "1");
        fleetscheduledstatus = localStorage.getItem("FLEETSCHEDULEDSEARCHGRIDDISPLAYSTATUS");
        activeStatus = fleetscheduledstatus;
        $('#fleetschduledsearchListul li').first().addClass('activeState');
        $('#fleetscheduledsearchtitle').text(getResourceValue("AlertActive"));
    }

});
$(document).on('click', '#liPdf', function () {   
    var params = {       
        customQueryDisplayId : localStorage.getItem("FLEETSCHEDULEDSEARCHGRIDDISPLAYSTATUS"),
        colname: order,
        coldir: orderDir,
        ClientLookupId: LRTrim($("#EquipmentID").val()),
        Name: LRTrim($("#Name").val()),
        ServiceTaskDesc: LRTrim($("#ServiceTasksDesc").val()),
        SearchText: LRTrim($(document).find('#txtColumnSearch').val())
    };
    fleetScheduledPrintParams = JSON.stringify({ 'fleetScheduledPrintParams': params });
    $.ajax({
        "url": "/FleetScheduledService/SetPrintData",
        "data": fleetScheduledPrintParams,
        contentType: 'application/json; charset=utf-8',
        "dataType": "json",
        "type": "POST",
        "success": function () {
            window.open('/FleetScheduledService/PrintASPDF', '_self');
            return;
        }
    });
    funcCloseExportbtn();
});
$(document).on('click', '#liPrint', function () {    
    var params = {
        customQueryDisplayId:localStorage.getItem("FLEETSCHEDULEDSEARCHGRIDDISPLAYSTATUS"),
        colname: order,
        coldir: orderDir,
        ClientLookupId : LRTrim($("#EquipmentID").val()),
        Name: LRTrim($("#Name").val()),
        ServiceTaskDesc: LRTrim($("#ServiceTasksDesc").val()),    
        SearchText:LRTrim($(document).find('#txtColumnSearch').val())

    };
    fleetScheduledPrintParams = JSON.stringify({ 'fleetScheduledPrintParams': params });
    $.ajax({
        "url": "/FleetScheduledService/SetPrintData",
        "data": fleetScheduledPrintParams,
        contentType: 'application/json; charset=utf-8',
        "dataType": "json",
        "type": "POST",
        "success": function () {
            window.open('/FleetScheduledService/ExportASPDF', '_blank');
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

  function generateScheduledServiceDataTable() {
    var printCounter = 0;
      var IsScheduledServiceAccessSecurity = false;
  var LocalizedFuelUnit="";
      if ($(document).find('#scheduleServiceSearch').hasClass('dataTable')) {
        dtTable.destroy();
    }
      dtTable = $("#scheduleServiceSearch").DataTable({
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
                    data.order[0][0] = order;
                    data.order[0][1] = orderDir;
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
                        var LayoutInfo = JSON.parse(json.LayoutInfo);
                        order = LayoutInfo.order[0][0];
                        orderDir = LayoutInfo.order[0][1];
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
                extend: 'print',
                title: 'Fleet Scheduled List'
            },
            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Fleet Scheduled List',
                extension: '.csv'
            },
            {
                extend: 'excelHtml5',
                title: 'Fleet Scheduled List'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                exportOptions: {
                    columns: ':visible'
                },
                css: 'display:none',
                title: 'Fleet Scheduled List',
                orientation: 'landscape',
                pageSize: 'A3'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/FleetScheduledService/GetScheduledServicetGridData",
            "type": "post",
            "datatype": "json",
            cache: false,
            data: function (d) {     
                d.customQueryDisplayId = localStorage.getItem("FLEETSCHEDULEDSEARCHGRIDDISPLAYSTATUS");
                d.ClientLookupId = LRTrim($("#EquipmentID").val());
                d.Name = LRTrim($("#Name").val());
                d.ServiceTaskDesc = LRTrim($("#ServiceTasksDesc").val());         
                d.SearchText = LRTrim($(document).find('#txtColumnSearch').val());
                d.Order = order;
            },
            "dataSrc": function (result) {
                searchcount = result.recordsTotal;
                let colOrder = dtTable.order();
                orderDir = colOrder[0][1];
                IsScheduledServiceAccessSecurity = result.IsScheduledServiceAccessSecurity;
                if (result.data.length < 1) {
                    $(document).find('#btnScServExport').prop('disabled', true);
                }
                else {
                    $(document).find('#btnScServExport').prop('disabled', false);
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
                targets: [5], render: function (a, b, data, d) {
                    if (data.InactiveFlag==false) {
                        var statusnow = true;
                        if (IsScheduledServiceAccessSecurity) {
                            return '<a class="btn btn-outline-primary ActivateInactivateFleetScheduleService gridinnerbutton" data-eqid="' + data.EquipmentId + '" data-scserveid="' + data.ScheduledServiceId + '" data-clntid="' + data.ClientLookupId + '" data-statusnow="' + statusnow + '"  title="Inactivate"><i class="fa fa-toggle-on"></i></a>' +
                                '<a class="btn btn-outline-success editScheduledServiceBttn gridinnerbutton" data-eqid="' + data.EquipmentId + '" data-clntid="' + data.ClientLookupId + '" data-scserveid="' + data.ScheduledServiceId + '" title= "Edit"> <i class="fa fa-pencil"></i></a>';
                        }                        
                    }
                    else {
                        var statusnow2 = false;
                        if (IsScheduledServiceAccessSecurity) {
                            return '<a class="btn btn-outline-primary ActivateInactivateFleetScheduleService gridinnerbutton" data-eqid="' + data.EquipmentId + '" data-scserveid="' + data.ScheduledServiceId + '" data-clntid="' + data.ClientLookupId + '" data-statusnow="' + statusnow2 + '" title="Activate"><i class="fa fa-toggle-off"></i></a>' +
                                '<a class="btn btn-outline-success editScheduledServiceBttn gridinnerbutton" data-eqid="' + data.EquipmentId + '" data-clntid="' + data.ClientLookupId + '" data-scserveid="' + data.ScheduledServiceId + '" title= "Edit"> <i class="fa fa-pencil"></i></a>';
                        }
                        
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
                className: 'text-left',               
                "mRender": function (data, type, full) {
                    return dtSecheduleServ.replace('#val0', full.ImageUrl).replace('#val1', full.ClientLookupId).replace('#val2', full.Name);
                }
             
            },
            
            { "data": "ServiceTask", "autoWidth": true, "bSearchable": true, "bSortable": true,className: 'text-left' },       
            {
                    "data": "TimeInterval",
                    "autoWidth": true,
                "bSearchable": true,
                "bSortable": true,
                    className: 'text-left',
                    "mRender": function (data, type, full) {
                        var Schedule = "";
                        if (full.TimeInterval > 0 || full.Meter1Interval > 0 || full.Meter2Interval > 0) {
                            Schedule = 'Every ';
                            if (full.TimeInterval > 0) {
                                Schedule += full.TimeInterval + ' ' + full.TimeIntervalType + ' or ';
                            }
                            if (full.Meter1Interval > 0) {
                                Schedule += full.Meter1Interval + ' ' + full.Meter1Units + ' or ';
                            }
                            if (full.Meter2Interval > 0) {
                                Schedule += full.Meter2Interval + ' ' + full.Meter2Units + ' or ';
                            }
                            Schedule = Schedule.slice(0, -1).split(" ").slice(0, -1).join(" ");
                           
                        }
                                            
                        return Schedule;
                    }
                },
                {
                    "data": "NextDueDate",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": true,
                    className: 'text-left',
                    "mRender": function (data, type, full) {
                        var NextDue = "";
                        if (full.TimeInterval > 0) {
                            NextDue = '<img src="/Images/calenderIcon.jpg"/> ' + full.NextDueDate + ' from now </br>';
                        }

                        var meterNextDue = "";
                        if (full.Meter1Interval > 0 || full.Meter2Interval > 0) {
                            meterNextDue = '<img src="/Images/angleIcon.jpg"/> ';
                            if (full.Meter1Interval > 0) {
                                meterNextDue += full.NextDueMeter1 + ' ' + full.Meter1Units.substring(0, 2) + ' from now or ';
                            }
                            if (full.Meter2Interval > 0) {
                                meterNextDue += full.NextDueMeter2 + ' ' + full.Meter2Units.substring(0, 2) + ' from now ';
                            }
                            else {
                                meterNextDue = meterNextDue.slice(0, -1).split(" ").slice(0, -1).join(" ");
                            }
                        }
                         
                        NextDue = NextDue + meterNextDue;
                       
                        return NextDue;
                    }
                },

                {
                    "data": "LastPerformedDate",
                    "autoWidth": false,
                    "bSearchable": true,
                    "bSortable": true,
                    className: 'text-left',
                    "mRender": function (data, type, full) {
                        if (full.LastPerformedDate) {
                            var ResourceLastCompleted = getResourceValue("spnLastCompletedOn") + ' ';
                            var LastCompletedLine1 = ResourceLastCompleted + ' ' + full.LastPerformedDate;
                            var LastCompletedLine2 = full.LastPerformedMeter1 + ' ' + full.Meter1Units + ' | ' + full.LastPerformedMeter2 + ' ' + full.Meter2Units;
                            return dtLastCompleted.replace('#val1', LastCompletedLine1).replace('#val2', LastCompletedLine2);
                        }
                        else {
                            var ResourceNeverPerformedForThisAsset = getResourceValue("spnNeverPerformedForThisAsset");
                            return dtLastCompleted.replace('#val1', ResourceNeverPerformedForThisAsset).replace('#val2', '');
                        }
                       
                    }

                },
                { "data": "ScheduledServiceId", "autoWidth": true, "bSearchable": false, "bSortable": false, className: 'text-center' }
        ],      
        initComplete: function () {           
            SetPageLengthMenu();          
            $("#scServGridAction :input").removeAttr("disabled");
            $("#scServGridAction :button").removeClass("disabled");
            
            DisableExportButton($("#scheduleServiceSearch"), $(document).find('.import-export'));
        }
    });
}
$(document).on('click', '#scheduleServiceSearch_paginate .paginate_button', function () {
    run = true;
});
$(document).on('change', '#scheduleServiceSearch_length .searchdt-menu', function () {
    run = true;
});
$('#scheduleServiceSearch').find('th').click(function () {
    run = true;
    order = $(this).data('col');
});

function ScheduledServiceAdvSearch() {
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
    $('#advsearchsidebar').find("select").val("").trigger('change.select2');
    $('.adv-item').val("");
    selectCount = 0;
    $("#spnControlCounter").text(selectCount);
    $('#liSelectCount').text(selectCount + ' filters applied');
    newEle = "";
    $('#dvFilterSearchSelect2').find('span').html('');
    $('#dvFilterSearchSelect2').find('span').removeClass('tagTo');  
    StartReadingDate = '';
    EndReadingDate = '';
}
$(document).on('change', '#dtgridadvsearchReadingDate', function () {
    var thisval = $(this).val();
    switch (thisval) {
        case '2':
            $('#ReadingDatetimeperiodcontainer').hide();
            StartReadingDate = today;
            EndReadingDate = today;
            break;
        case '3':
            $('#ReadingDatetimeperiodcontainer').hide();
            StartReadingDate = PreviousDateByDay(7);
            EndReadingDate = today;
            break;
        case '4':
            $('#ReadingDatetimeperiodcontainer').hide();
            StartReadingDate = PreviousDateByDay(30);
            EndReadingDate = today;
            break;
        case '5':
            $('#ReadingDatetimeperiodcontainer').hide();
            StartReadingDate = PreviousDateByDay(60);
            EndReadingDate = today;
            break;
        case '6':
            $('#ReadingDatetimeperiodcontainer').hide();
            StartReadingDate = PreviousDateByDay(90);
            EndReadingDate = today;
            break;
        case '10':
            $('#ReadingDatetimeperiodcontainer').show();
            StartReadingDate = today;
            EndReadingDate = today;
            $('#advreadingDatedaterange').val(StartReadingDate + ' - ' + EndReadingDate);
            $(document).find('#advreadingDatedaterange').daterangepicker(daterangepickersetting, function (start, end, label) {
                StartReadingDate = start.format('MM/DD/YYYY');
                EndReadingDate = end.format('MM/DD/YYYY');
            });
            break;
        default:
            $('#ReadingDatetimeperiodcontainer').hide();
            $(document).find('#advreadingDatedaterange').daterangepicker({
                format: 'MM/DD/YYYY'
            });
            StartReadingDate = '';
            EndReadingDate = '';
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
   
    ScheduledServiceAdvSearch();
    dtTable.page('first').draw('page');
});
function SetFleetFuelDetailEnvironment() {
    CloseLoader();
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
                console.log("Successfully uploaded :" + imgName);
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

    function SetSchduledServeControls() {
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
//#region ColumnVisibility
$(document).on('click', '#liCustomize', function () {
    funCustomizeBtnClick(dtTable);
});
$(document).on('click', '.saveConfig', function () {
    var colOrder = [0];
    funCustozeSaveBtn(dtTable, colOrder);
    run = true;
    dtTable.state.save(run);
});
//#endregion

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

//#region New Search button  scservesearctxtboxs
    $(document).on('keyup', '#scservesearctxtbox', function (e) {
        //var tagElems = $(document).find('#fleetfuelsearchListul').children();
        var tagElems = $(document).find('#scservesearchListul').children();
        $(tagElems).hide();
        for (var i = 0; i < tagElems.length; i++) {
            var tag = $(tagElems).eq(i);
            if ($(tag).text().toLowerCase().includes($(this).val().toLowerCase()) == true || $(this).val().toLowerCase().includes($(tag).text().toLowerCase()) == true) {
                $(tag).show();
            }
        }
    });
    $(document).on('click', "#SrchBttnNew", function () {
        $.ajax({
            url: '/Base/PopulateNewSearchList',
            type: 'GET',
            data: { tableName: 'ScheduledService' },
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
            data: { tableName: 'ScheduledService', searchText: txtSearchval, isClear: isClear },
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
            generateScheduledServiceDataTable();
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
        window.location.href = "../FleetScheduledService/Index?page=Scheduled_Service";
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

    //For add or edit fleetScheduled
    $(document).on('click', '#AddNewScServ', function () {
        $.ajax({
            url: "/FleetScheduledService/ScheduledServiceAddOrEdit",
            type: "GET",
            dataType: 'html',
            data: { EquipmentId: 0, SchedServiceId: 0 },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                $('#ScheduleServePopup').html(data);
                $("#ScheduledServiceModel_TimeIntervalType").val("Days").trigger('change');
                $("#ScheduledServiceModel_TimeThresoldType").val("Days").trigger('change');

                $('#scServModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
            },
            complete: function () {
                SetSchduledServeControls();
            },
            error: function () {
                CloseLoader();
            }
        });
    });
$(document).on('click', '.editScheduledServiceBttn', function () {
   
        $(document).find('.fleeterrormessage').css('display', 'none');
        var equipmentId = $(this).data("eqid");
        var SchedServiceId = $(this).data("scserveid");
        $.ajax({
            url: "/FleetScheduledService/ScheduledServiceAddOrEdit",
            type: "GET",
            dataType: 'html',
            data: { EquipmentId: equipmentId, SchedServiceId: SchedServiceId },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                $('#ScheduleServePopup').html(data);
                var Meter1Type = "Odometer Interval";
                var Meter2Type = "Hour Interval";
                var Meter1Unit = "Miles";
                var Meter2Unit = "Hours";
                var Meter1Threshold = "Odometer Threshold";
                var Meter2Threshold = "Hour Threshold";
                if ($("#hidMeter1Type").val()) {
                    Meter1Type = $("#hidMeter1Type").val() + ' Interval';
                    Meter1Threshold = $("#hidMeter1Type").val() + ' Threshold';
                    $(".hiddenInitialMeter1").show();
                    $("#spnMeter1Interval").text(Meter1Type);
                    $("#spnMeter1Threshold").text(Meter1Threshold);
                }
                if ($("#hidMeter2Type").val()) {
                    Meter2Type = $("#hidMeter2Type").val() + ' Interval';
                    Meter2Threshold = $("#hidMeter2Type").val() + ' Threshold';
                    $(".hiddenInitialMeter2").show();
                    $("#spnMeter2Interval").text(Meter2Type);
                    $("#spnMeter2Threshold").text(Meter2Threshold);
                } 
                if ($("#hidMeter1Units").val()) {
                    Meter1Unit = $("#hidMeter1Units").val();
                    $("#spnMeter1IntervalType").text(Meter1Unit);
                    $("#spnMeter1ThresholdType").text(Meter1Unit);
                }
                if ($("#hidMeter2Units").val()) {
                    Meter2Unit = $("#hidMeter2Units").val();
                    $("#spnMeter2IntervalType").text(Meter2Unit);
                    $("#spnMeter2ThresholdType").text(Meter2Unit);
                }
                $(".hiddenInitialDueSoonSettings").show();
                $(".hiddenInitialServiceTask").show();
                $(".hiddenRepairReason").show();
                $(".hiddenSystem").show();
                $(".hiddenAssembly").show();
                $(".hiddenInitialTimeInterval").show();
                $(".hiddenInitialbtnSave").show();
                $(".hiddenInitialTimeThreshold").show();
                $('#scServModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
                $(document).find("#btnScheduleServiceRecord").removeAttr('disabled');
                $(document).find("#ScheduledServiceModel_ScheduledServiceId").val(SchedServiceId);
                $(document).find("#ScheduledServiceModel_Pagetype").val("Edit");
                $(document).find("#divSearch").css('display', 'none');
                $("#ScheduledServiceModel_ServiceTaskId").attr("disabled", "disabled"); 

            },
            complete: function () {
                SetSchduledServeControls();
            },
            error: function () {
                CloseLoader();
            }
        });
    });

      function FleetScheduledServiceAddOnSuccess(data) {
        CloseLoader();
        if (data.Result == "success") {
            if (data.Mode == "Add") {
                SuccessAlertSetting.text = getResourceValue("FleetScheduledServiceAddAlert");
            }
            else {
                SuccessAlertSetting.text = getResourceValue("FleetScheduledServiceUpdateAlert");
            }
                $("#scServModalpopup").modal('hide');
                swal(SuccessAlertSetting, function () {
                    titleText = getResourceValue("AlertActive");
                    window.location.href = '/FleetScheduledService/Index?page=Fleet_Scheduled';
                });
        }
        else {
            ShowGenericErrorOnAddUpdate(data);
            $(document).find(".Chckerror").css("display", "block");
        }
    }
$(document).on('change', '#ScheduledServiceModel_System', function () {
    var system = $(this).val();
    var id = 'ScheduledServiceModel_Assembly';
    $(document).find('#' + id).val('').trigger('change');
    PopulateHierarchicalData('VMRS_Code', system, id);
});
function PopulateHierarchicalData(listname, level1Value, targetid) {
    $.ajax({
        url: '/FleetScheduledService/GetHierarchicalList',
        type: 'POST',
        data: {
            ListName: listname,
            List1Value: level1Value
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            var elem = $(document).find('#' + targetid);
            elem.html('');
            if (data.Result === "success") {
                elem.append('<option value="">--Select--</option>');
                $.each(data.data, function (i, item) {
                    elem.append('<option value="' + item.Value + '">' + item.Text + '</option>');
                });
            }
        },
        error: function () {
            CloseLoader();
        },
        complete: function () {
            $(document).find('.select2picker').select2({});
            CloseLoader();
        },
    });
}
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
$(document).on('keyup', '#fleetschduledsearctxtbox', function (e) {
    var tagElems = $(document).find('#fleetschduledsearchListul').children();
    $(tagElems).hide();
    for (var i = 0; i < tagElems.length; i++) {
        var tag = $(tagElems).eq(i);
        if ($(tag).text().toLowerCase().includes($(this).val().toLowerCase()) == true || $(this).val().toLowerCase().includes($(tag).text().toLowerCase()) == true) {
            $(tag).show();
        }
    }
});
$(document).on('click', '.fleetschduledsearchdrpbox', function (e) {
    run = true;
    if ($(document).find('#txtColumnSearch').val() !== '')
        $("#dvFilterSearchSelect2").html('');
    $(document).find('#txtColumnSearch').val('');
    run = true;
    if ($(this).attr('id') != '0') {
        $('#fleetscheduledsearchtitle').text($(this).text());
    }
    else {
        $('#fleetscheduledsearchtitle').text("ScheduleService");
    }
    $(".searchList li").removeClass("activeState");
    $(this).addClass('activeState');
    $(document).find('#searcharea').hide("slide");
    var optionval = $(this).attr('id');
    localStorage.setItem("FLEETSCHEDULEDSEARCHGRIDDISPLAYSTATUS", optionval);
    if (optionval.length !== 0) {
        ShowbtnLoaderclass("LoaderDrop");
        dtTable.page('first').draw('page');
    }
});
$(document).on('click', "#SrchBttnNew", function () {
    $.ajax({
        url: '/Base/PopulateNewSearchList',
        type: 'GET',
        data: { tableName: 'ScheduledService' },
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
    window.location.href = "../FleetScheduled/Index?page=Fleet_Scheduled";
});
//#endregion

//#region Inactive / Active ScheduledService
$(document).on('click', '.ActivateInactivateFleetScheduleService', function () {
    var _scServeid = $(this).data('scserveid');
    var inactiveFlag =false;
    var curstatus = localStorage.getItem("FLEETSCHEDULEDSEARCHGRIDDISPLAYSTATUS");
    if (curstatus == 1) {
        inactiveFlag = false;
    }
    if (curstatus == 2) {
        inactiveFlag = true;
    }
    var row = $(this).parents('tr');
    var data = dtTable.row(row).data();
    var activeInactive = data.InactiveFlag;
    var salerttext = "";
    var falerttext = "";
    var Act_Inact_alt = "";
    if (activeInactive == false) {
        salerttext = getResourceValue("FleetScheduledInActiveSuccessAlert");
        falerttext = getResourceValue("ScheduledServiceInActiveFailedAlert");
        Act_Inact_alt = getResourceValue("ScheduledServiceInActivateAlert");
    }
    else {
        salerttext = getResourceValue("FleetScheduledActiveSuccessAlert");
        falerttext = getResourceValue("ScheduledServiceActiveFailedAlert");
        Act_Inact_alt = getResourceValue("ScheduledServiceActivateAlert");
    }
    CancelAlertSettingForCallback.text = Act_Inact_alt;
    swal(CancelAlertSettingForCallback, function () {
        $.ajax({
            url: "/FleetScheduledService/UpdateFleetScheduledStatus",
            type: "POST",
            dataType: "json",
            data: {
                _scServeid: _scServeid,
                inactiveFlag: inactiveFlag
            },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.result == 'success') {
                    SuccessAlertSetting.text = salerttext;
                    swal(SuccessAlertSetting, function () {
                        dtTable.destroy();
                        generateScheduledServiceDataTable();
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
$(document).on('click', "#btnScheduleServicecancel", function () {
    swal(CancelAlertSetting, function () {
        $("#scServModalpopup").modal('hide');
        resetValidation();
        $(document).find('.errormessage').css("display", "none");
        $(document).find('div.ui-tooltip').css("display", "none");
    });
});

$(document).on('click', "#btnFleetScheduledServiceClose", function () {
    $("#scServModalpopup").modal('hide');
    resetValidation();
});

//#region Reset Validation
function resetValidation() {
    $(document).find("#ScheduledServiceModel_ClientLookupId").val("").trigger('change');
    $(document).find("#ScheduledServiceModel_ServiceTaskId").val("").trigger('change');
    var areaChargeType = $(document).find("#ScheduledServiceModel_ClientLookupId").attr('aria-describedby');
    $('#' + areaChargeType).hide();
    var areaQuantity = $(document).find("#ScheduledServiceModel_ServiceTaskId").attr('aria-describedby');
    $('#' + areaQuantity).hide();
    $(document).find('form').find("#ScheduledServiceModel_ClientLookupId").removeClass("input-validation-error");
    $(document).find('form').find("#ScheduledServiceModel_ServiceTaskId").removeClass("input-validation-error");
    $(document).find('form').find("#ScheduledServiceModel_RepairReason").removeClass("input-validation-error");
    $(document).find('form').find("#ScheduledServiceModel_System").removeClass("input-validation-error");
    $(document).find('form').find("#ScheduledServiceModel_Assembly").removeClass("input-validation-error");
}
$(document).on("hidden.bs.modal", "#ScheduleServiceModal", function (e) {
    $("body").addClass("modal-open");
});
//#endregion