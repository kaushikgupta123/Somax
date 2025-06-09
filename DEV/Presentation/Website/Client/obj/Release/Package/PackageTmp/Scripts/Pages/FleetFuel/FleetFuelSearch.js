var dtTable;
var selectCount = 0;
var selectedcount = 0;
var totalcount = 0;
var searchcount = 0;
var run = false;
var titleText = '';
var StartReadingDate = '';
var EndReadingDate = '';
var today = $.datepicker.formatDate('mm/dd/yy', new Date());
var dtFuel = '<div class="symbol"><img src="#val0" /></div><div class="dtlsBox"><h2>#val1</h2><p>#val2</p></div>';
var colstr = '<div class="dtlsBox"><h2>#val1</h2><p>#val2</p></div>';
//Search Retention
var gridname = "FleetFuel_Search";
var orderbycol = 0;
var orderDir = 'asc';
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
    $("#fleetfuelGridAction :input").attr("disabled", "disabled");
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
    generateFleetFuelDataTable();
    $("#btnupdateequip").click(function () {
        $(".actionDrop2").slideToggle();
    });
    $(".actionDrop2 ul li a").click(function () {
        $(".actionDrop2").fadeOut();
    });
    $("#btnupdateequip").focusout(function () {
        $(".actionDrop2").fadeOut();
    });
    $("#btnFleetFuelDataAdvSrch").on('click', function (e) {
        run = true;
        $(document).find('#txtColumnSearch').val('');
        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();       
        FleetFuelAdvSearch();
        dtTable.page('first').draw('page');
    });  
   
});
$(document).on('click', '#liPdf', function () {   
    var params = {       
        colname: orderbycol,
        coldir: orderDir,
        ClientLookupId: LRTrim($("#EquipmentID").val()),
        Name: LRTrim($("#Name").val()),
        Make: LRTrim($("#Make").val()),
        Model: LRTrim($("#ModelNumber").val()),
        VIN: LRTrim($("#VIN").val()),
        StartReadingDate: ValidateDate(StartReadingDate),
        EndReadingDate: ValidateDate(EndReadingDate),
        SearchText: LRTrim($(document).find('#txtColumnSearch').val())
    };
    fleetFuelPrintParams = JSON.stringify({ 'fleetFuelPrintParams': params });
    $.ajax({
        "url": "/FleetFuel/SetPrintData",
        "data": fleetFuelPrintParams,
        contentType: 'application/json; charset=utf-8',
        "dataType": "json",
        "type": "POST",
        "success": function () {
            window.open('/FleetFuel/PrintASPDF', '_self');
            return;
        }
    });
    funcCloseExportbtn();
});

