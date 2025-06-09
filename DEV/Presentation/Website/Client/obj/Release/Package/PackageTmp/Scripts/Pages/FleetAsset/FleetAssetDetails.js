var dtTable;
var dtPartsTable;
var editor;
var PartsDataTable;
var _EquipmentId;
var dtNotesTable;
var dtAttachmentTable;
var _ObjectId;
var dtLastCompleted = '<div class="dtlsBox"><h2>#val1</h2><p>#val2</p><p></p></div>';
var colstr = '<div class="dtlsBox"><h2>#val1</h2><p>#val2</p></div>';
var colorArray = ["#fe0000",
    "#ff7f00",
    "#fffe01",
    "#00bd3f",
    "#0068ff",
    "#7a01e6",
    "#d300c9",
    "#940100",
    "#066d7c",
    "#66cbff"];

function openCity2(evt, cityName) {
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent2");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tablinks2");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(cityName).style.display = "block";
    evt.currentTarget.className += " active";
}
function openCity(evt, cityName) {
    evt.preventDefault();
    $('#FleetAssettab').hide();
    switch (cityName) {        
        case "Attachment":
            generateAttachmentDataTable();
            hideAudit();
            break;      
        case "PartsContainer":
            generatePartsDataTable();
            hideAudit();
            break;      
        case "Equipment":
            $('#FleetAssettab').show();
            $('.tabcontentIdentification').show();
            $('#btnIdentification').addClass('active');
            ShowAudit();
            break;
        case "SOContainer":
            generateSODataTable();
            hideAudit();
            break; 
        case "ScheduledServiceContainer":
            generateScheduledServiceDataTable();
            hideAudit();
            break;
        case "FuelContainer":
            generateFleetFuelDataTable();
            hideAudit();
            break;
        case "MeterContainer":
            generateFleetMeterDataTable();
            hideAudit();
            break;
        case "FleetIssueContainer":
            generateFleetIssueDataTable();
            hideAudit();
            break;
            
    }
    var i, tabcontent
    tabcontent = document.getElementsByClassName("tabcontent2");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    RemovedTabsActive();
    document.getElementById(cityName).style.display = "block";
    if (typeof evt.currentTarget !== "undefined") {
        evt.currentTarget.className += " active";
    }
}

