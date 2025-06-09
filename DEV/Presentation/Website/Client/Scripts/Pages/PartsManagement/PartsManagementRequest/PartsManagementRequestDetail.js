//#region Attachment
var mrAttachmentTable
$(function () {
    $(document).on('change', '#colorselector', function (evt) {
        $(document).find('.tabsArea').hide();
        openCity(evt, $(this).val());
        $('#' + $(this).val()).show();
    });

});
function generateMrAttachmentsGrid() {
    if ($(document).find('#tblpmrattachment').hasClass('dataTable')) {
        mrAttachmentTable.destroy();
    }
    mrAttachmentTable = $("#tblpmrattachment").DataTable({
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
            "url": "/PartsManagementRequest/PopulateMSAttachments",
            "type": "POST",
            data: function (d) {
                d.PartMasterRequestId = LRTrim($(document).find('#partsManagementRequestModel_PartMasterRequestId').val()),
                    d.RequestType = $('#partsManagementRequestModel_RequestType').val();
            },
            "datatype": "json"
        },
        columnDefs: [
            {
                targets: [4], render: function (a, b, data, d) {
                    return '<a class="btn btn-outline-primary editAttachmentBttn gridinnerbutton" title="Edit"><i class="fa fa-pencil"></i></a>' +
                        '<a class="btn btn-outline-success dnldAttachmentBttn gridinnerbutton" title= "Download"> <i class="fa fa-download"></i></a>' +
                        '<a class="btn btn-outline-danger delAttachmentBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                }
            }
        ],
        "columns":
        [
            {
                "data": "FileName", "autoWidth": true, "bSearchable": true, "bSortable": true,
                "mRender": function (data, type, row) { return row.FullName; }
            },
            //{ "data": "FullName", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "Subject", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "UploadedBy", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "CreateDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "type": "date " },
            {
                "data": "ObjectId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center"
            }
        ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', '.dnldAttachmentBttn', function (e) {
    e.preventDefault();
    var row = $(this).parents('tr');
    var data = mrAttachmentTable.row(row).data();
    var FileAttachmentId = data.FileAttachmentId;
    var PartMasterRequestId = $(document).find('#partsManagementRequestModel_PartMasterRequestId').val();
    $.ajax({
        type: "post",
        url: '/Base/IsOnpremiseCredentialValid',
        success: function (data) {
            if (data === true) {
                window.location = '/PartsManagementRequest/DownloadAttachment?_fileinfoId=' + FileAttachmentId + '&PMPartMasterRequestId=' + PartMasterRequestId;
            }
            else {
                ShowErrorAlert(getResourceValue("NotAuthorisedDownloadFileAlert"));
            }
        }

    });

});


$(document).on('click', '.delAttachmentBttn', function () {
    var data = mrAttachmentTable.row($(this).parents('tr')).data();
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/PartsManagementRequest/DeleteAttachments',
            data: {
                _fileAttachmentId: data.FileAttachmentId
            },
            beforeSend: function () {
                ShowLoader();
            },
            type: "POST",
            datatype: "json",
            success: function (data) {
                if (data.Result == "success") {
                    mrAttachmentTable.state.clear();
                    ShowDeleteAlert(getResourceValue("attachmentDeleteSuccessAlert"));
                }
                else {
                    ShowErrorAlert(data.Message);
                }
            },
            complete: function () {
                generateMrAttachmentsGrid();
                CloseLoader();
            }
        });
    });
});

function EditAttachmentOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        $("#PartMRequestEditAssignment").modal('hide');
        $('.modal-backdrop').remove();
        SuccessAlertSetting.text = getResourceValue("attachmentUpdateSuccessAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToPartMasterRequestDetail(data.PartMasterRequestId, data.RequestType, "attachment");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data, '#PartMRequestEditAssignment');
    }
}

$(document).on('click', '.editAttachmentBttn', function (e) {
    var data = mrAttachmentTable.row($(this).parents('tr')).data();
    $(document).find('#partsManagementAttachmentModel_Description').val(data.Subject);
    $(document).find('#partsManagementAttachmentModel_AttachmentId').val(data.FileAttachmentId);
    $(document).find('#partsManagementAttachmentModel_PartMasterRequestId').val(data.PartMasterRequestId);
    $(document).find('#partsManagementAttachmentModel_RequestType').val(data.RequestType);
    $('#PartMRequestEditAssignment').modal('show');
    $(this).blur();
});