$(document).on('click', '#liPrint', function () {    
    var params = {
        colname: orderbycol,
        coldir: orderDir,
       ClientLookupId : LRTrim($("#EquipmentID").val()),
      Name: LRTrim($("#Name").val()),
      Make:LRTrim($("#Make").val()),
       Model:LRTrim($("#ModelNumber").val()),
        VIN:LRTrim($("#VIN").val()),
       StartReadingDate: ValidateDate(StartReadingDate),
       EndReadingDate:ValidateDate(EndReadingDate),          
       SearchText:LRTrim($(document).find('#txtColumnSearch').val())

    };
    fleetFuelPrintParams = JSON.stringify({ 'fleetFuelPrintParams': params });
    $.ajax({
        "url": "/FleetFuel/SetPrintData",
        "data": fleetFuelPrintParams,
        contentType: 'application/json; charset=utf-8',
        "dataType": "json",
        "type": "POST",
        "success": function () {
            window.open('/FleetFuel/ExportASPDF', '_blank');
            return;
        }
    });
    funcCloseExportbtn();
});
function generateFleetFuelDataTable() {
    var printCounter = 0;
    var IsFleetFuelAccessSecurity = false;
  var LocalizedFuelUnit="";
    if ($(document).find('#fleetfuelSearch').hasClass('dataTable')) {
        dtTable.destroy();
    }
    dtTable = $("#fleetfuelSearch").DataTable({
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
                extend: 'print',
                title: 'Fleet Fuel List'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                exportOptions: {
                    columns: ':visible'
                },
                css: 'display:none',
                title: 'Fleet Fuel List',
                orientation: 'landscape',
                pageSize: 'A3'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/FleetFuel/GetFleetFuelGridData",
            "type": "post",
            "datatype": "json",
            cache: false,
            data: function (d) {               
                d.ClientLookupId = LRTrim($("#EquipmentID").val());
                d.Name = LRTrim($("#Name").val());
                d.Make = LRTrim($("#Make").val());
                d.Model = LRTrim($("#ModelNumber").val());
                d.VIN = LRTrim($("#VIN").val());
                d.StartReadingDate = ValidateDate(StartReadingDate);
                d.EndReadingDate = ValidateDate(EndReadingDate);             
                d.SearchText = LRTrim($(document).find('#txtColumnSearch').val());
            },
            "dataSrc": function (result) {
                let colOrder = dtTable.order();
                orderbycol = colOrder[0][0];
                orderDir = colOrder[0][1];
                searchcount = result.recordsTotal;
                IsFleetFuelAccessSecurity = result.IsFleetFuelAccessSecurity;
                if (result.data.length < 1) {
                    $(document).find('#btnFleetFuelExport').prop('disabled', true);
                }
                else {
                    $(document).find('#btnFleetFuelExport').prop('disabled', false);
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
                    if (data.Void == false)
                    {
                        if (IsFleetFuelAccessSecurity)
                            {
                            return '<a class="btn btn-outline-primary voidFleetFuelBttn gridinnerbutton" data-eqid="' + data.EquipmentId + '" data-fmid="' + data.FleetMeterReadingId + '" data-clntid="' + data.ClientLookupId + '" title="Void"><i class="fa fa-toggle-on"></i></a>' +
                                '<a class="btn btn-outline-success editFleetFuelBttn gridinnerbutton" data-eqid="' + data.EquipmentId + '" data-clntid="' + data.ClientLookupId + '" data-FFid="' + data.FuelTrackingId + '" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                                '<a class="btn btn-outline-danger delFleetFuelBttn gridinnerbutton" data-ffid="' + data.FuelTrackingId + '" data-void="' + data.Void + '" data-reading="' + data.Reading + '" title="Delete"> <i class="fa fa-trash"></i></a>';
                            }                      
                        else
                                {
                            return '<a class="btn btn-outline-primary voidFleetFuelBttn gridinnerbutton" data-eqid="' + data.EquipmentId + '" data-fmid="' + data.FleetMeterReadingId + '" data-clntid="' + data.ClientLookupId + '" title="Void"><i class="fa fa-toggle-on"></i></a>';

                                }
                     }
                    else
                    {
                        if (IsFleetFuelAccessSecurity)
                        {
                            return '<a class="btn btn-outline-primary UnvoidFleetFuelBttn gridinnerbutton" data-eqid="' + data.EquipmentId + '" data-fmid="' + data.FleetMeterReadingId + '" data-clntid="' + data.ClientLookupId + '" title="UnVoid"><i class="fa fa-toggle-off"></i></a>' +
                                '<a class="btn btn-outline-success editFleetFuelBttn gridinnerbutton" data-eqid="' + data.EquipmentId + '" data-clntid="' + data.ClientLookupId + '" data-FFid="' + data.FuelTrackingId + '"  title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                                '<a class="btn btn-outline-danger delFleetFuelBttn gridinnerbutton" data-ffid="' + data.FuelTrackingId + '" data-void="' + data.Void + '" data-reading="' + data.Reading + '" title="Delete"> <i class="fa fa-trash"></i></a>';
                        }                      
                        else {
                            return '<a class="btn btn-outline-primary UnvoidFleetFuelBttn gridinnerbutton" data-eqid="' + data.EquipmentId + '" data-fmid="' + data.FleetMeterReadingId + '" data-clntid="' + data.ClientLookupId + '" title="UnVoid"><i class="fa fa-toggle-off"></i></a>';
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
                    return dtFuel.replace('#val0', full.ImageUrl).replace('#val1', full.ClientLookupId).replace('#val2', full.Name);
                }
             
            },
                { "data": "ReadingDate", "autoWidth": true, "bSearchable": true, "bSortable": true, className: 'text-center'},
            {
                "data": "Amount", "autoWidth": true, "bSearchable": true, "bSortable": true,className: 'text-right',
                mRender: function (data, type, full, meta) {
                    if (full.FuelUnits == "Liters")
                    {
                        LocalizedFuelUnit = getResourceValue("LitersAlert");
                    }
                    else if (full.FuelUnits == "Units")
                    {
                        LocalizedFuelUnit = getResourceValue("UnitAlert");
                    }
                    else
                    {
                        LocalizedFuelUnit = full.FuelUnits;
                    }
                    return colstr.replace('#val1', full.FuelAmount).replace('#val2', LocalizedFuelUnit);
                }
            },
            { "data": "UnitCost", "autoWidth": true, "bSearchable": true, "bSortable": true,className: 'text-right' },
            {"data": "TotalCost", "autoWidth": true, "bSearchable": true, "bSortable": true, className: 'text-right'},          
           { "data": "FuelTrackingId", "autoWidth": true, "bSearchable": false, "bSortable": false, className: 'text-center' }

        ],      
        initComplete: function () {           
            SetPageLengthMenu();
            //  v2-557 
            $("#fleetfuelGridAction :input").removeAttr("disabled");
            $("#fleetfuelGridAction :button").removeClass("disabled");
            DisableExportButton($("#fleetfuelSearch"), $(document).find('.import-export'));
        }
    });
}
$(document).on('click', '#fleetfuelSearch_paginate .paginate_button', function () {
    run = true;
});
$(document).on('change', '#fleetfuelSearch_length .searchdt-menu', function () {
    run = true;
});

//V2-557 


function FleetFuelAdvSearch() {
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
   
    FleetFuelAdvSearch();
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

function SetFleetFuelControls() {
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

//#region New Search button
$(document).on('keyup', '#fleetfuelsearctxtbox', function (e) {
    var tagElems = $(document).find('#fleetfuelsearchListul').children();
    $(tagElems).hide();
    for (var i = 0; i < tagElems.length; i++) {
        var tag = $(tagElems).eq(i);
        if ($(tag).text().toLowerCase().includes($(this).val().toLowerCase()) == true || $(this).val().toLowerCase().includes($(tag).text().toLowerCase()) == true) {
            $(tag).show();
        }
    }
});
$(document).on('click', '.fleetfuelsearchdrpbox', function (e) {
    run = true;
    if ($(document).find('#txtColumnSearch').val() !== '')
        $("#dvFilterSearchSelect2").html('');
    $(document).find('#txtColumnSearch').val('');
    run = true;
    if ($(this).attr('id') != '0') {
        $('#fleetfuelsearchtitle').text($(this).text());
    }
    else {
        $('#fleetfuelsearchtitle').text("Asset");
    }
    $(".searchList li").removeClass("activeState");
    $(this).addClass('activeState');
    $(document).find('#searcharea').hide("slide");
    ShowbtnLoaderclass("LoaderDrop");
    dtTable.page('first').draw('page');
});
$(document).on('click', "#SrchBttnNew", function () {
    $.ajax({
        url: '/Base/PopulateNewSearchList',
        type: 'GET',
        data: { tableName: 'FleetFuel' },
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
        data: { tableName: 'FleetFuel', searchText: txtSearchval, isClear: isClear },
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
        generateFleetFuelDataTable();
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
    window.location.href = "../FleetFuel/Index?page=Fleet_Fuel";
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

//For add or edit fleetfuel
$(document).on('click', '.AddFuel', function () {
    $(document).find('.fleeterrormessage').css('display', 'none');
    $.ajax({
        url: "/FleetFuel/FleetFuelAddOrEdit",
        type: "GET",
        dataType: 'html',
        data: { EquipmentId: 0, FuelTrackingId: 0},
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#FleetFuelPopup').html(data);
            $('#fleetFuelModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });  
        },
        complete: function () {
            SetFleetFuelControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', '.editFleetFuelBttn', function () {
    $(document).find('.fleeterrormessage').css('display', 'none');
    var equipmentId = $(this).data("eqid");
    var fuelTrackingId = $(this).data("ffid");
    $.ajax({
        url: "/FleetFuel/FleetFuelAddOrEdit",
        type: "GET",
        dataType: 'html',
        data: { EquipmentId: equipmentId, FuelTrackingId: fuelTrackingId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#FleetFuelPopup').html(data);
            $('#fleetFuelModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });  
            $(document).find("#FleetFuelModel_MtrCurrentReadingDate").removeAttr('disabled');
            $(document).find("#FleetFuelModel_StartTimeValue").removeAttr('disabled');
            $(document).find("#FleetFuelModel_Void").removeAttr('disabled');
            $(document).find("#FleetFuelModel_UnitCost").removeAttr('disabled');
            $(document).find("#FleetFuelModel_FuelType").removeAttr('disabled');
            $(document).find("#FleetFuelModel_FuelAmount").removeAttr('disabled');
            $(document).find("#FleetFuelModel_Reading").removeAttr('disabled');
            $(document).find("#btnFleetFuelRecord").removeAttr('disabled');
            $(document).find("#FleetFuelModel_FuelTrackingId").val(fuelTrackingId);
            $(document).find("#FleetFuelModel_Pagetype").val("Edit");
            $(document).find("#divSearch").css('display', 'none');

           $(document).find(".spnFuelUnit label").text($(document).find("#FleetFuelModel_FuelUnit").val());
            //validation set for meter reading value
            var getdiff = GetDateDiffEdit($(document).find("#FleetFuelModel_Mtr1CurrentReadingDate").val());
            var MUnit = $(document).find("#FleetFuelModel_Meter1Units").val();
            if (MUnit != "") {
                MUnit = MUnit.substring(0, 2);
            }
            var chkerr = getResourceValue("LastReadingAlert")+": " + $(document).find("#FleetFuelModel_Meter1CurrentReading").val() + " " + MUnit+" (" + getdiff + ")";
            $(document).find(".Chckerror").text(chkerr);
            $(document).find(".Chckerror").css("display", "none");

        },
        complete: function () {
            SetFleetFuelControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', '.delFleetFuelBttn', function () {
    $(document).find('.fleeterrormessage').css('display', 'none');
    var fuelTrackingId = $(this).data("ffid");
    var isVoid = $(this).data("void");
    var Reading = $(this).data("reading");
    swal(CancelAlertSettingForCallback, function () {    
        if (isVoid == true) {
        //V2 391 Comment  If FleetMeterReading.Reading >0 then the fuel tracking record should not be deleted.
        if (Reading == "0" || Reading == "0.0") {
            swal(CancelAlertSetting, function () {
                $.ajax({
                    url: "/FleetFuel/DeleteFuelTracking",
                    type: "GET",
                    data: { fuelTrackingId: fuelTrackingId },
                    beforeSend: function () {
                        ShowLoader();
                    },
                    success: function (data) {
                        var message = "";
                        if (data == "success") {
                            SuccessAlertSetting.text = getResourceValue("FuelTrackingDeleteAlert");                           
                            swal(SuccessAlertSetting, function () {
                                if (data == "success") {
                                    dtTable.destroy();
                                    generateFleetFuelDataTable();
                                }
                            });
                        }
                        else {
                            message = getResourceValue("FuelTrackingDeleteFailedAlert");
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
        }
        else {
            message = getResourceValue("FuelTrackingDeleteFailedReadingAlert");
            ShowErrorAlert(message);
        }    
    }
    else {
        message = getResourceValue("FuelTrackingDeleteFailedUnvoidAlert");
        ShowErrorAlert(message);
    }
    });
});

$(document).on('click', ".UnvoidFleetFuelBttn", function () {
    $(document).find('.fleeterrormessage').css('display', 'none');
    var fleetmeterreadingId = $(this).data("fmid");
    var eqid = $(this).data("eqid");
    $.ajax({
        url: "/FleetFuel/ValidateFleetFuelForUnvoid",
        type: "POST",
        dataType: "json",
        data: { _eqid: eqid, mtrreadingid: fleetmeterreadingId, Meter2Indicator: false },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data == "success") {     
                    SuccessAlertSetting.text = getResourceValue("FleetMeterUnVoidAlert");             
                    swal(SuccessAlertSetting, function () {
                        if (data == "success") {
                            dtTable.destroy();
                            generateFleetFuelDataTable();
                        }
                    });
                }         
                else {                  
                    ShowFleetUnvoidError(data);
            }
           
        },
        complete: function () {
            CloseLoader();
        },
        error: function (jqxhr) {
            CloseLoader();
        }
    });

});

$(document).on('click', ".voidFleetFuelBttn", function () {
    $(document).find('.fleeterrormessage').css('display', 'none');
    var fleetmeterreadingId = $(this).data("fmid");
    var eqid = $(this).data("eqid");
    var clientlookupid = $(this).data("clntid");
    $.ajax({
        url: "/FleetFuel/UpdateFleetFuelForvoid",
        type: "POST",
        dataType: "json",
        data: { _eqid: eqid, mtrreadingid: fleetmeterreadingId, FF_ClientLookupId: clientlookupid},
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.validationStatus == true) {
                SuccessAlertSetting.text = getResourceValue("FleetMeterVoidAlert");
                swal(SuccessAlertSetting, function () {
                    if (data.validationStatus == true) {
                        dtTable.destroy();
                        generateFleetFuelDataTable();
                    }
                });
            }
            else {
                GenericSweetAlertMethod(data);
            }

        },
        complete: function () {
            CloseLoader();
        },
        error: function (jqxhr) {
            CloseLoader();
        }
    });

});

function FleetFuelAddOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        if (data.Mode == "Add")
        {
            SuccessAlertSetting.text = getResourceValue("FuelTrackingAddAlert");
        }
        else
        {
            SuccessAlertSetting.text = getResourceValue("FuelTrackingUpdateAlert");
        }
       
        if (data.Command == "save") {
            swal(SuccessAlertSetting, function () {
                titleText = getResourceValue("AlertActive");
                window.location.href = '/FleetFuel/Index?page=Fleet_Fuel';
            });
        }
        else {
            ResetErrorDiv();
            $(document).find('#equipmentabdidtab').addClass('active').trigger('click');
            swal(SuccessAlertSetting, function () {
                $(document).find('form').trigger("reset");
                $(document).find('form').find("select").not("#colorselector").val("").trigger('change.select2');
                $(document).find('form').find("select").removeClass("input-validation-error");
                $(document).find('form').find("input").removeClass("input-validation-error");
                $(document).find('form').find("textarea").removeClass("input-validation-error");
            });
        }
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
        $(document).find(".Chckerror").css("display", "block");
        $(document).find("#FleetFuelModel_Reading").addClass("input-validation-error");
    }
}
function checkformvalid() {
    $("#fleetFuelform").submit(function () {
        if ($(this).valid()) {
            return;
        }
    });
}

$(document).on('click', "#btnFleetFuelcancel", function () {
    swal(CancelAlertSetting, function () {
        $("#fleetFuelModalpopup").modal('hide');
        $(document).find('.errormessage').css("display", "none");
    });
});
function GetDateDiffEdit(fromdate) {
    var today = new Date();
    var Fdate = new Date(fromdate);
    var diffMs = (today - Fdate); // milliseconds between now & Christmas
    var diffDays = Math.floor(diffMs / 86400000); // days
    var diffHrs = Math.floor((diffMs % 86400000) / 3600000); // hours
    var diffMins = Math.round(((diffMs % 86400000) % 3600000) / 60000); // minutes  
    if (parseInt(diffDays) > 0) {
        return diffDays + "  " + getResourceValue("DaysAgoAlert");
    }
    else {
        diffDays = 0;
        return diffDays + "  " + getResourceValue("DaysAgoAlert");
    }
    //else if (parseInt(diffHrs) > 0) {
    //    return diffHrs + "  " + getResourceValue("HoursAgoAlert");
    //}
    //else {
    //    return diffMins + "  " + getResourceValue("MinutesAgoAlert");
    //}   
}

$(document).on('click', ".clearerrdiv", function () {
    $(document).find('.errormessage').css("display", "none");
});
function ShowFleetUnvoidError(errorObject, thisformId) {
    var errorMessageContainer;
    var errorString = "";
    errorMessageContainer = $(document).find('.fleeterrormessage');
    if (typeof errorObject !== "string") {
        $.each(errorObject, function (index, item) {
            errorString = errorString + '<div class="m-alert m-alert--icon m-alert--icon-solid m-alert--outline alert alert-danger alert-dismissible fade show" role="alert"><div class="m-alert__icon"><i class="flaticon-danger"></i></div>' +
                '<div class="m-alert__text">' + item + '</div><div class="m-alert__close">' +
                '<button type="button" class="close" data-dismiss="alert" aria-label="Close"></button></div></div>';
        });
    }
    else {
        errorString = errorString + '<div class="m-alert m-alert--icon m-alert--icon-solid m-alert--outline alert alert-danger alert-dismissible fade show" role="alert"><div class="m-alert__icon"><i class="flaticon-danger"></i></div>' +
            '<div class="m-alert__text">' + errorObject + '</div><div class="m-alert__close">' +
            '<button type="button" class="close" data-dismiss="alert" aria-label="Close"></button></div></div>';
    }
    errorMessageContainer.html(errorString).show();
    window.scrollTo(0, 0);
}