function RemovedTabsActive() {
    var tablinks;
    tablinks = document.getElementsByClassName("tablinks2");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }

}
function hideAudit() {
    $('#btnnblock').removeClass('col-xl-6').addClass('col-xl-12');
    $('#auditlogcontainer').removeClass('col-xl-6').addClass('col-xl-12').hide();
}
function ShowAudit() {
    $('#btnnblock').removeClass('col-xl-12').addClass('col-xl-6');
    $('#auditlogcontainer').removeClass('col-xl-12').addClass('col-xl-6').show();
}
$(document).ready(function () {
    $(document).find(".sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $(document).on('click', '.dismiss, .overlay', function () {
        $('.sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });
});
$(function () {
    $('[data-toggle="tooltip"]').tooltip();
    var myLineChart;
    var WObyPriorityChart;
    var fontSize = 12;
    $(document).on('click', '#btnAddParts', function (e) {
        var equipmentid = $(document).find('#FleetAssetModel_EquID').val();
        AddParts(equipmentid);
    });   
    $(document).find(".tabsArea").hide();
    $(document).find("ul.vtabs li:first").addClass("active").show();
    $(document).find(".tabsArea:first").show();
    $(document).on('change', '#colorselector', function (evt) {
        $(document).find('.tabsArea').hide();
        openCity(evt, $(this).val());
        $('#' + $(this).val()).show();
    });
    //#region Equiment Edit
    $(document).on('click', '#ImgShowAccount', function (e) {
        $("#ImgCloseAccount").show();
        $("#dvAccountContainer").show();
        $("#ImgShowAccount").hide();
    });
    $(document).on('click', '#ImgCloseAccount', function (e) {
        $("#dvAccountContainer").hide();
        $("#ImgShowAccount").show();
        $("#ImgCloseAccount").hide();
    });
    //#endregion
    $('#tabselector2').on('change', function (evt) {
        var cityName = $(this).val();
        openCity(evt, cityName);
    });
});
function EquipmentEditOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("EquipmentUpdateAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToFleetAssetDetail(data.equipmentid, "equipment");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$('#searchTable3 tbody').on('change', 'input[type="checkbox"]', function () {
    if (!this.checked) {
        var el = $('#example-select-all').get(0);
        if (el && el.checked && ('indeterminate' in el)) {
            el.indeterminate = true;
        }
    }
});

function generatePartsDataTable() {
    var EquipmentId = $('#FleetAssetModel_EquID').val();
    var rCount = 0;
    var visibility;
    if ($(document).find('#partsTable').hasClass('dataTable')) {
        dtPartsTable.destroy();
    }
    dtPartsTable = $("#partsTable").DataTable({
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
            "url": "/FleetAsset/GetFleetAsset_Parts",
            data: { EquipmentId: EquipmentId },
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (json) {
                visibility = json.partSecurity;
                rCount = json.data.length;
                return json.data;
            }
        },
        columnDefs: [
            {
                targets: [5],
                render: function (a, b, data, d) {
                    if (visibility == true) {
                        return '<a class="btn btn-outline-primary addBtnParts gridinnerbutton" title= "Add"> <i class="fa fa-plus"></i></a>' +
                            '<a class="btn btn-outline-success editBttnParts gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delBtnParts gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
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
                "data": "Part_ClientLookupId",
                "autoWidth": true,
                "bSearchable": true,
                "bSortable": true,
                "mRender": function (data, type, row) {
                    return '<a class=link_part_detail href="javascript:void(0)">' + data + '</a>';
                }
            },
            {
                "data": "Part_Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                mRender: function (data, type, full, meta) {
                    return "<div class='text-wrap width-200'>" + data + "</div>";
                }
            },
            { "data": "QuantityNeeded", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "QuantityUsed", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "Comment", "autoWidth": true, "bSearchable": true, "bSortable": true },
            {
                "data": "EquipmentId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center"
            }
        ],
        initComplete: function () {
            var column = this.api().column(5);
            if (visibility == false) {
                column.visible(false);
            }
            else {
                column.visible(true);
            }
            if (rCount > 0 || visibility == false) { $("#btnAddParts").hide(); }
            else {
                $("#btnAddParts").show();
            }
            SetPageLengthMenu();
        }
    });
}
function clearDropzone() {
    deleteServer = false;
    if ($(document).find('#dropzoneForm').length > 0) {
        Dropzone.forElement("div#dropzoneForm").destroy();
    }
}
$(document).on('click', '.setImage', function () {
    var imageName = $(this).data('image');
    var EquimentId = $('#FleetAssetModel_EquID').val();
    $.ajax({
        url: '../base/SaveUploadedFileToServer',
        type: 'POST',

        data: { 'fileName': imageName, objectId: EquimentId, TableName: "Equipment", AttachObjectName: "Equipment" },
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
});
//#region Photos
$(document).on('click', '#deleteImg', function () {
    var EquipmentId = $('#FleetAssetModel_EquID').val();
    var ClientOnPremise = $('#FleetAssetModel_ClientOnPremise').val();
    if (ClientOnPremise == 'True') {
        DeleteOnPremiseImage(EquipmentId);
    }
    else {
        DeleteAzureImage(EquipmentId);
    }
});
function DeleteAzureImage(EquipmentId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/FleetAsset/DeleteImageFromAzure',
            type: 'POST',
            data: { _EquimentId: EquipmentId, TableName: "Equipment", Profile: true, Image: true },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data === "success" || data === "not found") {
                    RedirectToFleetAssetDetail(EquipmentId, "AzureImageReload");
                    ShowImageDeleteSuccessAlert();
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}

function DeleteOnPremiseImage(EquipmentId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/FleetAsset/DeleteImageFromOnPremise',
            type: 'POST',
            data: { _EquimentId: EquipmentId, TableName: "Equipment", Profile: true, Image: true },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data === "success" || data === "not found") {
                    RedirectToFleetAssetDetail(EquipmentId, "OnPremiseImageReload");
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

//#region Attachment
$(document).on('click', '.delAttchBttn', function () {
    var data = dtAttachmentTable.row($(this).parents('tr')).data();
    DeleteEquipmentAttachment(data.FileAttachmentId);
});
function DeleteEquipmentAttachment(fileAttachmentId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/FleetAsset/DeleteAttachment',
            data: {
                _fileAttachmentId: fileAttachmentId
            },
            beforeSend: function () {
                ShowLoader();
            },
            type: "POST",
            datatype: "json",
            success: function (data) {
                if (data.Result == "success") {
                    dtAttachmentTable.state.clear();
                    ShowDeleteAlert(getResourceValue("attachmentDeleteSuccessAlert"));
                }
                else {
                    ShowErrorAlert(data.Message);
                }
            },
            complete: function () {
                generateAttachmentDataTable();
                CloseLoader();
            }
        });
    });
}
$(document).on('click', "#btnAddAttachment", function () {
    var equipmentid = $(document).find('#FleetAssetModel_EquID').val();
    var ClientlookUpId = $(document).find('#FleetAssetModel_ClientLookupId').val();
    var Name = $('#FleetAssetModel_Name').val();
    $.ajax({
        url: "/FleetAsset/ShowAddAttachment",
        type: "GET",
        dataType: 'html',
        data: { EquipmentId: equipmentid, ClientlookUpId: ClientlookUpId, Name: Name },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#fleetassetmaincontainer').html(data);
        },
        complete: function () {
           SetFleetAssetControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
function generateAttachmentDataTable() {
    var equipmentid = $(document).find('#FleetAssetModel_EquID').val();
    if ($(document).find('#AttachTable').hasClass('dataTable')) {
        dtAttachmentTable.destroy();
    }
    var visibility;
    var attchCount = 0;
    dtAttachmentTable = $("#AttachTable").DataTable({
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

            "url": "/FleetAsset/PopulateAttachment?EquipmentId=" + equipmentid,
            "type": "post",
            "datatype": "json",
            "dataSrc": function (response) {
                attchCount = response.recordsTotal;
                if (attchCount > 0) {
                    $(document).find('#asstAttachmentCount').show();
                    $(document).find('#asstAttachmentCount').html(attchCount);
                }
                else {
                    $(document).find('#asstAttachmentCount').hide();
                }
                return response.data;
            }
        },
        columnDefs: [
            {
                targets: [5], render: function (a, b, data, d) {
                    return '<a class="btn btn-outline-danger delAttchBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                }
            }
        ],
        "columns":
        [
            {
                "data": "Subject", "autoWidth": true, "bSearchable": true, "bSortable": true,
                "mRender": function (data, type, row) {
                    return "<div class='text-wrap width-200'>" + data + "</div>";
                }
            },
            {
                "data": "FileName",
                "autoWidth": true,
                "bSearchable": true,
                "bSortable": true,
                "mRender": function (data, type, row) {
                    return '<a class=lnk_download_attachment href="javascript:void(0)"  target="_blank">' + row.FullName + '</a>';
                }
            },
            { "data": "FileSizeWithUnit", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "OwnerName", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "CreateDate", "type": "date " },
            { "data": "ObjectId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center" }
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
$(document).on('submit', "#frmeqpattachmentadd", function (e) {
    e.preventDefault();
    var form = document.querySelector('#frmeqpattachmentadd');
    if (!$('#frmeqpattachmentadd').valid()) {
        return;
    }
    var data = new FormData(form);
    $.ajax({
        type: "POST",
        url: "/FleetAsset/AddAttachment",
        data: data,
        processData: false,
        contentType: false,
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.Result == "success") {
                var EquipmentId = data.equipmentid;
                SuccessAlertSetting.text = getResourceValue("AddAttachmentAlerts");
                swal(SuccessAlertSetting, function () {
                    RedirectToFleetAssetDetail(EquipmentId, "attachment");
                });
            }
            else {
                ShowGenericErrorOnAddUpdate(data);
            }
        },
        complete: function () {
            CloseLoader();
        },
        error: function (xhr) {
            CloseLoader();
        }
    });
});
$(function () {
    $(document).on('click', "#btnattachmentcancel", function () {
        var equipmentid = $(document).find('#attachmentModel_EquipmentId').val();
        swal(CancelAlertSetting, function () {
            RedirectToFleetAssetDetail(equipmentid, "attachment");
        });
    });
});
$(document).on('click', '.lnk_download_attachment', function (e) {
    e.preventDefault();
    var row = $(this).parents('tr');
    var data = dtAttachmentTable.row(row).data();
    var FileAttachmentId = data.FileAttachmentId;
    $.ajax({
        type: "post",
        url: '/Base/IsOnpremiseCredentialValid',
        success: function (data) {
            if (data === true) {
                window.location = '/FleetAsset/DownloadAttachment?_fileinfoId=' + FileAttachmentId;
            }
            else {
                ShowErrorAlert(getResourceValue("NotAuthorisedDownloadFileAlert"));
            }
        }
    });

});
//#endregion
//#region Parts
$(document).on('click', '.addBtnParts', function () {
    var data = dtPartsTable.row($(this).parents('tr')).data();
    AddParts(data.EquipmentId);
});
$(document).on('click', '.editBttnParts', function () {
    var data = dtPartsTable.row($(this).parents('tr')).data();
    EditParts(data.EquipmentId, data.Equipment_Parts_XrefId, encodeURIComponent(data.Part_ClientLookupId), data.QuantityNeeded, data.QuantityUsed, (encodeURIComponent(data.Comment)).replace(/%20/g, "&#32;"), data.UpdatedIndex, "update");
});
$(document).on('click', '.delBtnParts', function () {
    var data = dtPartsTable.row($(this).parents('tr')).data();
    DeleteParts(data.Equipment_Parts_XrefId);
});
function AddParts(eqid) {
    var ClientlookUpId = $(document).find('#FleetAssetModel_ClientLookupId').val();
    var Name = $('#FleetAssetModel_Name').val();
    $.ajax({
        url: '/FleetAsset/PartsAdd',
        data: { EquipmentId: eqid, ClientlookUpId: ClientlookUpId, Name: Name },
        type: "GET",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#fleetassetmaincontainer').html(data);
        },
        complete: function () {
           SetFleetAssetControls();
        }
    });
}
$(document).on('click', '#Partsaddcancelbutton,#partseditcancelbutton', function (e) {
    var equipmentid = $(document).find('#partsSessionData_EquipmentId').val();
    swal(CancelAlertSetting, function () {
        RedirectToFleetAssetDetail(equipmentid, "parts");
    });
});
function PartAddOnSuccess(data) {
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("AddPartsAlerts");
        swal(SuccessAlertSetting, function () {
            RedirectToFleetAssetDetail(data.equipmentid, "parts");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
function EditParts(eqid, Equipment_Parts_XrefId, Part_ClientLookupId, QuantityNeeded, QuantityUsed, Comment, UpdatedIndex) {
    var ClientlookUpId = $(document).find('#FleetAssetModel_ClientLookupId').val();
    var Name = $('#FleetAssetModel_Name').val();
    $.ajax({
        url: '/FleetAsset/PartsEdit',
        data: { EquipmentId: eqid, Equipment_Parts_XrefId: Equipment_Parts_XrefId, ClientlookUpId: ClientlookUpId, Name: Name },
        type: "GET",
        datatype: "html",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#fleetassetmaincontainer').html(data);
        },
        complete: function () {
           SetFleetAssetControls();
        }
    });
};
function PartsEditOnSuccess(data) {
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("UpdatePartsAlerts");
        swal(SuccessAlertSetting, function () {
            RedirectToFleetAssetDetail(data.equipmentid, "parts");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
function DeleteParts(eqid) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/FleetAsset/PartsDelete',
            data: { _EquipmentPartSpecsId: eqid },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    ShowDeleteAlert(getResourceValue("partDeleteSuccessAlert"));
                }
            },
            complete: function () {
                dtPartsTable.state.clear;
                generatePartsDataTable();
                CloseLoader();
            }
        });
    });
}
$(document).on('click', '.link_part_detail', function (e) {
    var row = $(this).parents('tr');
    var data = dtPartsTable.row(row).data();
    var partId = data.PartId;
    clearDropzone();
    window.location.href = "../Parts/PartsDetailFromFleetAssets?partId=" + partId;
});
//#endregion
$(document).on('click', '.lithisfleetasset', function () {
    var equipmentid = $(this).attr('data-val');
    RedirectToFleetAssetDetail(equipmentid, "equipment");
});

$(document).on('click', '.lnkNotesDetails', function () {
    var msg_details = $(this).next().val();
    $(document).find('#NotesContent').text(msg_details);
    $(document).find('#Noteslogdetail').modal('show');
});
var dtPartsXRefTable;
function generatePartsXRefDataTable() {
    var EquipmentId = $('#FleetAssetModel_EquID').val();
    var rCount = 0;
    var visibility;
    if ($(document).find('#partsTable').hasClass('dataTable')) {
        dtPartsXRefTable.destroy();
    }
    dtPartsXRefTable = $("#partsTable").DataTable({
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
            url: dataTableLocalisationUrl,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Base/GetEquipmentPartsXrefGridData",
            data: { EquipmentId: EquipmentId },
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (json) {
                rCount = json.data.length;
                return json.data;
            }
        },
        "columns":
        [
            { "data": "CleintLookUpId", "autoWidth": true, "bSearchable": true, "bSortable": true },
            {
                "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                mRender: function (data, type, full, meta) {
                    return "<div class='text-wrap width-400'>" + data + "</div>";
                }
            },
            { "data": "Manufacturer", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "ManufacturerID", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "StockType", "autoWidth": true, "bSearchable": true, "bSortable": true },
        ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}

//#region For Service Order
function generateSODataTable() {
    var EquipmentId = $('#FleetAssetModel_EquID').val();
    var rCount = 0;
    var visibility;
    if ($(document).find('#SOTable').hasClass('dataTable')) {
        dtSOTable.destroy();
    }
    dtSOTable = $("#SOTable").DataTable({
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
            "url": "/FleetAsset/GetFleetAsset_ServiceOrder",
            data: { EquipmentId: EquipmentId },
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (json) {
                visibility = json.partSecurity;
                rCount = json.data.length;
                return json.data;
            }
        },
        columnDefs: [
            {
                targets: [0, 1],
                className: 'noVis'
            }
        ],
        "columns":
            [
                {
                    "data": "ServiceOrderId",
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
                //{ "data": "ServiceOrderId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": false,
                    "mRender": function (data, type, row) {
                        return '<a class=link_Redirectfleetservice_detail href="javascript:void(0)">' + data + '</a>';
                    }
                },
                { "data": "EquipmentClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "AssetName", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    render: function (data, type, row, meta) {

                        if (data == statusCode.Canceled) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--orange m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Complete) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--green m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Open) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--purple m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Scheduled) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--blue m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }

                        else {
                            return getStatusValue(data);
                        }
                    }
                },
                { "data": "Type", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "CreateDate", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "CompleteDate", "autoWidth": true, "bSearchable": true, "bSortable": true }
            ],
        initComplete: function () {
            
            SetPageLengthMenu();
        }
    });
}
//#endregion
//#region Line Item Grid
$(document).on('click', '#SOTable tbody td img', function (e) {
    var tr = $(this).closest('tr');
    var row = dtSOTable.row(tr);
    if (this.src.match('details_close')) {
        this.src = "../../Images/details_open.png";
        row.child.hide();
        tr.removeClass('shown');
    }
    else {
        this.src = "../../Images/details_close.png";
        var ServiceOrderID = $(this).attr("rel");
        $.ajax({
            url: "/FleetAsset/GetServiceOrderInnerGrid",
            data: {
                ServiceOrderID: ServiceOrderID
            },
            beforeSend: function () {
                ShowLoader();
            },
            dataType: 'html',
            success: function (json) {
                row.child(json).show();
                dtinnerGrid = row.child().find('.ServiceOrderinnerDataTable').DataTable(
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
                            { className: 'text-right', targets: [4] }
                        ],
                        "footerCallback": function (row, data, start, end, display) {
                            var api = this.api(),
                                // Total over all pages
                                total = api.column(4).data().reduce(function (a, b) {
                                    return parseFloat(a) + parseFloat(b);
                                }, 0);
                            // Update footer
                            $(api.column(4).footer()).html(total.toFixed(2));
                        },
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
//#endregion

//#region For Scheduled Service
function generateScheduledServiceDataTable() {
    var EquipmentId = $('#FleetAssetModel_EquID').val();
    var rCount = 0;
    var visibility;
    if ($(document).find('#SSTable').hasClass('dataTable')) {
        dtSSTable.destroy();
    }
    dtSSTable = $("#SSTable").DataTable({
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
            "url": "/FleetAsset/GetFleetAsset_ScheduledService",
            data: { EquipmentId: EquipmentId },
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (json) {
                visibility = json.partSecurity;
                rCount = json.data.length;
                return json.data;
            }
        },
        columnDefs: [
            {
                targets: [0],
                className: 'noVis'
            }
        ],
        "columns":
            [
               
                { "data": "ServiceTaskDescription", "autoWidth": true, "bSearchable": true, "bSortable": false, className: 'text-left' }, 
                {
                    "data": "TimeInterval",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": false,
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
                    "bSortable": false,
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
                    "bSortable": false,
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

                }
                
                
            ],
        initComplete: function () {
            
            SetPageLengthMenu();
        }
    });
}
//#endregion

//#region For Fleet Fuel
function generateFleetFuelDataTable() {
    var EquipmentId = $('#FleetAssetModel_EquID').val();
    var rCount = 0;
    var visibility;
    if ($(document).find('#FuelTable').hasClass('dataTable')) {
        dtFFTable.destroy();
    }
    dtFFTable = $("#FuelTable").DataTable({
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
            "url": "/FleetAsset/GetFleetAsset_FuelTracking",
            data: { EquipmentId: EquipmentId },
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (json) {
                visibility = json.partSecurity;
                rCount = json.data.length;
                return json.data;
            }
        },
        columnDefs: [
            {
                targets: [0],
                className: 'noVis'
            }
        ],
        "columns":
            [

                { "data": "ReadingDate", "autoWidth": true, "bSearchable": true, "bSortable": false, className: 'text-left' },
                {
                    "data": "Amount", "autoWidth": true, "bSearchable": true, "bSortable": false, className: 'text-left',
                    mRender: function (data, type, full, meta) {
                        if (full.FuelUnits == "Liters") {
                            LocalizedFuelUnit = getResourceValue("LitersAlert");
                        }
                        else if (full.FuelUnits == "Units") {
                            LocalizedFuelUnit = getResourceValue("UnitAlert");
                        }
                        else {
                            LocalizedFuelUnit = full.FuelUnits;
                        }
                        return colstr.replace('#val1', full.FuelAmount).replace('#val2', LocalizedFuelUnit);
                    }
                },
                { "data": "UnitCost", "autoWidth": true, "bSearchable": true, "bSortable": false, className: 'text-left' },
                { "data": "TotalCost", "autoWidth": true, "bSearchable": true, "bSortable": false, className: 'text-left' }   


            ],
        initComplete: function () {

            SetPageLengthMenu();
        }
    });
}
//#endregion

//#region For Fleet Meter
function generateFleetMeterDataTable() {
    var EquipmentId = $('#FleetAssetModel_EquID').val();
    var rCount = 0;
    var visibility;
    if ($(document).find('#MeterTable').hasClass('dataTable')) {
        dtFFTable.destroy();
    }
    dtFFTable = $("#MeterTable").DataTable({
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
            "url": "/FleetAsset/GetFleetAsset_FuelMeter",
            data: { EquipmentId: EquipmentId },
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (json) {
                visibility = json.partSecurity;
                rCount = json.data.length;
                return json.data;
            }
        },
        columnDefs: [
            {
                targets: [0],
                className: 'noVis'
            }
        ],
        "columns":
            [

                {
                    "data": "ReadingDate", "autoWidth": true, "bSearchable": true, "bSortable": false, "name": "1",
                    mRender: function (data, type, full, meta) {
                        return colstr.replace('#val1', full.ReadingDate).replace('#val2', full.NoOfDays + "  " + getResourceValue("DaysAgoAlert"));
                    }
                },
                {
                    "data": "ReadingLine1", "autoWidth": true, "bSearchable": true, "bSortable": false, "name": "2", className: 'text-right',
                    mRender: function (data, type, full, meta) {
                        if (full.Meter2Indicator == true) {
                            if (full.ReadingLine2 == "CR") {
                                return colstr.replace('#val1', full.ReadingLine1 + "  " + '<span class="fmbadgelabel">' + getResourceValue("SecondaryMeterAlert") + '</span>').replace('#class-2', 'm-text-green').replace('#val2', getResourceValue("CurrentReadingAlert"));
                            }
                            else {
                                return colstr.replace('#val1', full.ReadingLine1 + "  " + '<span class="fmbadgelabel">' + getResourceValue("SecondaryMeterAlert") + '</span>').replace('#class-2', 'm-text-grey').replace('#val2', full.ReadingLine2);
                            }

                        }
                        else {
                            if (full.ReadingLine2 == "CR") {
                                return colstr.replace('#val1', full.ReadingLine1).replace('#class-2', 'm-text-green').replace('#val2', getResourceValue("CurrentReadingAlert"));
                            }
                            else { return colstr.replace('#val1', full.ReadingLine1).replace('#class-2', 'm-text-grey').replace('#val2', full.ReadingLine2); }
                        }
                    }
                },
                {
                    "data": "SourceType", "autoWidth": true, "bSearchable": true, "bSortable": false, "name": "3", className: 'text-center'
                }                
            ],
        initComplete: function () {

            SetPageLengthMenu();
        }
    });
}
//#endregion

//#region For Fleet Issue
function generateFleetIssueDataTable() {
    var EquipmentId = $('#FleetAssetModel_EquID').val();
    var rCount = 0;
    var visibility;
    if ($(document).find('#FleetIssueTable').hasClass('dataTable')) {
        dtFITable.destroy();
    }
    dtFITable = $("#FleetIssueTable").DataTable({
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
            "url": "/FleetAsset/GetFleetAsset_FleetIssue",
            data: { EquipmentId: EquipmentId },
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (json) {
                visibility = json.partSecurity;
                rCount = json.data.length;
                return json.data;
            }
        },
        columnDefs: [
            {
                targets: [0],
                className: 'noVis'
            }
        ],
        "columns":
            [

                { "data": "RecordDate", "autoWidth": true, "bSearchable": true, "bSortable": false },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": false,
                    mRender: function (data, type, full, meta) {
                        return "<div class='text-wrap width-300'>" + data + "</div>";
                    }
                }, 
                { "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": false },
                {
                    "data": "Defects", "autoWidth": true, "bSearchable": false, "bSortable": false,
                    mRender: function (data, type, full, meta) {
                        return "<div class='text-wrap width-100'>" + data + "</div>";
                    }
                },
                {
                    "data": "CompleteDate", "autoWidth": true, "bSearchable": false, "bSortable": false,
                    mRender: function (data, type, full, meta) {
                        if (full.CompleteDate) {
                            return colstr.replace('#val1', full.CompleteDate).replace('#val2', full.ServiceOrderClientLookupId);
                        }
                        else {
                            return colstr.replace('#val1', "").replace('#val2', full.ServiceOrderClientLookupId);
                        }
                    }
                }
            ],
        initComplete: function () {

            SetPageLengthMenu();
        }
    });
}
//#endregion

//#region Redirect To Service Order Details 
$(document).on('click', '.link_Redirectfleetservice_detail', function () {
    var row = $(this).parents('tr');
    var data = dtSOTable.row(row).data();
    var ServiceOrderId = data.ServiceOrderId;
    clearDropzone();
    window.location.href = "../FleetService/DetailFromFleetAsset?ServiceOrderId=" + ServiceOrderId;
});

//#endregion

//#region Asset Availability
function AssetAvailabilityUpdateOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        var Remove= $("#FleetAssetModel_RemoveFromService").val();
        if (Remove == 'False') {
            SuccessAlertSetting.text = getResourceValue("RemoveServiceAlerts");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("OutofServiceAlerts");
        }
        
            swal(SuccessAlertSetting, function () {
                $("#AssetAvailabilityModal").modal('hide');
                RedirectToFleetAssetDetail(data.EquipmentId, "equipment");
                $('.modal-backdrop').hide();
            });
       
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}

$(document).on('click', '#btnReturn', function () {  
    var EquipmentId = $(this).attr('data-equipid');
    $.ajax({
        type: "POST",
        url: "/FleetAsset/ReturnToInservice",
        data: { EquipmentId: EquipmentId},
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.Result == "success") {
                SuccessAlertSetting.text = getResourceValue("InServiceAlerts");
                swal(SuccessAlertSetting, function () {
                    RedirectToFleetAssetDetail(EquipmentId, "equipment");
                });
            }
            else {
                ShowGenericErrorOnAddUpdate(data);
            }
        },
        complete: function () {
            CloseLoader();
        },
        error: function (xhr) {
            CloseLoader();
        }
    });

});

$(document).on('click', '#btnRemove,#btnUpdate', function () {
    $(document).find('form').trigger("reset");
    $(document).find('form').find("select").not("#colorselector").val("").trigger('change.select2');
    $(document).find('form').find("input").removeClass("input-validation-error");
    $(document).find('form').find("textarea").removeClass("input-validation-error");
});
//#endregion