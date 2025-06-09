$(document).on('click', "#editInvoiceMatch", function (e) {


    e.preventDefault();
    var InvoiceMatchId = $(document).find('#InvoiceMatchHeaderModel_InvoiceMatchHeaderId').val();
    $.ajax({
        url: "/Invoice/EditInvoiceMatch",
        type: "GET",
        dataType: 'html',
        data: { InvoiceMatchId: InvoiceMatchId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderinvoice').html(data);

            if ($("#InvoiceMatchHeaderModel_VendorClientLookupId").val() == '') {
                $(".InvoiceClearVendorModalPopupGridData").hide();
            }
            else {
                $(".InvoiceClearVendorModalPopupGridData").show();
            }
            if ($("#InvoiceMatchHeaderModel_POClientLookUpId").val() == '') {
                $(".InvoiceClearPOModalPopupGridData").hide();
            }
            else {
                $(".InvoiceClearPOModalPopupGridData").show();
            }
            if ($("#InvoiceMatchHeaderModel_ResponsibleWithClientLookupId").val() == '  ') {
                $(".InvoiceClearPersonnalModalPopupGridData").hide();
            }
            else {
                $(".InvoiceClearPersonnalModalPopupGridData").show();
            }
        },
        complete: function () {
            SetControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
function SetControls() {
    CloseLoader();
    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse(document);
    $('input, form').blur(function () {
        $(this).valid();
    });
    $('.select2picker, form').change(function () {
        var areaddescribedby = $(this).attr('aria-describedby');
        if ($(this).valid()) {
            if (typeof areaddescribedby !== 'undefined') {
                $('#' + areaddescribedby).hide();
            }
        }
        else {
            if (typeof areaddescribedby !== 'undefined') {
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
};
function InvoiceMatchHeaderAddOnSuccess(data) {
    CloseLoader();
    if (data.data === "success") {
        var message;
        if (data.Command == "save") {
            if (data.mode == "add") {
                SuccessAlertSetting.text = getResourceValue("InvoiceHeaderAddedAlert");
            }
            else {
                SuccessAlertSetting.text = getResourceValue("InvoiceHeaderUpdatedAlert");
            }
            swal(SuccessAlertSetting, function () {
                localStorage.setItem("InvoiceMatchingstatustext", getResourceValue("OpenAlert"));
                localStorage.setItem("InvoiceMatchingstatus", "1");
                RedirectToInvoiceDetail(data.InvoiceMatchHeaderId);
            });
            if (data.mode == "add") {
                CreateLog(data.InvoiceMatchHeaderId, "Create");
            }
        }
        else {
            SuccessAlertSetting.text = getResourceValue("InvoiceHeaderAddedAlert");
            ResetErrorDiv();
            CreateLog(data.InvoiceMatchHeaderId, "Create");
            swal(SuccessAlertSetting, function () {
                $(document).find('form').trigger("reset");
                $(document).find('form').find("input").removeClass("input-validation-error");
            });
        }

    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', '#linkToSearch', function () {
    window.location.href = "../Invoice/Index?page=Procurement_Invoice_Matching";
});
//function RedirectToInvoiceDetail(invoiceId) {
//    $.ajax({
//        url: "/Invoice/InvoiceDetails",
//        type: "POST",
//        data: { invoiceId: invoiceId },
//        dataType: 'html',
//        beforeSend: function () {
//            ShowLoader();
//        },
//        success: function (data) {
//            $('#renderinvoice').html(data);
//            CloseLoader();
//        },
//        complete: function () {
//            generateInvoiceLineItemListDataTable();
//        },
//        error: function () {
//            CloseLoader();
//        }
//    });
//}
//#region Notes
var notesTable;
$(document).on('click', '.editNoteBttn', function () {
    var data = InvNotesTable.row($(this).parents('tr')).data();
    EditInvNote(data.NotesId);
});
$(document).on('click', '.delNoteBttn', function () {
    var data = InvNotesTable.row($(this).parents('tr')).data();
    DeleteWoNote(data.NotesId);
});
$(document).on('click', "#btnAddNote", function (e) {
    e.preventDefault();
    var InvoiceMatchId = $(document).find('#InvoiceMatchHeaderModel_InvoiceMatchHeaderId').val();
    var ClientLookUpId = $(document).find('#InvoiceMatchHeaderModel_ClientLookupId').val();
    $.ajax({
        url: "/Invoice/AddNote",
        type: "GET",
        dataType: 'html',
        data: { InvoiceId: InvoiceMatchId, ClientLookUpId: ClientLookUpId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderinvoice').html(data);
            CloseLoader();
        },
        complete: function () {
            SetControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
function InvNotesAddOnSuccess(data) {
    CloseLoader();
    var InvID = data.InvoiceId;
    if (data.Result == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("NoteAddedAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("NoteUpdatedAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToInvoiceDetail(InvID, "notes");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', "#btnInvnotescancel", function () {
    var InvID = $(document).find('#NotesModel_InvoiceId').val();
    RedirectToDetailOncancelInvoice(InvID, "notes");
});
$(document).on('click', "#btnInvAttachmentcancel", function () {
    var InvID = $(document).find('#AttachmentModel_InvoiceId').val();
    RedirectToDetailOncancelInvoice(InvID, "attachment");
});
$(document).on('click', "#btnCancelEdit", function (e) {
    var InvoiceMatchId = $(document).find('#InvoiceMatchHeaderId').val();
    if (typeof InvoiceMatchId !== "undefined" && InvoiceMatchId != 0) {
        RedirectToDetailOncancelInvoice(InvoiceMatchId, "overview");
    }
    else {
        swal(CancelAlertSetting, function () {
            window.location.href = "../Invoice/Index?page=Procurement_Invoice_Matching";
        });
    }
});
$(document).on('click', "#btnCancelReceiptEdit", function (e) {
    var InvoiceMatchId = $(document).find('#InvoiceMatchItemModel_InvoiceMatchHeaderId').val();
    RedirectToDetailOncancelInvoice(InvoiceMatchId, "overview");
});
$(document).on('click', "#btnInvReceiptCancel", function (e) {
    var InvoiceMatchId = $(document).find('#InvoiceMatchItemModel_InvoiceMatchHeaderId').val();
    RedirectToDetailOncancelInvoice(InvoiceMatchId, "overview");
});
function RedirectToDetailOncancelInvoice(InvID, mode) {
    swal(CancelAlertSetting, function () {
        RedirectToInvoiceDetail(InvID, mode);
    });
}
function RedirectToInvoiceDetail(InvID, mode) {
    $.ajax({
        url: "/Invoice/InvoiceDetails",
        type: "POST",
        dataType: 'html',
        data: { invoiceId: InvID },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderinvoice').html(data);
            $(document).find('#spnlinkToSearch').text(localStorage.getItem("InvoiceMatchingstatustext"));
        },
        complete: function () {
            CloseLoader();
            if (mode === "overview") {
                $('#InvoiceOverViewSidebar').trigger('click');
                $('#InvoiceOverview').trigger('click');
                $('#colorselector').val('InvoiceOverview');
            }
            if (mode === "notes") {
                $('#InvoiceNote').trigger('click');
                $('#colorselector').val('INVNotes');
            }
            if (mode === "attachment") {
                $('#InvoiceAttachment').trigger('click');
                $('#colorselector').val('INVAttachments');
            }
            generateInvoiceLineItemListDataTable();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function generateInvNotesGrid(deln) {
    var InvoiceID = $(document).find('#InvoiceMatchHeaderModel_InvoiceMatchHeaderId').val();
    if ($(document).find('#notesTable').hasClass('dataTable')) {
        InvNotesTable.destroy();
    }
    InvNotesTable = $("#notesTable").DataTable({
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
            "url": "/Invoice/PopulateNotes?InvoiceId=" + InvoiceID,
            "type": "POST",
            "datatype": "json"
        },

        columnDefs: [
            {
                targets: [3], render: function (a, b, data, d) {
                    return '<a class="btn btn-outline-success editNoteBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                        '<a class="btn btn-outline-danger delNoteBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                }
            }
        ],
        "columns":
            [
                { "data": "Subject", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "OwnerName", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "ModifiedDate",
                    "type": "date "
                },
                { "width": "10%", "bSortable": false, "className": "text-center" }
            ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}
function EditInvNote(notesid) {
    var InvoiceId = $(document).find('#InvoiceMatchHeaderModel_InvoiceMatchHeaderId').val();
    var ClientLookUpId = $(document).find('#InvoiceMatchHeaderModel_ClientLookupId').val();
    $.ajax({
        url: "/Invoice/EditNotes",
        type: "GET",
        dataType: 'html',
        data: { InvoiceId: InvoiceId, _notesId: notesid, ClientLookUpId: ClientLookUpId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderinvoice').html(data);
        },
        complete: function () {
            SetControls();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
function DeleteWoNote(notesId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/Invoice/DeleteNotes',
            data: {
                _notesId: notesId
            },
            type: "POST",
            datatype: "json",
            success: function (data) {
                if (data.Result == "success") {
                    CloseLoader();
                    ShowDeleteAlert(getResourceValue("noteDeleteSuccessAlert"));
                }
            },
            complete: function () {
                InvNotesTable.state.clear();
                generateInvNotesGrid(1);
            }
        });
    });
}
//#endregion Notes
//#Region Attachment
$(document).on('click', "#btnAddInvAttachment", function () {
    var InvoiceId = $(document).find('#InvoiceMatchHeaderModel_InvoiceMatchHeaderId').val();
    var ClientLookUpId = $(document).find('#InvoiceMatchHeaderModel_ClientLookupId').val();
    $.ajax({
        url: "/Invoice/AddAttachments",
        type: "GET",
        dataType: 'html',
        data: { InvoiceId: InvoiceId, ClientLookUpId: ClientLookUpId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderinvoice').html(data);
        },
        complete: function () {
            SetControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
function AttachmentInvAddOnSuccess(data) {
    var Invid = $(document).find("#AttachmentModel_InvoiceId").val();
    CloseLoader();
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("AddAttachmentAlerts");
        swal(SuccessAlertSetting, function () {
            RedirectToInvoiceDetail(Invid, "attachment");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
function generateInvAttachmentGrid(deln) {
    var InvoiceId = $(document).find('#InvoiceMatchHeaderModel_InvoiceMatchHeaderId').val();
    var attchCount = 0;
    if ($(document).find('#InvAttachmentTable').hasClass('dataTable')) {
        InvAttachmentTable.destroy();
    }
    InvAttachmentTable = $("#InvAttachmentTable").DataTable({
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
            "url": "/Invoice/PopulateAttachment?InvoiceId=" + InvoiceId,
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (response) {
                attchCount = response.recordsTotal;
                if (attchCount > 0) {
                    $(document).find('#invAttachmentCount').show();
                    $(document).find('#invAttachmentCount').html(attchCount);
                }
                else {
                    $(document).find('#invAttachmentCount').hide();
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
                {
                    "data": "CreateDate",
                    "type": "date "
                },
                { "width": "10%", "bSortable": false, "className": "text-center" }
            ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', '.delAttchBttn', function () {
    var data = InvAttachmentTable.row($(this).parents('tr')).data();
    DeleteInvAttachment(data.FileAttachmentId);
});
function DeleteInvAttachment(fileAttachmentId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/Invoice/DeleteAttachments',
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
                    InvAttachmentTable.state.clear();
                    ShowDeleteAlert(getResourceValue("attachmentDeleteSuccessAlert"));
                }
                else {
                    ShowErrorAlert(data.Message);
                }
            },
            complete: function () {
                generateInvAttachmentGrid(1);
                CloseLoader();
            }
        });
    });
}
//#endregion Attachment
function RedirectToPmDetail(InvoiceMatchHeaderId, mode) {
    $.ajax({
        url: "/Invoice/InvoiceDetails",
        type: "POST",
        dataType: 'html',
        data: { invoiceId: InvoiceMatchHeaderId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderinvoice').html(data);
            $(document).find('#spnlinkToSearch').text(localStorage.getItem("InvoiceMatchingstatustext"));
        },
        complete: function () {
            generateInvoiceLineItemListDataTable();
            if (mode === "notes") {
                $('#INVNotes').trigger('click');
            }
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', '#AuthorizedToPayOptions', function () {
    var InvoiceID = $(document).find('#InvoiceMatchHeaderModel_InvoiceMatchHeaderId').val();
    ChangeOptions(InvoiceID, "AuthorizedToPay", "");
});
$(document).on('click', '#PaidOptions', function () {
    var InvoiceID = $(document).find('#InvoiceMatchHeaderModel_InvoiceMatchHeaderId').val();
    ChangeOptions(InvoiceID, "Paid", "");
});
$(document).on('click', '#ReopenOptions', function () {
    var InvoiceID = $(document).find('#InvoiceMatchHeaderModel_InvoiceMatchHeaderId').val();
    ChangeOptions(InvoiceID, "Reopen", "");
});
$(document).on('click', '#deleteInv', function () {
    var InvoiceID = $(document).find('#InvoiceMatchHeaderModel_InvoiceMatchHeaderId').val();
    DeleteInvoice(InvoiceID);
});
function DeleteInvoice(InvoiceID) {
    CancelAlertSetting.text = getResourceValue("InvoiceDeleteWarningAlert");
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/Invoice/DeleteInvoice',
            data: { invoiceId: InvoiceID },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    SuccessAlertSetting.text = getResourceValue("InvoiceDeleteAlert");
                    swal(SuccessAlertSetting, function () {
                        window.location.href = "../Invoice/Index?page=Procurement_Invoice_Matching";
                    });

                }
                else {
                    ErrorAlertSetting.text = getResourceValue("FailedAlert");
                    swal(ErrorAlertSetting, function () {
                        CloseLoader();
                    });
                }
            },
            complete: function ()
            {
                CloseLoader();
            }
        });
    });
}

function ChangeInvoiceSuccess(data) {
    CloseLoader();
    if (data.data == "success") {
        $('#ChangeInvoice').modal('hide');
        $('.modal-backdrop').remove();
        SuccessAlertSetting.text = getResourceValue("InvoiceUpdateAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToPmDetail(data.invoiceId, "");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data, '#ChangeInvoice');
    }
}

function ChangeInvoiceOptions(InvoiceID, mode) {
    $.ajax({
        url: "/Invoice/ChangeOptions",
        type: "GET",
        dataType: 'html',
        data: { invoiceId: InvoiceID, Mode: mode },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('.modal').modal('hide');
            switch (mode) {
                case "AuthorizedToPay":
                    SuccessAlertSetting.text = getResourceValue("StatusChangedAlert");
                    break;
                case "Paid":
                    SuccessAlertSetting.text = getResourceValue("StatusChangedAlert");
                    break;
                case "ChangeInvoice":
                    SuccessAlertSetting.text = getResourceValue("InvoiceUpdateAlert");
                    break;
            }
            swal(SuccessAlertSetting, function () {
                RedirectToPmDetail(InvoiceID, "");
            });
        },
        complete: function () {
            generateInvoiceLineItemListDataTable();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}

function ChangeOptions(InvoiceID, mode) {
    $.ajax({
        url: "/Invoice/ChangeOptions",
        type: "GET",
        dataType: 'json',
        data: { invoiceId: InvoiceID, Mode: mode },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('.modal').modal('hide');
            switch (mode) {
                case "AuthorizedToPay":
                    SuccessAlertSetting.text = getResourceValue("StatusChangedAlert");
                    break;
                case "Paid":
                    SuccessAlertSetting.text = getResourceValue("StatusChangedAlert");
                    break;
                case "ChangeInvoice":
                    SuccessAlertSetting.text = getResourceValue("InvoiceUpdateAlert");
                    break;
                case "Reopen":
                    SuccessAlertSetting.text = getResourceValue("StatusChangedAlert");
                    break;
            }
            if (data.data == "success") {
                swal(SuccessAlertSetting, function () {
                    RedirectToPmDetail(InvoiceID, "");
                    CreateLog(InvoiceID, mode);
                });
            }
            else {
                ErrorAlertSetting.text = data.data;
                swal(ErrorAlertSetting, function () {
                });

            }

        },
        complete: function () {
            generateInvoiceLineItemListDataTable();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function CreateLog(InvoiceID, mode) {
    $.ajax({
        url: "/Invoice/CreateInvoiceEventLog",
        type: "POST",
        dataType: 'json',
        data: { invoiceId: InvoiceID, eventVal: mode },
        success: function () {
        },
        complete: function () {
        },
        error: function () {
        }
    });
}
$(document).on('click', '.lnk_sensor_1', function (e) {
    e.preventDefault();
    var row = $(this).parents('tr');
    var data = InvAttachmentTable.row(row).data();
    var FileAttachmentId = data.FileAttachmentId;
    $.ajax({
        type: "post",
        url: '/Base/IsOnpremiseCredentialValid',
        success: function (data) {
            if (data === true) {
                window.location = '/Invoice/DownloadAttachment?_fileinfoId=' + FileAttachmentId;
            }
            else {
                ShowErrorAlert(getResourceValue("NotAuthorisedDownloadFileAlert"));
            }
        }

    });

});
$(document).on('click', "#BackToDetails", function () {
    var Id = $(this).attr('data-val');
    RedirectToInvoiceDetail(Id, "overview");
});
