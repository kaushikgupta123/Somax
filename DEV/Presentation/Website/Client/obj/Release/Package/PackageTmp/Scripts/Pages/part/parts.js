//#region search-page
var partsSearchdt;
var dtviewstatus = false;
var filteritemcount = 0;
var partToQR = [];
var selectCount = 0;
var selectedcount = 0;
var totalcount = 0;
var searchcount = 0;
var searchresult = [];
var PartToClientLookupIdQRcode = [];
var CustomQueryDisplayId = "1";
var run = false;
var partIds = [];
var titleText = '';
var dtPOPRTable;
var partStatus;
//#region V2-840 
var layoutType = 2;
var cardviewstartvalue = 0;
var cardviewlwngth = 10;
var grdcardcurrentpage = 1;
var currentorderedcolumn = 1;
var currentorder = 'asc';
//#endregion
function filterinfo(id, value) {
    this.key = id;
    this.value = value;
}
function openCity(evt, cityName) {
    evt.preventDefault();
    switch (cityName) {
        case "Photos":
            LoadImages($(document).find('#PartModel_PartId').val());
            break;
        case "Notes":
            GeneratePNotesGrid();
            break;
        case "Attachments":
            GeneratePAttachmentGrid();
            break;
        case "NewVendors":
            GeneratePVendorGrid();
            break;
        case "NewEquipment":
            GeneratePEquipmentGrid();
            break;
        case "History":
            $('#HistoryDaterange').val('2').trigger('change');
            $(document).find(".sidebar").mCustomScrollbar({
                theme: "minimal"
            });
            GenerateHistoryGrid();
            break;
        case "Receiptscontainer":
            $('#receiptdtselector').val('2').trigger('change');
            $(document).find(".sidebar").mCustomScrollbar({
                theme: "minimal"
            });
            generateReceiptDataTable();
            break;
        case "ReviewSitecontainer":
            GenerateReviewSiteGrid();
            $(document).find(".sidebar").mCustomScrollbar({
                theme: "minimal"
            });
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

    document.getElementById(cityName).style.display = "block";
    if (typeof evt.currentTarget !== "undefined") {
        evt.currentTarget.className += " active";
    }
    else {
        evt.target.className += " active";
    }
}

var IsPartDetailsFromEquipmentstatus = $('#IsPartDetailsFromEquipment').val();
if (IsPartDetailsFromEquipmentstatus == 'True') {
    //#region V2-808
    var isMobile = CheckLoggedInFromMob();
    if (isMobile) {
        $(document).find('#divAddImage').remove();
    }
    else {
        $(document).find('#liAddimage').remove();
    }
    //#endregion
    //var stexts = localStorage.getItem("partstatustext");
    var stexts = $(document).find('#PartModel_partStatusForRedirection').val();
    $(document).find('#spnlinkToSearch').text(stexts);
    var prtid = $(document).find('#PartModel_PartId').val();
    LoadActivity(prtid);
    SetPartDetailEnvironment();
    generatePOPRdataTable(prtid);
}
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
$(document).on('click', "ul.vtabs li", function () {
    $("ul.vtabs li").removeClass("active");
    $(this).addClass("active");
    $(".tabsArea").hide();
    var activeTab = $(this).find("a").attr("href");
    $(activeTab).fadeIn();
    return false;
});
$(document).ready(function () {
    $(".actionBar").fadeIn();
    $("#PartGridAction :input").attr("disabled", "disabled");
});
$(function () {
    ShowbtnLoaderclass("LoaderDrop");
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
    $("#PartBulksidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $('#dismiss, .overlay').on('click', function () {
        $('#sidebar,#Bulksidebar').removeClass('active');
        $('.overlay').fadeOut();
    });

    $('#sidebarCollapse').on('click', function () {
        $('#sidebar').addClass('active');
        $('.overlay').fadeIn();
        $('.collapse.in').toggleClass('in');
        $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    });
   
    var IsDetailFromNotification = $('#IsDetailFromNotification').val();
    if (IsDetailFromNotification == 'True') {
        //#region V2-808
        var isMobile = CheckLoggedInFromMob();
        if (isMobile) {
            $(document).find('#divAddImage').remove();
        }
        else {
            $(document).find('#liAddimage').remove();
        }
        //#endregion


        $(document).find('#spnlinkToSearch').text(getResourceValue("AlertActive"));
        localStorage.setItem("CURRENTTABSTATUS", '1');
        var prtid = $(document).find('#PartModel_PartId').val();
        LoadActivity(prtid);
        SetPartDetailEnvironment();
        generatePOPRdataTable(prtid);
    }

    var partStatusVal = $('#PartStatusVal').val();
    if (partStatusVal != "") {
        localStorage.setItem("CURRENTTABSTATUS", partStatusVal);
    }
    //#region Load Grid With Status
    var partcurrentstatus = localStorage.getItem("CURRENTTABSTATUS");
    if (partcurrentstatus != 'undefined' && partcurrentstatus != null && partcurrentstatus != "") {
        CustomQueryDisplayId = partcurrentstatus;
        $('#partsearchListul li').each(function (index, value) {
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
        $("#partsearchListul li").first().addClass("activeState");
    }
    //V2-840
    var currentlayoutstatus = localStorage.getItem("layoutTypeval");
    if (currentlayoutstatus)
        layoutType = currentlayoutstatus;
    if (layoutType == 2) {
        generatePartsDataTable();
    }
    else {
        generatePartsDataTable();
        layoutVal = $(document).find('#cardviewliLayout').text();
        $('#layoutsortmenu').text("Layout" + " : " + layoutVal);
        $(document).find('#Active').hide();
        $(document).find('#ActiveCard').show();
        HidebtnLoader("layoutsortmenu");
        $(document).find('#liCustomize').prop("disabled", true);
        cardviewstartvalue = 0;
        grdcardcurrentpage = 1;
        GetDatatableLayout();
        LayoutFilterinfoUpdate();
        ShowCardView();
    }
    //#endregion
    $(document).on('click', '#liPrint', function () {
        $(".buttons-print")[0].click();
        funcCloseExportbtn();
    });
    $(document).on('click', '#liCsv', function () {
        $(".buttons-csv")[0].click();
        funcCloseExportbtn();
    });
    $(document).on('click', '#liPdf', function () {
        $(".buttons-pdf")[0].click();
        funcCloseExportbtn();
    });
    $(document).on('click', "#liExcel", function () {
        $(".buttons-excel")[0].click();
        funcCloseExportbtn();
    });
    function AdvanceSearch() {
        $('.filteritemcount').text(filteritemcount);
    }
    $("#btnPartsDataAdvSrch").on('click', function (e) {
        run = true;
        var searchitemhtml = "";
        $(document).find('#txtColumnSearch').val('');
        filteritemcount = 0;
        $('#txtsearchbox').val('');
        $('#advsearchsidebar').find('.adv-item').each(function (index, item) {
            if ($(this).val()) {
                filteritemcount++;
                searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
            }
        });
        searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
        $('#advsearchfilteritems').html(searchitemhtml);
        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();
        AdvanceSearch();
        //V2-840
        if (layoutType == 2) {
            partsSearchdt.page('first').draw('page');
        }
        else {
            cardviewstartvalue = 0;
            grdcardcurrentpage = 1;
            LayoutFilterinfoUpdate();
            ShowCardView();
        }

    });
    //#region V2-840
    function cardviewstate(currentpage, start, length, currentorderedcolumn, sorttext, order) {
        this.currentpage = currentpage;
        this.start = start;
        this.length = length;
        this.currentorderedcolumn = currentorderedcolumn;
        this.sorttext = sorttext;
        this.order = order;
    }
    //#endregion

    $(document).on('click', '.lnk_part', function (e) {
        var row = $(this).parents('tr');
        var data = partsSearchdt.row(row).data();
        var titletext = $('#partsearchtitle').text();
        localStorage.setItem("partstatustext", titletext);
        //V2-840
        if (layoutType == 1) {
            PartId = $(this).attr('id');
            var currentcardviewstate = new cardviewstate(grdcardcurrentpage, cardviewstartvalue, cardviewlwngth, currentorderedcolumn, $('#btnsortmenu').text(), currentorder);
            localStorage.setItem("PARTCURRENTCARDVIEWSTATE", JSON.stringify(currentcardviewstate));
            localStorage.setItem("PARTDETAILFROM", "CV");
        }
        else {
            var data = partsSearchdt.row(row).data();
            PartId = data.PartId;
        }

        $.ajax({
            //url: "/Parts/PartDetails",
            url: "/Parts/PartDetailsDynamic",
            type: "POST",
            data: { PartId: PartId },
            dataType: 'html',
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                $('#renderparts').html(data);
                $(document).find('#spnlinkToSearch').text(titletext);
            },
            complete: function () {
                LoadActivity(PartId);
                LoadImages(PartId);
                generatePartsVendorCatalogItemGrid();
                generatePOPRdataTable(PartId);
                ZoomImage($(document).find('#EquipZoom'));
                //#region V2-808
                var isMobile = CheckLoggedInFromMob();
                if (isMobile) {
                    $(document).find('#divAddImage').remove();
                }
                else {
                    $(document).find('#liAddimage').remove();
                }
                //#endregion
                CloseLoader();
                //Dropzone.autoDiscover = false;
                //if ($(document).find('#dropzoneForm').length > 0) {
                //    var myDropzone = new Dropzone("div#dropzoneForm", {
                //        url: "../Base/SaveUploadedFile",
                //        addRemoveLinks: true,
                //        paramName: 'file',
                //        maxFilesize: 10, // MB
                //        maxFiles: 4,
                //        dictDefaultMessage: getResourceValue("FileUploadAlert"),//'Drag files here to upload, or click to select one',
                //        acceptedFiles: ".jpeg,.jpg,.png",
                //        init: function () {
                //            this.on("removedfile", function (file) {
                //                if (file.type != 'image/jpeg' && file.type != 'image/jpg' && file.type != 'image/png') {
                //                    ShowErrorAlert(getResourceValue("spnValidImage"));
                //                }
                //            });
                //        },
                //        autoProcessQueue: true,
                //        dictRemoveFile: "X",
                //        uploadMultiple: true,
                //        dictRemoveFileConfirmation: getResourceValue("CancelAlertSure"),// "Do you want to remove this file ?",
                //        dictCancelUpload: "X",
                //        parallelUploads: 1,
                //        dictMaxFilesExceeded: "You can vist upload any more files.",
                //        success: function (file, response) {
                //            var imgName = response;
                //            file.previewElement.classList.add("dz-success");

                //            var radImage = '<a class="lnkContainer setImage" data-toggle="tooltip" title="Upload a selective Image!" data-image="' + file.name + '" style="cursor:pointer;"><i class="fa fa-upload" style="cursor:pointer;"></i></a>';
                //            $(file.previewElement).append(radImage);
                //        },
                //        error: function (file, response) {

                //            if (file.size > (1024 * 1024 * 10)) // not more than 10mb
                //            {
                //                ShowImageSizeExceedAlert();
                //            }
                //            file.previewElement.classList.add("dz-error");
                //            var _this = this;
                //            _this.removeFile(file);

                //        }
                //    });

                //}
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
    $(document).on('click', '.btnCross', function () {
        run = true;
        var btnCrossedId = $(this).parent().attr('id');
        var searchtxtId = btnCrossedId.split('_')[1];
        $('#' + searchtxtId).val('').trigger('change.select2');

        $(this).parent().remove();
        if (searchtxtId == 'StockType') {
            stockType = null;
        }
        if (filteritemcount > 0) filteritemcount--;
        AdvanceSearch();
        //V2-840
        if (layoutType == 2) {
            partsSearchdt.page('first').draw('page');
        }
        else {
            cardviewstartvalue = 0;
            grdcardcurrentpage = 1;
            LayoutFilterinfoUpdate()
            ShowCardView();
        }
    });
    $(document).find('.select2picker').select2({
    });
});
var order = '1';//Part Sorting
var orderDir = 'asc';//Part Sorting
function generatePartsDataTable() {
    if ($(document).find('#tblparts').hasClass('dataTable')) {
        partsSearchdt.destroy();
    }
    partsSearchdt = $("#tblparts").DataTable({
        colReorder: {
            fixedColumnsLeft: 2
        },
        rowGrouping: true,
        searching: true,
        //"pagingType": "full_numbers",
        "pagingType": "input",
        "bProcessing": true,
        "bDeferRender": true,
        serverSide: true,
        "order": [[1, "asc"]],
        stateSave: true,
        "stateSaveCallback": function (settings, data) {
            // Send an Ajax request to the server with the state object
            if (run == true) {
                if (data.order) {
                    data.order[0][0] = order;
                    data.order[0][1] = orderDir;
                }//Part Sorting
                var filterinfoarray = getfilterinfoarray($("#txtColumnSearch"), $('#advsearchsidebar'));
                $.ajax({
                    "url": gridStateSaveUrl,//"/Base/CreateUpdateState",
                    "data": {
                        GridName: "Part_Search",
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
                    GridName: "Part_Search"
                },
                "async": false,
                "dataType": "json",
                "success": function (json) {
                    filteritemcount = 0;
                    if (json.LayoutInfo !== '') {
                        var LayoutInfo = JSON.parse(json.LayoutInfo);//Part Sorting
                        order = LayoutInfo.order[0][0];//Part Sorting
                        orderDir = LayoutInfo.order[0][1]; //Part Sorting
                        callback(JSON.parse(json.LayoutInfo));
                        if (json.FilterInfo !== '') {
                            setsearchui(JSON.parse(json.FilterInfo), $("#txtColumnSearch"), $(".filteritemcount"), $("#advsearchfilteritems"));
                        }
                    }
                    else {
                        callback(json.LayoutInfo);
                    }

                }
            });
            //return o;
        },
        scrollX: true,
        fixedColumns: {
            leftColumns: 2,
        },
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'Parts List'
            },
            {
                extend: 'print',
                title: 'Parts List'
            },
            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Parts List',
                extension: '.csv'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                css: 'display:none',
                title: 'Parts List',
                orientation: 'landscape',
                pageSize: 'A2'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/Parts/GetPartsMainGrid",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.SearchText = LRTrim($(document).find('#txtColumnSearch').val());
                d.CustomQueryDisplayId = CustomQueryDisplayId;
                d.PartID = LRTrim($("#PartID").val());
                d.Description = LRTrim($("#Description").val());
                d.Manufacturer = LRTrim($("#Manufacturer").val());
                d.ManufacturerID = LRTrim($('#ManufacturerID').val());
                d.Section = LRTrim($("#Section").val());
                d.Row = LRTrim($('#Row').val());
                d.Shelf = LRTrim($('#Shelf').val());
                d.Bin = LRTrim($('#Bin').val());
                d.PlaceArea = LRTrim($('#PlaceArea').val());
                d.StockType = LRTrim($('#StockType').val());
                d.Order = order;//Part Sorting
                //d.orderDir = orderDir;//Part Sorting
            },
            "dataSrc": function (result) {
                let colOrder = partsSearchdt.order();
                orderDir = colOrder[0][1];

                HidebtnLoader("btnsortmenu");
                HidebtnLoaderclass("LoaderDrop");

                if (result.data.length == "0") {
                    // $(document).find('.import-export').attr('disabled', 'disabled');
                    $(document).find('#btnWoExport').attr('disabled', 'disabled');
                }
                else {
                    // $(document).find('.import-export').removeAttr('disabled');
                    $(document).find('#btnWoExport').removeAttr('disabled');
                }
                return result.data;
            },
            global: true
        },
        "columns":
            [
                {
                    "data": "PartId",
                    "bSortable": false,
                    className: 'select-checkbox dt-body-center',
                    targets: 0,
                    "name": "1",
                    'render': function (data, type, full, meta) {
                        if (partToQR.indexOf(data) != -1) {
                            return '<input type="checkbox" checked="checked" name="id[]" data-eqid="' + data + '" class="chksearch ' + data + '"  value="'
                                + $('<div/>').text(data).html() + '">';
                        }
                        else {
                            return '<input type="checkbox" name="id[]" data-eqid="' + data + '" class="chksearch ' + data + '"  value="'
                                + $('<div/>').text(data).html() + '">';
                        }
                    }
                },
                {
                    "data": "ClientLookupId",
                    "autoWidth": false,
                    "bSearchable": true,
                    "bSortable": true,
                    className: 'text-left',
                    "mRender": function (data, type, row) {
                        return '<a class=lnk_part href="javascript:void(0)">' + data + '</a>';
                    }
                },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2",
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                { "data": "OnHandQuantity", "autoWidth": true, "bSearchable": true, "bSortable": true, className: 'text-right' },
                { "data": "MinimumQuantity", "autoWidth": true, "bSearchable": true, "bSortable": true, className: 'text-right' },
                { "data": "Manufacturer", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "5" },
                { "data": "ManufacturerID", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "6" },
                { "data": "StockType", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Section", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Row", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Bin", "autoWidth": true, "bSearchable": true, "bSortable": true },

                { "data": "UPCCode", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Shelf", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "PreviousId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "PlaceArea", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "Consignment", "autoWidth": true, "bSearchable": true, "bSortable": true, className: 'text-center',
                    'render': function (data, type, full, meta) {
                        if (data == true) {
                            return '<input type="checkbox" checked="checked" class="chkpassive" >';
                        }
                        else {
                            return '<input type="checkbox" class="chkpassive" >';
                        }
                    }
                },
                //***V2-888***
                { "data": "Maximum", "autoWidth": true, "bSearchable": true, "bSortable": true }
                //***
            ],
        columnDefs: [
            {
                targets: [0, 1],
                className: 'noVis'
            }
        ],
        initComplete: function () {
            SetPageLengthMenu();
            //var currestsortedcolumn = $('#tblparts').dataTable().fnSettings().aaSorting[0][0];
            //var currestsortedorder = $('#tblparts').dataTable().fnSettings().aaSorting[0][1];
            //var column = this.api().column(currestsortedcolumn);
            //var columnId = $(column.header()).attr('id');
            //$(document).find('.srtpartcolumn').removeClass('sort-active');
            //$(document).find('.srtpartorder').removeClass('sort-active');
            //switch (columnId) {
            //    case "thPartid":
            //        $(document).find('.srtpartcolumn').eq(0).addClass('sort-active');
            //        break;
            //    case "thdescription":
            //        $(document).find('.srtpartcolumn').eq(1).addClass('sort-active');
            //        break;
            //    case "thmanufacturer":
            //        $(document).find('.srtpartcolumn').eq(2).addClass('sort-active');
            //        break;
            //    case "thmanufacturerid":
            //        $(document).find('.srtpartcolumn').eq(3).addClass('sort-active');
            //        break;
            //}
            //switch (currestsortedorder) {
            //    case "asc":
            //        $(document).find('.srtpartorder').eq(0).addClass('sort-active');
            //        break;
            //    case "desc":
            //        $(document).find('.srtpartorder').eq(1).addClass('sort-active');
            //        break;
            //}
            //$('#btnsortmenu').text(getResourceValue("spnSorting") + " : " + column.header().innerHTML);
            $("#PartGridAction :input").removeAttr("disabled");
            $("#PartGridAction :button").removeClass("disabled");
            $(document).on('click', '.chkpassive', function (e) {
                e.preventDefault();
            });
            // DisableExportButton($("#tblparts"), $(document).find('.import-export'));
            DisableExportButton($("#tblparts"), $(document).find('#btnWoExport'));
        }
    });
}
$(document).on('click', '#partmoreaddescription', function () {
    $(document).find('#partdetaildesmodal').modal('show');
    $(document).find('#partdetaildesmodaltext').text($(this).data("des"));
});
$(document).on('click', '#tblparts_paginate .paginate_button', function () {

    //#region V2-840
    //if (layoutType == 1) {
    //    var currentselectedpage = parseInt($(document).find('#tblparts_paginate .pagination').find('.active').text());
    //    cardviewlwngth = $(document).find('#partcardviewpagelengthdrp').val();
    //    cardviewstartvalue = cardviewlwngth * (parseInt($(this).find('.page-link').text()) - 1);
    //    var lastpage = parseInt($(this).prev('li').data('currentpage'));

    //    if ($(this).attr('id') == 'tbl_previous') {
    //        if (currentselectedpage == 1) {
    //            return false;
    //        }
    //        cardviewstartvalue = cardviewlwngth * (currentselectedpage - 1);
    //        grdcardcurrentpage = grdcardcurrentpage - 1;
    //    }
    //    else if ($(this).attr('id') == 'tbl_next') {
    //        if (currentselectedpage == lastpage) {
    //            return false;
    //        }
    //        cardviewstartvalue = cardviewlwngth * (currentselectedpage - 1);
    //        grdcardcurrentpage = grdcardcurrentpage + 1;
    //    }
    //    else if ($(this).attr('id') == 'tbl_first') {
    //        if (currentselectedpage == 1) {
    //            return false;
    //        }
    //        grdcardcurrentpage = 1;
    //        cardviewstartvalue = 0;
    //    }
    //    else if ($(this).attr('id') == 'tbl_last') {
    //        if (currentselectedpage == lastpage) {
    //            return false;
    //        }
    //        grdcardcurrentpage = parseInt($(this).prevAll('li').eq(1).text());
    //        cardviewstartvalue = cardviewlwngth * (grdcardcurrentpage - 1);
    //    }
    //    else {
    //        grdcardcurrentpage = $(this).data('currentpage');
    //    }

    //    LayoutUpdate('pagination');
    //    ShowCardView();
    //}
    if (layoutType == 1) {
        var currentselectedpage = parseInt($(document).find('#txtcurrentpage').val());
        cardviewlwngth = $(document).find('#partcardviewpagelengthdrp').val();
        cardviewstartvalue = cardviewlwngth * currentselectedpage;
        var lastpage = parseInt($(document).find('#spntotalpages').text());

        if ($(this).attr('id') == 'tbl_previous') {
            if (currentselectedpage == 1) {
                return false;
            }
            cardviewstartvalue = cardviewlwngth * (currentselectedpage - 2);
            grdcardcurrentpage = grdcardcurrentpage - 1;
        }
        else if ($(this).attr('id') == 'tbl_next') {
            if (currentselectedpage == lastpage) {
                return false;
            }
            cardviewstartvalue = cardviewlwngth * (currentselectedpage);
            grdcardcurrentpage = grdcardcurrentpage + 1;
        }
        else if ($(this).attr('id') == 'tbl_first') {
            if (currentselectedpage == 1) {
                return false;
            }
            grdcardcurrentpage = 1;
            cardviewstartvalue = 0;
        }
        else if ($(this).attr('id') == 'tbl_last') {
            if (currentselectedpage == lastpage) {
                return false;
            }
            grdcardcurrentpage = parseInt($(document).find('#spntotalpages').text());
            cardviewstartvalue = cardviewlwngth * (grdcardcurrentpage - 1);
        }

        LayoutUpdate('pagination');
        ShowCardView();
    }

    //#endregion
    run = true;
});
$(document).on('change', '#tblparts_length .searchdt-menu', function () {
    run = true;
});
$('#tblparts').find('th').click(function () {
    run = true;
    order = $(this).data('col');
    currentorderedcolumn = order; //V2-840
});
$(function () {
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            if (this.context[0].sInstance == "tblparts") {
                var searchText = LRTrim($("#txtColumnSearch").val());
                var clientLookupId = LRTrim($("#PartID").val());
                var Description = LRTrim($("#Description").val());
                var Manufacturer = LRTrim($("#Manufacturer").val());
                var ManufacturerID = LRTrim($('#ManufacturerID').val());
                var Section = LRTrim($("#Section").val());
                var Row = LRTrim($('#Row').val());
                var Bin = LRTrim($('#Bin').val());
                var StockType = $('#StockType').val();
                partsSearchdt = $("#tblparts").DataTable();
                var colname = order;//Part Sorting
                var coldir = orderDir;//Part Sorting

                var jsonResult = $.ajax({
                    "url": "/Parts/GetPartsPrintData?page=all",
                    "type": "get",
                    "datatype": "json",
                    data: {
                        SearchText: searchText,
                        CustomQueryDisplayId: CustomQueryDisplayId,
                        ClientLookupId: clientLookupId,
                        Description: Description,
                        Manufacturer: Manufacturer,
                        ManufacturerID: ManufacturerID,
                        Section: Section,
                        Row: Row,
                        Shelf: LRTrim($('#Shelf').val()),
                        Bin: Bin,
                        PlaceArea: LRTrim($('#PlaceArea').val()),
                        StockType: StockType,
                        colname: colname,//Part Sorting
                        coldir: coldir
                    },
                    success: function (result) {
                    },
                    async: false
                });
                var thisdata = JSON.parse(jsonResult.responseText).data;
                var visiblecolumnsIndex = $("#tblparts thead tr th").map(function (key) {
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

                    if (item.Section != null) {
                        item.Section = item.Section;
                    }
                    else {
                        item.Section = "";
                    }
                    if (item.OnHandQty != null) {
                        item.OnHandQty = item.OnHandQty;
                    }
                    else {
                        item.OnHandQty = "";
                    }
                    if (item.MinimumQty != null) {
                        item.MinimumQty = item.MinimumQty;
                    }
                    else {
                        item.MinimumQty = "";
                    }

                    if (item.Manufacturer != null) {
                        item.Manufacturer = item.Manufacturer;
                    }
                    else {
                        item.Manufacturer = "";
                    }
                    if (item.ManufacturerID != null) {
                        item.ManufacturerID = item.ManufacturerID;
                    }
                    else {
                        item.ManufacturerID = "";
                    }
                    if (item.StockType != null) {
                        item.StockType = item.StockType;
                    }
                    else {
                        item.StockType = "";
                    }
                    if (item.Row != null) {
                        item.Row = item.Row;
                    }
                    else {
                        item.Row = "";
                    }
                    if (item.Bin != null) {
                        item.Bin = item.Bin;
                    }
                    else {
                        item.Bin = "";
                    }
                    if (item.UPCCode != null) {
                        item.UPCCode = item.UPCCode;
                    }
                    else {
                        item.UPCCode = "";
                    }
                    if (item.Shelf != null) {
                        item.Shelf = item.Shelf;
                    }
                    else {
                        item.Shelf = "";
                    }
                    if (item.PreviousId != null) {
                        item.PreviousId = item.PreviousId;
                    }
                    else {
                        item.PreviousId = "";
                    }
                    if (item.PlaceArea != null) {
                        item.PlaceArea = item.PlaceArea;
                    }
                    else {
                        item.PlaceArea = "";
                    }
                    if (item.Consignment != null) {
                        if (item.Consignment == true) {
                            item.Consignment = getResourceValue("CancelAlertYes");
                        }
                        else {
                            item.Consignment = getResourceValue("CancelAlertNo");
                        }
                    }
                    else {
                        item.Consignment = "";
                    }
                    //******V2-888
                    if (item.Maximum != null) {
                        item.Maximum = item.Maximum;
                    }
                    else {
                        item.Maximum = "";
                    }
                    //********
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
                    header: $("#tblparts thead tr th div").not(":eq(0)").map(function (key) {
                        return this.innerHTML;
                    }).get()
                };
            }
            if (this.context[0].sInstance == "pvhistoryTable") {
                var daterange = $('#partsHistoryModel_HistoryDaterange').val();
                var partid = $(document).find('#PartModel_PartId').val();
                var partclientlookup = $(document).find('#partclientlookupid').text();
                var TransactionType = LRTrim($("#rgridadvsearchTransactionType").val());
                var Requestor_Name = LRTrim($("#rgridadvsearchRequestorName").val());
                var PerformBy_Name = LRTrim($("#rgridadvsearchPerformByName").val());
                var TransactionDate = LRTrim($("#rgridadvsearchTransactionDate").val());
                var TransactionQuantity = LRTrim($('#rgridadvsearchTransactionQuantity').val());
                var Cost = $('#rgridadvsearchCost').val();
                var ChargeType_Primary = LRTrim($("#rgridadvsearchChargeTypePrimary").val());
                var ChargeTo_ClientLookupId = LRTrim($("#rgridadvsearchChargeToName").val());
                var Account_ClientLookupId = LRTrim($("#rgridadvsearchAccountId").val());
                var PurchaseOrder_ClientLookupId = LRTrim($("#rgridadvsearchPurchaseOrderClientLookupId").val());
                var Vendor_ClientLookupId = LRTrim($('#rgridadvsearchVendorClientLookupId').val());
                var Vendor_Name = $('#rgridadvsearchVendorName').val();

                var jsonResult = $.ajax({
                    url: '/Parts/GetPartsHistoryPrintData?page=all',
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
                    header: $("#pvhistoryTable thead tr th").map(function (key) {
                        return this.innerHTML;
                    }).get()
                };
            }
            if (this.context[0].sInstance == "receiptsTable") {
                var partid = $(document).find('#PartModel_PartId').val();
                var daterange = $(document).find('#partsReceiptModel_receiptdtselector').val();
                var partclientlookup = $(document).find('#partclientlookupid').text();
                var PurchaseOrderId = LRTrim($("#PurchaseOrder").val());
                var ReceivedDate = LRTrim($("#ReceiptDate").val());
                var VendorClientLookupId = LRTrim($("#rgridReceiptadvsearchVendorClientLookupId").val());
                var VendorName = LRTrim($("#VendorName").val());
                var OrderQuantity = LRTrim($("#Quantity").val());
                var UnitCost = LRTrim($('#UnitCost').val());
                var jsonResult = $.ajax({
                    url: '/Parts/GetPartsReceiptPrintData?page=all',
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
                    header: $("#receiptsTable thead tr th").map(function (key) {
                        return this.innerHTML;
                    }).get()
                };
            }
        }
    });
});
//$(document).find('.srtpartcolumn').click(function () {
//    ShowbtnLoader("btnsortmenu");
//    order = $(this).data('col');//Part Sorting
//    var txtColumnSearch = LRTrim($(document).find('#txtColumnSearch').val());
//    if (txtColumnSearch != "") {
//        TextSearch();//Part Sorting
//    }
//    else {
//        $("#tblparts").DataTable().draw();//Part Sorting
//    }
//    $('#tblparts').DataTable().draw();//Part Sorting
//    $('#btnsortmenu').text(getResourceValue("spnSorting") + " : " + $(this).text());
//    $(document).find('.srtpartcolumn').removeClass('sort-active');
//    $(this).addClass('sort-active');
//    run = true;
//});
//$(document).find('.srtpartorder').click(function () {
//    ShowbtnLoader("btnsortmenu");
//    orderDir = $(this).data('mode');//Part Sorting
//    var txtColumnSearch = LRTrim($(document).find('#txtColumnSearch').val());
//    if (txtColumnSearch != "") {
//        TextSearch();//Part Sorting
//    }
//    else {
//        $("#tblparts").DataTable().draw();//Part Sorting
//    }

//    $(document).find('.srtpartorder').removeClass('sort-active');
//    $(this).addClass('sort-active');
//    run = true;
//});
$(document).on('keyup', '#partsearchtxtbox', function (e) {
    var tagElems = $(document).find('#partsearchListul').children();
    $(tagElems).hide();
    for (var i = 0; i < tagElems.length; i++) {
        var tag = $(tagElems).eq(i);
        if ($(tag).text().toLowerCase().includes($(this).val().toLowerCase()) == true || $(this).val().toLowerCase().includes($(tag).text().toLowerCase()) == true) {
            $(tag).show();
        }
    }
});
$(document).on('click', '.partsearchdrpbox', function (e) {
    if ($(document).find('#txtColumnSearch').val() !== '')
        $("#advsearchfilteritems").html('');
    $(document).find('#txtColumnSearch').val('');
    run = true;
    if ($(this).attr('id') != '0') {
        $('#partsearchtitle').text($(this).text());
    }
    else {
        $('#partsearchtitle').text("Part");
    }
    $(".searchList li").removeClass("activeState");
    $(this).addClass('activeState');
    $(document).find('#searcharea').hide("slide");
    var optionval = $(this).attr('id');
    localStorage.setItem("CURRENTTABSTATUS", optionval);
    CustomQueryDisplayId = optionval;
    ShowbtnLoaderclass("LoaderDrop");
    //V2-840
    if (layoutType == 2) {
        partsSearchdt.page('first').draw('page');
    }
    else {
        cardviewstartvalue = 0;
        grdcardcurrentpage = 1;
        LayoutFilterinfoUpdate();
        ShowCardView();
    }
});
//#endregion
//#region Parts Add-Edit
$.validator.setDefaults({ ignore: null });
$(document).on('click', ".addparts", function (e) {
    $.ajax({
        //url: "/Parts/AddParts",
        url: "/Parts/AddPartsDynamic",
        type: "GET",
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderparts').html(data);
        },
        complete: function () {
            SetPartControls();
        },
        error: function () {
            CloseLoader();
        }
    });
})
$(document).on('click', "#editparts", function (e) {
    e.preventDefault();
    var partId = $('#PartModel_PartId').val();
    $.ajax({
        url: "/Parts/EditPartsDynamic",
        type: "GET",
        dataType: 'html',
        data: { PartId: partId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderparts').html(data);
        },
        complete: function () {
            SetPartControls();
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
                SuccessAlertSetting.text = getResourceValue("AddPartsAlerts");
            }
            else {
                SuccessAlertSetting.text = getResourceValue("UpdatePartsAlerts");
            }
            swal(SuccessAlertSetting, function () {
                RedirectToPartDetail(data.PartId);
            });
        }
        else {
            ResetErrorDiv();
            $('#identificationtab').addClass('active').trigger('click');
            SuccessAlertSetting.text = getResourceValue("AddPartsAlerts");
            swal(SuccessAlertSetting, function () {
                $(document).find('form').trigger("reset");
                $(document).find('form').find("select").not("#colorselector").val("").trigger('change');
                $(document).find('form').find("select").removeClass("input-validation-error");
                $(document).find('form').find("input").removeClass("input-validation-error");
                $(document).find('form').find("textarea").removeClass("input-validation-error");
            });
        }
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', "#btnCancelAddPart", function () {
    var PartId = $('#PartModel_PartId').val();
    if (typeof PartId !== "undefined" && PartId != 0) {
        swal(CancelAlertSetting, function () {
            RedirectToPartDetail(PartId);
        });
    }
    else {
        swal(CancelAlertSetting, function () {
            window.location.href = "../Parts/Index?page=Inventory_Parts";
        });
    }
});
$(document).on('click', "#brdpart", function () {
    var PartId = $(this).attr('data-val');
    RedirectToPartDetail(PartId)
});
$(document).on('click', '#btnSaveAnotherOpenPart,#btnSavePart', function () {
    if ($(document).find("form").valid()) {
        return;
    }
    else {
        var errorTab = $(document).find(".input-validation-error").parents('div:eq(0)').attr('id');
        if (errorTab == "Identification") {
            $('#identificationtab').trigger('click');
        }
        else if (errorTab == "Location") {
            $('#locationtab').trigger('click');
        }
        else if (errorTab == "QuantitiesCost") {
            $('#qcosttab').trigger('click');
        }
    }
});
//#endregion
//#region part detail
$(document).on('click', "#partst", function () {
    $(document).find('#btnidentification').addClass('active');
    $(document).find('#Identification').show();
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
function clearDropzone() {
    deleteServer = false;
    if ($(document).find('#dropzoneForm').length > 0) {
        Dropzone.forElement("div#dropzoneForm").destroy();
    }
}
$(document).on('click', '.setImage', function () {
    var PartId = $('#PartModel_PartId').val();
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
                $('#EquipZoom').attr('src', data.imageurl);
                $('.equipImg').attr('src', data.imageurl);
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

$(document).on('click', '#linkToSearch', function () {

    var titletext = $('#spnlinkToSearch').text();
    window.location.href = "../Parts/PartFromPartDetails?partStatus=" + titletext;
});
//#endregion
//#region common
function RedirectToPartDetail(PartId, mode) {
    $.ajax({
        // url: "/Parts/PartDetails",
        url: "/Parts/PartDetailsDynamic",
        type: "POST",
        dataType: 'html',
        data: { PartId: PartId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderparts').html(data);
            $(document).find('#spnlinkToSearch').text(localStorage.getItem("partstatustext"));
        },
        complete: function () {
            LoadActivity(PartId);
            LoadImages(PartId);
            generatePOPRdataTable(PartId);
            ZoomImage($(document).find('#EquipZoom'));
            generatePartsVendorCatalogItemGrid();
            //#region V2-808
            var isMobile = CheckLoggedInFromMob();
            if (isMobile) {
                $(document).find('#divAddImage').remove();
            }
            else {
                $(document).find('#liAddimage').remove();
            }
            //#endregion
            CloseLoader();

            //Dropzone.autoDiscover = false;
            //if ($(document).find('#dropzoneForm').length > 0) {
            //    var myDropzone = new Dropzone("div#dropzoneForm", {
            //        url: "../Base/SaveUploadedFile",

            //        addRemoveLinks: true,
            //        paramName: 'file',
            //        maxFilesize: 10, // MB
            //        maxFiles: 4,
            //        dictDefaultMessage: getResourceValue("FileUploadAlert"),
            //        acceptedFiles: ".jpeg,.jpg,.png",
            //        init: function () {
            //            this.on("removedfile", function (file) {
            //                if (file.type != 'image/jpeg' && file.type != 'image/jpg' && file.type != 'image/png') {
            //                    ShowErrorAlert(getResourceValue("spnValidImage"));
            //                }
            //            });
            //        },
            //        addRemoveLinks: true,
            //        autoProcessQueue: true,
            //        dictRemoveFile: "X",
            //        uploadMultiple: true,
            //        dictRemoveFileConfirmation: getResourceValue("CancelAlertSure"),
            //        dictCancelUpload: "X",
            //        parallelUploads: 1,
            //        dictMaxFilesExceeded: "You can not upload any more files.",
            //        success: function (file, response) {
            //            var imgName = response;
            //            file.previewElement.classList.add("dz-success");
            //            var radImage = '<a class="lnkContainer setImage" data-toggle="tooltip" title="Upload a selective Image!" data-image="' + file.name + '" style="cursor:pointer;"><i class="fa fa-upload" style="cursor:pointer;"></i></a>';
            //            $(file.previewElement).append(radImage);
            //        },
            //        error: function (file, response) {
            //            if (file.size > (1024 * 1024 * 10)) // not more than 10mb
            //            {
            //                ShowImageSizeExceedAlert();
            //            }
            //            file.previewElement.classList.add("dz-error");
            //            var _this = this;
            //            _this.removeFile(file);

            //        }
            //    });
            //    SetFixedHeadStyle();
            //}
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
            if (mode === "notes") {
                $('#notest').trigger('click');
                $('#colorselector').val('Notes');
            }
            if (mode === "attachment") {
                $('#attachmentst').trigger('click');
                $('#colorselector').val('Attachments');
            }
            if (mode === "vendor") {
                $('#vendorst').trigger('click');
                $('#colorselector').val('NewVendors');
            }
            if (mode === "equipment") {
                $('#equipmentt').trigger('click');
                $('#colorselector').val('NewEquipment');
            }
            if (mode === "history") {
                $('#historyt').trigger('click');
                $('#colorselector').val('History');
            }
            if (mode === "receipt") {
                $('#receiptt').trigger('click');
                $('#colorselector').val('Receiptscontainer');
            }
            if (mode === "ReviewSite") {
                $('#reviewtt').trigger('click');
                $('#colorselector').val('ReviewSitecontainer');
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
}
function SetPartControls() {
    CloseLoader();
    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse(document);
    $('input, form').blur(function () {
        if ($(this)) {
            $(this).valid();
        }
    });
    $('.select2picker, form').change(function () {
        var areaddescribedby = $(this).attr('aria-describedby');
        if ($(this) && $(this).valid()) {
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

//#endregion
//#region QR Code
$(document).on('change', '.chksearch', function () {
    var data = partsSearchdt.row($(this).parents('tr')).data();
    if (!this.checked) {
        selectedcount--;
        var index = partToQR.indexOf(data.PartId);
        partToQR.splice(index, 1);
        PartToClientLookupIdQRcode.splice(index, 1);
    }
    else {
        partToQR.push(data.PartId);
        selectedcount = selectedcount + partToQR.length;
        var Location = '';
        var PlaceArea = data.PlaceArea;
        var Section = data.Section;
        var Row = data.Row;
        var Shelf = data.Shelf;
        var Bin = data.Bin;
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
        PartToClientLookupIdQRcode.push(data.ClientLookupId + '][' + data.Description + '][' + (Location == null ? " " : Location) + '][' + data.IssueUnit + '][' + data.MinimumQuantity + '][' + data.Maximum + '][' + data.Manufacturer + '][' + data.ManufacturerID);
    }
    if (partToQR.length > 0) {
        $('#printQrcode').removeAttr("disabled");
        $(".actionBar").hide();
        $(".updateArea").fadeIn();
    }
    else {
        $('#printQrcode').prop("disabled", "disabled");
        $(".updateArea").hide();
        $(".actionBar").fadeIn();
    }
    $('.itemcount').text(partToQR.length);
});
$('#printQrcode').on('click', function () {
    var partClientLookups = PartToClientLookupIdQRcode;
    if (partClientLookups.length > 50) {
        var datamsg = 'Cannot Print more than 50 items';
        var msg = getResourceValue(datamsg);
        GenericSweetAlertMethod(msg);
        return false;
    }
    else {
        $.ajax({
            //url: "/Parts/PartQRcode",
            url: "/Parts/PartDetailsQRcode",
            data: {
                PartClientLookups: partClientLookups
            },
            type: "POST",
            beforeSend: function () {
                ShowLoader();
            },
            datatype: "html",
            success: function (data) {
                $('#printPartqrcodemodalcontainer').html(data);
            },
            complete: function () {
                //$('#printPartQrCodeModal').modal('show');
                $('#printDetailsPartQrCodeModal').modal('show');
                $('#btnGenerateQr').prop("disabled", "disabled");
                $(document).find('.itemQRcount').text(partClientLookups.length);
                $(document).find('.itemQRcount').parent().css('display', 'block');
                CloseLoader();
            },
            error: function () {
                CloseLoader();
            }
        });
    }
});
function PartsQROnSuccess(data) {
    CloseLoader();
    if (data.success === 0) {
        var smallLabel = $('#SmallLabel').prop('checked');
        //window.open('/Parts/QRCodeGenerationUsingRotativa?SmallLabel=' + encodeURI(smallLabel), '_blank');
        window.open('/Parts/QRCodeGenerationUsingDevExpress?SmallLabel=' + encodeURI(smallLabel), '_blank');

        $('#printDetailsPartQrCodeModal').modal('hide');
        partToQR = [];
        PartToClientLookupIdQRcode = [];
        //-- when called from grid         
        if ($(document).find('#tblparts').find('.chksearch').length > 0) {
            $('#printQrcode').prop("disabled", "disabled");
            $('.itemQRcount').text(0);
            $('.itemcount').text(0);
            $(document).find('.DTFC_LeftBodyLiner').find('.chksearch').prop('checked', false);
            $(document).find(".updateArea").hide();
            $(document).find(".actionBar").fadeIn();
        }
        //--
    }
}
$(document).on('click', '#printDetailsPartQrcode', function () {
    var ClientLookupId = $(document).find('#PartModel_ClientLookupId').val();
    var Description = $(document).find('#PartModel_Description').val();
    var Location = '';
    var PlaceArea = $(document).find('#PartModel_PlaceArea').val();
    var Section = $(document).find('#PartModel_Section').val();
    var Row = $(document).find('#PartModel_Row').val();
    var Shelf = $(document).find('#PartModel_Shelf').val();
    var Bin = $(document).find('#PartModel_Bin').val();

    var IssueUnit = $(document).find('#PartModel_IssueUnit').val();
    var QtyMinimum = $(document).find('#PartModel_Minimum').val();
    var QtyMaximum = $(document).find('#PartModel_Maximum').val();
    var Manufacturer = $(document).find('#PartModel_Manufacturer').val();
    var ManufacturerID = $(document).find('#PartModel_ManufacturerID').val();
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
    //var Consignment = $(document).find('#PartModel_Consignment').val();
    //var RepairablePart = $(document).find('#PartModel_RepairablePart').val();
    PartToClientLookupIdQRcode.push(ClientLookupId + '][' + Description + '][' + (Location == null ? " " : Location) + '][' + IssueUnit + '][' + QtyMinimum + '][' + QtyMaximum + '][' + Manufacturer + '][' + ManufacturerID);
    var partClientLookups = PartToClientLookupIdQRcode;
    $.ajax({
        url: "/Parts/PartDetailsQRcode",
        data: {
            PartClientLookups: partClientLookups
        },
        type: "POST",
        beforeSend: function () {
            ShowLoader();
        },
        datatype: "html",
        success: function (data) {
            $('#printPartDetailsqrcodemodalcontainer').html(data);
        },
        complete: function () {
            $('#printDetailsPartQrCodeModal').modal('show');
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});
//#endregion
//#region ColumnVisibility
$(document).on('click', '#liCustomize', function () {
    funCustomizeBtnClick(partsSearchdt, true);
});
$(document).on('click', '.saveConfig', function () {
    var colOrder = [0, 1];
    funCustozeSaveBtn(partsSearchdt, colOrder);
    run = true;
    partsSearchdt.state.save(run);
});
//#endregion
//#region BulkUpdate
$('#bulkUpdate').on('click', function () {
    var PartIds = partToQR;
    $.ajax({
        url: "/Parts/ShowBulkUpdate",
        data: {
            PartIds: PartIds
        },
        type: "POST",
        beforeSend: function () {
            ShowLoader();
        },
        datatype: "html",
        success: function (data) {
            $('#partbulkupdatemodalcontainer').html(data);
        },
        complete: function () {
            $(document).find('.select2picker').select2({});
            $('#PartBulksidebar').addClass('active');
            $('.overlay').fadeIn();
            $('.collapse.in').toggleClass('in');
            $('a[aria-expanded=true]').attr('aria-expanded', 'false');
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', '#btnCancelBulkUpdate,#dismiss, .overlay', function () {
    $('#PartBulksidebar').removeClass('active');
    $('.overlay').fadeOut();
});
function BulkUploadOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        var message = "Total Records Updated : " + data.UpdatedItemCount;
        SuccessAlertSetting.text = message;
        swal(SuccessAlertSetting, function () {
            partToQR = [];
            $('#PartBulksidebar').removeClass('active');
            $('.overlay').fadeOut();
            $(document).find('.chksearch').prop("checked", false);
            $('.updateArea').hide();
            $(document).find('.DTFC_LeftBodyLiner').find('.chksearch').prop('checked', false);
            $(".actionBar").fadeIn();
            $('#partbulkupdatemodalcontainer').empty();
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data, "#PartBulksidebar");
    }
    partsSearchdt.page('first').draw('page');
}
//#endregion BulkUpdate

//#region CKEditor
$(document).on("focus", "#parttxtcommentsnew", function () {
    $(document).find('.ckeditorarea').show();
    var noteitem = $(document).find('.editcomments').parents('.kt-notes__item');
    noteitem.find('.ckeditorarea').hide();
    noteitem.find('.kt-notes__body').show();
    noteitem.find('.commenteditdelearea').show();
    //ClearEditor();
    LoadCkEditor('parttxtcomments');
    $("#parttxtcommentsnew").hide();
    $(".ckeditorfield").show();
});

$(document).on('click', '#btnsavecommands', function () {
    var selectedUsers = [];
    const data = getDataFromTheEditor();
    if (!data) {
        return false;
    }
    var partid = $(document).find('#PartModel_PartId').val();
    var PartClientLookupId = $(document).find('#PartModel_ClientLookupId').val();
    var noteId = 0;
    if (LRTrim(data) == "") {
        return false;
    }
    else {
        $(document).find('.ck-content').find('.mention').each(function (item, index) {
            selectedUsers.push($(index).data('user-id'));
        });
        $.ajax({
            url: '/Parts/AddOrUpdateComment',
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
                        RedirectToPartDetail(partid);
                    });
                }
                else {
                    ShowGenericErrorOnAddUpdate(data);
                    CloseLoader();
                }
            },
            complete: function () {
                ClearEditor();
                $("#parttxtcommentsnew").show();
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
    $("#parttxtcommentsnew").show();
    $(".ckeditorfield").hide();
});
$(document).on('click', '.editcomments', function () {
    $(document).find(".ckeditorarea").each(function () {
        $(this).html('');
    });
    $("#parttxtcommentsnew").show();
    $(".ckeditorfield").hide();
    var notesitem = $(this).parents('.kt-notes__item').eq(0);
    notesitem.find('.ckeditorarea').html(CreateEditorHTML('parttxtcommentsEdit'));
    var noteitem = $(document).find('.editcomments').parents('.kt-notes__item');
    noteitem.find('.kt-notes__body').show();
    noteitem.find('.commenteditdelearea').show();

    var rawHTML = $.parseHTML($(this).parents('.kt-notes__item').find('.kt-notes__body').find('.originalContent').html());
    LoadCkEditorEdit('parttxtcommentsEdit', rawHTML);
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
    var partid = $(document).find('#PartModel_PartId').val();
    var ClientLookupId = $(document).find('#PartModel_ClientLookupId').val();
    var noteId = $(this).parents('.kt-notes__item').find('.editcomments').attr('id');
    var updatedindex = $(this).parents('.kt-notes__item').find('.hdnupdatedindex').val();
    $.ajax({
        url: '/Parts/AddOrUpdateComment',
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
                    RedirectToPartDetail(partid);

                });
            }
            else {
                ShowGenericErrorOnAddUpdate(data);
                CloseLoader();
            }
        },
        complete: function () {
            //ClearEditorEdit();
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
                        RedirectToPartDetail($(document).find('#PartModel_PartId').val());
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
        "url": "/Parts/LoadComments",
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
//#region POPR
function generatePOPRdataTable(PartId) {
    if ($(document).find('#tblPOPR').hasClass('dataTable')) {
        dtPOPRTable.destroy();
    }
    dtPOPRTable = $("#tblPOPR").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "bProcessing": true,
        "order": [[0, "asc"]],
        stateSave: false,
        "pagingType": "full_numbers",
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Parts/PopulatePOPR",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.PartId = PartId;
            },
            "dataSrc": function (result) {
                rCount = result.data.length;
                return result.data;
            },
            global: true
        },
        columnDefs: [
            {

            }
        ],
        "columns":
            [
                {
                    "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<a class=link_RedirectPOPR_detail href="javascript:void(0)" data-type="' + row.POType + '" data-poprid=' + row.PoPrId + ' data-status="' + row.Status + '">' + data + '</a>';
                    }
                },
                { "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Vendor", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "VendorName", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "CreateDate", "autoWidth": true, "bSearchable": true, "bSortable": true }

            ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}
//#endregion
//#region Activity
function LoadActivity(PartID) {
    $.ajax({
        "url": "/Parts/LoadActivity",
        data: { PartID: PartID },
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

            LoadComments(PartID);
        }
    });
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
                //V2-840

                if (layoutType == 2) {
                    partsSearchdt.page('first').draw('page');
                }
                else {
                    cardviewstartvalue = 0;
                    grdcardcurrentpage = 1;
                    LayoutFilterinfoUpdate();
                    ShowCardView();
                }

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
    var partcurrentstatus = localStorage.getItem("CURRENTTABSTATUS");
      if (partcurrentstatus != 'undefined' && partcurrentstatus != null) {
        CustomQueryDisplayId = partcurrentstatus;
        $('#partsearchListul li').each(function (index, value) {
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
        //V2-840
        // partsSearchdt.page('first').draw('page');
        if (layoutType == 2) {
            partsSearchdt.page('first').draw('page')
        }
        else {
            cardviewstartvalue = 0;
            grdcardcurrentpage = 1;
            LayoutFilterinfoUpdate();
            ShowCardView();
        }
    }
    var container = $(document).find('#searchBttnNewDrop');
    container.hide("slideToggle");
}
function clearAdvanceSearch() {
    filteritemcount = 0;
    $('#advsearchsidebar').find('input:text').val('');
    $(document).find("#StockType").val("").trigger('change');
    $(document).find("#Consignment").val("").trigger('change');
    $('.filteritemcount').text(filteritemcount);
    $('#advsearchfilteritems').find('span').html('');
    $('#advsearchfilteritems').find('span').removeClass('tagTo');
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

//#region V2-389
function getfilterinfoarray(txtsearchelement, advsearchcontainer) {
    var filterinfoarray = [];
    var f = new filterinfo('searchstring', LRTrim(txtsearchelement.val()));
    filterinfoarray.push(f);
    advsearchcontainer.find('.adv-item').each(function (index, item) {
        if ($(this).parent('div').is(":visible")) {
            f = new filterinfo($(this).attr('id'), $(this).val());
            filterinfoarray.push(f);

            if ($(this).parent('div').find('div').hasClass('range-timeperiod')) {
                if ($(this).parent('div').find('input').val() !== '' && $(this).val() == '10') {
                    f = new filterinfo('this-' + $(this).attr('id'), $(this).parent('div').find('input').val());
                    filterinfoarray.push(f);
                }
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
                $('#' + item.key).val(item.value).trigger('change');
                if (item.value) {
                    filteritemcount++;
                    searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + item.key + '">' + $("label[for='" + item.key + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
                }
            }
            advcountercontainer.text(filteritemcount);
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    searchstringcontainer.html(searchitemhtml);

}
//#endregion

//#region v2-501
$(document).on('click', '.link_RedirectPOPR_detail', function () {
    var PoPrType = $(this).attr('data-type');
    var poprid = $(this).attr('data-poprid');
    var status = $(this).attr('data-status');
    if (PoPrType == "Purchase Order") {
        //code for redirect to purchase order
        var PurchaseOrderId = poprid; //need to change 
        clearDropzone();
        window.location.href = "../Purchasing/DetailFromPart?PurchaseOrderId=" + PurchaseOrderId + "&Status=" + status;
    }
    else {
        //code for redirect to purchase Request
        var PurchaseRequestId = poprid; //need to change 
        clearDropzone();
        window.location.href = "../PurchaseRequest/DetailFromPart?PurchaseRequestId=" + PurchaseRequestId + "&Status=" + status;
    }
});
//#endregion

//#region Webcam v2-555
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
    //$('body').addClass('sudip');
}

$(document).delegate("#webcam-switch", "click", function () {
    InitWebCam();
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
    //if (UseWebcam.CameraCounts() > 1) {
    //    $("#cameraFlip").removeClass('d-none');
    //} else {
    //    $("#cameraFlip").addClass('d-none');
    //}

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
    //document.querySelector('#download-photo').href = picture;
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
    UseWebcam.StopCamera();
    $("#webcam-switch").prop("checked", false).change();
    $('#radio-1').prop('checked', true).trigger('click');
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
                $('#radio-1').prop('checked', true).trigger('click');
                $('.md-modal').removeClass('md-show');
                $('body').removeClass('hideScroll');
                var PartId = $('#PartModel_PartId').val();
                SaveUploadedFileToServer(PartId, file.name);
            },
            error: function (xhr) {
                CloseLoader();
            }
        });
    }
});

//#endregion

//#region V2-641
function PartsEditOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        var message;
        SuccessAlertSetting.text = getResourceValue("UpdatePartsAlerts");
        swal(SuccessAlertSetting, function () {
            RedirectToPartDetail(data.partid);
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
//#endregion
//#region V2-840 Change View
var layoutVal;
$(document).on('click', "#cardviewliLayout", function () {
    if (layoutType == 1) {
        return;
    }
    ShowbtnLoader("layoutsortmenu");
    layoutType = 1;
    localStorage.setItem("layoutTypeval", layoutType);
    layoutVal = $(document).find('#cardviewliLayout').text();
    $('#layoutsortmenu').text("Layout" + " : " + layoutVal);
    $(document).find('#Active').hide();
    $(document).find('#ActiveCard').show();
    HidebtnLoader("layoutsortmenu");
    $(document).find('#liCustomize').prop("disabled", true);
    var info = partsSearchdt.page.info();
    var pageclicked = info.page;
    cardviewlwngth = info.length;
    cardviewstartvalue = cardviewlwngth * pageclicked;
    grdcardcurrentpage = pageclicked + 1;
    currentorderedcolumn = order;
    currentorder = orderDir;
    GetDatatableLayout();
    LayoutFilterinfoUpdate();
    ShowCardView();
});
function ShowCardView() {
    $.ajax({
        url: '/Parts/GetCardViewData',
        type: 'POST',
        data: {
            currentpage: grdcardcurrentpage,
            start: cardviewstartvalue,
            length: cardviewlwngth,
            currentorderedcolumn: currentorderedcolumn,
            currentorder: currentorder,
            CustomQueryDisplayId: CustomQueryDisplayId,
            SearchText: LRTrim($(document).find('#txtColumnSearch').val()),
            PartID: LRTrim($("#PartID").val()),
            Description: LRTrim($("#Description").val()),
            Manufacturer: LRTrim($("#Manufacturer").val()),
            ManufacturerID: LRTrim($('#ManufacturerID').val()),
            Section: LRTrim($("#Section").val()),
            Row: LRTrim($('#Row').val()),
            Shelf: LRTrim($('#Shelf').val()),
            Bin: LRTrim($('#Bin').val()),
            PlaceArea: LRTrim($('#PlaceArea').val()),
            StockType: LRTrim($('#StockType').val()),
            Order: order
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#ActiveCard').show();
            $(document).find('#ActiveCard').html(data).show();
            $(document).find('#tblparts_paginate li').each(function (index, value) {
                $(this).removeClass('active');
                if ($(this).data('currentpage') == grdcardcurrentpage) {
                    $(this).addClass('active');
                }
            });
        },
        complete: function () {
            //if ($(document).find('#spnNoData').length > 0) {
            //    $(document).find('.import-export').prop('disabled', true);
            //}
            //else {
            //    $(document).find('.import-export').prop('disabled', false);
            //}
            //#region V2-886 Page Navigation Show Hide
            if ($(document).find('#spntotalpages').text() <= 1) {
                $(document).find('.pagenavdiv').hide();
            }
            else {
                $(document).find('.pagenavdiv').show();
            }
            //#endregion
            $(document).find('#partcardviewpagelengthdrp').select2({ minimumResultsForSearch: -1 }).val(cardviewlwngth).trigger('change.select2');
            HidebtnLoader("btnsortmenu");
            HidebtnLoader("layoutsortmenu");
            HidebtnLoader("SrchBttnNew");
            HidebtnLoader("sidebarCollapse");
            HidebtnLoader("txtColumnSearch");

            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', "#tableviewliLayout", function () {
    if (layoutType == 2) {
        return;
    }
    $(document).find('#tblparts').show();
    layoutType = 2;
    localStorage.setItem("layoutTypeval", layoutType);
    ShowbtnLoader("layoutsortmenu");
    layoutVal = $(document).find('#tableviewliLayout').text();
    $('#layoutsortmenu').text("Layout" + " : " + layoutVal);
    $(document).find('#ActiveCard').hide();
    $(document).find('#Active').show();
    $('#liCustomize').prop("disabled", false);
    HidebtnLoader("layoutsortmenu");
    localStorage.removeItem("PARTDETAILFROM");
    localStorage.removeItem("PARTCURRENTCARDVIEWSTATE");
    if (partsSearchdt) {
        partsSearchdt.page.len(cardviewlwngth).order([[currentorderedcolumn, currentorder]]).page(grdcardcurrentpage - 1).draw('page');
        $(document).find('#tblparts_length .searchdt-menu').val(cardviewlwngth).trigger('change.select2');
    }
    else {
        generatePartsDataTable();
    }

});
//cardview pagelength change
$(document).on('change', '#partcardviewpagelengthdrp', function () {
    cardviewlwngth = $(this).val();
    grdcardcurrentpage = parseInt(cardviewstartvalue / cardviewlwngth) + 1;
    cardviewstartvalue = parseInt((grdcardcurrentpage - 1) * cardviewlwngth);

    LayoutUpdate('pagelength');
    ShowCardView();
});
//#region Card view griddatalayout update
//For column , order , page and page length change
function LayoutUpdate(area) {
    $.ajax({
        "url": "/Base/GetLayout",
        "data": {
            GridName: "Part_Search"
        },
        "async": false,
        "dataType": "json",
        "success": function (json) {

            hGridfilteritemcount = 0;
            if (json.LayoutInfo !== '') {
                var gridstate = JSON.parse(json.LayoutInfo);
                gridstate.start = cardviewstartvalue;
                if (area === 'column' || area === 'order') {
                    gridstate.order[0] = [currentorderedcolumn, currentorder];
                }
                else if (area === 'pagination') {//
                }
                else if (area === 'pagelength') {
                    gridstate.length = cardviewlwngth;
                }

                if (json.FilterInfo !== '') {
                    setsearchui(JSON.parse(json.FilterInfo), $("#txtColumnSearch"), $(".filteritemcount"), $("#advsearchfilteritems"));
                }
                $.ajax({
                    "url": gridStateSaveUrl,
                    "data": {
                        GridName: "Part_Search",
                        LayOutInfo: JSON.stringify(gridstate)
                    },
                    "async": false,
                    "dataType": "json",
                    "type": "POST",
                    "success": function () { return; }
                });
            }
        }
    });
}
function LayoutFilterinfoUpdate() {
    $.ajax({
        "url": "/Base/GetLayout",
        "data": {
            GridName: "Part_Search"
        },
        "async": false,
        "dataType": "json",
        "success": function (json) {
            if (json.LayoutInfo !== '') {
                var gridstate = JSON.parse(json.LayoutInfo);
                gridstate.start = cardviewstartvalue;
                var filterinfoarray = getfilterinfoarray($("#txtColumnSearch"), $('#advsearchsidebar'));
                $.ajax({
                    "url": gridStateSaveUrl,
                    "data": {
                        GridName: "Part_Search",
                        LayOutInfo: JSON.stringify(gridstate),
                        FilterInfo: JSON.stringify(filterinfoarray)
                    },
                    "async": false,
                    "dataType": "json",
                    "type": "POST",
                    "success": function () { return; }
                });
            }
        }
    });
}

//#endregion
//#endregion
//#region V2-853 Reset Grid
$('#liResetGridClearBtn').click(function (e) {
    CancelAlertSetting.text = getResourceValue("ResetGridAlertMessage");
    swal(CancelAlertSetting, function () {
        var localstorageKeys = [];
        localstorageKeys.push("CURRENTTABSTATUS");
        localstorageKeys.push("layoutTypeval");
        localstorageKeys.push("partstatustext");
        localstorageKeys.push("PARTCURRENTCARDVIEWSTATE");
        localstorageKeys.push("PARTDETAILFROM");
        DeleteGridLayout('Part_Search', partsSearchdt, localstorageKeys);
        GenerateSearchList('', true);
        window.location.href = "../Parts/Index?page=Inventory_Parts";
    });
});
//#endregion
//#region V2-880 VendorCatalogItem Grid
var dtVendorCatalogItemTable;
function generatePartsVendorCatalogItemGrid() {
    var PartMasterId = $('#partMasterModel_PartMasterId').val();
    if ($(document).find('#tblVCItem').hasClass('dataTable')) {
        dtVendorCatalogItemTable.destroy();
    }
    dtVendorCatalogItemTable = $("#tblVCItem").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        stateSave: false,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Parts/GetParts_VendorCatalogItem",
            "type": "POST",
            data: { PartMasterId: PartMasterId },
            "datatype": "json",
            "dataSrc": function (json) {
                return json.data;
            }
        },
        "columns":
            [
                {
                    "data": "Vendor_Name", "autoWidth": true, "bSearchable": true, "bSortable": true
                },
                { "data": "UnitCost", "autoWidth": true, "bSearchable": true, "bSortable": true, "class": "text-right" },
                { "data": "PurchaseUOM", "autoWidth": true, "bSearchable": true, "bSortable": true }
            ],
        initComplete: function () {
            $(document).find("#pmwidget").mCustomScrollbar({
                theme: "minimal"
            });
            SetPageLengthMenu();
        }
    });
}
//#endregion
//#region V2-886
$(document).on('keyup', '#tblparts_paginate .paginate_input', function () {

    if (layoutType == 1) {
        var currentselectedpage = parseInt($(document).find('#txtcurrentpage').val());
        var lastpage = parseInt($(document).find('#spntotalpages').text());
        if (currentselectedpage > lastpage) {
            currentselectedpage = lastpage;
        }
        if (currentselectedpage < 1) {
            currentselectedpage = 1;
        }
        cardviewlwngth = $(document).find('#partcardviewpagelengthdrp').val();
        cardviewstartvalue = cardviewlwngth * (currentselectedpage - 1);
        grdcardcurrentpage = currentselectedpage;

        LayoutUpdate('pagination');
        ShowCardView();
    }
    run = true;
});
//#region load CardView with previous state
var DefaultLayoutInfo = '{"time":currentTime,"start":0,"length":10,"order":[[1,"asc"]],"search":{"search":"","smart":true,"regex":false,"caseInsensitive":true},"columns":[],"ColReorder":[]}';
function GetDatatableLayout() {
    $.ajax({
        "url": "/Base/GetLayout",
        "data": {
            GridName: "Part_Search"
        },
        "async": false,
        "dataType": "json",
        "success": function (json) {
            if (json.LayoutInfo !== '') {
                var LayoutInfo = JSON.parse(json.LayoutInfo);
                var pageclicked = (LayoutInfo.start / LayoutInfo.length);
                cardviewlwngth = LayoutInfo.length;
                cardviewstartvalue = cardviewlwngth * pageclicked;
                grdcardcurrentpage = pageclicked + 1;
                order = LayoutInfo.order[0][0];
                orderDir = LayoutInfo.order[0][1];

                if (json.FilterInfo !== '') {
                    setsearchui(JSON.parse(json.FilterInfo), $("#txtColumnSearch"), $(".filteritemcount"), $("#advsearchfilteritems"));
                }
            }
            else {
                DefaultLayoutInfo = DefaultLayoutInfo.replace('currentTime', new Date().getTime());
                var filterinfoarray = getfilterinfoarray($("#txtColumnSearch"), $('#advsearchsidebar'));
                $.ajax({
                    "url": gridStateSaveUrl,
                    "data": {
                        GridName: "Part_Search",
                        LayOutInfo: (DefaultLayoutInfo),
                        FilterInfo: JSON.stringify(filterinfoarray)
                    },
                    "async": false,
                    "dataType": "json",
                    "type": "POST",
                    "success": function () { return; }
                });
            }
        }
    });
}
//#endregion
//#endregion