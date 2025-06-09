var dtmspAttachmentTable;
var dtmspVendorTable;
var dtmspEquipmentTable;

//#region Change Part Id
function ChangePartIdOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        $('#menuPartIdChange').modal('hide');
        $('.modal-backdrop').remove();
        SuccessAlertSetting.text = getResourceValue("spnChangePartSuccess");
        swal(SuccessAlertSetting, function () {
            RedirectToMultiStoreroomPartDetail($(document).find('#PartId').val())
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', '#optchangemultistoreroompartid', function (e) {
    var clientlookupid = $(document).find('#OldClientLookupId').val();
    $(document).find('#ChangePartIdModel_ClientLookupId').val(clientlookupid).removeClass('input-validation-error');
    $('#menuPartIdChange').modal('show');
    $.validator.unobtrusive.parse(document);
    $(this).blur();
});
//#endregion



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
    var PartId = $('#PartId').val();
    var ClientOnPremise = $('#ClientOnPremise').val();
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
            url: '/MultiStoreroomPart/DeleteImageFromAzure',
            type: 'POST',
            data: { _PartId: PartId, TableName: "Part", Profile: true, Image: true },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data == "success" || data === "not found") {
                    RedirectToMultiStoreroomPartDetail(PartId, "AzureImageReload");
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
            url: '/MultiStoreroomPart/DeleteImageFromOnPremise',
            type: 'POST',
            data: { _PartId: PartId, TableName: "Part", Profile: true, Image: true },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data == "success" || data === "not found") {
                    RedirectToMultiStoreroomPartDetail(PartId, "OnPremiseImageReload");
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
function GenerateMSPAttachmentGrid() {
    if ($(document).find('#mspattachmentTable').hasClass('dataTable')) {
        dtmspAttachmentTable.destroy();
    }
    var partid = $(document).find('#PartId').val();
    var visibility;
    var attchCount = 0;
    dtmspAttachmentTable = $("#mspattachmentTable").DataTable({
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
            "url": "/MultiStoreroomPart/PopulateAttachment?_partId=" + partid,
            "type": "post",
            "datatype": "json",
            "dataSrc": function (response) {
                attchCount = response.recordsTotal;
                if (attchCount > 0) {
                    $(document).find('#MSPAttachmentCount').show();
                    $(document).find('#MSPAttachmentCount').html(attchCount);
                }
                else {
                    $(document).find('#MSPAttachmentCount').hide();
                }
                return response.data;
            }
        },
        columnDefs: [
            {
                targets: [5], render: function (a, b, data, d) {
                    return '<a class="btn btn-outline-danger delMSPAttchBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
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
                        return '<a class=lnk_attachment_file href="javascript:void(0)"  target="_blank">' + row.FullName + '</a>'
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
$(document).on('click', '.lnk_attachment_file', function (e) {
    e.preventDefault();
    var row = $(this).parents('tr');
    var data = dtmspAttachmentTable.row(row).data();
    var FileAttachmentId = data.FileAttachmentId;
    $.ajax({
        type: "post",
        url: '/Base/IsOnpremiseCredentialValid',
        success: function (data) {
            if (data === true) {
                window.location = '/MultiStoreroomPart/DownloadAttachment?_fileinfoId=' + FileAttachmentId;
            }
            else {
                ShowErrorAlert(getResourceValue("NotAuthorisedDownloadFileAlert"));
            }
        }

    });
});
$(document).on('click', '.delMSPAttchBttn', function () {
    var data = dtmspAttachmentTable.row($(this).parents('tr')).data();
    DeleteMSPAttachment(data.FileAttachmentId);
});
function DeleteMSPAttachment(fileAttachmentId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/MultiStoreroomPart/DeleteMSPAttachment',
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
                    dtmspAttachmentTable.state.clear();
                    ShowDeleteAlert(getResourceValue("attachmentDeleteSuccessAlert"));
                }
                else {
                    ShowErrorAlert(data.Message);
                }
            },
            complete: function () {
                GenerateMSPAttachmentGrid();
                CloseLoader();
            }
        });
    });
}
$(document).on('click', "#btnAddMSPAttachment", function () {
    var ClientLookupId = $(document).find('#ClientLookupId').val();
    var partId = $(document).find('#PartId').val();
    $.ajax({
        url: "/MultiStoreroomPart/AddMSPAttachment",
        type: "GET",
        dataType: 'html',
        data: { partId: partId, ClientLookupId: ClientLookupId },
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
function AttachmentMSPAddOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("AddAttachmentAlerts");
        swal(SuccessAlertSetting, function () {
            RedirectToMultiStoreroomPartDetail(data.partid, "attachment");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', "#btnMSPattpachmentcancel", function () {
    var partId = $(document).find('#AttachmentModel_PartId').val();
    swal(CancelAlertSetting, function () {
        RedirectToMultiStoreroomPartDetail(partId, "attachment");
    });
});
//#endregion

//#region Vendor
$(document).on('click', '.addBttnMSPVendor', function () {
    var data = dtmspVendorTable.row($(this).parents('tr')).data();
    AddMSPVendor(data.PartId);
});
$(document).on('click', '.editBttnMSPVendor', function () {
    var data = dtmspVendorTable.row($(this).parents('tr')).data();
    EditMSPVendor(data.PartId, data.Part_Vendor_XrefId, data.UpdateIndex, "update");
});
$(document).on('click', '.delBttnMSPVendor', function () {
    var data = dtmspVendorTable.row($(this).parents('tr')).data();
    dtmspVendorTable.state.clear();
    DeleteMSPVendor(data.Part_Vendor_XrefId);
});
function GenerateMSPVendorGrid() {
    var visibility;
    var rCount = 0;
    var partid = $(document).find('#PartId').val();
    if ($(document).find('#mspVendorTable').hasClass('dataTable')) {
        dtmspVendorTable.destroy();
    }
    dtmspVendorTable = $("#mspVendorTable").DataTable({
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
            "url": "/MultiStoreroomPart/PopulateVendor",
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
                        return '<a class="btn btn-outline-primary addBttnMSPVendor gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                            '<a class="btn btn-outline-success editBttnMSPVendor gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delBttnMSPVendor gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
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
                $("#btnAddMSPVendor").hide();
            }
            else {
                if (visibility == "true") {
                    $("#btnAddMSPVendor").show();
                }
                else {
                    $("#btnAddMSPVendor").hide();
                }
            }
            SetPageLengthMenu();
        }
    });
}
function DeleteMSPVendor(PartVendorXrefId) {
    //var partid = $(document).find('#PartId').val();
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/MultiStoreroomPart/MSPVendorDelete',
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
                    dtmspVendorTable.state.clear();
                    ShowDeleteAlert(getResourceValue("vendorDeleteSuccessAlert"));
                }
            },
            complete: function () {
                GenerateMSPVendorGrid();
                CloseLoader();
            }
        });
    });
}
//function AddPartsVendor(PartId) {
//    AddMSPVendor(PartId);
//}
function AddMSPVendor(PartId) {
    ClientLookupId = $(document).find('#ClientLookupId').val();
    $.ajax({
        url: "/MultiStoreroomPart/MSPVedndorAdd",
        type: "GET",
        dataType: 'html',
        data: { _partId: PartId, ClientLookupId: ClientLookupId },
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
}
$(document).on('click', "#btnAddMSPVendor", function () {
    var partId = $(document).find('#PartId').val();
    AddMSPVendor(partId);
});
function MSPVendorAddOnSuccess(data) {
    if (data.Result == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("VendorAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("VendorUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToMultiStoreroomPartDetail(data.partid, "vendor");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
function EditMSPVendor(part_Id, part_Vendor_XrefId, updatedIndex) {
    ClientLookupId = $(document).find('#ClientLookupId').val();
    $.ajax({
        url: "/MultiStoreroomPart/MSPVedndorEdit",
        type: "GET",
        dataType: 'html',
        data: { partId: part_Id, _part_Vendor_XrefId: part_Vendor_XrefId, updatedIndex: updatedIndex, ClientLookupId: ClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            CloseLoader();
            $('#multistoreroompartcontainer').html(data);
        },
        complete: function () {
            SetMSPControls();
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', "#btnmspvendorcancel", function () {
    var partId = $(document).find('#MSPVendorModel_PartId').val();
    swal(CancelAlertSetting, function () {
        RedirectToMultiStoreroomPartDetail(partId, "vendor");
    });
});
$(document).on('change', "#MSPVendorModel_VendorClientLookupId", function () {
    var tlen = $(document).find("#MSPVendorModel_VendorClientLookupId").val();
    var areaddescribedby = $(document).find("#MSPVendorModel_VendorClientLookupId").attr('aria-describedby');
    if (tlen.length > 0) {
        $('#' + areaddescribedby).hide();
        $(document).find('form').find("#MSPVendorModel_VendorClientLookupId").removeClass("input-validation-error");
    }
    else {
        $('#' + areaddescribedby).show();
        $(document).find('form').find("#MSPVendorModel_VendorClientLookupId").addClass("input-validation-error");
    }
});
//#endregion

//#region Asset
function GenerateMSPEquipmentGrid() {
    var rCount = 0;
    var visibility;
    var partid = $(document).find('#PartId').val();
    if ($(document).find('#mspEequipmentTable').hasClass('dataTable')) {
        dtmspEquipmentTable.destroy();
    }
    dtmspEquipmentTable = $("#mspEequipmentTable").DataTable({
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
            "url": "/MultiStoreroomPart/PopulateEquipment?_partId=" + partid,
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
                        return '<a class="btn btn-outline-primary addBttnMSPEquipment gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                            '<a class="btn btn-outline-success editBttnMSPEquipment gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delBttnMSPEquipment gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
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
                $("#btnAddmspEquipment").hide();
            }
            else {
                if (visibility == "true") {
                    $("#btnAddmspEquipment").show();
                }
                else {
                    $("#btnAddmspEquipment").hide();
                }
            }
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', "#btnAddmspEquipment,.addBttnMSPEquipment", function () {
    var partId = $(document).find('#PartId').val();
    var clientLookupId = $(document).find('#ClientLookupId').val();
    AddMSPEquipment(partId, clientLookupId);
});
$(document).on('click', '.editBttnMSPEquipment', function () {
    var data = dtmspEquipmentTable.row($(this).parents('tr')).data();
    EditMSPEquipment(data.PartId, data.Equipment_Parts_XrefId, data.UpdateIndex, "update");
});
$(document).on('click', '.delBttnMSPEquipment', function () {
    var data = dtmspEquipmentTable.row($(this).parents('tr')).data();
    DeleteMSPEquipment(data.Equipment_Parts_XrefId);
});
//function AddPartsEquipment(PartId) {
//    AddMSPEquipment(PartId);
//}
function AddMSPEquipment(partId, clientLookupId) {
    $.ajax({
        url: "/MultiStoreroomPart/MSPEquipmentAdd",
        type: "GET",
        dataType: 'html',
        data: { partId: partId, clientLookupId: clientLookupId },
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
            RedirectToMultiStoreroomPartDetail(data.partid, "equipment");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
function EditMSPEquipment(part_Id, part_Equipment_XrefId, updatedIndex) {
    var ClientLookupId = $(document).find('#ClientLookupId').val();
    $.ajax({
        url: "/MultiStoreroomPart/MSPEquipmentEdit",
        type: "GET",
        dataType: 'html',
        data: { partId: part_Id, _equipment_Parts_XrefId: part_Equipment_XrefId, updatedIndex: updatedIndex, ClientLookupId: ClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            CloseLoader();
            $('#multistoreroompartcontainer').html(data);
        },
        complete: function () {
            SetMSPControls();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function DeleteMSPEquipment(Equipment_Parts_XrefId) {
    //var partid = $(document).find('#PartId').val();
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/MultiStoreroomPart/DeleteMSPEquipmentXref',
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
                    dtmspEquipmentTable.state.clear();
                    ShowDeleteAlert(getResourceValue("equipmentDeleteSuccessAlert"));
                }
            },
            complete: function () {
                GenerateMSPEquipmentGrid();
                CloseLoader();
            }
        });
    });
}
$(document).on('click', "#btnmspequipmentcancel", function () {
    var partId = $(document).find('#MSPEquipmentXrefModel_PartId').val();
    swal(CancelAlertSetting, function () {
        RedirectToMultiStoreroomPartDetail(partId, "equipment");
    });
});
//#endregion

//#region History
var typeValTransactionType;
var typeValChargeType;
var typeValVendor;
var dtmspHistoryTable;
var hGridfilteritemcount = 0;
var daterange = "0";
$(document).on('click', "#msphistorysidebarCollapse", function () {
    $('#msphistoryadvsearchcontainer').find('.sidebar').addClass('active');
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
$(document).on('click', "#btnMSPDataAdvSrchHistory", function (e) {
    dtmspHistoryTable.state.clear();
    var searchitemhtml = "";
    hGridfilteritemcount = 0
    //$('#txtsearchbox').val('');
    $('#mspadvsearchsidebarHistory').find('.adv-item').each(function (index, item) {
        if ($(this).hasClass('dtpicker')) {
            $(this).val(ValidateDate($(this).val()));
        }
        if ($(this).val() && $(this).val() != "0") {
            hGridfilteritemcount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossHistory" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $('#msplineitemadvsearchfilteritems').html(searchitemhtml);
    $('#msphistoryadvsearchcontainer').find('.sidebar').removeClass('active');
    $('.overlay').fadeOut();
    hGridAdvanceSearch();
    typeValTransactionType = $("#mspgridadvsearchTransactionType").val();
    typeValChargeType = $("#mspgridadvsearchChargeTypePrimary").val();
    typeValVendor = $("#mspgridadvsearchVendorClientLookupId").val();
});
$(document).on('click', '.btnCrossHistory', function () {
    var btnCrossedId = $(this).parent().attr('id');
    var searchtxtId = btnCrossedId.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    hGridfilteritemcount--;
    if (searchtxtId == "mspgridadvsearchTransactionType") {
        typeValTransactionType = null;
    }
    else if (searchtxtId == "mspgridadvsearchChargeTypePrimary") {
        typeValChargeType = null;
    }
    else if (searchtxtId == "mspgridadvsearchVendorClientLookupId") {
        typeValVendor = null;
    }
    hGridAdvanceSearch();
});
$(document).on('click', '#msplineitemClearAdvSearchFilter', function () {
    $(document).find('#MSPHistoryModel_HistoryDaterange').val("0").trigger('change.select2');
    MSPhGridclearAdvanceSearch();
    GenerateMSPHistoryGrid();
});
function MSPhGridclearAdvanceSearch() {
    var filteritemcount = 0;
    $(document).find("#mspgridadvsearchTransactionType").val("").trigger('change');
    $(document).find("#mspgridadvsearchChargeTypePrimary").val("").trigger('change');
    //$(document).find("#mspgridadvsearchPurchaseOrderClientLookupId").val("").trigger('change');
    $(document).find("#mspgridadvsearchVendorClientLookupId").val("").trigger('change');
    $(document).find('#mspadvsearchsidebarHistory').find('input:text').val('');
    $(document).find('.lifilteritemcount').text(filteritemcount);
    $(document).find('.filteritemcount').text(filteritemcount);
    $(document).find('#msplineitemadvsearchfilteritems').find('span').html('');
    $(document).find('#msplineitemadvsearchfilteritems').find('span').removeClass('tagTo');
    typeValTransactionType = $("#mspgridadvsearchTransactionType").val();
    typeValChargeType = $("#mspgridadvsearchChargeTypePrimary").val();
    typeValVendor = $("#mspgridadvsearchVendorClientLookupId").val();
}
function hGridAdvanceSearch() {
    GenerateMSPHistoryGrid();
    $('.lifilteritemcount').text(hGridfilteritemcount);
    $('.filteritemcount').text(hGridfilteritemcount);
}
function VendorsObjArray(id, name) {
    this.id = id;
    this.name = name;
}
function GenerateMSPHistoryGrid() {
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
    if ($(document).find('#MSPhistoryTable').hasClass('dataTable')) {
        dtmspHistoryTable.destroy();
    }
    dtmspHistoryTable = $("#MSPhistoryTable").DataTable({
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
            "url": "/MultiStoreroomPart/PopulateMSPHistoryReview",
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
                $("#mspgridadvsearchTransactionType").empty();
                $("#mspgridadvsearchTransactionType").append("<option value=''>" + "--Select--" + "</option>");
                for (var i = 0; i < TTlen; i++) {
                    var id = TransTyp[i];
                    var name = TransTyp[i];
                    $("#mspgridadvsearchTransactionType").append("<option value='" + id + "'>" + name + "</option>");
                }
                var CTlen = ChrgTyp.length;
                $("#mspgridadvsearchChargeTypePrimary").empty();
                $("#mspgridadvsearchChargeTypePrimary").append("<option value='" + 0 + "'>" + "--Select--" + "</option>");
                for (var i = 0; i < CTlen; i++) {
                    var id = ChrgTyp[i];
                    var name = ChrgTyp[i];
                    $("#mspgridadvsearchChargeTypePrimary").append("<option value='" + id + "'>" + name + "</option>");
                }
                var VenLen = result.length;
                $("#mspgridadvsearchVendorClientLookupId").empty();
                $("#mspgridadvsearchVendorClientLookupId").append("<option value='" + 0 + "'>" + "--Select--" + "</option>");
                for (var i = 0; i < VenLen; i++) {
                    var id = result[i].id;
                    var name = result[i].name;
                    $("#mspgridadvsearchVendorClientLookupId").append("<option value='" + id + "'>" + name + "</option>");
                }
                if (typeValTransactionType) {
                    $("#mspgridadvsearchTransactionType").val(typeValTransactionType);
                }
                if (typeValChargeType) {
                    $("#mspgridadvsearchChargeTypePrimary").val(typeValChargeType);
                }
                if (typeValVendor) {
                    $("#mspgridadvsearchVendorClientLookupId").val(typeValVendor);
                }
                return json.data;
            }
        },
        "columns":
            [
                { "data": "TransactionType", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Storeroom", "autoWidth": true, "bSearchable": true, "bSortable": true },//V2-1033
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
$(document).on('change', '#MSPHistoryModel_HistoryDaterange', function () {
    dtmspHistoryTable.state.clear();
    MSPhGridclearAdvanceSearch();
    GenerateMSPHistoryGrid();
});
$(document).on('click', "#mspdownloahistorydata", function () {
    $(document).find('.historygridexport').trigger('click');
});


function onlyUnique(value, index, self) {
    return self.indexOf(value) === index;
}

//#endregion

//#region Receipts
var rgridfilteritemcount = 0;
var dtMSPReceipt;
var typeValVendor;
$(function () {
    $(document).find('#mspreceiptadvsearchcontainer').find(".sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $(document).on('click', "#MSPdownloadreceiptdata", function () {
        $(document).find('.mspreceiptgridexport').trigger('click');
    });
});
$(document).on('change', '#colorselector', function (evt) {
    $('.tabsArea').hide();
    openSideMenuTab(evt, $(this).val());
    $('#' + $(this).val()).show();
});
$(document).on('click', '.dismiss, .overlay', function () {
    $(document).find('#mspreceiptadvsearchcontainer').find(".sidebar").removeClass('active');
    $('.overlay').fadeOut();
});
$(document).on('click', '#MSPreceiptssidebarCollapse', function () {
    $(document).find('#mspreceiptadvsearchcontainer').find(".sidebar").addClass('active');
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
function generateMSPReceiptDataTable() {
    var partid = $(document).find('#PartId').val();
    var daterange = $(document).find('#MSPReceiptModel_receiptdtselector').val();
    var PurchaseOrderId = LRTrim($("#mspPurchaseOrder").val());
    var partclientlookup = $(document).find('#partclientlookupid').text();
    var POClientLookupId = LRTrim($("#mspPurchaseOrder").val());
    var ReceivedDate = LRTrim($("#mspReceiptDate").val());
    var VendorClientLookupId = LRTrim($("#msprgridReceiptadvsearchVendorClientLookupId").val());
    var VendorName = LRTrim($("#mspVendorName").val());
    var OrderQuantity = LRTrim($("#mspQuantity").val());
    var UnitCost = LRTrim($('#mspUnitCost').val());
    if (typeof dtMSPReceipt !== "undefined") {
        dtMSPReceipt.destroy();
    }
    dtMSPReceipt = $("#mspreceiptsTable").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        serverSide: true,
        "order": [[0, "asc"]],
        stateSave: true,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [
            {
                extend: 'excel',
                filename: partclientlookup + '_Receipts',
                extension: '.xlsx',
                className: "datatable-btn-export mspreceiptgridexport"
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/MultiStoreroomPart/populateMSPartReceipt",
            "type": "POST",
            "datatype": "json",
            data: function (d) {
                d.partid = partid;
                d.daterange = daterange;
                d.POClientLookupId = POClientLookupId;
                d.ReceivedDate = ReceivedDate;
                d.VendorClientLookupId = VendorClientLookupId;
                d.VendorName = VendorName;
                d.OrderQuantity = OrderQuantity;
                d.UnitCost = UnitCost;
            },
            "dataSrc": function (json) {
                var myDataSource = json;
                var Vendor = [];
                var VendorObj;
                for (var i = 0; i < myDataSource.dataAll.length; ++i) {
                    VendorObj = new VendorsObjArray(myDataSource.dataAll[i].VendorClientLookupId, myDataSource.dataAll[i].VendorClientLookupId + '-' + myDataSource.dataAll[i].VendorName);
                    Vendor.push(VendorObj);
                }
                Vendor = Vendor.filter(function (v) { return v.id !== '' });
                var result = [];
                $.each(Vendor, function (i, e) {
                    var matchingItems = $.grep(result, function (item) {
                        return item.id === e.id && item.name === e.name;
                    });
                    if (matchingItems.length === 0) result.push(e);
                });
                var VenLen = result.length;
                $("#msprgridReceiptadvsearchVendorClientLookupId").empty();
                $("#msprgridReceiptadvsearchVendorClientLookupId").append("<option value='" + 0 + "'>" + "--Select--" + "</option>");
                for (var i = 0; i < VenLen; i++) {
                    var id = result[i].id;
                    var name = result[i].name;
                    $("#msprgridReceiptadvsearchVendorClientLookupId").append("<option value='" + id + "'>" + name + "</option>");
                }
                if (typeValVendor) {
                    $("#msprgridReceiptadvsearchVendorClientLookupId").val(typeValVendor);
                }
                return json.data;
            }
        },
        "columns":
            [
                { "data": "POClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "ReceivedDate", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "VendorClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "VendorName", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "OrderQuantity", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "UnitCost", "autoWidth": true, "bSearchable": true, "bSortable": true }
            ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}
$(document).on('change', '#MSPReceiptModel_receiptdtselector', function () {
    dtMSPReceipt.state.clear();
    MSPRGridClearAdvanceSearch();
    generateMSPReceiptDataTable();
});

function VendorsObjArray(id, name) {
    this.id = id;
    this.name = name;
}
$(document).on('click', '#btnMSPReceiptDataAdvSrch', function () {
    dtMSPReceipt.state.clear();
    var searchitemhtml = "";
    rgridfilteritemcount = 0
    $('#mspreceiptgridadvsearchsidebar').find('.adv-item').each(function (index, item) {
        if ($(this).hasClass('dtpicker')) {
            $(this).val(ValidateDate($(this).val()));
        }
        if ($(this).val() && $(this).val() != "0") {
            rgridfilteritemcount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times rbtnCross" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $('#msprgridadvsearchfilteritems').html(searchitemhtml);
    $(document).find('#mspreceiptadvsearchcontainer').find(".sidebar").removeClass('active');
    $('.overlay').fadeOut();
    MSPRGridAdvanceSearch();
    typeValVendor = $("#msprgridReceiptadvsearchVendorClientLookupId").val();
});
function MSPRGridAdvanceSearch() {
    var purchaseOrder = LRTrim($('#mspPurchaseOrder').val());
    var ReceiptDate = ValidateDate($('#mspReceiptDate').val());
    var Vendor = $('#msprgridReceiptadvsearchVendorClientLookupId').val();
    var VendorName = LRTrim($('#mspVendorName').val());
    var Quantity = LRTrim($('#mspQuantity').val());
    var UnitCost = LRTrim($('#mspUnitCost').val());
    generateMSPReceiptDataTable();
    $('.receiptfilteritemcount').text(rgridfilteritemcount);
}
$(document).on('click', '#msprgridClearAdvSearchFilter', function () {
    $(document).find('#MSPReceiptModel_receiptdtselector').val("0").trigger('change.select2');
    MSPRGridClearAdvanceSearch();
    generateMSPReceiptDataTable();
});
function MSPRGridClearAdvanceSearch() {
    var rgridfilteritemcount = 0;
    $(document).find("#msprgridReceiptadvsearchVendorClientLookupId").val("").trigger('change');
    $('#mspreceiptgridadvsearchsidebar').find('input:text').val('');
    $('.receiptfilteritemcount').text(rgridfilteritemcount);
    $('#msprgridadvsearchfilteritems').find('span').html('').removeClass('tagTo');
    typeValVendor = $("#msprgridReceiptadvsearchVendorClientLookupId").val();
}
$(document).on('click', '.rbtnCross', function () {
    var btnCrossedId = $(this).parent().attr('id');
    var searchtxtId = btnCrossedId.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    rgridfilteritemcount--;
    if (searchtxtId == "msprgridReceiptadvsearchVendorClientLookupId") {
        typeValVendor = null;
    }
    MSPRGridAdvanceSearch();
});


//#endregion
//#region Show Full description
$(document).on('click', '#multistoreroompartmoreaddescription', function () {
    $(document).find('#multistoreroompartdetaildesmodal').modal('show');
    $(document).find('#mspartdetaildesmodaltext').text($(this).data("des"));
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
    var _isMobile = CheckLoggedInFromMob();
    var val = $(this).val();
    var imageName = val.replace(/^.*[\\\/]/, '')
    //console.log(imageName);
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
                        var partId = $(document).find('#PartId').val();
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
        url: '/MultiStoreroomPart/GetImages',
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
    var PartId = $('#PartId').val();
    cardviewlwngth = $(this).val();
    grdcardcurrentpage = parseInt(cardviewstartvalue / cardviewlwngth) + 1;
    cardviewstartvalue = parseInt((grdcardcurrentpage - 1) * cardviewlwngth) + 1;
    LoadImages(PartId);

});
$(document).on('click', '#tblimages_paginate .paginate_button', function () {
    var PartId = $('#PartId').val();
    if (layoutTypeWO == 1) {
        var currentselectedpage = parseInt($(document).find('#tblimages_paginate .pagination').find('.active').text());
        cardviewlwngth = $(document).find('#cardviewpagelengthdrp').val();
        cardviewstartvalue = cardviewlwngth * (parseInt($(this).find('.page-link').text()) - 1);
        var lastpage = parseInt($(this).prev('li').data('currentpage'));

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
    var PartId = $('#PartId').val();
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
    var PartId = $('#PartId').val();
    var ClientOnPremise = $('#ClientOnPremise').val();
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
                    RedirectToMultiStoreroomPartDetail(PartId, "OnPremiseImageReload");
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
                    RedirectToMultiStoreroomPartDetail(PartId, "AzureImageReload");
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
    var PartId = $('#PartId').val();
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
//#region V2-1007
$(document).on('click', '#linkToEquipment', function (e) {
    clearDropzone();
    var EquipmentId = $(document).find('#EquipmentId').val();
    window.location.href = "../Equipment/DetailFromWorkOrder?EquipmentId=" + EquipmentId;
});
//#endregion

//#region V2-1070 Activate/InActivate
$(document).on('click', '#actinctivatePartStoreroom', function () {
    var partId = $('#PartId').val();
    var clientLookupId = $(document).find('#ClientLookupId').val();
    var InActiveFlag = $(document).find('#parthiddeninactiveflag').val();
    $.ajax({
        url: "/MultiStoreroomPart/ValidateForActiveInactive",
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
                            url: "/MultiStoreroomPart/MakeActiveInactive",
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
                                        RedirectToMultiStoreroomPartDetail(partId);
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
$(document).on('click', '.partsConfigureAutoPurchasingBtn', function (e) {
    ShowConfigureAutoPurchasing();
    var row = $(this).parents('tr');
    var Rowdata = dtStoreroomsTable.row(row).data();
    $(document).find("#partsConfigureAutoPurchasingModel_PartStoreroomId").val('');
    $(document).find("#partsConfigureAutoPurchasingModel_PartStoreroomId").val(Rowdata.PartStoreroomId);
    
});
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
            RedirectToMultiStoreroomPartDetail(data.PartId);
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