//#region PO Add-Edit
$(document).on('click', ".addPO,.AddPO", function (e) {
    $.ajax({
        url: "/Purchasing/AddPO",
        type: "GET",
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpurchasing').html(data);
        },
        complete: function () {
            CloseLoader();
            SetPOControls();
        },
        error: function () {
            CloseLoader();
        }
    });
})
function POAddOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        if (data.Command == "save") {
            var message;
            if (data.mode == "add") {
                SuccessAlertSetting.text = getResourceValue("PurchaseOrderAddedAlert");
            }
            else {
                SuccessAlertSetting.text = getResourceValue("PurchaseOrderUpdatedAlert");
            }
            swal(SuccessAlertSetting, function () {
                RedirectToPODetail(data.PurchaseOrderId, "overview");
            });
        }
        else {
            ResetErrorDiv();
            SuccessAlertSetting.text = getResourceValue("PurchaseOrderAddedAlert");
            swal(SuccessAlertSetting, function () {
                $(document).find('form').trigger("reset");
                $(document).find('form').find("select").val("").trigger('change.select2');
                $(document).find('form').find("select").removeClass("input-validation-error");
                $(document).find('form').find("input").removeClass("input-validation-error");
            });
        }
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', "#btnCancelAddPO", function () {
    var PurchaseOrderId = $('#PurchaseOrderModel_PurchaseOrderId').val();
    if (typeof PurchaseOrderId !== "undefined" && PurchaseOrderId != 0) {
        swal(CancelAlertSetting, function () {
            RedirectToDetailOncancel(PurchaseOrderId, "overview");
        });
    }
    else {
        swal(CancelAlertSetting, function () {
            window.location.href = "../Purchasing/Index?page=Procurement_Orders";
        });
    }
});
$(document).on('click', "#btnCancelSendEmail", function () {
    var PurchaseOrderId = $('#PurchaseOrderModel_PurchaseOrderId').val();
    $('.modal').modal('hide');
    $('.modal-backdrop').hide();
});
$(function () {
    $(document).on('click', "#btnSaveAnotherOpenPO,#btnSavePO", function () {
        if ($(document).find("form").valid()) {
            return;
        }
    });
});
$(document).on('click', "#brdPO", function () {
    var POId = $(this).attr('data-val');
    RedirectToPODetail(POId);
});
$(document).on('click', "#editPO", function (e) {
    e.preventDefault();
    var PurchaseOrderId = $('#PurchaseOrderModel_PurchaseOrderId').val();
    $.ajax({
        url: "/Purchasing/EditPO",
        type: "GET",
        dataType: 'html',
        data: { PurchaseOrderId: PurchaseOrderId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpurchasing').html(data);
        },
        complete: function () {
            SetPOControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', "#btnCancelAddPO", function () {
    var PurchasingorderId = $(document).find('#PurchaseOrderModel_PurchaseOrderId').val();
    if (typeof PurchasingorderId !== "undefined" && PurchasingorderId != 0) {
        RedirectToDetailOncancel(PurchasingorderId, "overview");
    }
    else {
        swal(CancelAlertSetting, function () {
            window.location.href = "../Purchasing/Index?page=Procurement_Orders";
        });
    }
});
$(document).on('click', "#updatevoidlstatus", function () {
    var voidreason = $(document).find('#txtvoidreason').val();
    if (!voidreason) {
        var message = getResourceValue("SelectReasonAlert");
        ShowTextMissingCommonAlert(message);
        return false;
    }
    else {
        UpdatePurchaseOrderStatus("void", voidreason, "", "", "");
    }
});
$(document).on('click', "#btnforceComplete", function () {
    UpdatePurchaseOrderStatus("ForceComplete", "", "", "", "");

});
$(document).on('click', '#emailtovendor', function () {
    var ToEmail = LRTrim($('#txtemailto').val());
    var CCEmail = LRTrim($('#txtemailCC').val());
    if (!ValidateEmail(ToEmail)) { //|| !ValidateEmail(CCEmail)
        var message = getResourceValue("EnterEmailAlert");
        ShowTextMissingCommonAlert(message);
        return false;
    }
    var MailBodyComments = $('#txtmailcomments').val();
    var status = "EmailToVendor";
    UpdatePurchaseOrderStatus(status, "", ToEmail, CCEmail, MailBodyComments);
});
function EmailSuccess() {
    var PurchaseOrderId = $(document).find('#PurchaseOrderModel_PurchaseOrderId').val();
    $('.modal').modal('hide');
    $('.modal-backdrop').hide();
    SuccessAlertSetting.text = getResourceValue("MailSentSuccessAlert");
    swal(SuccessAlertSetting, function () {
        //RedirectToPODetail(PurchaseOrderId, "overview");
    });
    CloseLoader();
    SetPOControls();
}
function UpdatePurchaseOrderStatus(Status, VoidComntValue, ToEmail, CCEmail, MailBodyComments) {
    var PurchaseOrderId = $(document).find('#PurchaseOrderModel_PurchaseOrderId').val();
    var message;
    $.ajax({
        url: '/Purchasing/UpdateStatus',
        data: { PurchaseOrderId: PurchaseOrderId, Status: Status, VoidComntValue: VoidComntValue, ToEmail: ToEmail, CCEmail: CCEmail, MailBodyComments: MailBodyComments },
        type: "GET",
        datatype: "html",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('.modal').modal('hide');
            $('.modal-backdrop').hide();
            if (data.Result == "success") {
                switch (Status) {
                    case "void":
                    case "ForceComplete":
                    case "Cancel":
                        SuccessAlertSetting.text = getResourceValue("PurchaseOrderUpdatedAlert");
                        break;
                    case "EmailToVendor":
                        SuccessAlertSetting.text = getResourceValue("MailSentSuccessAlert");
                        break;
                    case "SendforApproval":
                        SuccessAlertSetting.text = getResourceValue("ApprovalRequestSentAlert");
                        break;
                    case "ReturnToRequester":
                        SuccessAlertSetting.text = getResourceValue("PurchaseRequestUpdatedAlert");
                        break;
                }
                swal(SuccessAlertSetting, function () {
                    RedirectToPODetail(PurchaseOrderId, "overview");
                });
            }
            else
            { GenericSweetAlertMethod(data.Result); }
        },
        complete: function () {
            CloseLoader();
            SetPOControls();
        }
    });
};
//#endregion
//#region Line Item
$(function () {
    $("#sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $(document).on('click', '#dismiss, .overlay', function () {
        $(document).find('#sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });
    $(document).on('click', '#wfSidebarCollapse', function () {
        $('#sidebar').addClass('active');
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
    $(document).on('click', '#lineitemsidebarCollapse', function () {
        $('.sidebar').addClass('active');
        $('.overlay').fadeIn();
        $('.collapse.in').toggleClass('in');
        $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    });
    $('#sidebarCollapse2').on('click', function () {
        $('.sidebar').addClass('active');
        $('.overlay').fadeIn();
        $('.collapse.in').toggleClass('in');
        $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    });
});

$(document).on('click', "#opengrid", function () {
    
    var textChargeToId = $("#lineItem_ChargeType option:selected").val();
    if (textChargeToId == "Account")
    { generateAccountDataTable();}
    else if (textChargeToId == "WorkOrder")
    { generateWorkOrderDataTable();}
    else if (textChargeToId == "Equipment")
    { generateEquipmentDataTable(); }
  
});

$(document).on('click', "#openEditgrid", function () {
    var textChargeToId = $("#lineItem_ChargeType option:selected").val();
    if (textChargeToId == "Account")
    { generateAccountDataTable(); }
    else if (textChargeToId == "WorkOrder")
    { generateWorkOrderDataTable(); }
    else if (textChargeToId == "Equipment")
    { generateEquipmentDataTable(); }

});
//#endregion
//#region Common
function RedirectToDetailOncancel(PurchasingorderId, mode) {
    swal(CancelAlertSetting, function () {
        RedirectToPODetail(PurchasingorderId, mode);
    });
}
function RedirectToPODetail(PurchasingorderId, mode) {
    $.ajax({
        url: "/Purchasing/Details",
        type: "POST",
        dataType: 'html',
        data: { PurchaseOrderId: PurchasingorderId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
          $('#renderpurchasing').html(data);
          if ($(document).find('.AddPO').length === 0) { $(document).find('#poactiondiv').css('margin-right', '0px'); }
        },
        complete: function () {
            CloseLoader();
            if (mode === "overview") {
                $('#PurchasingOrderSidebar').trigger('click');
                $('#Detailstab').trigger('click');
                $('#colorselector').val('PurchasingOverview');
            }
            if (mode === "notes") {
                $('#PONotestab').trigger('click');
                $('#colorselector').val('PONotes');
            }
            if (mode === "attachment") {
                $('#POAttachmentstab').trigger('click');
                $('#colorselector').val('POAttachments');
            }
            if (mode === "workflowhistory") {
                $('#POWorkflows').trigger('click');
                $('#colorselector').val('POWorkflows');
            }
            generateLineiItemdataTable(PurchasingorderId, "");
        },
        error: function () {
            CloseLoader();
        }
    });
}
//#endregion
//#region Notes
var dtpNotesTable;
function GeneratePONotesGrid() {
    var PurchaseOrderId = $(document).find('#PurchaseOrderModel_PurchaseOrderId').val();
    if ($(document).find('#ponotesTable').hasClass('dataTable')) {
        dtpNotesTable.destroy();
    }
    dtpNotesTable = $("#ponotesTable").DataTable({
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
            "url": "/Purchasing/PopulateNotes?PurchaseOrderId=" + PurchaseOrderId,
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [
            {
                targets: [3], render: function (a, b, data, d) {
                    return '<a class="btn btn-outline-success editponote gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                        '<a class="btn btn-outline-danger delponote gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                }
            }
        ],
        "columns":
        [
            { "data": "Subject", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "OwnerName", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "ModifiedDate", "type": "date " },
            {
                "data": "ObjectId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center"
            }
        ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', "#btnPOAddNote", function () {
    var PurchaseOrderId = $(document).find('#PurchaseOrderModel_PurchaseOrderId').val();
    var ClientLookupId = $(document).find('#PurchaseOrderModel_ClientLookupId').val();
    $.ajax({
        url: "/Purchasing/AddNotes",
        type: "GET",
        dataType: 'html',
        data: { PurchaseOrderId: PurchaseOrderId, ClientLookupId: ClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpurchasing').html(data);
        },
        complete: function () {
            SetPOControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', '.editponote', function () {
    var data = dtpNotesTable.row($(this).parents('tr')).data();
    EditPONote(data.NotesId);
});
$(document).on('click', '.delponote', function () {
    var data = dtpNotesTable.row($(this).parents('tr')).data();
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/Purchasing/DeleteNotes',
            data: {
                _notesId: data.NotesId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    ShowDeleteAlert(getResourceValue("noteDeleteSuccessAlert"));
                }
            },
            complete: function () {
                CloseLoader();
                dtpNotesTable.state.clear();
                GeneratePONotesGrid();
            },
            error: function () {
                CloseLoader();
            }
        });
    });
});
$(document).on('click', '#brdponotes', function () {
    var PurchaseOrderId = $(this).attr('data-val');
    RedirectToPODetail(PurchaseOrderId);
});
$(document).on('click', "#btnponotescancel", function () {
    var PurchaseOrderId = $(document).find('#notesModel_PurchaseOrderId').val();
    RedirectToDetailOncancel(PurchaseOrderId, "notes");
});
function EditPONote(notesid) {
    var PurchaseOrderId = $(document).find('#PurchaseOrderModel_PurchaseOrderId').val();
    var ClientLookupId = $(document).find('#PurchaseOrderModel_ClientLookupId').val();
    $.ajax({
        url: "/Purchasing/EditNote",
        type: "GET",
        dataType: 'html',
        data: { PurchaseOrderId: PurchaseOrderId, NotesId: notesid, ClientLookupId: ClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpurchasing').html(data);
        },
        complete: function () {
            SetPOControls();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
function PONotesAddOnSuccess(data) {
    CloseLoader();
    var PurchaseOrderId = data.PurchaseOrderId;
    if (data.Result == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("AddNoteAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("UpdateNoteAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToPODetail(PurchaseOrderId, "notes");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
//#endregion
//#region Attachments
var POAttachmentTable;
function GeneratePOAttachmentsGrid() {
    var PurchaseOrderId = $(document).find('#PurchaseOrderModel_PurchaseOrderId').val();
    var attchCount = 0;
    if ($(document).find('#POAttachmentTable').hasClass('dataTable')) {
        POAttachmentTable.destroy();
    }
    POAttachmentTable = $("#POAttachmentTable").DataTable({
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
            "url": "/Purchasing/PopulateAttachments?PurchaseOrderId=" + PurchaseOrderId,
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (response) {
                attchCount = response.recordsTotal;
                if (attchCount > 0) {
                    $(document).find('#poAttachmentCount').show();
                    $(document).find('#poAttachmentCount').html(attchCount);
                }
                else {
                    $(document).find('#poAttachmentCount').hide();
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
            { "data": "CreateDate", "type": "date " },
            {
                "data": "ObjectId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center"
            }
        ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', "#btnPOAddAttachment", function () {
    var PurchaseOrderId = $(document).find('#PurchaseOrderModel_PurchaseOrderId').val();
    var ClientLookupId = $(document).find('#PurchaseOrderModel_ClientLookupId').val();
    $.ajax({
        url: "/Purchasing/AddAttachments",
        type: "GET",
        dataType: 'html',
        data: { PurchaseOrderId: PurchaseOrderId, ClientLookupId: ClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpurchasing').html(data);
        },
        complete: function () {
            SetPOControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', '.delAttchBttn', function () {
    var data = POAttachmentTable.row($(this).parents('tr')).data();
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/Purchasing/DeleteAttachments',
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
                    POAttachmentTable.state.clear();
                    ShowDeleteAlert(getResourceValue("attachmentDeleteSuccessAlert"));
                }
            },
            complete: function () {
                GeneratePOAttachmentsGrid();
                CloseLoader();
            }
        });
    });
});
$(document).on('click', '.lnk_sensor_1', function (e) {
    e.preventDefault();
    var row = $(this).parents('tr');
    var data = POAttachmentTable.row(row).data();
    window.location = '/Purchasing/DownloadAttachment?_fileinfoId=' + data.FileAttachmentId;
});
$(document).on('click', '#brdpoattachment', function () {
    var PurchaseOrderId = $(this).attr('data-val');
    RedirectToDetailOncancel(PurchaseOrderId);
});
$(document).on('click', "#btnpoattachmentcancel", function () {
    var PurchaseOrderId = $(document).find('#attachmentModel_PurchaseOrderId').val();
    RedirectToDetailOncancel(PurchaseOrderId, "attachment");
});
function AttachmentPOAddOnSuccess(data) {
    var PurchaseOrderId = data.PurchaseOrderId;
    CloseLoader();
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("AddAttachmentAlerts");
        swal(SuccessAlertSetting, function () {
            RedirectToPODetail(PurchaseOrderId, "attachment");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
//#endregion
//#region Event Log
var POEventLogTable;
function GenerateSJEventLogGrid() {
    var PurchaseOrderId = $(document).find('#PurchaseOrderModel_PurchaseOrderId').val();

    if ($(document).find('#POEventLogTable').hasClass('dataTable')) {
        POEventLogTable.destroy();
    }
    POEventLogTable = $("#POEventLogTable").DataTable({
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
            "url": "/Purchasing/PopulateEventLog?PurchaseOrderId=" + PurchaseOrderId,
            "type": "POST",
            "datatype": "json"
        },
        "columns":
        [
            { "data": "Events", "autoWidth": false, "bSearchable": true, "bSortable": true, "width": "20%" },
            { "data": "Personnel", "autoWidth": true, "bSearchable": true, "bSortable": true },
            {
                "data": "TransactionDate",
                "autoWidth": true,
                "bSearchable": true,
                "bSortable": true,
                "type": "date"
            },
            { "data": "Comments", "autoWidth": true, "bSearchable": true, "bSortable": true },
        ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });

}
//#endregion
