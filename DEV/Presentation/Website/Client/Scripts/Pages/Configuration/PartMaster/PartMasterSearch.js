//#region Common
var activeStatus = false;
var run = false;
var prtMasterTable;
var CategoryVal;
var ManufacturerVal;
var ManufacturerIDVal;
var CatDescriptionVal;
var zoomConfig = {
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
};
    $('#dismiss, .overlay').on('click', function () {
        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });
    $('#sidebarCollapse').on('click', function () {
        $('#sidebar').addClass('active');
        $('.overlay').fadeIn();
        $('.collapse.in').toggleClass('in');
        $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    });
$(document).ready(function () {
    $(".actionBar").fadeIn();
    $("#partmasterGridAction :input").attr("disabled", "disabled");
    $(document).find('.select2picker').select2({});
    activeStatus = localStorage.getItem("PARTMASTERGRIDDISPLAYSTATUS");
    if (activeStatus) {
        if (activeStatus == "false") {
            activeStatus = false;
            $('#equipDropdown').val("1").trigger('change.select2');
        }
        else {
            activeStatus = true;
            $('#equipDropdown').val("2").trigger('change.select2');
        }
    }
    else {
        activeStatus = false;
    }
    generatePartMasterTable();
    //SetPartMasterDetailEnvironment();

});
function SetPartMasterControl() {
    CloseLoader();
    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse(document);
    $('input, form').blur(function () {
        $(this).valid();
    });
    $(document).find('.select2picker').select2({});
    // ZoomImage($(document).find('#EquipZoom'));
    $('.select2picker, form').change(function () {
        var areaddescribedby = $(this).attr('aria-describedby');
        if (areaddescribedby) {
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
        }
    });
}

$(document).on('click', "#brdpartmaster", function () {
    var partMasterId = $(this).attr('data-val');
    RedirectToPartMasterDetail(partMasterId);
});

$(document).on('click', "#btnEdtCancelPartMaster", function () {
    var partMasterId = $(document).find('#brdpartmaster').attr('data-val');
    swal(CancelAlertSetting, function () {
        RedirectToPartMasterDetail(partMasterId);
    });
});

$(document).on('click', "#btnAddCancelPartMaster", function () {
    swal(CancelAlertSetting, function () {
        window.location.href = "../PartMaster/Index?page=Masters_Part_Part_Master";
    });
});

function RedirectToPartMasterDetail(partMasterId, mode, isdelete) {
    
    $.ajax({
        url: "/PartMaster/PartMasterDetails",
        type: "POST",
        dataType: 'html',
        data: { partMasterId: partMasterId, delf: isdelete },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpartmaster').html(data);
        },
        complete: function () {
            CloseLoader();
            SetPartMasterDetailEnvironment();
            if (mode === "AzureImageReload" || mode === "OnPremiseImageReload") {
                $('#PartMaster').hide();
                $('#auditlogcontainer').hide();
                $('.imageDropZone').show();
                $(document).find('#PartMasterOverview').removeClass("active");
                $(document).find('#photot').addClass("active");              
            }
        },
        error: function () {
            CloseLoader();
        }
    });
}
//#endregion Common

