//#region Common
var run = false;
var totalcount = 0;
var CustomQueryDisplayId = 1;

var selectCount = 0;
var gridname = "MultiStoreroomPart_Search";
function filterinfo(id, value) {
    this.key = id;
    this.value = value;
}
var dtPStoreroomsTable;
function RedirectToMultiStoreroomPartDetail(PartId, mode) {
    var titletext = $('#partsearchtitle').text();
    $.ajax({
        url: "/MultiStoreroomPart/MultiStoreroomPartDetails",
        type: "POST",
        data: { PartId: PartId },
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#multistoreroompartcontainer').html(data);
            var partinactiveflag = $('#parthiddeninactiveflag').val();
            if (mode == "FindPart") {
                if (partinactiveflag == "False") {
                    localStorage.setItem("CURRENTTABSTATUS", '1');
                    localStorage.setItem("partstatustext", getResourceValue("AlertActive"));
                }
                else {
                    localStorage.setItem("CURRENTTABSTATUS", '2');
                    localStorage.setItem("partstatustext", getResourceValue("AlertInactive"));
                }
            }
            $(document).find('#spnlinkToSearch').text(localStorage.getItem("partstatustext"));
            SetFixedHeadStyle();
        },
        complete: function () {
            LoadComments(PartId);
            LoadImages(PartId);
            generateStoreRoomdataTable(PartId);
            ZoomImage($(document).find('#EquipZoom'));
            CloseLoader();
            Dropzone.autoDiscover = false;
            if ($(document).find('#dropzoneForm').length > 0) {
                var myDropzone = new Dropzone("div#dropzoneForm", {
                    url: "../Base/SaveUploadedFile",
                    addRemoveLinks: true,
                    paramName: 'file',
                    maxFilesize: 10, // MB
                    maxFiles: 4,
                    dictDefaultMessage: getResourceValue("FileUploadAlert"),
                    acceptedFiles: ".jpeg,.jpg,.png",
                    init: function () {
                        this.on("removedfile", function (file) {
                            if (file.type != 'image/jpeg' && file.type != 'image/jpg' && file.type != 'image/png') {
                                ShowErrorAlert(getResourceValue("spnValidImage"));
                            }
                        });
                    },
                    addRemoveLinks: true,
                    autoProcessQueue: true,
                    dictRemoveFile: "X",
                    uploadMultiple: true,
                    dictRemoveFileConfirmation: getResourceValue("CancelAlertSure"),
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
                SetFixedHeadStyle();
            }
            if (mode === "AzureImageReload" || mode === "OnPremiseImageReload") {
                $('#overviewcontainer').hide();
                $('#PartsContainer').hide();
                $('.tabcontent2').hide();
                $('#auditlogcontainer').hide();
                $('.imageDropZone').show();
                $(document).find('#btnnblock').removeClass("col-xl-6");
                $(document).find('#btnnblock').addClass("col-xl-12");
                $(document).find('#partst').removeClass("active");
                $(document).find('#photot').addClass("active");
            }
            if (mode === "attachment") {
                $('#attachmentst').trigger('click');
                $('#colorselector').val('MSPAttachments');
            }
            if (mode === "vendor") {
                $('#vendorst').trigger('click');
                $('#colorselector').val('MSPVendors');
            }
            if (mode === "equipment") {
                $('#equipmentt').trigger('click');
                $('#colorselector').val('MSPEquipment');
            }
            if (mode === "history") {
                $('#historyt').trigger('click');
                $('#colorselector').val('History');
            }
            if (mode === "receipt") {
                $('#receiptt').trigger('click');
                $('#colorselector').val('Receiptscontainer');
            }

            //InitWebCam();
            $.validator.setDefaults({ ignore: null });
            $.validator.unobtrusive.parse(document);
        },
        error: function () {
            CloseLoader();
        }
    });
}
//#endregion

$(document).ready(function () {

    $(document).on('click', "#action", function () {
        $(".actionDrop").slideToggle();
    });
    $(".actionDrop ul li a").click(function () {
        $(".actionDrop").fadeOut();
    });
    $(document).on('focusout', "#action", function () {
        $(".actionDrop").fadeOut();
    });
    $(".actionBar").fadeIn();
    $("#MultiStoreroomPartGridAction :input").attr("disabled", "disabled");

    if ($('.select2picker').length > 0) {
        $(document).find('.select2picker').select2({});
    }
    $('.select2picker, form').change(function () {
        var areaddescribedby = $(this).attr('aria-describedby');
        var validity = false;
        if ($(this).closest('form').length > 0) {
            validity = $(this).valid();
        }
        if (validity == true) {
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
    $(".sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $(document).on('click', '.dismiss, .overlay', function () {
        $(document).find('.sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });
    $(document).on('click', '#sidebarCollapse', function () {
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
    //#region Load Grid With Status
    var partcurrentstatus = localStorage.getItem("CURRENTTABSTATUS");
    if (partcurrentstatus != 'undefined' && partcurrentstatus != null && partcurrentstatus != "") {
        CustomQueryDisplayId = partcurrentstatus;
        $('#mspsearchListul li').each(function (index, value) {
            if ($(this).attr('id') == CustomQueryDisplayId && $(this).attr('id') != '0') {
                $('#partsearchtitle').text($(this).text());
                $(".searchList li").removeClass("activeState");
                $(this).addClass('activeState');
            }
        });
    }
    else {
        CustomQueryDisplayId = "1";
        $('#partsearchtitle').text(getResourceValue("AlertActive"));
        $("#mspsearchListul li").first().addClass("activeState");
    }
    //#endregion
    generateMSPDataTable();
    //V2-1007
    var IsPartFromEquipment = $(document).find('#IsPartFromEquipment').val();
    var PartId = $(document).find('#PartId').val();
    if (IsPartFromEquipment != "" && IsPartFromEquipment != undefined && IsPartFromEquipment != 'False') {
        LoadComments(PartId);
        LoadImages(PartId);
        generateStoreRoomdataTable(PartId);
        var parthiddeninactiveflag = $(document).find('#parthiddeninactiveflag').val();
        if (parthiddeninactiveflag == "False") {
            $(document).find('#spnlinkToSearch').text(getResourceValue("AlertActive"));
            localStorage.setItem("CURRENTTABSTATUS", '1');
        }
        else {
            $(document).find('#spnlinkToSearch').text(getResourceValue("AlertInactive"));
            localStorage.setItem("CURRENTTABSTATUS", '2');
        }
        SetPartDetailEnvironment();
    }

    //V2-1147
    var IsDetailFromNotification = $('#IsDetailFromNotification').val();
    if (IsDetailFromNotification == 'True') {
        LoadComments(PartId);
        LoadImages(PartId);
        generateStoreRoomdataTable(PartId);
        $(document).find('#spnlinkToSearch').text(getResourceValue("AlertActive"));
        localStorage.setItem("CURRENTTABSTATUS", '1');
        SetPartDetailEnvironment();
    }
});
//V2-1147
function SetPartDetailEnvironment() {
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
//#region Search
var dtMSPTable;

$("#btnMSPDataAdvSrch").on('click', function (e) {
    run = true;
    searchresult = [];
    MSPAdvSrch();
    $('.sidebar').removeClass('active');
    $('.overlay').fadeOut();
    dtMSPTable.page('first').draw('page');
});
$(document).on('click', '.btnCross', function () {
    run = true;
    var btnCrossed = $(this).parent().attr('id');
    var searchtxtId = btnCrossed.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    selectCount--;
    MSPAdvSrch();
    dtMSPTable.page('first').draw('page');
});
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
    $("#multistorepartSearch thead tr th").map(function (key) {
        var thisdiv = $(this).find('div');
        if ($(this).parents('.multiStoreroomPartinnerDataTable').length == 0 && thisdiv.html()) {
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
        CustomQueryDisplayId: CustomQueryDisplayId,
        PartId: LRTrim($("#PartId").val()),
        Description: LRTrim($("#Description").val()),
        StockType: LRTrim($("#StockType").val()),
        txtsearchval: LRTrim($("#txtColumnSearch").val()),
        Storerooms: $("#Storeroom").val()
    };
    mspPrintParams = JSON.stringify({ 'mspPrintParams': params });
    $.ajax({
        "url": "/MultiStoreroomPart/SetPrintData",
        "data": mspPrintParams,
        contentType: 'application/json; charset=utf-8',
        "dataType": "json",
        "type": "POST",
        "success": function () {
            if (thisid == 'liPdf') {
                window.open('/MultiStoreroomPart/ExportASPDF?d=PDF', '_self');
            }
            else if (thisid == 'liPrint') {
                window.open('/MultiStoreroomPart/ExportASPDF', '_blank');
            }

            return;
        }
    });
    $('#mask').trigger('click');
});

function MSPAdvSrch() {
    var InactiveFlag = false;
    var searchitemhtml = "";
    $(document).find('#txtColumnSearch').val('');
    selectCount = 0;
    $('#advsearchsidebar').find('.adv-item').each(function (index, item) {
        if ($(this).hasClass('dtpicker')) {
            $(this).val(ValidateDate($(this).val()));
        }
        if ($(this).attr('id') == 'advcreatedaterange' || $(this).attr('id') == 'advprocessdedbydaterange') {
            if ($(this).val()) {
                searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
            }
        }
        else if ($(this).attr('id') == "Storeroom") {
            if ($(this).val() != "" && $(this).val() != "0") {
                selectCount++;
                searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + ' <a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
            }
        }
        else {
            if ($(this).val()) {
                selectCount++;
                searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
            }
        }

    });

    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $("#advsearchfilteritems").html(searchitemhtml);

    $(".filteritemcount").text(selectCount);
}
function clearAdvanceSearch() {
    selectCount = 0;
    $("#PartId").val("");
    $("#Description").val("");
    $("#StockType").val("").trigger('change');
    $("#Storeroom").val("");

    $("#advsearchfilteritems").html('');
    $(".filteritemcount").text(selectCount);

}
var titleArray = [];
var classNameArray = [];
var order = '1';
var orderDir = 'asc';
function generateMSPDataTable() {
    var printCounter = 0;
    if ($(document).find('#multistorepartSearch').hasClass('dataTable')) {
        dtMSPTable.destroy();
    }
    dtMSPTable = $("#multistorepartSearch").DataTable({
        colReorder: {
            fixedColumnsLeft: 2
        },
        rowGrouping: true,
        searching: true,
        serverSide: true,
        //"pagingType": "full_numbers",
        "pagingType": "input",
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
        //fixedColumns: {
        //    leftColumns: 3,
        //},
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'Multi Storeroom Part List'
            },
            {
                extend: 'print',
                title: 'Multi Storeroom Part List'
            },
            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Multi Storeroom Part List',
                extension: '.csv',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                orientation: 'landscape',
                pageSize: 'A3',
                title: 'Multi Storeroom Part List'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/MultiStoreroomPart/GetMultiPartStoreroomMainGrid",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.CustomQueryDisplayId = CustomQueryDisplayId;
                d.PartID = LRTrim($("#PartId").val());
                d.Description = LRTrim($("#Description").val());
                d.StockType = LRTrim($("#StockType").val());
                d.SearchText = LRTrim($(document).find('#txtColumnSearch').val());
                d.Order = order;
                //d.orderDir = orderDir;
                d.Storerooms = $('#Storeroom').val();
            },
            "dataSrc": function (result) {
                let colOrder = dtMSPTable.order();
                orderDir = colOrder[0][1];

                //var i = 0;
                //SetMultiSelectAction(CustomQueryDisplayId);
                totalcount = result.recordsTotal;

                //HidebtnLoader("btnsortmenu");
                HidebtnLoaderclass("LoaderDrop");
                if (result.data.length == "0") {
                    $(document).find('.import-export').attr('disabled', 'disabled');
                }
                else {
                    $(document).find('.import-export').removeAttr('disabled');
                }

                return result.data;
            },
            global: true
        },
        "columns":
            [
                {
                    "data": "PartId",
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
                    "name": "1",//
                    "mRender": function (data, type, row) {
                        return '<a class=lnk_psearch href="javascript:void(0)">' + data + '</a>';
                    }
                },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2",

                },
                {
                    "data": "DefStoreroom", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3",

                },
                { "data": "StockType", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "4" }, //--previous name : 5
                { "data": "AppliedCost", "autoWidth": true, "bSearchable": true, "bSortable": true, "type": "date ", "name": "5", "className": "text-right" },

            ],
        columnDefs: [
            {
                targets: [0, 1],
                className: 'noVis'
            }
        ],
        initComplete: function (settings, json) {
            SetPageLengthMenu();
            //----------conditional column hiding-------------//
            //var api = new $.fn.dataTable.Api(settings);
            //var columns = dtMSPTable.settings().init().columns;
            //var arr = [];
            //var j = 0;
            //while (j < json.hiddenColumnList.length) {
            //    var clsname = '.' + json.hiddenColumnList[j];
            //    var title = dtMSPTable.columns(clsname).header();
            //    titleArray.push(title[0].innerHTML);
            //    classNameArray.push(clsname);
            //    dtMSPTable.columns(clsname).visible(false);
            //    var sortMenuItem = '.dropdown-menu' + ' ' + clsname;
            //    $(sortMenuItem).remove();

            //    //---hide adv search items---
            //    var advclsname = '.' + "prc-" + json.hiddenColumnList[j];
            //    $(document).find(advclsname).hide();
            //    j++;
            //}
            //----------------------------------------------//

            $("#MultiStoreroomPartGridAction :input").removeAttr("disabled");
            $("#MultiStoreroomPartGridAction :button").removeClass("disabled");
            DisableExportButton($("#multistorepartSearch"), $(document).find('.import-export'));
            CloseLoader();
        }
    });
}
$('#multistorepartSearch').find('th').click(function () {
    if ($(this).data('col')) {
        run = true;
        order = $(this).data('col');
    }

});
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
        else if (item.key == 'Storeroom' && item.value) {
            $("#Storeroom").val(item.value).trigger("change.select2");
            if (item.value) {
                if (item.value != "") {
                    selectCount++;
                    searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + item.key + '">' + $("label[for='" + item.key + "']").text().replace(':', '') + ' <a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
                }
            }
        }
        else if ($('#' + item.key).parent('div').is(":visible")) {
            $('#' + item.key).val(item.value).trigger('change');
            if (item.value) {
                selectCount++;
                searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + item.key + '">' + $("label[for='" + item.key + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
            }
        }
        advcountercontainer.text(selectCount);
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    searchstringcontainer.html(searchitemhtml);
}

$(document).find('#multistorepartSearch').on('click', 'tbody td img', function (e) {
    var tr = $(this).closest('tr');
    var row = dtMSPTable.row(tr);
    if (this.src.match('details_close')) {
        this.src = "../../Images/details_open.png";
        row.child.hide();
        tr.removeClass('shown');
    }
    else {
        this.src = "../../Images/details_close.png";
        var PartID = $(this).attr("rel");
        $.ajax({
            url: "/MultiStoreroomPart/GetMSPInnerGrid",
            data: {
                PartID: PartID
            },
            beforeSend: function () {
                ShowLoader();
            },
            dataType: 'html',
            success: function (json) {
                row.child(json).show();
                dtinnerGrid = row.child().find('.multiStoreroomPartinnerDataTable').DataTable(
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
                            { className: 'text-right', targets: [1, 2, 3] },
                            {
                                "render": function (data, type, row) {
                                    return "<div class='text-wrap'>" + data + "</div>";
                                }
                                , targets: [0]
                            }
                        ],
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
$(document).on('click', '#multistorepartSearch_paginate .paginate_button', function () {
    //MSPAdvSrch();
    run = true;
});
$(document).on('change', '#multistorepartSearch_length .searchdt-menu', function () {
    //MSPAdvSrch();
    run = true;
});

$(function () {
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            if (this.context[0].sInstance == "multistorepartSearch") {
                var PartID = LRTrim($("#PartId").val());
                var Description = LRTrim($("#Description").val());
                var StockType = $("#StockType").val();
                var colname = order;
                var coldir = orderDir;
                var txtsearchval = LRTrim($("#txtColumnSearch").val());
                var Storerooms = $('#Storeroom').val().join();

                var jsonResult = $.ajax({
                    url: '/MultiStoreroomPart/GetMultiPartStoreroomPrintData?page=all',
                    "type": "GET",
                    "datatype": "json",
                    data: {
                        CustomQueryDisplayId: CustomQueryDisplayId,
                        SearchText: txtsearchval,
                        PartID: PartID,
                        Description: Description,
                        StockType: StockType,
                        Order: colname,
                        coldir: coldir,
                        Storerooms: Storerooms
                    },
                    success: function (result) {
                    },
                    async: false
                });
                var thisdata = JSON.parse(jsonResult.responseText).data;
                var visiblecolumnsIndex = $("#multistorepartSearch thead tr th").map(function (key) {
                    return this.getAttribute('data-th-index');
                }).get();
                var d = [];
                $.each(thisdata, function (index, item) {
                    if (item.PartID != null) {
                        item.PartID = item.PartID;
                    }
                    else {
                        item.PartID = "";
                    }
                    if (item.Description != null) {
                        item.Description = item.Description;
                    }
                    else {
                        item.Description = "";
                    }
                    if (item.DefStoreroom != null) {
                        item.DefStoreroom = item.DefStoreroom;
                    }
                    else {
                        item.DefStoreroom = "";
                    }
                    if (item.StockType != null) {
                        item.StockType = item.StockType;
                    }
                    else {
                        item.StockType = "";
                    }
                    if (item.AppliedCost != null) {
                        item.AppliedCost = item.AppliedCost;
                    }
                    else {
                        item.AppliedCost = "";
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
                    header: $("#multistorepartSearch thead tr th").find('div').map(function (key) {
                        if ($(this).parents('.multiStoreroomPartinnerDataTable').length == 0 && this.innerHTML) {
                            return this.innerHTML;
                        }
                    }).get()
                };
            }
            if (this.context[0].sInstance == "MSPhistoryTable") {
                var daterange = $('#MSPHistoryModel_HistoryDaterange').val();
                var partid = $(document).find('#PartId').val();
                var partclientlookup = $(document).find('#partclientlookupid').text();
                var TransactionType = LRTrim($("#mspgridadvsearchTransactionType").val());
                var Requestor_Name = LRTrim($("#mspgridadvsearchRequestorName").val());
                var PerformBy_Name = LRTrim($("#mspgridadvsearchPerformByName").val());
                var TransactionDate = LRTrim($("#mspgridadvsearchTransactionDate").val());
                var TransactionQuantity = LRTrim($('#mspgridadvsearchTransactionQuantity').val());
                var Cost = $('#mspgridadvsearchCost').val();
                var ChargeType_Primary = LRTrim($("#mspgridadvsearchChargeTypePrimary").val());
                var ChargeTo_ClientLookupId = LRTrim($("#mspgridadvsearchChargeToName").val());
                var Account_ClientLookupId = LRTrim($("#mspgridadvsearchAccountId").val());
                var PurchaseOrder_ClientLookupId = LRTrim($("#mspgridadvsearchPurchaseOrderClientLookupId").val());
                var Vendor_ClientLookupId = LRTrim($('#mspgridadvsearchVendorClientLookupId').val());
                var Vendor_Name = $('#mspgridadvsearchVendorName').val();

                var jsonResult = $.ajax({
                    url: '/MultiStoreroomPart/GetMSPHistoryPrintData?page=all',
                    data: {
                        partid: partid,
                        daterange: daterange,
                        TransactionType: TransactionType,
                        Requestor_Name: Requestor_Name,
                        PerformBy_Name: PerformBy_Name,
                        TransactionDate: TransactionDate,
                        TransactionQuantity: TransactionQuantity,
                        Cost: Cost,
                        ChargeType_Primary: ChargeType_Primary,
                        ChargeTo_ClientLookupId: ChargeTo_ClientLookupId,
                        Account_ClientLookupId: Account_ClientLookupId,
                        PurchaseOrder_ClientLookupId: PurchaseOrder_ClientLookupId,
                        Vendor_ClientLookupId: Vendor_ClientLookupId,
                        Vendor_Name: Vendor_Name
                    },
                    success: function (result) {

                    },
                    async: false
                });
                var thisdata = JSON.parse(jsonResult.responseText).data;
                return {
                    body: thisdata.map(
                        function (el) {
                            return Object.keys(el).map(function (key) {
                                return el[key];
                            })
                        }),
                    header: $("#MSPhistoryTable thead tr th").map(function (key) {
                        return this.innerHTML;
                    }).get()
                };
            }
            if (this.context[0].sInstance == "mspreceiptsTable") {
                var partid = $(document).find('#PartId').val();
                var daterange = $(document).find('#MSPReceiptModel_receiptdtselector').val();
                var partclientlookup = $(document).find('#partclientlookupid').text();
                var POClientLookupId = LRTrim($("#mspPurchaseOrder").val());
                var PurchaseOrderId = LRTrim($("#mspPurchaseOrder").val());
                var ReceivedDate = LRTrim($("#mspReceiptDate").val());
                var VendorClientLookupId = LRTrim($("#rgridReceiptadvsearchVendorClientLookupId").val());
                var VendorName = LRTrim($("#mspVendorName").val());
                var OrderQuantity = LRTrim($("#mspQuantity").val());
                var UnitCost = LRTrim($('#mspUnitCost').val());

                var jsonResult = $.ajax({
                    url: '/MultiStoreroomPart/GetMSPartsReceiptPrintData?page=all',
                    data: {
                        partId: partid,
                        dateRange: daterange,
                        POClientLookupId: PurchaseOrderId,
                        ReceivedDate: ReceivedDate,
                        VendorClientLookupId: VendorClientLookupId,
                        VendorName: VendorName,
                        OrderQuantity: OrderQuantity,
                        UnitCost: UnitCost,
                        ChargeType_Primary: ChargeType_Primary,
                        ChargeTo_ClientLookupId: ChargeTo_ClientLookupId,
                        Account_ClientLookupId: Account_ClientLookupId,
                        PurchaseOrder_ClientLookupId: PurchaseOrder_ClientLookupId,
                        Vendor_ClientLookupId: Vendor_ClientLookupId,
                        Vendor_Name: Vendor_Name
                    },
                    success: function (result) {

                    },
                    async: false
                });
                var thisdata = JSON.parse(jsonResult.responseText).data;
                return {
                    body: thisdata.map(
                        function (el) {
                            return Object.keys(el).map(function (key) {
                                return el[key];
                            })
                        }),
                    header: $("#mspreceiptsTable thead tr th").map(function (key) {
                        return this.innerHTML;
                    }).get()
                };
            }
        }
    });
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
//#endregion

//#region Side-Menu functions
function openSideMenuTab(evt, tabName) {
    evt.preventDefault();
    switch (tabName) {
        case "Photos":
            LoadImages($(document).find('#PartId').val());
            break;
        case "MSPAttachments":
            GenerateMSPAttachmentGrid();
            break;
        case "MSPVendors":
            GenerateMSPVendorGrid();
            break;
        case "MSPEquipment":
            GenerateMSPEquipmentGrid();
            break;
        case "History":
            $('#HistoryDaterange').val('2').trigger('change');
            $(document).find(".sidebar").mCustomScrollbar({
                theme: "minimal"
            });
            GenerateMSPHistoryGrid();
            break;
        case "Receiptscontainer":
            $('#receiptdtselector').val('2').trigger('change');
            $(document).find(".sidebar").mCustomScrollbar({
                theme: "minimal"
            });
            generateMSPReceiptDataTable();
            break;
    };
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }

    if (typeof evt.currentTarget !== "undefined") {
        evt.currentTarget.className += " active";
    }
    else {
        evt.target.className += " active";
    }
}
$(document).on('click', "ul.vtabs li", function () {
    $("ul.vtabs li").removeClass("active");
    $(this).addClass("active");
    $(".tabsArea").hide();
    var activeTab = $(this).find("a").attr("href");
    $(activeTab).fadeIn();
    return false;
});

//#region Photos tab
function clearDropzone() {
    deleteServer = false;
    if ($(document).find('#dropzoneForm').length > 0) {
        Dropzone.forElement("div#dropzoneForm").destroy();
    }
}
$(document).on('click', '.setImage', function () {
    var PartId = $('#PartId').val();
    var imageName = $(this).data('image');
    SaveUploadedFileToServer(PartId, imageName);
});
function SaveUploadedFileToServer(partId, imageName) {
    $.ajax({
        url: '../Base/SaveUploadedFileToServer',
        type: 'POST',
        data: { 'fileName': imageName, objectId: partId, TableName: "Part", AttachObjectName: "Part" },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.result == "0") {
                $('.partImage').attr('src', data.imageurl);
                $(document).find('#AzureImage').append('<a href="javascript:void(0)" id="deleteImg" class="trashIcon" title="Delete"><i class="fa fa-trash"></i></a>');
                $('#EquipZoom').data('zoomImage', data.imageurl).elevateZoom(
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
                $("#EquipZoom").on('load', function () {
                    CloseLoader();
                    ShowImageSaveSuccessAlert();
                });
            }
            else {
                CloseLoader();
                var errorMessage = getResourceValue("NotAuthorisedUploadFileAlert");
                ShowErrorAlert(errorMessage);

            }
        },
        error: function () {
            CloseLoader();
        }
    });
}
//#region Webcam
$(document).delegate('[name="imageUploadRadio"]', "click", function () {
    if ($(this).val() == "false") {
        $('#forDevice').show();
        $('#forWebcam').hide();
        Dropzone.forElement("div#dropzoneForm").removeAllFiles(true);
    }
    else {
        $('#forDevice').hide();
        $('#forWebcam').show();

    }
});

function InitWebCam() {
    var webcamElement = $('#webcam');
    var canvasElement = $('#canvas');
    var snapSound = $('#snapSound');
    UseWebcam.InitWebcam(webcamElement, canvasElement, snapSound);
}

$(document).delegate("#webcam-switch", "click", function () {
    if (this.checked) {
        UseWebcam.StartCamera(function () {
            cameraStarted();
            $('.md-modal').addClass('md-show');
            $('body').addClass('hideScroll');
        }, function () {
            displayError();
        });
    }
    else {
        cameraStopped();
        UseWebcam.StopCamera();
        $('body').removeClass('hideScroll');
    }
});

$(document).delegate("#cameraFlip", 'mousedown', function () {
    UseWebcam.FlipCamera();
});

$(document).delegate("#closeError", 'click', function () {
    $("#webcam-switch").click();
});

function displayError(err = '') {
    var msg = "Fail to start camera, please allow permision to access camera." +
        "If you are browsing through social media built in browsers, you would need to open the page in Sarafi(iPhone) / Chrome(Android)";
    ShowErrorAlert(msg);
    $('.offCamera').addClass("d-none");
    $('.onCamera').addClass("d-none");
}

function cameraStarted() {
    $("#errorMsg").addClass("d-none");
    $('.flash').hide();
    $('.onCamera').removeClass("d-none");
    $('.offCamera').addClass("d-none");
}

function cameraStopped() {
    $("#errorMsg").addClass("d-none");
    $('.offCamera').removeClass("d-none");
    $('.onCamera').addClass("d-none");
    $('.md-modal').removeClass('md-show');
}

$(document).delegate("#take-photo", 'click', function () {
    beforeTakePhoto();
    let picture = UseWebcam.Snap();
    document.querySelector('#imgDisply').src = picture;
    afterTakePhoto();
});

function beforeTakePhoto() {
    $('.flash')
        .show()
        .animate({ opacity: 0.3 }, 500)
        .fadeOut(500)
        .css({ 'opacity': 0.7 });
    window.scrollTo(0, 0);
}

function afterTakePhoto() {
    UseWebcam.StopCamera();
    $('.offCamera').removeClass("d-none");
    $(document).find('.onCamera').addClass("d-none");
}

function removeCapture() {
    $('.offCamera').addClass("d-none");
    $('.onCamera').removeClass("d-none");
    if (UseWebcam.CameraCounts() > 1) {
        $("#cameraFlip").removeClass('d-none');
    } else {
        $("#cameraFlip").addClass('d-none');
    }
}

$(document).delegate("#resume-camera", 'click', function () {
    UseWebcam.ResumeCamera(function () {
        removeCapture();
    });
});

$(document).delegate("#exit-app", 'click', function () {
    removeCapture();
    $("#webcam-switch").prop("checked", false).change();
    $('.md-modal').removeClass('md-show');
    $('body').removeClass('hideScroll');
});
function dataURLtoFile(dataurl, filename) {

    var arr = dataurl.split(','),
        mime = arr[0].match(/:(.*?);/)[1],
        bstr = atob(arr[1]),
        n = bstr.length,
        u8arr = new Uint8Array(n);

    while (n--) {
        u8arr[n] = bstr.charCodeAt(n);
    }

    return new File([u8arr], filename, { type: mime });
}

$(document).delegate("#UploadWebCamImg", 'click', function () {
    $('body').removeClass('hideScroll');
    if ($('#imgDisply').attr('src') != '') {
        var ticks = ((new Date().getTime() * 10000) + 621355968000000000);
        var file = dataURLtoFile($('#imgDisply').attr('src'), 'imageFromWebCam_' + ticks + '.png');
        if (file.size > (1024 * 1024 * 10)) // not more than 10mb
        {
            ShowImageSizeExceedAlert();
            return false;
        }
        var formData = new FormData();
        formData.append(file.name, file);
        $.ajax({
            url: "../Base/SaveUploadedFile",
            type: "POST",
            data: formData,
            contentType: false,
            processData: false,
            beforeSend: function () {
                ShowLoader();
            },
            success: function (res) {
                removeCapture();
                UseWebcam.StopCamera();
                $("#webcam-switch").prop("checked", false).change();
                $('.md-modal').removeClass('md-show');
                $('body').removeClass('hideScroll');
                var PartId = $('#PartId').val();
                SaveUploadedFileToServer(PartId, file.name);
            },
            error: function (xhr) {
                CloseLoader();
            }
        });
    }
});

//#endregion
//#endregion

//#endregion

//#region Parts Add-Edit
$.validator.setDefaults({ ignore: null });
$(document).on('click', ".addparts", function (e) {
    $.ajax({
        url: "/MultiStoreroomPart/AddPartsDynamic",
        type: "GET",
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#multistoreroompartcontainer').html(data);
        },
        complete: function () {
            SetMSPControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', "#editparts", function (e) {
    e.preventDefault();
    var partId = $('#PartId').val();
    $.ajax({
        url: "/MultiStoreroomPart/EditPartsDynamic",
        type: "GET",
        dataType: 'html',
        data: { PartId: partId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#multistoreroompartcontainer').html(data);
            SetFixedHeadStyle();
        },
        complete: function () {
            SetMSPControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});

function PartsAddOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        if (data.Command == "save") {
            var message;
            if (data.mode == "add") {
                localStorage.setItem("CURRENTTABSTATUS", '1');
                localStorage.setItem("partstatustext", getResourceValue("AlertActive"));
                SuccessAlertSetting.text = getResourceValue("PartAddAlert");
            }
            else {
                SuccessAlertSetting.text = getResourceValue("UpdatePartsAlerts");
            }
            swal(SuccessAlertSetting, function () {
                RedirectToMultiStoreroomPartDetail(data.PartId);
            });
        }
        else {
            ResetErrorDiv();
            SuccessAlertSetting.text = getResourceValue("PartAddAlert");
            swal(SuccessAlertSetting, function () {
                $(document).find('form').trigger("reset");
                $(document).find('form').find("select").val("").trigger('change');
                $(document).find('form').find("select").removeClass("input-validation-error");
                $(document).find('form').find("input").removeClass("input-validation-error");
                $(document).find('form').find("textarea").removeClass("input-validation-error");
                $(document).find('form').find(".ClearAccountModalPopupGridData").css("display", "none");
            });
        }
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', "#btnCancelAddPart", function () {
    var PartId = $('#MultiStoreroomPartModel_PartId').val();
    if (typeof PartId !== "undefined" && PartId != 0) {
        swal(CancelAlertSetting, function () {
            RedirectToMultiStoreroomPartDetail(PartId);
        });
    }
    else {
        swal(CancelAlertSetting, function () {
            window.location.href = "../MultiStoreroomPart/Index?page=Inventory_Part";
        });
    }
});
$(document).on('click', "#brdpart", function () {
    var PartId = $(this).attr('data-val');
    RedirectToMultiStoreroomPartDetail(PartId)
});

//#endregion

//#region common
function SetMSPControls() {
    CloseLoader();
    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse(document);
    $('input, form').blur(function () {
        $(this).valid();
    });
    $('.select2picker, form').change(function () {
        var validstate = "";
        try {
            validstate = $(this).valid();
        } catch (e) {
            validstate = undefined;
        }
        var areaddescribedby = $(this).attr('aria-describedby');
        if (areaddescribedby) {
            if (validstate) {
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
    ZoomImage($(document).find('#EquipZoom'));
    $(document).find('.select2picker').select2({});
    $(document).find('.dtpicker').datepicker({
        changeMonth: true,
        changeYear: true,
        beforeShow: function (i) { if ($(i).attr('readonly')) { return false; } },
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy');
    SetFixedHeadStyle();

}

$(document).on('click', '.mspsearchdrpbox', function (e) {
    if ($(document).find('#txtColumnSearch').val() !== '')
        $("#advsearchfilteritems").html('');
    $(document).find('#txtColumnSearch').val('');
    run = true;
    $('#mspsearctxtbox').text($(this).text());
    $(".searchList li").removeClass("activeState");
    $(this).addClass('activeState');
    $(document).find('#searcharea').hide("slide");
    var optionval = $(this).attr('id');
    localStorage.setItem("CURRENTTABSTATUS", optionval);
    CustomQueryDisplayId = optionval;
    activeStatus = optionval;
    $(document).find('#partsearchtitle').text($(this).text());
    if (optionval.length !== 0) {
        ShowbtnLoaderclass("LoaderDrop");
        dtMSPTable.page('first').draw('page');
    }
});
$(document).on('click', '#linkToSearch', function () {
    window.location.href = "../MultiStoreroomPart/Index?page=MultiStoreroomPart_Search";
});

//#endregion

//#region ColumnVisibility

$(document).on('click', '#liCustomize', function () {
    funCustomizeBtnClick(dtMSPTable, true);
});
$(document).on('click', '.saveConfig', function () {
    var colOrder = [0, 1];
    funCustozeSaveBtn(dtMSPTable, colOrder);
    run = true;
    dtMSPTable.state.save(run);
});

$(document).on('keyup', '#mspsearctxtbox', function (e) {
    var tagElems = $(document).find('#mspsearchListul').children();
    $(tagElems).hide();
    for (var i = 0; i < tagElems.length; i++) {
        var tag = $(tagElems).eq(i);
        if ($(tag).text().toLowerCase().includes($(this).val().toLowerCase()) == true || $(this).val().toLowerCase().includes($(tag).text().toLowerCase()) == true) {
            $(tag).show();
        }
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
function TextSearch() {
    run = true;
    clearAdvanceSearch();
    var partcurrentstatus = localStorage.getItem("CURRENTTABSTATUS");
    if (partcurrentstatus != 'undefined' && partcurrentstatus != null) {
        CustomQueryDisplayId = partcurrentstatus;
        $('#mspsearchListul li').each(function (index, value) {
            if ($(this).attr('id') == CustomQueryDisplayId && $(this).attr('id') != '0') {
                $('#partsearchtitle').text($(this).text());
                $(".searchList li").removeClass("activeState");
                $(this).addClass('activeState');
            }
        });
    }
    else { $('#partsearchtitle').text(getResourceValue("AlertActive")); }
    var txtSearchval = LRTrim($(document).find('#txtColumnSearch').val());
    if (txtSearchval) {
        GenerateSearchList(txtSearchval, false);
        var searchitemhtml = "";
        searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $('#txtColumnSearch').attr('id') + '">Search: ' + txtSearchval + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
        searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
        $("#advsearchfilteritems").html(searchitemhtml);
    }
    else {
        dtMSPTable.page('first').draw('page');
    }
    var container = $(document).find('#searchBttnNewDrop');
    container.hide("slideToggle");
}

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
$(document).on('click', "#SrchBttnNew", function () {
    $.ajax({
        url: '/Base/PopulateNewSearchList',
        type: 'GET',
        data: { tableName: 'Part' },
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
        data: { tableName: 'Part', searchText: txtSearchval, isClear: isClear },
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
                dtMSPTable.page('first').draw('page');
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
//#endregion

//#region details
$(document).on('click', '#linkToSearch', function () {

    var titletext = $('#spnlinkToSearch').text();
    window.location.href = "../MultiStoreroomPart/Index?page=Inventory_Part"

});
$(document).on('click', '.lnk_psearch', function (e) {
    var row = $(this).parents('tr');
    var data = dtMSPTable.row(row).data();
    var titletext = $('#partsearchtitle').text();
    localStorage.setItem("partstatustext", titletext);
    $.ajax({
        url: "/MultiStoreroomPart/MultiStoreroomPartDetails",
        type: "POST",
        data: { PartId: data.PartId },
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#multistoreroompartcontainer').html(data);
            $(document).find('#spnlinkToSearch').text(titletext);
            SetFixedHeadStyle();
        },
        complete: function () {
            LoadComments(data.PartId);
            LoadImages(data.PartId);
            generateStoreRoomdataTable(data.PartId);
            ZoomImage($(document).find('#EquipZoom'));
            CloseLoader();
            Dropzone.autoDiscover = false;
            if ($(document).find('#dropzoneForm').length > 0) {
                var myDropzone = new Dropzone("div#dropzoneForm", {
                    url: "../Base/SaveUploadedFile",
                    addRemoveLinks: true,
                    paramName: 'file',
                    maxFilesize: 10, // MB
                    maxFiles: 4,
                    dictDefaultMessage: getResourceValue("FileUploadAlert"),//'Drag files here to upload, or click to select one',
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
                    dictRemoveFileConfirmation: getResourceValue("CancelAlertSure"),// "Do you want to remove this file ?",
                    dictCancelUpload: "X",
                    parallelUploads: 1,
                    dictMaxFilesExceeded: "You can vist upload any more files.",
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
            //InitWebCam();
            SetFixedHeadStyle();
            $.validator.setDefaults({ ignore: null });
            $.validator.unobtrusive.parse(document);
        },
        error: function () {
            CloseLoader();
        }
    });
});
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

//#region Storeroom Widget
$(document).on('click', '.StoreroomGridinnerdata', function (e) {

    var tr = $(this).closest('tr');
    var row = dtStoreroomsTable.row(tr);
    if (this.src.match('details_close')) {
        this.src = "../../Images/details_open.png";
        row.child.hide();
        tr.removeClass('shown');
    }
    else {

        this.src = "../../Images/details_close.png";
        var PartStoreroomId = $(this).attr("rel");
        $.ajax({
            url: "/MultiStoreroomPart/GetStoreroomInnerGridViewData",
            data: {
                PartStoreroomId: PartStoreroomId
            },
            beforeSend: function () {
                ShowLoader();
            },
            dataType: 'html',
            success: function (json) {
                row.child(json).show();
                CloseLoader();
            }
        });

    }
});

function generateStoreRoomdataTable(PartId) {
  let  IsParts_ConfigureAutoPurchasing = $(document).find('#IsParts_ConfigureAutoPurchasing').val();

    if ($(document).find('#tblPStorerooms').hasClass('dataTable')) {
        dtStoreroomsTable.destroy();
    }
    dtStoreroomsTable = $("#tblPStorerooms").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[1, "asc"]],
        stateSave: false,
        "pagingType": "full_numbers",
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/MultiStoreroomPart/PopulateStoreroomDetails",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.PartId = PartId;
            },
            "dataSrc": function (result) {
                rCount = result.data.length;
                if (rCount > 0) {
                    $(document).find("#AddStoreroom").hide();
                }
                else {
                    $(document).find("#AddStoreroom").show();
                }
                return result.data;
            },
            global: true
        },

        "columns":
            [
                {
                    "data": "PartStoreroomId",
                    "bVisible": true,
                    "bSortable": false,
                    "autoWidth": false,
                    "bSearchable": false,
                    "mRender": function (data, type, row) {

                        return '<img class="StoreroomGridinnerdata"  src="../../Images/details_open.png" alt="expand/collapse" rel="' + data + '" style="cursor: pointer;"/>';

                    }
                },

                { "data": "StoreroomNameWithDescription", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "QtyOnHand", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "data": "QtyMaximum", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "data": "QtyReorderLevel", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "data": "TotalOnRequest", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "data": "TotalOnOrder", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
                {
                    "data": "Maintain", "autoWidth": true, "bSearchable": true, "bSortable": false, "className": "text-center",
                    "mRender": function (data, type, row) {
                        var issue = '';
                        var physicalinventory = '';
                        var PartTransferRequest = '';
                        var addtoAutoTransfer = '';
                        var removefromAutoTransfer = '';
                        var Parts_ConfigureAutoPurchasing = '';
                        if (row.Issue === true) {
                            issue = '<a class="btn btn-blue partCheckOutBtn gridinnerbutton" style="margin-left: 0;" title="Add"">Part Checkout</a>';
                        }
                        if (row.PhysicalInventory === true) {
                            PartTransferRequest = '<a class="btn btn-blue partTransferRequestBtn gridinnerbutton" style="margin-left: 0;" title="Add">Part Transfer Request</a>';
                            physicalinventory = '<a class="btn btn-blue adjustOnHandBtn gridinnerbutton" style="margin-left: 0;" title="Add">Adjust On Hand Quantity</a>';
                        }
                        if (row.AutoTransfer === 0) {

                            addtoAutoTransfer = '<a class="btn btn-blue addtoAutoTransferStoreroomBtn gridinnerbutton" style="margin-left: 0;" title= "Add to Auto Transfer">Add to Auto Transfer</a>';
                        }
                        if (row.AutoTransfer === 1) {
                            removefromAutoTransfer = '<a class="btn btn-blue removefromAutoTransferStoreroomBtn gridinnerbutton" style="margin-left: 0;" title= "Remove from Auto Transfer ">Remove from Auto Transfer</a>';
                        }
                        if (IsParts_ConfigureAutoPurchasing == "True") {
                            Parts_ConfigureAutoPurchasing = '<a class="btn btn-blue partsConfigureAutoPurchasingBtn gridinnerbutton" style="margin-left: 0;" title= "Auto Purchasing Configuration ">Auto Purchasing Configuration</a>';
                        }
                        if (data === true) {
                            return '<button type="button" class="btn btn-blue actionbtns" style="border:0;outline:0" data-id="' + row.PartStoreroomId + '"><strong>...</strong></button>' +
                                '<div id="' + row.PartStoreroomId + '" class="actionbtndiv" style="display:none;">' +
                                '<a class="btn btn-blue addStoreroomBtn gridinnerbutton" style="margin-left: 0;" title="Add">Add</a>' +
                                '<a class="btn btn-blue editStoreroomBtn gridinnerbutton" style="margin-left: 0;" title= "Edit">Edit</a>' +
                                addtoAutoTransfer +
                                removefromAutoTransfer +
                                '<a class="btn btn-blue printStoreroomQRCodeBtn gridinnerbutton" style="margin-left: 0;" title= "Print">Print</a>' + Parts_ConfigureAutoPurchasing +
                                issue +
                                physicalinventory +
                                PartTransferRequest +
                                '</div >' +
                                '<div class="maskaction ' + row.PartStoreroomId + '" data-id="' + row.PartStoreroomId + '"  onclick="funcCloseActionbtn()"></div>';
                        }
                        else {
                            return '<button type="button" class="btn btn-blue actionbtns" style="border:0;outline:0" data-id="' + row.PartStoreroomId + '"><strong>...</strong></button>' +
                                '<div id="' + row.PartStoreroomId + '" class="actionbtndiv" style="display:none;">' +
                                '<a class="btn btn-blue printStoreroomQRCodeBtn gridinnerbutton" style="margin-left: 0;" title= "Print">Print</a>' + Parts_ConfigureAutoPurchasing +
                                issue +
                                physicalinventory +
                                '</div >' +
                                '<div class="maskaction ' + row.PartStoreroomId + '" data-id="' + row.PartStoreroomId + '"  onclick="funcCloseActionbtn()"></div>';
                        }
                    }
                }

            ],

        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}
$.validator.setDefaults({ ignore: null });
$(document).on('click', ".addStoreroomBtn, #AddStoreroom", function (e) {
    var partId = $(document).find("#PartId").val();
    $.ajax({
        url: "/MultiStoreroomPart/AddEditStoreroom",
        type: "GET",
        dataType: 'html',
        data: {
            'partId': partId,
            'storeroomId': 0,
            'PartStoreroomId': 0
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#StoreroomPopUp').html(data);
            $('#AddStoreroomModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            SetMSPControls();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', '.editStoreroomBtn', function (e) {
    var row = $(this).parents('tr');
    var data = dtStoreroomsTable.row(row).data();
    var partId = $(document).find("#PartId").val();
    $.ajax({
        url: "/MultiStoreroomPart/AddEditStoreroom",
        type: "GET",
        dataType: 'html',
        data: {
            'partId': partId,
            'storeroomId': data.StoreroomId,
            'PartStoreroomId': data.PartStoreroomId
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#StoreroomPopUp').html(data);
            $('#AddStoreroomModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            SetMSPControls();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});

function StoreroomAddOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        $(document).find('#AddStoreroomModalpopup').modal("hide");
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("StoreroomAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("StoreroomUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            dtStoreroomsTable.page('first').draw('page');
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}

$(document).on('click', '.clearstate', function () {
    $(document).find('#AddStoreroomModalpopup').modal("hide");
    var areaStoreroomId = "";
    $(document).find('#AddStoreroomModalpopup select').each(function (i, item) {
        areaStoreroomId = $(document).find("#" + item.getAttribute('id')).attr('aria-describedby');
        $('#' + areaStoreroomId).hide();
    });
});

//#endregion
//#endregion

//#region QR Code
$(document).on('click', '.printStoreroomQRCodeBtn', function () {
    var ClientLookupId = $(document).find('#ClientLookupId').val();
    var Description = $(document).find('#Description').val();

    var row = $(this).parents('tr');
    var Rowdata = dtStoreroomsTable.row(row).data();
    var Location = '';
    var PlaceArea = Rowdata.Location1_5;
    var Section = Rowdata.Location1_1;
    var Row = Rowdata.Location1_2;
    var Shelf = Rowdata.Location1_3;
    var Bin = Rowdata.Location1_4;

    var IssueUnit = $(document).find('#IssueUnit').val();
    var QtyMinimum = Rowdata.QtyReorderLevel;
    var QtyMaximum = Rowdata.QtyMaximum;
    var Manufacturer = $(document).find('#Manufacturer').val();
    var ManufacturerID = $(document).find('#ManufacturerID').val();
    PartToClientLookupIdQRcode = [];
    if (PlaceArea != "") {
        Location = PlaceArea;
    }
    if (Section != "") {
        if (Location != "") {
            Location = Location + "-" + Section;
        }
        else {
            Location = Section;
        }
    }

    if (Row != "") {
        if (Location != "") {
            Location = Location + "-" + Row;
        }
        else {
            Location = Row;
        }
    }
    if (Shelf != "") {
        if (Location != "") {
            Location = Location + "-" + Shelf;
        }
        else {
            Location = Shelf;
        }
    }
    if (Bin != "") {
        if (Location != "") {
            Location = Location + "-" + Bin;
        }
        else {
            Location = Bin;
        }
    }
    PartToClientLookupIdQRcode.push(ClientLookupId + '][' + Description + '][' + (Location == null ? " " : Location) + '][' + IssueUnit + '][' + QtyMinimum + '][' + QtyMaximum + '][' + Manufacturer + '][' + ManufacturerID);
    var partClientLookups = PartToClientLookupIdQRcode;
    if ($('#EPMInvoiceImportInUse').val() == "True") {
        generateQRforEPM(partClientLookups);
    }
    else {
        $.ajax({
            url: "/MultiStoreroomPart/PartDetailsQRcode",
            data: {
                PartClientLookups: partClientLookups
            },
            type: "POST",
            beforeSend: function () {
                ShowLoader();
            },
            datatype: "html",
            success: function (data) {
                $('#printMultiStoreroomPartDetailsqrcodemodalcontainer').html(data);
            },
            complete: function () {
                $('#printMSPDetailsPartQrCodeModal').modal('show');
                CloseLoader();
            },
            error: function () {
                CloseLoader();
            }
        });
    }
});
function MultiPartsQROnSuccess(data) {
    CloseLoader()
    if (data.success === 0) {
        var smallLabel = $('#SmallLabel').prop('checked');
        //window.open("/MultiStoreroomPart/QRCodeGenerationUsingRotativa?SmallLabel=" + encodeURI(smallLabel), "_blank");
        window.open("/MultiStoreroomPart/QRCodeGenerationUsingDevExpress?SmallLabel=" + encodeURI(smallLabel), "_blank");
        $('#printMSPDetailsPartQrCodeModal').modal('hide');
        PartToClientLookupIdQRcode = [];
    }
}
//#endregion

//#region CKEditor
$(document).on("focus", "#msparttxtcommentsnew", function () {
    $(document).find('.ckeditorarea').show();
    var noteitem = $(document).find('.editcomments').parents('.kt-notes__item');
    noteitem.find('.ckeditorarea').hide();
    noteitem.find('.kt-notes__body').show();
    noteitem.find('.commenteditdelearea').show();
    //ClearEditor();
    LoadCkEditor('msparttxtcomments');
    $("#msparttxtcommentsnew").hide();
    $(".ckeditorfield").show();
});

$(document).on('click', '#btnsavecommands', function () {
    var selectedUsers = [];
    const data = getDataFromTheEditor();
    if (!data) {
        return false;
    }
    var partid = $(document).find('#PartId').val();
    var PartClientLookupId = $(document).find('#ClientLookupId').val();
    var noteId = 0;
    if (LRTrim(data) == "") {
        return false;
    }
    else {
        $(document).find('.ck-content').find('.mention').each(function (item, index) {
            selectedUsers.push($(index).data('user-id'));
        });
        $.ajax({
            url: '/MultiStoreroomPart/AddOrUpdateComment',
            type: 'POST',
            beforeSend: function () {
                ShowLoader();
            },
            data: {
                partid: partid,
                content: data,
                PartClientLookupId: PartClientLookupId,
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
                        RedirectToMultiStoreroomPartDetail(partid);
                    });
                }
                else {
                    ShowGenericErrorOnAddUpdate(data);
                    CloseLoader();
                }
            },
            complete: function () {
                ClearEditor();
                $("#msparttxtcommentsnew").show();
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
    $("#msparttxtcommentsnew").show();
    $(".ckeditorfield").hide();
});
$(document).on('click', '.editcomments', function () {
    $(document).find(".ckeditorarea").each(function () {
        $(this).html('');
    });
    $("#msparttxtcommentsnew").show();
    $(".ckeditorfield").hide();
    var notesitem = $(this).parents('.kt-notes__item').eq(0);
    notesitem.find('.ckeditorarea').html(CreateEditorHTML('msparttxtcommentsEdit'));
    var noteitem = $(document).find('.editcomments').parents('.kt-notes__item');
    noteitem.find('.kt-notes__body').show();
    noteitem.find('.commenteditdelearea').show();

    var rawHTML = $.parseHTML($(this).parents('.kt-notes__item').find('.kt-notes__body').find('.originalContent').html());
    LoadCkEditorEdit('msparttxtcommentsEdit', rawHTML);
    $(document).find('.ckeditorarea').hide();
    notesitem.find('.ckeditorarea').show();
    notesitem.find('.kt-notes__body').hide();
    notesitem.find('.commenteditdelearea').hide();
});

$(document).on('click', '.deletecomments', function () {
    DeletePartNote($(this).attr('id'));
});
$(document).on('click', '.btneditcomments', function () {
    var data = getDataFromTheEditor();
    var partid = $(document).find('#PartId').val();
    var ClientLookupId = $(document).find('#ClientLookupId').val();
    var noteId = $(this).parents('.kt-notes__item').find('.editcomments').attr('id');
    var updatedindex = $(this).parents('.kt-notes__item').find('.hdnupdatedindex').val();
    $.ajax({
        url: '/MultiStoreroomPart/AddOrUpdateComment',
        type: 'POST',
        beforeSend: function () {
            ShowLoader();
        },
        data: { partid: partid, content: LRTrim(data), noteId: noteId, updatedindex: updatedindex },
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
                    RedirectToMultiStoreroomPartDetail(partid);

                });
            }
            else {
                ShowGenericErrorOnAddUpdate(data);
                CloseLoader();
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
$(document).on('click', '.btncommandCancel', function () {
    ClearEditorEdit();
    $(document).find('.ckeditorarea').hide();
    $(this).parents('.kt-notes__item').find('.kt-notes__body').show();
    $(this).parents('.kt-notes__item').find('.commenteditdelearea').show();
});
function DeletePartNote(notesId) {
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
                        RedirectToMultiStoreroomPartDetail($(document).find('#PartId').val());
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

//#region Comment
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
function LoadComments(PartID) {
    $.ajax({
        "url": "/MultiStoreroomPart/LoadComments",
        data: { PartID: PartID },
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
//#endregion

//#region MultiStoreroom More Item Show Hide
function funcCloseActionbtn() {
    var btnid = $(this).attr("data-id");
    $(document).find(".actionbtndiv").hide();
    $(document).find(".maskaction").hide();
}

$(document).on('click', '.actionbtns', function (e) {
    var row = $(this).parents('tr');
    var data = dtStoreroomsTable.row(row).data();

    $(document).find("#" + data.PartStoreroomId).show();
    $(document).find("." + data.PartStoreroomId).show();
});
//#endregion
//#region V2-755
$(document).on('click', '.partCheckOutBtn', function (e) {
    var row = $(this).parents('tr');
    var data = dtStoreroomsTable.row(row).data();
    var partId = $(document).find("#PartId").val();
    var _PartClientLookupId = LRTrim($("#ClientLookupId").val());
    var Description = LRTrim($("#Description").val());
    var UPCCode = LRTrim($("#UPCCode").val());
    $.ajax({
        url: "/MultiStoreroomPart/PartCheckOut",
        type: "GET",
        dataType: 'html',
        data: {
            'partId': partId,
            'storeroomId': data.StoreroomId,
            'PartClientLookupId': _PartClientLookupId,
            'Description': Description,
            'UPCCode': UPCCode
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#partCheckOutPopUp').html(data);
            $('#partCheckOutModalPopUp').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            SetMSPControls();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', '.adjustOnHandBtn', function (e) {
    var row = $(this).parents('tr');
    var data = dtStoreroomsTable.row(row).data();
    var partId = $(document).find("#PartId").val();
    var _PartClientLookupId = LRTrim($("#ClientLookupId").val());
    var Description = LRTrim($("#Description").val());
    var UPCCode = LRTrim($("#UPCCode").val());
    $.ajax({
        url: "/MultiStoreroomPart/ChangeHandsOnQuantity",
        type: "GET",
        dataType: 'html',
        data: {
            'partId': partId,
            'storeroomId': data.StoreroomId,
            'PartClientLookupId': _PartClientLookupId,
            'Description': Description,
            'UPCCode': UPCCode
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#changeHandsOnQtyPopUp').html(data);
            $('#changeHandsOnQtyModalPopUp').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            SetMSPControls();
            var areaddescribedby = $(document).find("#inventoryModel_ReceiptQuantity").attr('aria-describedby');
            $('#' + areaddescribedby).hide();
            $(document).find('form').find("#inventoryModel_ReceiptQuantity").removeClass("input-validation-error");
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});
//#region Additional Part Functions
$(document).on('click', '#btnPartCheckOutClose,#btnPartCheckOutCancel,#btnEqChildrenClose,#btnWOChildrenClose,#btnLocChildrenClose', function () {
    if ($(this).attr('id') == 'btnPartCheckOutClose' || $(this).attr('id') == 'btnPartCheckOutCancel') {
        hidemodal($('#partCheckOutModalPopUp'));
        resetValidation();
    }
    if ($(this).attr('id') == 'btnEqChildrenClose') {
        hidemodal($('#EquipmentCHKModal'));
    }
    if ($(this).attr('id') == 'btnWOChildrenClose') {
        hidemodal($('#WorkOrderCHKModal'));
    }
    if ($(this).attr('id') == 'btnLocChildrenClose') {
        hidemodal($('#LocationModal'));
    }
});
function ValidationHanhsQtyOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        $(document).find('#changeHandsOnQtyModalPopUp').modal("hide");
        RedirectToMultiStoreroomPartDetail($(document).find('#inventoryModel_PartId').val());
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
function ValidateParCheckOutOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        $(document).find('#partCheckOutModalPopUp').modal("hide");
        RedirectToMultiStoreroomPartDetail($(document).find('#PartId').val());
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', "#openpartChkoutgrid", function () {
    $('.overlay').fadeOut();
    var textChargeToId = $("#inventoryCheckoutModel_ChargeType option:selected").val();
    if (textChargeToId == "WorkOrder") { generateWorkOrderDataTable(); }
    else if (textChargeToId == "Equipment") { generateEquipmentDataTable(); }
    else { generateLocationDataTable(); }
});
$(function () {
    $(document).find('.select2picker').select2({});
    $("#inventoryCheckoutModel_ChargeToId").attr('disabled', 'disabled');
    $(document).find("#imgChargeToTree").hide();
    $(document).on("change", "#inventoryCheckoutModel_ChargeType", function () {
        var option = '';
        chargeTypeText = $('option:selected', this).text();
        var type = $(this).val();
        chargeTypeSelected = type;
        if (type == "") {
            $(document).find("#imgChargeToTree").hide();
            option = "--Select--";
            $(document).find("#inventoryCheckoutModel_ChargeToId").val("").trigger('change');
            $("#inventoryCheckoutModel_ChargeToId,#openpartChkoutgrid").attr('disabled', 'disabled');
        }
        else {
            if (type == "Equipment") {
                $(document).find("#imgChargeToTree").show();
            }
            else {
                $(document).find("#imgChargeToTree").hide();
            }
            $("#inventoryCheckoutModel_ChargeToId,#openpartChkoutgrid").removeAttr('disabled');

        }
        $(document).find("#txtChargeToId").val("");
        var tlen = $(document).find("#inventoryCheckoutModel_ChargeType").val();
        if (tlen.length > 0) {
            var areaddescribedby = $(document).find("#inventoryCheckoutModel_ChargeType").attr('aria-describedby');
            $('#' + areaddescribedby).hide();
            $(document).find('form').find("#inventoryCheckoutModel_ChargeType").removeClass("input-validation-error");
        }
        else {
            var arectypeaddescribedby = $(document).find("#inventoryCheckoutModel_ChargeType").attr('aria-describedby');
            $('#' + arectypeaddescribedby).show();
            $(document).find('form').find("#inventoryCheckoutModel_ChargeType").addClass("input-validation-error");
        }
    });
    $(document).on("change", "#txtChargeToId", function () {
        chargeToSelected = $(this).val();
        chargeToText = $('option:selected', this).text();
        var tlen = $(document).find("#txtChargeToId").val();
        if (tlen.length > 0) {
            var areaddescribedby = $(document).find("#txtChargeToId").attr('aria-describedby');
            $('#' + areaddescribedby).hide();
            $(document).find('form').find("#txtChargeToId").removeClass("input-validation-error");
        }
        else {
            var arectoaddescribedby = $(document).find("#txtChargeToId").attr('aria-describedby');
            $('#' + arectoaddescribedby).show();
            $(document).find('form').find("#txtChargeToId").addClass("input-validation-error");
        }
    });
});
function resetValidation() {
    $(document).find("#inventoryCheckoutModel_ChargeType").val("").trigger('change');
    $(document).find("#inventoryCheckoutModel_ChargeToId").val("").trigger('change');
    $(document).find("#inventoryCheckoutModel_Quantity").val("1");
    var areaChargeToId = $(document).find("#txtChargeToId").attr('aria-describedby');
    $('#' + areaChargeToId).hide();
    var areaChargeType = $(document).find("#inventoryCheckoutModel_ChargeType").attr('aria-describedby');
    $('#' + areaChargeType).hide();
    var areaQuantity = $(document).find("#inventoryCheckoutModel_Quantity").attr('aria-describedby');
    $('#' + areaQuantity).hide();
    $(document).find('form').find("#inventoryCheckoutModel_ChargeType").removeClass("input-validation-error");
    $(document).find('form').find("#txtChargeToId").removeClass("input-validation-error");
    $(document).find('form').find("#inventoryCheckoutModel_Quantity").removeClass("input-validation-error");

}

//#endregion
//#endregion
//#region V-751
$(document).on('click', '.partTransferRequestBtn', function (e) {
    var row = $(this).parents('tr');
    var data = dtStoreroomsTable.row(row).data();
    var partId = $(document).find("#PartId").val();
    var ClientLookupId = $(document).find("#ClientLookupId").val();
    var Description = $(document).find("#Description").val();
    $.ajax({
        url: "/MultiStoreroomPart/PartTransferRequest",
        type: "GET",
        dataType: 'html',
        data: {
            'partId': partId,
            'ClientLookupId': ClientLookupId,
            'PartStoreroomId': data.PartStoreroomId,
            'StoreroomId': data.StoreroomId,
            'Description': Description,
            'StoreroomName': data.StoreroomName
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#PartTransferRequestPopUp').html(data);
            $('#AddPartTransferRequestModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            SetMSPControls();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});

function partTransferRequestAddOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        $(document).find('#AddPartTransferRequestModalpopup').modal("hide");

        SuccessAlertSetting.text = 'Part transfer request added successfully';

        swal(SuccessAlertSetting, function () {
            RedirectToMultiStoreroomPartDetail($(document).find('#PartId').val());
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}

$(document).on('click', '#btnStoreroomcancel,.clearstate', function () {
    $(document).find('#AddStoreroomModalpopup').modal("hide");
    var areaChargeToId = "";
    $(document).find('#AddPartTransferRequestModalpopup select').each(function (i, item) {
        areaChargeToId = $(document).find("#" + item.getAttribute('id')).attr('aria-describedby');
        $('#' + areaChargeToId).hide();
    });

});
//#endregion
//#region V2-886
$(document).on('keyup', '#multistorepartSearch_paginate .paginate_input', function () {
    run = true;
});
//#endregion

$(document).on('click', '.VendorTableModalPopupclose', function () {
    $(document).find('#VendorTableModalPopup').modal("hide");
});

//#region V2-1045
$(document).on('click', '#btnMSPFind', function () {
    $.ajax({
        url: "/MultiStoreroomPart/FindPart",
        type: "GET",
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#FindPartClientLookupIdPopUp').html(data);
            $('#FindPartClientLookupIdModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            SetMSPControls();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});

function ValidationFindPartOnSuccess(data) {
    CloseLoader();
    $(document).find('#FindPartClientLookupIdModalpopup').modal("hide");
    $('.modal-backdrop').remove();//Issue with the Part Find button
    if (data.Result == "success") {
        var CheckedPartId = data.PartId;
        RedirectToMultiStoreroomPartDetail(CheckedPartId, "FindPart");
    }
    else if (data.Result == "failed") {
        ShowErrorAlert(getResourceValue("PartNotFoundAlert").replace("{0}", data.PartClientLookupId));
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
//#endregion

//#region V2-1059
$(document).on('click', '.addtoAutoTransferStoreroomBtn', function (e) {
    var row = $(this).parents('tr');
    var data = dtStoreroomsTable.row(row).data();
    var partId = $(document).find("#PartId").val();

    $.ajax({
        url: "/MultiStoreroomPart/AddToAutoTransferStoreroom",
        type: "GET",
        dataType: 'html',
        data: {
            'partId': partId,
            'PartStoreroomId': data.PartStoreroomId,
            'StoreroomId': data.StoreroomId,
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#AddToAutoTransferStoreroomPopUp').html(data);
            $('#AddToAutoTransferStoreroomModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            SetMSPControls();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});
function addToAutotransferAddOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        $(document).find('#AddToAutoTransferStoreroomModalpopup').modal("hide");

        SuccessAlertSetting.text = getResourceValue("spnAddtoautotransfer");
        swal(SuccessAlertSetting, function () {
            RedirectToMultiStoreroomPartDetail($(document).find('#PartId').val());
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
function checkMinValue() {
    var MaxQty = $(document).find("#addToAutoTransfer_AutoTransferMaxQty").val();
    var MinQty = $(document).find("#addToAutoTransfer_AutoTransferMinQty").val();

    var maxQtyValue = parseFloat(MaxQty);
    var minQtyValue = parseFloat(MinQty);

    if ((maxQtyValue < minQtyValue) || (maxQtyValue === minQtyValue)) {
        ErrorAlertSetting.text = getResourceValue("ValidMinToMaxQty"); 
       swal(ErrorAlertSetting, function () {
            });
    return false;
    }
 }
$(document).on('click', '#btnStoreroomcancel,.clearstate', function () {
    $(document).find('#AddStoreroomModalpopup').modal("hide");
    var areaChargeToId = "";
    $(document).find('#AddToAutoTransferStoreroomModalpopup select').each(function (i, item) {
        areaChargeToId = $(document).find("#" + item.getAttribute('id')).attr('aria-describedby');
        $('#' + areaChargeToId).hide();
    });
});

//#region Remove from AutoTransfer
$(document).on('click', '.removefromAutoTransferStoreroomBtn', function (e) {
    var row = $(this).parents('tr');
    var data = dtStoreroomsTable.row(row).data();
    var partId = $(document).find("#PartId").val();
    swal({
        title: getResourceValue("spnAreyousure"),
        text: getResourceValue("AlartRemovingMsg"),
        type: "warning",
        showCancelButton: true,
        closeOnConfirm: false,
        confirmButtonClass: "btn-sm btn-primary",
        cancelButtonClass: "btn-sm",
        confirmButtonText: getResourceValue("CancelAlertYes"),
        cancelButtonText: getResourceValue("CancelAlertNo")
    }, function () {
        ShowLoader();
        $.ajax({
            url: "/MultiStoreroomPart/RemovefromAutoTransferStoreroom",
            data: {
                'partId': partId,
                'PartStoreroomId': data.PartStoreroomId,
                'StoreroomId': data.StoreroomId,
            },
            type: "GET",
            datatype: "json",
            success: function (data) {
                if (data.Result == "success") {
                    SuccessAlertSetting.text = getResourceValue("RemovedSuccessfullyAlert"); 
                    swal(SuccessAlertSetting, function () {
                         RedirectToMultiStoreroomPartDetail($(document).find('#PartId').val());
                    }
                    );
                }
                else {
                    GenericSweetAlertMethod(data.error);
                }
            }
            , complete: function () {
                CloseLoader();
            },
        });
    });
});
//#endregion
//#endregion
//#region V2-1115
function generateQRforEPM(partClientLookups) {
    $.ajax({
        type: "POST",
        url: "/MultiStoreroomPart/SetPartIdlistforEPM",
        data: {
            PartClientLookups: partClientLookups
        },
        success: function (data) {
            if (data.success === 0) {
                window.open('/MultiStoreroomPart/GenerateEPMPartQRcode', '_blank');
                partClientLookups = [];
            }
        },
        error: function (xhr, status, error) {
            console.error("Error generating QR code:", error);
        }
    });
}
//#endregion

//#region V2-1187
function PartsEditOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        var message;
        SuccessAlertSetting.text = getResourceValue("UpdatePartsAlerts");
        swal(SuccessAlertSetting, function () {
            RedirectToMultiStoreroomPartDetail(data.partid);
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
//#endregion