$(document).on('click', "#btnAddPartMasterRequestAttachment", function () {
    var partMasterRequestId = $(document).find('#partsManagementRequestModel_PartMasterRequestId').val();
    var clientLookupId = $(document).find('#partsManagementRequestModel_PartMaster_ClientLookupId').val();
    var requestType = $(document).find('#partsManagementRequestModel_RequestType').val();
    $.ajax({
        url: "/PartsManagementRequest/AddAttachment",
        type: "GET",
        dataType: 'html',
        data: { PartMasterRequestId: partMasterRequestId, ClientLookupId: clientLookupId, RequestType: requestType },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpartsreview').html(data);
        },
        complete: function () {
            $.validator.unobtrusive.parse(document);
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});


$(document).on('submit', "#frmpartmastreqattachmentadd", function (e) {

    e.preventDefault();
    var form = document.querySelector('#frmpartmastreqattachmentadd');
    if (!$('#frmpartmastreqattachmentadd').valid()) {
        return;
    }
    var data = new FormData(form);
    $.ajax({
        type: "POST",
        url: "/PartsManagementRequest/AddAttachment",
        data: data,
        processData: false,
        contentType: false,
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.Result == "success") {
                SuccessAlertSetting.text = getResourceValue("AddAttachmentAlerts");
                swal(SuccessAlertSetting, function () {
                    RedirectToPartMasterRequestDetail(data.partMasterRequestId, data.requestType, "attachment");
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


$(document).on('click', "#btnattachmentcancel", function () {
    var PartMasterRequestId = $('#partsManagementAttachmentModel_PartMasterRequestId').val();
    var RequestType = $('#partsManagementAttachmentModel_RequestType').val();
    swal(CancelAlertSetting, function () {
        RedirectToPartMasterRequestDetail(PartMasterRequestId, RequestType, "attachment");
    });
});
//#endregion Attachment
//#region ReviewLog
var mrReviewLogTable;
function generateMrReviewLogGrid() {
    if ($(document).find('#tblpmreviewlog').hasClass('dataTable')) {
        mrReviewLogTable.destroy();
    }
    var partMasterRequestId = LRTrim($(document).find('#partsManagementRequestModel_PartMasterRequestId').val());
    mrReviewLogTable = $("#tblpmreviewlog").DataTable({
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
            "url": "/PartsManagementRequest/PopulateReviewLog",
            "type": "POST",
            data: function (d) {
                d.PartMasterRequestId = LRTrim($(document).find('#partsManagementRequestModel_PartMasterRequestId').val());
                d.ReviewDate = LRTrim($(document).find('#ReviewDate').val());
                d.Reviewed_By = LRTrim($(document).find('#ReviewedBy').val());
                d.Comments = LRTrim($(document).find('#Comments').val());
            },
            "datatype": "json"
        },
        "columns":
        [
            { "data": "Reviewed_By", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "Comments", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "ReviewDate", "autoWidth": true, "bSearchable": true, "bSortable": true }
        ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', "#pmsidebarCollapse", function () {
    $('#pmreviewadvsearchcontainer').find('#sidebar').addClass('active');
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
$(document).on('click', "#btnPMReviewDataAdvSrch", function (e) {
    var searchitemhtml = "";
    woaGridfilteritemcount = 0
    $('#rladvsearchsidebar').find('.adv-item').each(function (index, item) {
        if ($(this).hasClass('dtpicker')) {
            $(this).val(ValidateDate($(this).val()));
        }
        if ($(this).val() && $(this).val() != "0") {
            woaGridfilteritemcount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossWOActive" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $('#rlitemadvsearchfilteritems').html(searchitemhtml);
    $('#pmreviewadvsearchcontainer').find('#sidebar').removeClass('active');
    $('.overlay').fadeOut();
    RLAdvanceSearch();
});
function RLAdvanceSearch() {
    mrReviewLogTable.page('first').draw('page');
    $('.filteritemcount').text(woaGridfilteritemcount);
}
$(document).on('click', '.btnCrossWOActive', function () {
    var btnCrossedId = $(this).parent().attr('id');
    var searchtxtId = btnCrossedId.split('_')[1];
    $('#' + searchtxtId).val('');
    $(this).parent().remove();
    woaGridfilteritemcount--;
    RLAdvanceSearch();
});
$(document).on('click', '#rlClearAdvSearchFilter', function () {
    RLclearAdvanceSearch();
    mrReviewLogTable.page('first').draw('page');
});
function RLclearAdvanceSearch() {
    var filteritemcount = 0;
    $(document).find('#rladvsearchsidebar').find('input:text').val('');
    $(document).find('.filteritemcount').text(filteritemcount);
    $(document).find('#rlitemadvsearchfilteritems').find('span').html('');
    $(document).find('#rlitemadvsearchfilteritems').find('span').removeClass('tagTo');
}
//#endregion ReviewLog
//#region Photos
$(document).on('click', '#deleteImg', function () {
    var PartMasterRequestId = $('#partsManagementRequestModel_PartMasterRequestId').val();
    var RequestType = $('#partsManagementRequestModel_RequestType').val();
    DeleteAzureImage(PartMasterRequestId, RequestType);
});
function DeleteAzureImage(PartMasterRequestId, RequestType) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/PartsManagementRequest/DeleteImageFromAzure',
            type: 'POST',
            data: { _PartMasterRequestId: PartMasterRequestId, TableName: "PartMasterRequest", Profile: true, Image: true },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data == "success" || data === "not found" ) {
                    RedirectToPartMasterRequestDetail(PartMasterRequestId, RequestType, "AzureImageReload", true);
                    ShowImageDeleteSuccessAlert();
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}
$(document).on('click', '.setImage', function () {
    var PartMasterRequestId = $('#partsManagementRequestModel_PartMasterRequestId').val();
    var RequestType = $('#partsManagementRequestModel_RequestType').val();
    var imageName = $(this).data('image');
    $.ajax({
        url: '../Base/SaveUploadedFileToServer',
        type: 'POST',
        data: { 'fileName': imageName, objectId: PartMasterRequestId, TableName: "PartMasterRequest", AttachObjectName: "PartMasterRequest" },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.result == "0") {
            var ShowDeleteBtnAfterUpload = $('#ShowDeleteBtnAfterUpload').val();
            $('#PartMasterRequestZoom').attr('src', data.imageurl);
            if (ShowDeleteBtnAfterUpload == 'True')
            { $(document).find('#AzureImage').append('<a href="javascript:void(0)" id="deleteImg" class="trashIcon" title="Delete"><i class="fa fa-trash"></i></a>'); }
            else {
                $(document).find('#deleteImg,.dz-remove').remove();
            }
            $('#PartMasterRequestZoom').data('zoomImage', data.imageurl).elevateZoom(zoomConfig);
            $("#PartMasterRequestZoom").on('load', function () {
                CloseLoader();
                RedirectToPartMasterRequestDetail(PartMasterRequestId, RequestType, "AzureImageReload", true);
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

//#endregion Common
//#region Common
function RedirectToPartMasterRequestDetail(PartMasterRequestId, RequestType, mode, isdelete) {
    $.ajax({
        url: "/PartsManagementRequest/GetPartMgmtDetail",
        type: "POST",
        dataType: 'html',
        data: { PartMasterRequestId: PartMasterRequestId, RequestType: RequestType, delf: isdelete },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpartsreview').html(data);
        },
        complete: function () {
            CloseLoader();
            SetPartMgmtDetailEnvironment();
            if (mode === "AzureImageReload") {
                $('#PMRContainer').hide();
                $('#PMRStatus').hide();
                $('#PMRAttachment').hide();
                $('#PMRReviewLog').hide();
                $('#auditlogcontainer').hide();
                $('.imageDropZone').show();
                $(document).find('#photot').addClass("active");
                $(document).find('#partst').removeClass("active");
            }
            if (mode === "attachment") {
                $('#pmrattachmentt').trigger('click');
                $('#colorselector').val('PMRAttachment');
            }
        },
        error: function () {
            CloseLoader();
        }
    });
}

$(document).on('click', '#btnSendForApproval', function (e) {

    $(document).find('#partMRequestSendApprovalModel_Comment').val('');
    $(document).find('#partMRequestSendApprovalModel_SendToId').val('').trigger('change.select2');
    $('#PartMRequestSendApprovalModal').modal('show');
    $(this).blur();
});

$(document).on('click', '#btnReturnToRequester', function (e) {
    $(document).find('#partMRequestReturnRequesterModel_Comment').val('');
    $('#PartMRequestReturnRequesterModal').modal('show');
    $(this).blur();
});

$(document).on('click', '#btnDeny', function (e) {
    $(document).find('#partMRequestDenyModel_Comment').val('');
    $('#PartMRequestDenyModal').modal('show');
    $(this).blur();
});
$(document).on('click', '#btnSiteApprove', function (e) {
    var PartMasterRequestId = LRTrim($(document).find('#partsManagementRequestModel_PartMasterRequestId').val());
    var RequestType = LRTrim($(document).find('#partsManagementRequestModel_RequestType').val());
    $.ajax({
        url: "/PartsManagementRequest/PartsManagementRequestSiteApprove",
        type: "POST",
        "datatype": "json",
        data: { PartMasterRequestId: PartMasterRequestId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            CloseLoader();
            if (data.Result == "success") {
                SuccessAlertSetting.text = getResourceValue("SiteApprovedSuccessAlert");
                swal(SuccessAlertSetting);
            }
            else {
                SuccessAlertSetting.text = getResourceValue("FailedAlert");
                swal(SuccessAlertSetting);
            }
        },
        complete: function () {
            CloseLoader();
            RedirectToPartMasterRequestDetail(PartMasterRequestId, RequestType);
        },
        error: function () {
            CloseLoader();
        }
    });


});

$(document).on('click', '#btnCancel', function (e) {
    var PartMasterRequestId = LRTrim($(document).find('#partsManagementRequestModel_PartMasterRequestId').val());
    var RequestType = LRTrim($(document).find('#partsManagementRequestModel_RequestType').val());
    $.ajax({
        url: "/PartsManagementRequest/PartsManagementRequestCancel",
        type: "POST",
        "datatype": "json",
        data: { PartMasterRequestId: PartMasterRequestId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            CloseLoader();
            if (data.Result == "success") {
                SuccessAlertSetting.text = getResourceValue("CancelSuccessAlert");
                swal(SuccessAlertSetting);
            }
            else {
                SuccessAlertSetting.text = getResourceValue("FailedAlert");
                swal(SuccessAlertSetting);
            }
        },
        complete: function () {
            CloseLoader();
            RedirectToPartMasterRequestDetail(PartMasterRequestId, RequestType);
        },
        error: function () {
            CloseLoader();
        }
    });
});

$(document).on('click', '#btnEnterpriseApprove', function (e) {
    var PartMasterRequestId = LRTrim($(document).find('#partsManagementRequestModel_PartMasterRequestId').val());
    var RequestType = LRTrim($(document).find('#partsManagementRequestModel_RequestType').val());
    $.ajax({
        url: "/PartsManagementRequest/PartsManagementRequestEnterpriseApprove",
        type: "POST",
        "datatype": "json",
        data: { PartMasterRequestId: PartMasterRequestId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            CloseLoader();
            if (data.Result == "success") {
                SuccessAlertSetting.text = getResourceValue("EnterpriseApproveSuccessAlert");
                swal(SuccessAlertSetting);
            }
            else {
                SuccessAlertSetting.text = getResourceValue("FailedAlert");
                swal(SuccessAlertSetting);
            }
        },
        complete: function () {
            RedirectToPartMasterRequestDetail(PartMasterRequestId, RequestType);
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });

});

//#endregion Common
//#region SendApproval
function PartsManagementRequestSendApprovalOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        $("#PartMRequestSendApprovalModal").modal('hide');
        $('.modal-backdrop').remove();
        SuccessAlertSetting.text = getResourceValue("SiteApprovedSuccessAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToPartMasterRequestDetail(data.PartMasterRequestId, data.RequestType);
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data, '#PartMRequestSendApprovalModal');
    }
}
//#endregion SendApproval
//#region ReturnRequester
function PartsManagementReturnRequesterOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        $("#PartMRequestReturnRequesterModal").modal('hide');
        $('.modal-backdrop').remove();
        SuccessAlertSetting.text = getResourceValue("ReturnRequestSuccessAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToPartMasterRequestDetail(data.PartMasterRequestId, data.RequestType);
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data, '#PartMRequestReturnRequesterModal');
    }
}
//#endregion ReturnRequester

//#region Deny
function PartsManagementRequestDenyOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        $("#PartMRequestDenyModal").modal('hide');
        $('.modal-backdrop').remove();
        SuccessAlertSetting.text = "Deny Successfully";
        swal(SuccessAlertSetting, function () {
            RedirectToPartMasterRequestDetail(data.PartMasterRequestId, data.RequestType);
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data, '#PartMRequestDenyModal');
    }
}
//#endregion