//#region Search
$(document).on('change', '#equipDropdown', function () {
    run = true;
    ShowbtnLoaderclass("LoaderDrop");
    var optionVal = $(document).find("#equipDropdown").val();
    if (optionVal == "1") {
        searchcount = 0;
        localStorage.setItem("PARTMASTERGRIDDISPLAYSTATUS", false);
        activeStatus = false;
        //prtMasterTable.page('first').draw('page');
    }
    else {
        localStorage.setItem("PARTMASTERGRIDDISPLAYSTATUS", true);
        activeStatus = true;
       // prtMasterTable.page('first').draw('page');
    }
    prtMasterTable.page('first').draw('page');
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
function generatePartMasterTable() {
    var printCounter = 0;
    if ($(document).find('#partMasterSearch').hasClass('dataTable')) {
        prtMasterTable.destroy();
    }
    prtMasterTable = $("#partMasterSearch").DataTable({
        rowGrouping: true,
        colReorder: {
            fixedColumnsLeft: 1
        },
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        stateSave: true,
        "stateSaveCallback": function (settings, data) {
            if (run == true) {
                $.ajax({
                    "url": gridStateSaveUrl,
                    "data": {
                        GridName: "Part_Master_Search",
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
            $.ajax({
                "url": gridStateLoadUrl,
                "data": {
                    GridName: "Part_Master_Search",
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
                title: 'Part Master'
            },
            {
                extend: 'print',
                title: 'Part Master',
            },
            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Part Master',
                extension: '.csv'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                orientation: 'landscape',
                pageSize: 'A3'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/PartMaster/GetPartMasterGrid",
            "type": "POST",
            "datatype": "json",
            data: function (d) {
                d.partid = LRTrim($('#PartID').val());
                d.manufacturer = LRTrim($("#Manufacturer").val());
                d.manufacturerid = LRTrim($("#ManufacturerID").val());
                d.category = LRTrim($('#Category').val());
                d.catdescription = LRTrim($('#CatDescription').val());
                d.description = LRTrim($('#Description').val());
                d.inactiveFlag = activeStatus;
            },
            "dataSrc": function (result) {
                $("#Category").empty();
                $("#Category").append("<option value=''>" + "--Select--" + "</option>");
                for (var i = 0; i < result.catList.length; i++) {
                    var category = result.catList[i];
                    $("#Category").append("<option value='" + category + "'>" + category + "</option>");
                }
                if (CategoryVal) {
                    $("#Category").val(CategoryVal);
                }

                $("#Manufacturer").empty();
                $("#Manufacturer").append("<option value=''>" + "--Select--" + "</option>");
                for (var i = 0; i < result.ManufactureAdvList.length; i++) {
                    var manufacturer = result.ManufactureAdvList[i];
                    $("#Manufacturer").append("<option value='" + manufacturer + "'>" + manufacturer + "</option>");
                }
                if (ManufacturerVal) {
                    $("#Manufacturer").val(ManufacturerVal);
                }

                $("#ManufacturerID").empty();
                $("#ManufacturerID").append("<option value=''>" + "--Select--" + "</option>");
                for (var i = 0; i < result.ManufacturePartIdAdvList.length; i++) {
                    var manufacturerId = result.ManufacturePartIdAdvList[i];
                    $("#ManufacturerID").append("<option value='" + manufacturerId + "'>" + manufacturerId + "</option>");
                }
                if (ManufacturerIDVal) {
                    $("#ManufacturerID").val(ManufacturerIDVal);
                }

                $("#CatDescription").empty();
                $("#CatDescription").append("<option value=''>" + "--Select--" + "</option>");
                for (var i = 0; i < result.CategoryDescriptionAdvList.length; i++) {
                    var catDescription = result.CategoryDescriptionAdvList[i];
                    $("#CatDescription").append("<option value='" + catDescription + "'>" + catDescription + "</option>");
                }
                if (CatDescriptionVal) {
                    $("#CatDescription").val(CatDescriptionVal);
                }

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
                "data": "ClientLookupId",
                "autoWidth": true,
                "bSearchable": true,
                "bSortable": true,
                "className": "text-left",
                "name": "0",
                "mRender": function (data, type, row) {
                    return '<a class=lnk_prtMaster href="javascript:void(0)">' + data + '</a>';
                }
            },
            { "data": "Manufacturer", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1" },
            { "data": "ManufacturerId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2" },
            { "data": "Category", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3" },
            { "data": "CategoryDescription", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "4" },
            { "data": "ShortDescription", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "5" }
        ],
        columnDefs: [
            {
                targets: [0],
                className: 'noVis'
            }
        ],
        initComplete: function () {
            SetPageLengthMenu();
            $("#partmasterGridAction :input").not('.import-export').removeAttr("disabled");
            $("#partmasterGridAction :button").not('.import-export').removeClass("disabled");
        }
    });
};
$(document).on('click', '#partMasterSearch_paginate .paginate_button', function () {
    run = true;
});
$(document).on('change', '#partMasterSearch_length .searchdt-menu', function () {
    run = true;
});
$(document).on('click', '#partMasterSearch_wrapper th', function () {
    run = true;
});
$(function () {
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            var inactiveFlag = false;
            var optionVal = $(document).find("#equipDropdown").val();
            if (optionVal == "1") {
                inactiveFlag = false;
            }
            else {
                inactiveFlag = true;
            }
            var currestsortedcolumn = $('#partMasterSearch').dataTable().fnSettings().aaSorting[0][0];
            var coldir = $('#partMasterSearch').dataTable().fnSettings().aaSorting[0][1];
            var colname = $('#partMasterSearch').dataTable().fnSettings().aoColumns[currestsortedcolumn].name;
            var jsonResult = $.ajax({
                "url": "/PartMaster/GetPartMasterPrintData",
                "type": "get",
                "datatype": "json",
                data: {
                    colname: colname,
                    coldir: coldir,
                    partid: LRTrim($('#PartID').val()),
                    manufacturer: LRTrim($("#Manufacturer").val()),
                    manufacturerid: LRTrim($("#ManufacturerID").val()),
                    category: LRTrim($('#Category').val()),
                    catdescription: LRTrim($('#CatDescription').val()),
                    description: LRTrim($('#Description').val()),
                    inactiveFlag: activeStatus
                },
                success: function (result) {
                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data;
            var visiblecolumnsIndex = $("#partMasterSearch thead tr th").map(function (key) {
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
                if (item.Manufacturer != null) {
                    item.Manufacturer = item.Manufacturer;
                }
                else {
                    item.Manufacturer = "";
                }
                if (item.ManufacturerId != null) {
                    item.ManufacturerId = item.ManufacturerId;
                }
                else {
                    item.ManufacturerId = "";
                }
                if (item.ShortDescription != null) {
                    item.ShortDescription = item.ShortDescription;
                }
                else {
                    item.ShortDescription = "";
                }
                if (item.Category != null) {
                    item.Category = item.Category;
                }
                else {
                    item.Category = "";
                }
                if (item.CategoryDescription != null) {
                    item.CategoryDescription = item.CategoryDescription;
                }
                else {
                    item.CategoryDescription = "";
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
                header: $("#partMasterSearch thead tr th div").map(function (key) {
                    return this.innerHTML;
                }).get()
            };
        }
    });
})
$(document).on('click', '.lnk_prtMaster', function (e) {
    e.preventDefault();
    var index_row = $('#partMasterSearch tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = prtMasterTable.row(row).data();
    var PartMasterId = data.PartMasterId;
    $.ajax({
        url: "/PartMaster/PartMasterDetails",
        type: "GET",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        data: { partMasterId: PartMasterId },
        success: function (data) {
            $('#renderpartmaster').html(data);
        },
        complete: function (data) {
            SetPartMasterDetailEnvironment();
            CloseLoader();
        },
        error: function (xhr, error) {
        }
    });
});
//#endregion Search

//#region AdvSearch
var hGridfilteritemcount = 0;
$(document).on('click', "#btnPrtDataAdvSrch", function (e) {
   // prtMasterTable.state.clear();
    var searchitemhtml = "";
    hGridfilteritemcount = 0;
    $('#advsearchsidebar').find('.adv-item').each(function (index, item) {
        if ($(this).val() && $(this).val() != "0") {
            hGridfilteritemcount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossSODL" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $('#advsearchfilteritems').html(searchitemhtml);
    $('#sidebar').removeClass('active');
    $(document).find('.overlay').fadeOut();
    CategoryVal = $("#Category").val();
    ManufacturerVal = $("#Manufacturer").val();
    ManufacturerIDVal = $("#ManufacturerID").val();
    CatDescriptionVal = $("#CatDescription").val();
    GridAdvanceSearch();
});

function GridAdvanceSearch() {
    run = true;
    prtMasterTable.page('first').draw('page');
   // generatePartMasterTable();
    $('.filteritemcount').text(hGridfilteritemcount);
}

$(document).on('click', '.btnCrossSODL', function () {
    var btnCrossedId = $(this).parent().attr('id');
    var searchtxtId = btnCrossedId.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    hGridfilteritemcount--;
    if (searchtxtId == "Category") {
        CategoryVal = null;
    }
    if (searchtxtId == "Manufacturer") {
        ManufacturerVal = null;
    }
    if (searchtxtId == "ManufacturerID") {
        ManufacturerIDVal = null;
    }
    if (searchtxtId == "CatDescription") {
        CatDescriptionVal = null;
    }
    GridAdvanceSearch();
});
$(document).on('click', '#liClearAdvSearchFilter', function () {
    run = true;
    clearAdvanceSearch();
    prtMasterTable.page('first').draw('page');
    //generatePartMasterTable();
});
function clearAdvanceSearch() {
    var filteritemcount = 0;
    $('#advsearchsidebar').find('input:text').val('');
    $('.filteritemcount').text(filteritemcount);
    $('#advsearchfilteritems').find('span').html('');
    $('#advsearchfilteritems').find('span').removeClass('tagTo');
    $("#Category").val("").trigger('change');
    $("#Manufacturer").val("").trigger('change');
    $("#ManufacturerID").val("").trigger('change');
    $("#CatDescription").val("").trigger('change');
    CategoryVal = $("#Category").val();
    ManufacturerVal = $("#Manufacturer").val();
    ManufacturerIDVal = $("#ManufacturerID").val();
    CatDescriptionVal = $("#CatDescription").val();
}
//#endregion AdvSearch

//#region ColumnVisibility
$(document).on('click', '#liCustomize', function () {
    funCustomizeBtnClick(prtMasterTable, true);
});
$(document).on('click', '.saveConfig', function () {
    var colOrder = [0];
    funCustozeSaveBtn(prtMasterTable, colOrder);
    run = true;
    prtMasterTable.state.save(run);
});
//#endregion

//#region Add_Edit Eqp Master
$(document).on('click', ".AddPartMaster", function () {
    $.ajax({
        url: "/PartMaster/AddPartMaster",
        type: "GET",
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpartmaster').html(data);
        },
        complete: function () {
            SetPartMasterControl();
        },
        error: function () {
            CloseLoader();
        }
    });
});

$(document).on('click', "#btnSaveAnotherOpenPartMaster,#btnSavePartMaster", function () {
    if ($(document).find("form").valid()) {
        return;
    }
    else {
        var activetagid = $('.vtabs li.active').attr('id');
        if (activetagid !== 'identificationtab') {
            $('#identificationtab').trigger('click');
        }
    }
});

function PartMasterAddOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        var message;
        if (data.Command == "save") {
            if (data.mode == "add") {
                SuccessAlertSetting.text = "Part Master added successfully";
            }
            else {
                SuccessAlertSetting.text = "Part Master updated successfully";
            }
            swal(SuccessAlertSetting, function () {
                RedirectToPartMasterDetail(data.partMasterId, "");
            });
        }
        else {
            SuccessAlertSetting.text = "Part Master added successfully";
            ResetErrorDiv();
            swal(SuccessAlertSetting, function () {
                $(document).find('form').trigger("reset");
                $(document).find('form').find("select").val("").trigger('change.select2');
                //$(document).find('form').find("#purchaseRequestModel_VendorId").removeClass("input-validation-error");
                $(document).find('form').find("input").removeClass("input-validation-error");
                $(document).find('form').find("textarea").removeClass("input-validation-error");
            });
        }
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
};

$(document).on('click', "#btnEditPartMaster", function () {
    var ClientlookupId = $(document).find('#PartMasterModel_ClientLookupId').val();
    var PartId = $(document).find('#PartMasterModel_PartMasterId').val();
    var Manufacturer = $(document).find('#PartMasterModel_Manufacturer').val();
    var ManufacturerId = $(document).find('#PartMasterModel_ManufacturerId').val();
    var Category = $(document).find('#PartMasterModel_Category').val();
    var UnitCost = $(document).find('#PartMasterModel_UnitCost').val();
    var UnitOfMeasure = $(document).find('#PartMasterModel_UnitOfMeasure').val();
    var ShortDescription = $(document).find('#PartMasterModel_ShortDescription').val();
    var InactiveFlag = $(document).find('#PartMasterModel_InactiveFlag').is(":checked");
    var LongDescription = $(document).find('#PartMasterModel_LongDescription').val();
    var OEMPart = $(document).find('#PartMasterModel_OEMPart').is(":checked");
    var EXPartId = $(document).find('#PartMasterModel_EXPartId').val();
    var UPCCode = $(document).find('#PartMasterModel_UPCCode').val();
    $.ajax({
        url: "/PartMaster/EditPartMaster",
        type: "GET",
        dataType: 'html',
        data: { partid: PartId, clientlookupid: ClientlookupId, manufacturer: Manufacturer, manufacturerid: ManufacturerId, category: Category, unitcost: UnitCost, description: ShortDescription, unitofmeasure: UnitOfMeasure, inactiveFlag: InactiveFlag, longdescription: LongDescription, altpartid: EXPartId, oempart: OEMPart, upccode: UPCCode },
        // data: { equipmentMasterId: EquipmentMasterId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpartmaster').html(data);
        },
        complete: function () {
            SetPartMasterControl();
        },
        error: function () {
            CloseLoader();
        }
    });
});


function openCity(evt, cityName) {
    evt.preventDefault();
    switch (cityName) {
        case "Photos":
            // GetImageURL();
            $('#Photos').show();

            break;
        default:
            $('#PartMaster').show();
    }
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
//#endregion


//#region Photos
$(document).on('click', '#deleteImg', function () {
    var PartMasterId = $('#PartMasterModel_PartMasterId').val();
    var ClientOnPremise = $('#PartMasterModel_ClientOnPremise').val();
    if (ClientOnPremise == 'True') {
        DeleteOnPremiseImage(PartMasterId);
    }
    else {
        DeleteAzureImage(PartMasterId);
    }
   
});

function DeleteAzureImage(PartMasterId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/PartMaster/DeleteImageFromAzure',
            type: 'POST',
            data: { _PartMasterId: PartMasterId, TableName: "PartMaster", Profile: true, Image: true },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data == "success" || data=="not found") {
                    RedirectToPartMasterDetail(PartMasterId, "AzureImageReload", true);
                    ShowImageDeleteSuccessAlert();
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}
function DeleteOnPremiseImage(PartMasterId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/PartMaster/DeleteImageFromOnPremise',
            type: 'POST',
            data: { _PartMasterId: PartMasterId, TableName: "PartMaster", Profile: true, Image: true },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data == "success" || data == "not found") {
                    RedirectToPartMasterDetail(PartMasterId, "OnPremiseImageReload", true);
                    ShowImageDeleteSuccessAlert();
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}
function SetPartMasterDetailEnvironment() {
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
}
$(document).on('click', '.setImage', function () {
    var PartMasterId = $('#PartMasterModel_PartMasterId').val();
    var imageName = $(this).data('image');
    $.ajax({
        url: '../Base/SaveUploadedFileToServer',
        type: 'POST',
        data: { 'fileName': imageName, objectId: PartMasterId, TableName: "PartMaster", AttachObjectName: "PartMaster" },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.result == "0") {
            var ShowDeleteBtnAfterUpload = $('#ShowDeleteBtnAfterUpload').val();
            $('#EquipZoom').attr('src', data.imageurl);
            if (ShowDeleteBtnAfterUpload == 'True')
            { $(document).find('#AzureImage').append('<a href="javascript:void(0)" id="deleteImg" class="trashIcon" title="Delete"><i class="fa fa-trash"></i></a>');}
            else
            {
               $(document).find('#deleteImg,.dz-remove').remove();
            }
            $('#EquipZoom').data('zoomImage', data.imageurl).elevateZoom(zoomConfig);
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
        error: function ()
        {
            CloseLoader();
        }
        //complete: function () {
        //    CloseLoader();
        //}
    });
});
function ZoomImage(element) {
    element.elevateZoom(zoomConfig);
}
function AddDeleteIcon() {
    if ($(document).find('#deleteImg').length == 0) {
        $(document).find('#AzureImage').append('<div id="imgdelcontailer"><a href="javascript:void(0)" id="deleteImg" class="trashIcon" title="Delete"><i class="fa fa-trash"></i></a></div>');
    }
};
function clearDropzone() {
    deleteServer = false;
    if ($(document).find('#dropzoneForm').length > 0) {
        Dropzone.forElement("div#dropzoneForm").destroy();
    }
}
//#endregion