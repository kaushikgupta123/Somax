var dtpNotesTable;
var dtpAttachmentTable;
var dtpVendorTable;
var dtpEquipmentTable;
var dtpReviewSiteTable;
var _ObjectId;
$(document).ready(function () {
    $(document).find(".sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $(document).on('click', '.dismiss, .overlay', function () {
        $('.sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });
    $(document).on('change', '#colorselector', function (evt) {
        $('.tabsArea').hide();
        openCity(evt, $(this).val());
        $('#' + $(this).val()).show();
    });
});

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
//#region Photos
$(document).on('click', '#deleteImg', function () {
    var PartId = $('#PartModel_PartId').val();
    var ClientOnPremise = $('#PartModel_ClientOnPremise').val();
    if (ClientOnPremise == 'True') {
        DeleteOnPremiseImage(PartId);
    }
    else {
        DeleteAzureImage(PartId);
    }
 
});
function DeleteAzureImage(PartId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/Parts/DeleteImageFromAzure',
            type: 'POST',
            data: { _PartId: PartId, TableName: "Part", Profile: true, Image: true },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data == "success" || data === "not found") {
                    RedirectToPartDetail(PartId, "AzureImageReload");
                    ZoomImage($('#EquipZoom'), '', true);
                    $(document).find('#deleteImg').remove();
                    ShowImageDeleteSuccessAlert();
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}

function DeleteOnPremiseImage(PartId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/Parts/DeleteImageFromOnPremise',
            type: 'POST',
            data: { _PartId: PartId, TableName: "Part", Profile: true, Image: true },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data == "success" || data === "not found") {
                    RedirectToPartDetail(PartId, "OnPremiseImageReload");
                    ZoomImage($('#EquipZoom'), '', true);
                    $(document).find('#deleteImg').remove();
                    ShowImageDeleteSuccessAlert();
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}
function ZoomImage(element, dataurl, noimage) {
    $.removeData(element, 'elevateZoom');
    $('.zoomContainer').remove();
    if (noimage == true) {
        element.data('zoom-image', "../Scripts/ImageZoom/images/NoImage.jpg").elevateZoom(zoomConfig);
    }
    else if (!dataurl) {
        element.elevateZoom(zoomConfig);
    }
    else {
        element.data('zoom-image', dataurl).elevateZoom(zoomConfig);
    }
}
//#endregion
//#region Attachment
function GeneratePAttachmentGrid() {
    if ($(document).find('#pattachmentTable').hasClass('dataTable')) {
        dtpAttachmentTable.destroy();
    }
    var partid = $(document).find('#PartModel_PartId').val();
    var visibility;
    var attchCount = 0;
    dtpAttachmentTable = $("#pattachmentTable").DataTable({
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
            "url": "/Parts/PopulateAttachment?_partId=" + partid,
            "type": "post",
            "datatype": "json",
            "dataSrc": function (response) {
                attchCount = response.recordsTotal;
                if (attchCount > 0) {
                    $(document).find('#partAttachmentCount').show();
                    $(document).find('#partAttachmentCount').html(attchCount);
                }
                else {
                    $(document).find('#partAttachmentCount').hide();
                }
                return response.data;
            }
        },
        columnDefs: [
            {
                targets: [5], render: function (a, b, data, d) {
                    return '<a class="btn btn-outline-danger delPartAttchBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                }
            }
        ],
        "columns":
            [
                { "data": "Subject", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "FileName",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<a class=lnk_sensor_1 href="javascript:void(0)"  target="_blank">' + row.FullName + '</a>'
                    }
                },
                { "data": "FileSizeWithUnit", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "OwnerName", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "CreateDate",
                    "type": "date "
                },
                {
                    "data": "ObjectId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center"
                }
            ],
        initComplete: function () {
            if (visibility == "false") {
                var column = this.api().column(5);
                column.visible(false);
            }
            else {
                var column = this.api().column(5);
                column.visible(true);
            }
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', '.lnk_sensor_1', function (e) {
    e.preventDefault();
    var row = $(this).parents('tr');
    var data = dtpAttachmentTable.row(row).data();
    var FileAttachmentId = data.FileAttachmentId;
    $.ajax({
        type: "post",
        url: '/Base/IsOnpremiseCredentialValid',
        success: function (data) {
            if (data === true) {
                window.location = '/Parts/DownloadAttachment?_fileinfoId=' + FileAttachmentId;
            }
            else {
                ShowErrorAlert(getResourceValue("NotAuthorisedDownloadFileAlert"));
            }
        }

    });
});
$(document).on('click', '.delPartAttchBttn', function () {
    var data = dtpAttachmentTable.row($(this).parents('tr')).data();
    DeletePartAttachment(data.FileAttachmentId);
});
function DeletePartAttachment(fileAttachmentId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/Parts/DeletePAttachment',
            data: {
                _fileAttachmentId: fileAttachmentId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    dtpAttachmentTable.state.clear();
                    ShowDeleteAlert(getResourceValue("attachmentDeleteSuccessAlert"));
                }
                else {
                    ShowErrorAlert(data.Message);
                }
            },
            complete: function () {
                GeneratePAttachmentGrid();
                CloseLoader();
            }
        });
    });
}
$(document).on('click', "#btnAddpAttachment", function () {
    var ClientLookupId = $(document).find('#PartModel_ClientLookupId').val();
    var partId = $(document).find('#PartModel_PartId').val();
    $.ajax({
        url: "/Parts/AddPAttachment",
        type: "GET",
        dataType: 'html',
        data: { partId: partId, ClientLookupId: ClientLookupId },
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
function AttachmentPAddOnSuccess(data) {
    CloseLoader();
    var partid = data.partid;
    if (data.Result == "success") {
        if (data.IsduplicateAttachmentFileExist) {
            ShowErrorAlert(getResourceValue("AttachmentFileExistAlerts"));
        }
        else {
            SuccessAlertSetting.text = getResourceValue("AddAttachmentAlerts");
            swal(SuccessAlertSetting, function () {
                RedirectToPartDetail(data.partid, "attachment");
            });
        }
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', "#btnattpachmentcancel", function () {
    var partId = $(document).find('#attachmentModel_PartId').val();
    swal(CancelAlertSetting, function () {
        RedirectToPartDetail(partId, "attachment");
    });
});
//#endregion
//#region Vendor
$(document).on('click', '.addBtnPartsVendor', function () {
    var data = dtpVendorTable.row($(this).parents('tr')).data();
    AddVendor(data.PartId);
});
$(document).on('click', '.editBttnPartsVendor', function () {
    var data = dtpVendorTable.row($(this).parents('tr')).data();
    EditPartVendor(data.PartId, data.Part_Vendor_XrefId, data.UpdateIndex, "update");
});
$(document).on('click', '.delBtnPartsVendor', function () {
    var data = dtpVendorTable.row($(this).parents('tr')).data();
    dtpVendorTable.state.clear();
    DeletePartVendor(data.Part_Vendor_XrefId);
});
function GeneratePVendorGrid() {
    var visibility;
    var rCount = 0;
    var partid = $(document).find('#PartModel_PartId').val();
    if ($(document).find('#pvendorTable').hasClass('dataTable')) {
        dtpVendorTable.destroy();
    }
    dtpVendorTable = $("#pvendorTable").DataTable({
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
            "url": "/Parts/PopulateVendor",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d._partId = partid;
            },
            "dataSrc": function (json) {
                visibility = json.partVendorSecurity;
                rCount = json.data.length;
                return json.data;
            }
        },
        columnDefs: [
            {
                targets: [10], render: function (a, b, data, d) {
                    if (visibility == "true") {
                        return '<a class="btn btn-outline-primary addBtnPartsVendor gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                            '<a class="btn btn-outline-success editBttnPartsVendor gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delBtnPartsVendor gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                    else {
                        return "";
                    }
                }
            }
        ],
        "columns":
            [
                { "data": "Vendor_ClientLookupId", "autoWidth": false, "bSearchable": true, "bSortable": true },
                { "data": "Vendor_Name", "autoWidth": false, "bSearchable": true, "bSortable": true },
                { "data": "CatalogNumber", "autoWidth": false, "bSearchable": true, "bSortable": true },
                { "data": "Manufacturer", "autoWidth": false, "bSearchable": true, "bSortable": true },
                { "data": "ManufacturerId", "autoWidth": false, "bSearchable": true, "bSortable": true },
                { "data": "OrderQuantity", "autoWidth": false, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "data": "OrderUnit", "autoWidth": false, "bSearchable": true, "bSortable": true },
                { "data": "Price", "autoWidth": false, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "data": "IssueOrder", "autoWidth": false, "bSearchable": true, "bSortable": true, "className": "text-right" },
                {
                    "data": "UOMConvRequired", "autoWidth": false, "bSearchable": true, "bSortable": true, "className": "text-center",
                    'render': function (data, type, full, meta) {
                        if (data === true) {
                            return '<label class="m-checkbox m-checkbox--air m-checkbox--solid m-checkbox--state-brand somax-checkbox lbl-innergrid-checkbox">' +
                                '<input type="checkbox" checked="checked" class="status" onclick="return false"><span></span></label>';
                        }
                        else {
                            return '<label class="m-checkbox m-checkbox--air m-checkbox--state-brand somax-checkbox lbl-innergrid-checkbox">' +
                                '<input type="checkbox" class="status" onclick="return false"><span></span></label>';
                        }
                    }
                },
                
                {
                    "data": "VendorId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center"
                }
            ],
        initComplete: function () {
            var column = this.api().column(10);
            if (visibility == "false") {
                column.visible(false);
            }
            else {
                column.visible(true);
            }
            if (rCount > 0) {
                $("#btnAddpVendor").hide();
            }
            else {
                if (visibility == "true") {
                    $("#btnAddpVendor").show();
                }
                else {
                    $("#btnAddpVendor").hide();
                }
            }
            SetPageLengthMenu();
        }
    });
}
function DeletePartVendor(PartVendorXrefId) {
    var partid = $(document).find('#PartModel_PartId').val();
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/Parts/PartsVendorDelete',
            data: {
                _PartVendorXrefId: PartVendorXrefId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    dtpVendorTable.state.clear();
                    ShowDeleteAlert(getResourceValue("vendorDeleteSuccessAlert"));
                }
            },
            complete: function () {
                GeneratePVendorGrid();
                CloseLoader();
            }
        });
    });
}
function AddPartsVendor(PartId) {
    AddVendor(PartId);
}
function AddVendor(PartId) {
    ClientLookupId = $(document).find('#PartModel_ClientLookupId').val();
    $.ajax({
        url: "/Parts/PartsVedndorAdd",
        type: "GET",
        dataType: 'html',
        data: { _partId: PartId, ClientLookupId: ClientLookupId},
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
}
$(document).on('click', "#btnAddpVendor", function () {
    var partId = $(document).find('#PartModel_PartId').val();
    AddVendor(partId);
});
function PartsVendorAddOnSuccess(data) {
    if (data.Result == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("VendorAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("VendorUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToPartDetail(data.partid, "vendor");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
function EditPartVendor(part_Id, part_Vendor_XrefId, updatedIndex) {
    ClientLookupId = $(document).find('#PartModel_ClientLookupId').val();
    $.ajax({
        url: "/Parts/PartsVedndorEdit",
        type: "GET",
        dataType: 'html',
        data: { partId: part_Id, _part_Vendor_XrefId: part_Vendor_XrefId, updatedIndex: updatedIndex, ClientLookupId: ClientLookupId},
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            CloseLoader();
            $('#renderparts').html(data);
        },
        complete: function () {
            SetPartControls();
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', "#btnpvendorcancel", function () {
    var partId = $(document).find('#partsVendorModel_PartId').val();
    swal(CancelAlertSetting, function () {
        RedirectToPartDetail(partId, "vendor");
    });
});
$(document).on('change', "#partsVendorModel_VendorClientLookupId", function () {
    var tlen = $(document).find("#partsVendorModel_VendorClientLookupId").val();
    var areaddescribedby = $(document).find("#partsVendorModel_VendorClientLookupId").attr('aria-describedby');
    if (tlen.length > 0) {
        $('#' + areaddescribedby).hide();
        $(document).find('form').find("#partsVendorModel_VendorClientLookupId").removeClass("input-validation-error");
    }
    else {
        $('#' + areaddescribedby).show();
        $(document).find('form').find("#partsVendorModel_VendorClientLookupId").addClass("input-validation-error");
    }
});
//#endregion
//#region Equipment
function GeneratePEquipmentGrid() {
    var rCount = 0;
    var visibility;
    var partid = $(document).find('#PartModel_PartId').val();
    if ($(document).find('#pequipmentTable').hasClass('dataTable')) {
        dtpEquipmentTable.destroy();
    }
    dtpEquipmentTable = $("#pequipmentTable").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        "bProcessing": true,
        serverSide: true,
        "pagingType": "full_numbers",
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
            "url": "/Parts/PopulateEquipment?_partId=" + partid,
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (json) {
                visibility = json.partEquipmentSecurity;
                rCount = json.data.length;
                return json.data;
            }
        },
        columnDefs: [
            {
                targets: [5], render: function (a, b, data, d) {
                    if (visibility == "true") {
                        return '<a class="btn btn-outline-primary addBtnPartsEquipment gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                            '<a class="btn btn-outline-success editBttnPartsEquipment gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delBtnPartsEquipment gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                    else {
                        return "";
                    }
                }
            }
        ],
        "columns":
            [
                { "data": "Equipment_ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Equipment_Name", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "QuantityNeeded", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "QuantityUsed", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Comment", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "EquipmentId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center" }
            ],
        initComplete: function () {
            var column = this.api().column(5);
            if (visibility != "true") {
                column.visible(false);
            }
            else {
                column.visible(true);
            }
            if (rCount > 0) {
                $("#btnAddpEquipment").hide();
            }
            else {
                if (visibility == "true") {
                    $("#btnAddpEquipment").show();
                }
                else {
                    $("#btnAddpEquipment").hide();
                }
            }
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', "#btnAddpEquipment,.addBtnPartsEquipment", function () {
    var partId = $(document).find('#PartModel_PartId').val();
    var clientLookupId = $(document).find('#PartModel_ClientLookupId').val();
    AddEquipment(partId, clientLookupId);
});
$(document).on('click', '.editBttnPartsEquipment', function () {
    var data = dtpEquipmentTable.row($(this).parents('tr')).data();
    EditPartEquipment(data.PartId, data.Equipment_Parts_XrefId, data.UpdateIndex, "update");
});
$(document).on('click', '.delBtnPartsEquipment', function () {
    var data = dtpEquipmentTable.row($(this).parents('tr')).data();
    DeletePartEquipment(data.Equipment_Parts_XrefId);
});
function AddPartsEquipment(PartId) {
    AddEquipment(PartId);
}
function AddEquipment(partId, clientLookupId) {
    $.ajax({
        url: "/Parts/PartsEquipmentAdd",
        type: "GET",
        dataType: 'html',
        data: { partId: partId, clientLookupId: clientLookupId},
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
}
function PartsEquipmentAddOnSuccess(data) {
    if (data.Result == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("EquipmentSaveAlerts");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("EquipmentUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToPartDetail(data.partid, "equipment");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
function EditPartEquipment(part_Id, part_Equipment_XrefId, updatedIndex) {
    var ClientLookupId = $(document).find('#PartModel_ClientLookupId').val();
    $.ajax({
        url: "/Parts/PartsEquipmentEdit",
        type: "GET",
        dataType: 'html',
        data: { partId: part_Id, _equipment_Parts_XrefId: part_Equipment_XrefId, updatedIndex: updatedIndex, ClientLookupId: ClientLookupId},
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            CloseLoader();
            $('#renderparts').html(data);
        },
        complete: function () {
            SetPartControls();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function DeletePartEquipment(Equipment_Parts_XrefId) {
    var partid = $(document).find('#PartModel_PartId').val();
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/Parts/DeleteEquipmentPartXref',
            data: {
                _equipment_Parts_XrefId: Equipment_Parts_XrefId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    dtpEquipmentTable.state.clear();
                    ShowDeleteAlert(getResourceValue("equipmentDeleteSuccessAlert"));
                }
            },
            complete: function () {
                GeneratePEquipmentGrid();
                CloseLoader();
            }
        });
    });
}
$(document).on('click', "#btnpequipmentcancel", function () {
    var partId = $(document).find('#equipmentPartXrefModel_PartId').val();
    swal(CancelAlertSetting, function () {
        RedirectToPartDetail(partId, "equipment");
    });
});
//#endregion
//#region history grid
var typeValTransactionType;
var typeValChargeType;
var typeValVendor;
var dtpHistoryTable;
var hGridfilteritemcount = 0;
var daterange = "0";
$(document).on('click', "#historysidebarCollapse", function () {
    $('#historyadvsearchcontainer').find('.sidebar').addClass('active');
    $('.overlay').fadeIn();
    $('.collapse.in').toggleClass('in');
    $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    $(document).find('.dtpicker').datepicker({
        changeMonth: true,
        changeYear: true,
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy');
    $(document).find('.select2picker').select2({});
});
$(document).on('click', "#btnPartsDataAdvSrchHistory", function (e) {
    dtpHistoryTable.state.clear();
    var searchitemhtml = "";
    hGridfilteritemcount = 0
    $('#txtsearchbox').val('');
    $('#advsearchsidebarHistory').find('.adv-item').each(function (index, item) {
        if ($(this).hasClass('dtpicker')) {
            $(this).val(ValidateDate($(this).val()));
        }
        if ($(this).val() && $(this).val() != "0") {
            hGridfilteritemcount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossHistory" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $('#lineitemadvsearchfilteritems').html(searchitemhtml);
    $('#historyadvsearchcontainer').find('.sidebar').removeClass('active');
    $('.overlay').fadeOut();
    hGridAdvanceSearch();
    typeValTransactionType = $("#rgridadvsearchTransactionType").val();
    typeValChargeType = $("#rgridadvsearchChargeTypePrimary").val();
    typeValVendor = $("#rgridadvsearchVendorClientLookupId").val();
});
$(document).on('click', '.btnCrossHistory', function () {
    var btnCrossedId = $(this).parent().attr('id');
    var searchtxtId = btnCrossedId.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    hGridfilteritemcount--;
    if (searchtxtId == "rgridadvsearchTransactionType") {
        typeValTransactionType = null;
    }
    else if (searchtxtId == "rgridadvsearchChargeTypePrimary") {
        typeValChargeType = null;
    }
    else if (searchtxtId == "rgridadvsearchVendorClientLookupId") {
        typeValVendor = null;
    }
    hGridAdvanceSearch();
});
$(document).on('click', '#lineitemClearAdvSearchFilter', function () {
    $(document).find('#partsHistoryModel_HistoryDaterange').val("0").trigger('change.select2');
    hGridclearAdvanceSearch();
    GenerateHistoryGrid();
});
function hGridclearAdvanceSearch() {
    var filteritemcount = 0;
    $(document).find("#rgridadvsearchTransactionType").val("").trigger('change');
    $(document).find("#rgridadvsearchChargeTypePrimary").val("").trigger('change');
    $(document).find("#rgridadvsearchPurchaseOrderClientLookupId").val("").trigger('change');
    $(document).find('#advsearchsidebarHistory').find('input:text').val('');
    $(document).find('.lifilteritemcount').text(filteritemcount);
    $(document).find('#lineitemadvsearchfilteritems').find('span').html('');
    $(document).find('#lineitemadvsearchfilteritems').find('span').removeClass('tagTo');
    typeValTransactionType = $("#rgridadvsearchTransactionType").val();
    typeValChargeType = $("#rgridadvsearchChargeTypePrimary").val();
    typeValVendor = $("#rgridadvsearchPurchaseOrderClientLookupId").val();
}
function hGridAdvanceSearch() {
    GenerateHistoryGrid();
    $('.lifilteritemcount').text(hGridfilteritemcount);
}
function VendorsObjArray(id, name) {
    this.id = id;
    this.name = name;
}
function GenerateHistoryGrid() {
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
    if ($(document).find('#pvhistoryTable').hasClass('dataTable')) {
        dtpHistoryTable.destroy();
    }
    dtpHistoryTable = $("#pvhistoryTable").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        serverSide: true,
        "order": [[3, "desc"]],
        stateSave: false,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons:
            [
                {
                    extend: 'excel',
                    filename: partclientlookup + '_History',
                    extension: '.xlsx',
                    className: "datatable-btn-export historygridexport"
                }
            ],
        "orderMulti": true,
        "ajax":
        {
            "url": "/Parts/PopulatePartHistoryReview",
            "type": "POST",
            "datatype": "json",
            data: function (d) {
                d.partid = partid;
                d.daterange = daterange;
                d.TransactionType = TransactionType;
                d.Requestor_Name = Requestor_Name;
                d.PerformBy_Name = PerformBy_Name;
                d.TransactionDate = TransactionDate;
                d.TransactionQuantity = TransactionQuantity;
                d.Cost = Cost;
                d.ChargeType_Primary = ChargeType_Primary;
                d.ChargeTo_ClientLookupId = ChargeTo_ClientLookupId;
                d.Account_ClientLookupId = Account_ClientLookupId;
                d.PurchaseOrder_ClientLookupId = PurchaseOrder_ClientLookupId;
                d.Vendor_ClientLookupId = Vendor_ClientLookupId;
                d.Vendor_Name = Vendor_Name;
            },
            "dataSrc": function (json) {
                var myDataSource = json;
                var TransTyp = [];
                var ChrgTyp = [];
                var Vendor = [];
                var VendorObj;
                for (var i = 0; i < myDataSource.dataAll.length; ++i) {
                    TransTyp.push(myDataSource.dataAll[i].TransactionType);
                    ChrgTyp.push(myDataSource.dataAll[i].ChargeType_Primary);
                    VendorObj = new VendorsObjArray(myDataSource.dataAll[i].Vendor_ClientLookupId, myDataSource.dataAll[i].Vendor_ClientLookupId + '-' + myDataSource.dataAll[i].Vendor_Name);
                    Vendor.push(VendorObj);
                }
                TransTyp = TransTyp.filter(function (v) { return v !== '' });
                ChrgTyp = ChrgTyp.filter(function (v) { return v !== '' });
                Vendor = Vendor.filter(function (v) { return v.id !== '' });
                var result = [];
                $.each(Vendor, function (i, e) {
                    var matchingItems = $.grep(result, function (item) {
                        return item.id === e.id && item.name === e.name;
                    });
                    if (matchingItems.length === 0) result.push(e);
                });


                TransTyp = TransTyp.filter(onlyUnique);
                ChrgTyp = ChrgTyp.filter(onlyUnique);

                var TTlen = TransTyp.length;
                $("#rgridadvsearchTransactionType").empty();
                $("#rgridadvsearchTransactionType").append("<option value=''>" + "--Select--" + "</option>");
                for (var i = 0; i < TTlen; i++) {
                    var id = TransTyp[i];
                    var name = TransTyp[i];
                    $("#rgridadvsearchTransactionType").append("<option value='" + id + "'>" + name + "</option>");
                }
                var CTlen = ChrgTyp.length;
                $("#rgridadvsearchChargeTypePrimary").empty();
                $("#rgridadvsearchChargeTypePrimary").append("<option value='" + 0 + "'>" + "--Select--" + "</option>");
                for (var i = 0; i < CTlen; i++) {
                    var id = ChrgTyp[i];
                    var name = ChrgTyp[i];
                    $("#rgridadvsearchChargeTypePrimary").append("<option value='" + id + "'>" + name + "</option>");
                }
                var VenLen = result.length;
                $("#rgridadvsearchVendorClientLookupId").empty();
                $("#rgridadvsearchVendorClientLookupId").append("<option value='" + 0 + "'>" + "--Select--" + "</option>");
                for (var i = 0; i < VenLen; i++) {
                    var id = result[i].id;
                    var name = result[i].name;
                    $("#rgridadvsearchVendorClientLookupId").append("<option value='" + id + "'>" + name + "</option>");
                }
                if (typeValTransactionType) {
                    $("#rgridadvsearchTransactionType").val(typeValTransactionType);
                }
                if (typeValChargeType) {
                    $("#rgridadvsearchChargeTypePrimary").val(typeValChargeType);
                }
                if (typeValVendor) {
                    $("#rgridadvsearchVendorClientLookupId").val(typeValVendor);
                }
                return json.data;
            }
        },
        "columns":
            [
                { "data": "TransactionType", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Requestor_Name", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "PerformBy_Name", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "TransactionDate", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "TransactionQuantity", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Cost", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "ChargeType_Primary", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "ChargeTo_ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Account_ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "PurchaseOrder_ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Vendor_ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Vendor_Name", "autoWidth": true, "bSearchable": true, "bSortable": true }
            ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}
$(document).on('change', '#partsHistoryModel_HistoryDaterange', function () {
    dtpHistoryTable.state.clear();
    hGridclearAdvanceSearch();
    GenerateHistoryGrid();
});
$(document).on('click', "#downloahistorydata", function () {
    $(document).find('.historygridexport').trigger('click');
});
//#endregion
function ChangePartIDOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        $('#menuPartIdChange').modal('hide');
        $('.modal-backdrop').remove();
        SuccessAlertSetting.text = getResourceValue("spnChangePartSuccess");
        swal(SuccessAlertSetting, function () {
            RedirectToPartDetail(data.partid, "");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', '#optchangepartid', function (e) {
    var clientlookupid = $(document).find('#OldClientLookupId').val();
    $(document).find('#changePartIdModel_ClientLookupId').val(clientlookupid).removeClass('input-validation-error');
    $('#menuPartIdChange').modal('show');
    $.validator.unobtrusive.parse(document);
    $(this).blur();
});
//#region Review  Sites
$(document).find('#reviewadvsearchcontainer').find(".sidebar").mCustomScrollbar({
    theme: "minimal"
});
$(document).on('click', '#reviewsidebarCollapse', function () {
    $(document).find('#reviewadvsearchcontainer').find(".sidebar").addClass('active');
    $('.overlay').fadeIn();
    $('.collapse.in').toggleClass('in');
    $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    $(document).find('.dtpicker').datepicker({
        changeMonth: true,
        changeYear: true,
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy');
});
function GenerateReviewSiteGrid() {
    var visibility;
    var rCount = 0;
    var PartMasterId = $(document).find('#partMasterModel_PartMasterId').val();
    //var PartClientLookupId = $(document).find('#PartModel_ClientLookupId').val();
    //var PartDescription = $(document).find('#PartModel_Description').val();
    //console.log(PartClientLookupId);
    //console.log(PartDescription);
    if ($(document).find('#reviewSiteTable').hasClass('dataTable')) {
        dtpReviewSiteTable.destroy();
    }
    dtpReviewSiteTable = $("#reviewSiteTable").DataTable({
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
            "url": "/Parts/PopulateReviewSite",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.PartMasterId = PartMasterId;
                d.SiteName = LRTrim($('#RSSiteName').val());
                d.ClientLookupId = LRTrim($("#RSClientLookupId").val());
                d.Description = LRTrim($("#RSDescription").val());
                d.QtyOnHand = LRTrim($('#RSQtyOnHand').val());
                d.QtyOnOrder = LRTrim($('#RSQtyOnOrder').val());
                d.LastPurchaseDate = ValidateDate($("#RSLastPurchaseDate").val());
                d.LastPurchaseCost = LRTrim($('#RSLastPurchaseCost').val());
                d.LastPurchaseVendor = LRTrim($('#RSLastPurchaseVendor').val());
                d.InactiveFlag = LRTrim($('#RSisInactive').val());
            },
            "dataSrc": function (json) {
                return json.data;
            }
        },
        columnDefs: [
            {
                targets: [9], render: function (a, b, data, d) {
                    return '<a class="btn btn-outline-primary btnRequestTransfer gridinnerbutton" title="Request Transfer">Request Transfer</a>';
                }
            }
        ],
        "columns":
            [
                { "data": "SiteName", "autoWidth": false, "bSearchable": true, "bSortable": true },
                { "data": "ClientLookupId", "autoWidth": false, "bSearchable": true, "bSortable": true },
                { "data": "Description", "autoWidth": false, "bSearchable": true, "bSortable": true },
                { "data": "QtyOnHand", "autoWidth": false, "bSearchable": true, "bSortable": true },
                { "data": "QtyOnOrder", "autoWidth": false, "bSearchable": true, "bSortable": true },

                {
                    "data": "LastPurchaseDate", "autoWidth": true, "bSearchable": true, "bSortable": false, "type": "date",
                    render: function (data, type, row, meta) {
                        if (data == null) {
                            return '';
                        } else {
                            return data;
                        }
                    }
                },
                { "data": "LastPurchaseCost", "autoWidth": false, "bSearchable": true, "bSortable": true },

                { "data": "LastPurchaseVendor", "autoWidth": false, "bSearchable": true, "bSortable": true },
                {
                    "data": "InactiveFlag", "autoWidth": true, "bSearchable": true, "bSortable": true, className: 'text-center', "name": "2",
                    "mRender": function (data, type, row) {
                        if (data == true) {
                            return '<label class="m-checkbox m-checkbox--air m-checkbox--solid m-checkbox--state-brand somax-checkbox lbl-innergrid-checkbox">' +
                                '<input type="checkbox" checked="checked" class="status" ><span></span></label>';
                        }
                        else {

                            return '<label class="m-checkbox m-checkbox--air m-checkbox--state-brand somax-checkbox lbl-innergrid-checkbox">' +
                                '<input type="checkbox"  class="status"><span></span></label>';
                        }

                    }
                },
                {
                    "data": "PartId",
                    "autoWidth": true,
                    "bSearchable": false,
                    "bSortable": false,
                    className: 'text-left',
                    "mRender": function (data, type, row) {
                        //console.log(row);
                        if (row.RequestTransferStatus == true)
                            return '<a class="btn btn-outline-primary btnRequestTransfer gridinnerbutton" title="Request Transfer" >Request Transfer</a>';
                    }
                },
            ],
        initComplete: function () {
            $(document).on('click', '.status', function (e) {
                e.preventDefault();
            });
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', "#btnReviewSiteDataAdvSrch", function (e) {
    dtpReviewSiteTable.state.clear();
    var searchitemhtml = "";
    hGridfilteritemcount = 0;
    $('#reviewsitegridadvsearchsidebar').find('.adv-item').each(function (index, item) {
        if ($(this).val() && $(this).val() != "0") {
            hGridfilteritemcount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossRS" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $('#advsearchfilteritems').html(searchitemhtml);
    $('#reviewadvsearchcontainer').find('.sidebar').removeClass('active');
    $(document).find('.overlay').fadeOut();
    GridAdvanceSearch();
});
function GridAdvanceSearch() {
    GenerateReviewSiteGrid();
    $('.receiptfilteritemcount').text(hGridfilteritemcount);
}
$(document).on('click', '.btnCrossRS', function () {
    var btnCrossedId = $(this).parent().attr('id');
    var searchtxtId = btnCrossedId.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    hGridfilteritemcount--;
    GridAdvanceSearch();
    dtpReviewSiteTable.page('first').draw('page');
});
$(document).on('click', '#lirevClearAdvSearchFilter', function () {
    clearrevAdvanceSearch();
    GenerateReviewSiteGrid();
});
function clearrevAdvanceSearch() {
    var receiptfilteritemcount = 0;
    $('#reviewsitegridadvsearchsidebar').find('input:text').val('');
    $("#RSisInactive").val("").trigger('change');
    $('.receiptfilteritemcount').text(receiptfilteritemcount);
    $('#advsearchfilteritems').find('span').html('');
    $('#advsearchfilteritems').find('span').removeClass('tagTo');
}
$(document).on('click', '.btnRequestTransfer', function () {
    //var PartClientLookupId = $(document).find('#PartModel_ClientLookupId').val();
    //var PartDescription = $(document).find('#PartModel_Description').val();
    var RequestPartId = $(document).find('#PartModel_PartId').val();
    //console.log(PartClientLookupId);
    //console.log(PartDescription);
    var data = dtpReviewSiteTable.row($(this).parents('tr')).data();
    RequestTransfer(data.PartId, data.Description, data.ClientLookupId, data.SiteName, RequestPartId);
});
function RequestTransfer(IssuePartId, IssueDescription, IssueClientLookupId, SiteName, RequestPartId) {
    $.ajax({
        url: "/Parts/AddPartTransfer",
        type: "GET",
        dataType: 'html',
        data: { IssuePartId: IssuePartId, IssueDescription: IssueDescription, IssueClientLookupId: IssueClientLookupId, SiteName: SiteName, RequestPartId: RequestPartId},
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderparts').html(data);
        },
        complete: function () {
            SetPartControls();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
function PartTransferAddOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        var message;
        SuccessAlertSetting.text = getResourceValue("PartTransferCreateAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToPartDetail(data.PartId, "ReviewSite")
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
};
$(document).on('click', "#btnPartTransfercancel", function () {
    var partid = $(document).find('#partTransferModel_PartId').val();
    swal(CancelAlertSetting, function () {
        RedirectToPartDetail(partid, "ReviewSite");
    });
});
//#endregion

//#region ActivateInActivate
$(document).on('click', '#actinctivatepart', function () {
    var partId = $('#PartModel_PartId').val();
    var clientLookupId = $(document).find('#PartModel_ClientLookupId').val();
    var InActiveFlag = $(document).find('#parthiddeninactiveflag').val();
    $.ajax({
        url: "/Parts/ValidateForActiveInactive",
        type: "POST",
        dataType: "json",
        data: { InActiveFlag: InActiveFlag, PartId: partId, ClientLookupId: clientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.validationStatus == true) {
                if (InActiveFlag == "True") {
                    CancelAlertSetting.text = getResourceValue("ActivatePartAlert");
                }
                else {
                    CancelAlertSetting.text = getResourceValue("InActivatePartAlert");
                }
                swal(CancelAlertSetting, function (isConfirm) {
                    if (isConfirm == true) {
                        $.ajax({
                            url: "/Parts/MakeActiveInactive",
                            type: "POST",
                            dataType: "json",
                            data: { InActiveFlag: InActiveFlag, partId: partId },
                            beforeSend: function () {
                                ShowLoader();
                            },
                            success: function (data) {

                                if (data.Result == 'success') {
                                    if (InActiveFlag == "True") {
                                        SuccessAlertSetting.text = getResourceValue("PartActiveSuccessAlert");
                                        localStorage.setItem("CURRENTTABSTATUS", "1");
                                        titleText = getResourceValue("AlertActive");
                                        localStorage.setItem("partstatustext", titleText);
                                        CustomQueryDisplayId = "1";

                                    }
                                    else {
                                        SuccessAlertSetting.text = getResourceValue("PartInActiveSuccessAlert");
                                        localStorage.setItem("CURRENTTABSTATUS", "2");
                                        titleText = getResourceValue("AlertInactive");
                                        localStorage.setItem("partstatustext", titleText);
                                        CustomQueryDisplayId = "2";
                                    }
                                    swal(SuccessAlertSetting, function () {
                                        RedirectToPartDetail(partId);
                                        $(document).find('#spnlinkToSearch').text(titleText);
                                    });
                                }
                                else {
                                    ShowGenericErrorOnAddUpdate(data);
                                }
                            },
                            complete: function () {
                                CloseLoader();
                            },
                            error: function () {
                                CloseLoader();
                            }
                        });
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
//#endregion

//#region Additional Part Functions
$(document).on('click', '#btnPartCheckOutClose,#btnPartCheckOutCancel,#btnEqChildrenClose,#btnWOChildrenClose,#btnLocChildrenClose,#btnUpdatePartCostClose,#btnUpdatePartCostCancel', function () {
    if ($(this).attr('id') == 'btnPartCheckOutClose' || $(this).attr('id') == 'btnPartCheckOutCancel') {
        hidemodal($('#partCheckOutPopUp'));
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
    if ($(this).attr('id') == 'btnUpdatePartCostClose' || $(this).attr('id') == 'btnUpdatePartCostCancel') {
        hidemodal($('#partsUpdatePartCostsPopUp'));
        resetValidation();
    }
});

function ShowAdjustHandQty() {
    showmodal($('#changeHandsOnQtyPopUp'));
    var partId = LRTrim($('#PartModel_PartId').val());
    var _PartClientLookupId = LRTrim($("#PartModel_ClientLookupId").val());
    var Description = LRTrim($("#PartModel_Description").val());
    var UPCCode = LRTrim($("#PartModel_UPCCode").val());
    $('#inventoryModel_PartId').val(partId);
    $('#inventoryModel_PartClientLookupId').val(_PartClientLookupId);
    $('#inventoryModel_Description').val(Description);
    $('#inventoryModel_PartUPCCode').val(UPCCode);
    $('#inventoryModel_ReceiptQuantity').val('');
    var areaddescribedby = $(document).find("#inventoryModel_ReceiptQuantity").attr('aria-describedby');
    $('#' + areaddescribedby).hide();
    $(document).find('form').find("#inventoryModel_ReceiptQuantity").removeClass("input-validation-error");

}
function ShowPartCheckout() {
    showmodal($('#partCheckOutPopUp'));
    var partId = LRTrim($('#PartModel_PartId').val());
    var _PartClientLookupId = LRTrim($("#PartModel_ClientLookupId").val());
    var Description = LRTrim($("#PartModel_Description").val());
    var UPCCode = LRTrim($("#PartModel_UPCCode").val());
    $(document).find("#inventoryCheckoutModel_ChargeType").select2({});
    $(document).find("#inventoryCheckoutModel_PartId").val(partId);
    $(document).find("#inventoryCheckoutModel_PartClientLookupId").val(_PartClientLookupId);
    $(document).find("#inventoryCheckoutModel_PartDescription").val(Description);
    $(document).find("#inventoryCheckoutModel_UPCCode").val(UPCCode);

}
function ValidationHanhsQtyOnSuccess(data) {
    if (data.Result == "success") {
        RedirectToPartDetail(data.data.PartId);
        hidemodal($('#changeHandsOnQtyPopUp'));
        $('.modal-backdrop').remove();
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
        CloseLoader();
    }
}
function ValidateParCheckOutOnSuccess(data) {
    if (data.Result == "success") {
        RedirectToPartDetail(data.data.PartId);
        hidemodal($('#partCheckOutPopUp'));
        $('.modal-backdrop').remove();
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
        CloseLoader();
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

function onlyUnique(value, index, self) {
    return self.indexOf(value) === index;
}

//#endregion

//#region default value set for validation at the time of add vendor V2-553

$(document).on('change', '#UOMConvRequired', function () {
    if (this.checked) {
        $(document).find("#DefaultissueOrdervalue").val("1.000001");
        $(document).find("#DefaultPricevalue").val("0.00001");
    }
    else {
        $(document).find("#DefaultissueOrdervalue").val("0");
        $(document).find("#DefaultPricevalue").val("0");
        $("#partsVendorModel_IssueOrder").removeClass("input-validation-error")
        $("#partsVendorModel_Price").removeClass("input-validation-error")
    }
});

//#endregion

//#region Multiple Photo Upload
function CompressImage(files, imageName) {
    new Compressor(files, {
        quality: 0.6,
        convertTypes: ['image/png'],
        convertSize: 100000,
        // The compression process is asynchronous,
        // which means you have to access the `result` in the `success` hook function.
        success(result) {
            if (result.size < files.size) {
                SaveCompressedImage(result, imageName);
            }
            else {
                SaveCompressedImage(files, imageName);
            }
            console.log('file name ' + result.name + ' after compress ' + result.size);
        },
        error(err) {
            console.log(err.message);
        },
    });
}
function SaveCompressedImage(data, imageName) {
    var AddPhotoFileData = new FormData();
    AddPhotoFileData.append('file', data, imageName);
    $.ajax({
        url: '../base/SaveUploadedFile',
        type: "POST",
        contentType: false, // Not to set any content header  
        processData: false, // Not to process data  
        data: AddPhotoFileData,
        success: function (result) {
            //alert(result);
            SaveMultipleUploadedFileToServer(imageName);
        },
        error: function (err) {
            alert(err.statusText);
        }
    });
}
$(document).on('change', '#files', function () {
    var val = $(this).val();
    var _isMobile = CheckLoggedInFromMob();
    var imageName = val.replace(/^.*[\\\/]/, '')
    var fileUpload = $("#files").get(0);
    var files = fileUpload.files;
    var fileExt = imageName.substr(imageName.lastIndexOf('.') + 1).toLowerCase();
    if (fileExt != 'jpeg' && fileExt != 'jpg' && fileExt != 'png' && fileExt != 'JPEG' && fileExt != 'JPG' && fileExt != 'PNG') {
        ShowErrorAlert(getResourceValue("spnValidImage"));
        $('#files').val('');
        //e.preventDefault();
        return false;
    }
    else if (this.files[0].size > (1024 * 1024 * 10)) {
        ShowImageSizeExceedAlert();
        $('#files').val('');
        //e.preventDefault();
        return false;
    }
    else {
        //alert('Hello');
        swal(AddImageAlertSetting, function () {
            if (window.FormData !== undefined) {
                // Looping over all files and add it to FormData object  
                for (var i = 0; i < files.length; i++) {
                    console.log('file name ' + files[i].name + ' before compress ' + files[i].size);
                    if (_isMobile == true) {
                        var partId = LRTrim($('#PartModel_PartId').val());
                        var imgname = partId + "_" + Math.floor((new Date()).getTime() / 1000);
                        CompressImage(files[i], imgname + "." + fileExt);
                    }
                    else {
                        CompressImage(files[i], imageName);
                    }
                   
                }
            } else {
                //alert("FormData is not supported.");
            }
            $('#files').val('');

        });
    }
});
//#endregion

//#region Show Images
var cardviewstartvalue = 0;
var cardviewlwngth = 10;
var grdcardcurrentpage = 1;
var currentorderedcolumn = 1;
var layoutTypeWO = 1;
function LoadImages(PartId) {
    $.ajax({
        url: '/Parts/GetImages',
        type: 'POST',
        data: {
            currentpage: grdcardcurrentpage,
            start: cardviewstartvalue,
            length: cardviewlwngth,
            PartId: PartId
        },
        beforeSend: function () {
            $(document).find('#imagedataloader').show();
        },
        success: function (data) {
            /*if (data.TotalCount > 0) {*/
                $(document).find('#ImageGrid').show();
                $(document).find('#PartImages').html(data).show();
                $(document).find('#tblimages_paginate li').each(function (index, value) {
                    $(this).removeClass('active');
                    if ($(this).data('currentpage') == grdcardcurrentpage) {
                        $(this).addClass('active');
                    }
                });
            //}
            //else {
            //    $(document).find('#ImageGrid').hide();
            //}
        },
        complete: function () {
            $(document).find('#imagedataloader').hide();
            $(document).find('#cardviewpagelengthdrp').select2({ minimumResultsForSearch: -1 }).val(cardviewlwngth).trigger('change.select2');
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('change', '#cardviewpagelengthdrp', function () {
    var PartId = $('#PartModel_PartId').val();
    cardviewlwngth = $(this).val();
    grdcardcurrentpage = parseInt(cardviewstartvalue / cardviewlwngth) + 1;
    cardviewstartvalue = parseInt((grdcardcurrentpage - 1) * cardviewlwngth) + 1;
    LoadImages(PartId);

});
$(document).on('click', '#tblimages_paginate .paginate_button', function () {
    var PartId = $('#PartModel_PartId').val();
    if (layoutTypeWO == 1) {
        var currentselectedpage = parseInt($(document).find('#tblimages_paginate .pagination').find('.active').text());
        cardviewlwngth = $(document).find('#cardviewpagelengthdrp').val();
        cardviewstartvalue = cardviewlwngth * (parseInt($(this).find('.page-link').text()) - 1);
        var lastpage = parseInt($(this).prev('li').data('currentpage'));

        if ($(this).attr('id') == 'tbl_previous') {
            if (currentselectedpage == 1) {
                return false;
            }
            cardviewstartvalue = cardviewlwngth * (currentselectedpage - 1);
            grdcardcurrentpage = grdcardcurrentpage - 1;
        }
        else if ($(this).attr('id') == 'tbl_next') {
            if (currentselectedpage == lastpage) {
                return false;
            }
            cardviewstartvalue = cardviewlwngth * (currentselectedpage - 1);
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
            grdcardcurrentpage = parseInt($(this).prevAll('li').eq(1).text());
            cardviewstartvalue = cardviewlwngth * (grdcardcurrentpage - 1);
        }
        else {
            grdcardcurrentpage = $(this).data('currentpage');
        }
        LoadImages(PartId);

    }
    else {
        run = true;
    }
});
//#endregion
//#region Set As Default
$(document).on('click', '#selectidSetAsDefault', function () {
    var AttachmentId = $(this).attr('dataid');
    $('#' + AttachmentId).hide();
    var PartId = $('#PartModel_PartId').val();
    $('.modal-backdrop').remove();
    $.ajax({
        url: '../base/SetImageAsDefault',
        type: 'POST',

        data: { AttachmentId: AttachmentId, objectId: PartId, TableName: "Part" },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.result === "success") {
                //$('.modal-backdrop').remove();
                //$('#' + AttachmentId).hide();
                $('#EquipZoom').attr('src', data.imageurl);
                $('#EquipZoom').attr('data-zoom-image', data.imageurl);
                $('.equipImg').attr('src', data.imageurl);
                //$(document).find('#AzureImage').append('<a href="javascript:void(0)" id="deleteImg" class="trashIcon" title="Delete"><i class="fa fa-trash"></i></a>');
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
                    //LoadImages(EquimentId);
                    CloseLoader();
                    ShowImageSetSuccessAlert();
                });
                //RedirectToEquipmentDetail(EquimentId, "OnPremiseImageReload");
                //ShowImageSaveSuccessAlert();
            }
            //else {
            //    CloseLoader();
            //    //var errorMessage = getResourceValue("NotAuthorisedUploadFileAlert");
            //    //ShowErrorAlert(errorMessage);

            //}
        },
        complete: function () {
            //CloseLoader();
            LoadImages(PartId);
        },
        error: function () {
            CloseLoader();
        }
    });
});
//#endregion
//#region Delete Image
$(document).on('click', '#selectidDelete', function () {
    var AttachmentId = $(this).attr('dataid');
    $('#' + AttachmentId).hide();
    var PartId = $('#PartModel_PartId').val();
    var ClientOnPremise = $('#PartModel_ClientOnPremise').val();
    $('.modal-backdrop').remove();
    if (ClientOnPremise == 'True') {
        DeleteOnPremiseMultipleImage(PartId, AttachmentId);
    }
    else {
        DeleteAzureMultipleImage(PartId, AttachmentId);
    }
});

function DeleteOnPremiseMultipleImage(PartId, AttachmentId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '../base/DeleteMultipleImageFromOnPremise',
            type: 'POST',
            data: { AttachmentId: AttachmentId, objectId: PartId, TableName: "Part" },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data === "success" || data === "not found") {
                    RedirectToPartDetail(PartId, "OnPremiseImageReload");
                    ShowImageDeleteSuccessAlert();
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}
function DeleteAzureMultipleImage(PartId, AttachmentId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '../base/DeleteMultipleImageFromAzure',
            type: 'POST',
            data: { AttachmentId: AttachmentId, objectId: PartId, TableName: "Part" },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data === "success" || data === "not found") {
                    //LoadImages(EquimentId);
                    RedirectToPartDetail(PartId, "AzureImageReload");
                    ShowImageDeleteSuccessAlert();
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}
//#endregion
//#region Save Multiple Image
function SaveMultipleUploadedFileToServer(imageName) {
    var PartId = $('#PartModel_PartId').val();
    $.ajax({
        url: '../base/SaveMultipleUploadedFileToServer',
        type: 'POST',

        data: { 'fileName': imageName, objectId: PartId, TableName: "Part", AttachObjectName: "Part" },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.result == "0") {
                
                CloseLoader();
                ShowImageSaveSuccessAlert();
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
            LoadImages(PartId);
        },
        error: function () {
            CloseLoader();
        }
    });
}
//#endregion

//#region V2-906
function UpdatePartCosts() {
    showmodal($('#partsUpdatePartCostsPopUp'));
}
function ValidateUpdatePartCostsOnSuccess(data) {
    if (data.Result == "success") {
        RedirectToPartDetail(data.PartId);
        hidemodal($('#partCheckOutPopUp'));
        $('.modal-backdrop').remove();
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
        CloseLoader();
    }
}
//#endregion
//#region V2-1007
$(document).on('click', '#linkToEquipment', function (e) {
    clearDropzone();
    var EquipmentId = $(document).find('#EquipmentId').val();
    window.location.href = "../Equipment/DetailFromWorkOrder?EquipmentId=" + EquipmentId;
});
//#endregion

//#region V2-1196

function resetValidationConfigureAutoPurchasing() {
    $(document).find("#partsConfigureAutoPurchasingModel_IsAutoPurchase").prop("checked", false);
    $(document).find("#partsConfigureAutoPurchasingModel_PartVendorId").val("").trigger('change');
    $(document).find("#QtyReorderLevel").val("");
    $(document).find("#QtyMaximum").val("");
    var partsConfigureAutoPurchasingModel_PartVendorId = $(document).find("#partsConfigureAutoPurchasingModel_PartVendorId").attr('aria-describedby');
    $('#' + partsConfigureAutoPurchasingModel_PartVendorId).hide();
    $(document).find('form').find("#partsConfigureAutoPurchasingModel_PartVendorId").removeClass("input-validation-error");
    $(document).find('form').find("#QtyReorderLevel").removeClass("input-validation-error");
    $(document).find('form').find("#QtyMaximum").removeClass("input-validation-error");

}
function ShowConfigureAutoPurchasing() {
    $(document).find('.select2picker').select2({});
    $(document).find('#UpdatePartsConfigureAutoPurchasingPopUp').modal({ backdrop: 'static', keyboard: false, show: true });
}
function UpdatePartsConfigureAutoPurchasingOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("AutoPurchasingsetupupdatedsuccessfullyAlert");
        hidemodal($('#UpdatePartsConfigureAutoPurchasingPopUp'));
        $('.modal-backdrop').remove();
        swal(SuccessAlertSetting, function () {
            RedirectToPartDetail(data.PartId);
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
       
    }
}
$(document).on('click', '.UpdatePartsConfigureAutoPurchasingPopUpClose', function (e) {
    hidemodal($('#UpdatePartsConfigureAutoPurchasingPopUp'));
    $('.modal-backdrop').remove();
    resetValidationConfigureAutoPurchasing();
});
$(document).on("change", "#partsConfigureAutoPurchasingModel_PartVendorId", function () {
  
    ConfigureAutoPurchasingVendorError();
});
function ConfigureAutoPurchasingVendorError() {
    var tlen = $(document).find("#partsConfigureAutoPurchasingModel_PartVendorId").val();
    if (tlen.length > 0) {
        var areaddescribedby = $(document).find("#partsConfigureAutoPurchasingModel_PartVendorId").attr('aria-describedby');
        $('#' + areaddescribedby).hide();
        $(document).find('form').find("#partsConfigureAutoPurchasingModel_PartVendorId").removeClass("input-validation-error");
    }
    else {
            var areaddescribedby = $(document).find("#partsConfigureAutoPurchasingModel_PartVendorId").attr('aria-describedby');
            $('#' + areaddescribedby).show();
            $(document).find('form').find("#partsConfigureAutoPurchasingModel_PartVendorId").addClass("input-validation-error");
    }
}
$(document).on("change", ".partsConfigureQty", function () {
    var $document = $(document);
    var $autoPurchase = $document.find("#partsConfigureAutoPurchasingModel_IsAutoPurchase");
    var $qtyMax = $document.find("#QtyMaximum");
    var $qtyReorder = $document.find("#QtyReorderLevel");

    if ($autoPurchase.prop("checked")) {
        if (parseFloat($qtyMax.val()) > parseFloat($qtyReorder.val())) {
            $qtyReorder.removeClass("input-validation-error");
            $qtyMax.removeClass("input-validation-error");
        }
    }
});
//#endregion