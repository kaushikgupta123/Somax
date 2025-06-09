//#region PO Add-Edit
$(document).on('click', ".addPO,.AddPO", function (e) {
    $.ajax({
        url: "/Purchasing/ShowAddPurchaseOrderDynamic", /*"/Purchasing/AddPO",*/
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
    if (data.Result == "Error") {
        ErrorAlertSetting.text = /*"Validation error"*/getResourceValue("PurchaseOrderValidationAlert");
        swal(ErrorAlertSetting, function () {
            $(document).find('form').trigger("reset");
            $(document).find('form').find("select").val("").trigger('change.select2');
            $(document).find('form').find("select").removeClass("input-validation-error");
            $(document).find('form').find("input").removeClass("input-validation-error");
            $(document).find('form').find(".ClearVendorModalPopupGridData").hide();
        });
    } else if (data.Result == "ErrorAssetMgt") {
        ErrorAlertSetting.text = getResourceValue("PurchaseOrderAssetMgtValidationAlert");
        swal(ErrorAlertSetting, function () {
            $(document).find('form').trigger("reset");
            $(document).find('form').find("select").val("").trigger('change.select2');
            $(document).find('form').find("select").removeClass("input-validation-error");
            $(document).find('form').find("input").removeClass("input-validation-error");
            $(document).find('form').find(".ClearVendorModalPopupGridData").hide();
        });
    }
    //V2-1112
    else if (data.Result =="openModal") {
        $(document).find('#addCustomPurchaseOrder_Initials').val('').removeClass('input-validation-error');
        $(document).find('#addCustomPurchaseOrder_ShiptoSuffix').val("").trigger('change.select2');
        $(document).find("#addCustomPurchaseOrder_ShiptoSuffix").removeClass("input-validation-error");
        $("#customPurchaseOrderNumberModal").modal({ backdrop: 'static', keyboard: false }, 'show');
    }
    else {
        if (data.Result == "success") {
            if (data.Command == "save" || data.Command =="saveCustomEPMPO") {
                var message;
                if (data.mode == "add") {
                    localStorage.setItem("PURCHASEORDERSTATUS", '1')
                    localStorage.setItem("postatustext", getResourceValue("OpenAlert"));
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
                    $(document).find('form').find(".ClearVendorModalPopupGridData").hide();
                });
            }
        }
        else {
            ShowGenericErrorOnAddUpdate(data);
        }
    }
}
$(document).on('click', "#punchoutModal", function () {
    CreatePunchoutOrder();
});
function CreatePunchoutOrder() {
    var PurchaseOrderId = $(document).find('#PurchaseOrderModel_PurchaseOrderId').val();
    var SiteId = $(document).find('#PurchaseOrderModel_SiteId').val();
    var VendorId = $(document).find('#PurchaseOrderModel_VendorId').val();
    var CreatedBy_PersonnelId = $(document).find('#PurchaseOrderModel_Creator_PersonnelId').val();
    var ClientId = $(document).find('#PurchaseOrderModel_ClientId').val();

    $.ajax({
        url: '/Purchasing/CreatePunchOutOrder',
        data: { PurchaseOrderId: PurchaseOrderId, SiteId: SiteId, ClientId: ClientId, VendorId: VendorId, CreatedBy_PersonnelId: CreatedBy_PersonnelId },
        type: "POST",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            CloseLoader();
            if (data) {
                if (data.ResponseCode == '200') {
                    SuccessAlertSetting.text = getResourceValue("orderRequestSentSuccessAlert");
                    swal(SuccessAlertSetting, function () {
                        RedirectToPODetail(PurchaseOrderId, "overview");
                    });
                }
                else {
                    if (data.ResponseText) {
                        ShowErrorAlert(data.ResponseText);
                    }
                }
            }
        },
        complete: function () {
            CloseLoader();
        }
    });
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
        url: "/Purchasing/EditPurchaseOrdersDynamic", /*"/Purchasing/EditPO",*/
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
        localStorage.setItem("PURCHASEORDERSTATUS", "7");
        localStorage.setItem("postatustext", getResourceValue("VoidAlert"));
        UpdatePurchaseOrderStatus("void", voidreason, "", "", "");
    }
});
$(document).on('click', "#btnforceComplete", function () {
    localStorage.setItem("PURCHASEORDERSTATUS", "6");
    localStorage.setItem("postatustext", getResourceValue("CompleteAlert"));
    UpdateForceCompleteStatus();

});
$(document).on('click', '#openPOEmailModal', function () {
    var ToEmail = $('#hdnToEmailId').val();
    var CcEmail = $('#hdnCcEmailId').val();
    $(document).find('#POEmailModel_ToEmailId').removeClass('input-validation-error').val(ToEmail);
    $(document).find('#POEmailModel_CcEmailId').removeClass('input-validation-error').val(CcEmail);
    $(document).find('#POEmailModel_MailBodyComments').val('');
    $(document).find('#emailModal').removeClass('input-validation-error');
    $('#emailModal').modal({ backdrop: 'static', keyboard: false, show: true });
});
function EmailSuccess(data) {
    var PurchaseOrderId = $(document).find('#PurchaseOrderModel_PurchaseOrderId').val();
    $('.modal').modal('hide');
    $('.modal-backdrop').hide();
    if (data.Result == 'success') {
        SuccessAlertSetting.text = getResourceValue("MailSentSuccessAlert");
        swal(SuccessAlertSetting, function () {
        });
    }
    else {
        GenericSweetAlertMethod(data);
    }
    CloseLoader();
    SetPODetailsControls();
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
                    // case "ForceComplete":
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
            else { GenericSweetAlertMethod(data.Result); }
        },
        complete: function () {
            CloseLoader();
        }
    });
};
function UpdateForceCompleteStatus() {
    var PurchaseOrderId = $(document).find('#PurchaseOrderModel_PurchaseOrderId').val();
    var lineitemcount = $(document).find('#PurchaseOrderModel_CountLineItem').val();
    var message;
    $.ajax({
        url: '/Purchasing/UpdateforceCompleteStatus',
        data: { PurchaseOrderId: PurchaseOrderId, lineitemcount: lineitemcount},
        type: "GET",
        datatype: "html",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('.modal').modal('hide');
            $('.modal-backdrop').hide();
            if (data.Result == "success") {
                {
                    SuccessAlertSetting.text = getResourceValue("PurchaseOrderUpdatedAlert");
                }
                swal(SuccessAlertSetting, function () {
                    RedirectToPODetail(PurchaseOrderId, "overview");
                });
            }
            else { GenericSweetAlertMethod(data.Result); }
        },
        complete: function () {
            CloseLoader();
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
    if (textChargeToId == "Account") { generateAccountDataTable(); }
    else if (textChargeToId == "WorkOrder") { generateWorkOrderDataTable(); }
    else if (textChargeToId == "Equipment") { generateEquipmentDataTable(); }
});

$(document).on('click', "#openEditgrid", function () {
    var textChargeToId = $("#lineItem_ChargeType option:selected").val();
    if (textChargeToId == "Account") { generateAccountDataTable(); }
    else if (textChargeToId == "WorkOrder") { generateWorkOrderDataTable(); }
    else if (textChargeToId == "Equipment") { generateEquipmentDataTable(); }

});
//#endregion
//#region V2-653 UI configuration for Purchase Order
$(document).on('click', ".OpenChargeToModalPopupGrid", function () {
    var textChargeToId = $("#ChargeType option:selected").val();
    if (textChargeToId == "Account") { generateAccountDataTable(); }
    else if (textChargeToId == "WorkOrder") { generateWorkOrderDataTable(); }
    else if (textChargeToId == "Equipment") { generateEquipmentDataTable(); }
});
//$(document).on('click', '#ClearChargeToModalPopupGridData', function () {
//    $(document).find('#' + $(this).data('textfield')).val('').css("display", "none");
//    $(document).find('#' + $(this).data('valuefield')).val('').css("display", "block");
//    $(this).css('display', 'none');
//});
$(document).on('click', '.ClearEquipmentModalPopupGridData', function () {
    $(document).find('#' + $(this).data('textfield')).val('').css("display", "none");
    $(document).find('#' + $(this).data('valuefield')).val('').css("display", "block");
    $(this).css('display', 'none');
});
//$(document).on('change', '#ChargeType', function () {
//    $(document).find('#ChargeToClientLookupId').val('');
//    $(".ClearChargeToModalPopupGridData").hide();
//    var type = $(this).val();
//    if (type == "") {
//        $("#imgChargeToTreeLineItemDynamic").hide();
//    }
//    else {
//        if (type == "Equipment") {
//            $("#imgChargeToTreeLineItemDynamic").show();
//        }
//        else {
//            $("#imgChargeToTreeLineItemDynamic").hide();
//        }
//    }

//});
$(document).on('change', '#ChargeType', function () {
    $(document).find('#ChargeToClientLookupId').val('');
    $("#ClearChargeToModalPopupGridData").hide();
    $("#imgChargeToTreeLineItemDynamic").hide();

    if ($("#OpenChargeToModalPopupGrid").hasClass('OpenAccountModalPopupGrid')) {
        $("#OpenChargeToModalPopupGrid").removeClass('OpenAccountModalPopupGrid');
    }
    if ($("#ClearChargeToModalPopupGridData").hasClass('ClearAccountModalPopupGridData')) {
        $("#ClearChargeToModalPopupGridData").removeClass('ClearAccountModalPopupGridData');
    }

    if ($("#OpenChargeToModalPopupGrid").hasClass('OpenWorkOrderModalPopupGrid')) {
        $("#OpenChargeToModalPopupGrid").removeClass('OpenWorkOrderModalPopupGrid');
    }
    if ($("#ClearChargeToModalPopupGridData").hasClass('ClearWorkOrderModalPopupGridData')) {
        $("#ClearChargeToModalPopupGridData").removeClass('ClearWorkOrderModalPopupGridData');
    }

    if ($("#OpenChargeToModalPopupGrid").hasClass('OpenEquipmentModalPopupGrid')) {
        $("#OpenChargeToModalPopupGrid").removeClass('OpenEquipmentModalPopupGrid');
    }
    if ($("#ClearChargeToModalPopupGridData").hasClass('ClearEquipmentModalPopupGridData')) {
        $("#ClearChargeToModalPopupGridData").removeClass('ClearEquipmentModalPopupGridData');
    }

    var type = $(this).val();
    if (type == "") {
        $("#imgChargeToTreeLineItemDynamic").hide();
        return;
    }
    if (type == 'Account') {
        $('#OpenChargeToModalPopupGrid').addClass('OpenAccountModalPopupGrid');
        $('#ClearChargeToModalPopupGridData').addClass('ClearAccountModalPopupGridData');
        return;
    }
    if (type == 'WorkOrder') {
        $('#OpenChargeToModalPopupGrid').addClass('OpenWorkOrderModalPopupGrid');
        $('#ClearChargeToModalPopupGridData').addClass('ClearWorkOrderModalPopupGridData');
        return;
    }
    if (type == "Equipment") {
        $('#OpenChargeToModalPopupGrid').addClass('OpenEquipmentModalPopupGrid');
        $('#ClearChargeToModalPopupGridData').addClass('ClearEquipmentModalPopupGridData');
        $("#imgChargeToTreeLineItemDynamic").show();
        return;
    }
});
$(document).on('click', ".OpenEquipmentModalPopupGrid", function () {
    if ($('#DirectBuyLineItemModalpopup').hasClass('show')) {
        TextField = $(this).data('textfield');
        ValueField = $(this).data('valuefield');
    }
    generateEquipmentDataTable();
});
//#endregion
//#region Common
function RedirectToDetailOncancel(PurchasingorderId, mode) {
    swal(CancelAlertSetting, function () {
        RedirectToPODetail(PurchasingorderId, mode);
    });
}

$(document).on('click', '#linkToSearch', function () {
    window.location.href = "../Purchasing/Index?page=Procurement_Orders";
});

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
            $(document).find('#spnlinkToSearch').text(localStorage.getItem("postatustext"));
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
            SetPODetailsControls();
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
                targets: [6], render: function (a, b, data, d) {
                    {
                        return '<a class="btn btn-outline-success editAttchBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delAttchBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }

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
                    "data": "PrintwithForm", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-center",
                    "mRender": function (data, type, row) {
                        if (data == false) {
                            return '<label class="m-checkbox m-checkbox--air m-checkbox--state-brand somax-checkbox lbl-innergrid-checkbox">' +
                                '<input type="checkbox" class="status" onclick="return false"><span></span></label>';
                        }
                        else {
                            return '<label class="m-checkbox m-checkbox--air m-checkbox--solid m-checkbox--state-brand somax-checkbox lbl-innergrid-checkbox">' +
                                '<input type="checkbox" checked="checked" class="status" onclick="return false"><span></span></label>';
                        }
                    }
                },
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
                else {
                    ShowErrorAlert(data.Message);
                }
            },
            complete: function () {
                GeneratePOAttachmentsGrid();
                CloseLoader();
            }
        });
    });
});
$(document).on('click', '.editAttchBttn', function () {
    var data = POAttachmentTable.row($(this).parents('tr')).data();
    EditPOAttachment(data.FileAttachmentId);
});
function EditPOAttachment(FileAttachmentId) {
    var PurchaseOrderId = $(document).find('#PurchaseOrderModel_PurchaseOrderId').val();
    var ClientLookupId = $(document).find('#PurchaseOrderModel_ClientLookupId').val();
    $.ajax({
        url: "/Purchasing/EditAttachment",
        type: "GET",
        dataType: 'html',
        data: { PurchaseOrderId: PurchaseOrderId, FileAttachmentId: FileAttachmentId, ClientLookupId: ClientLookupId },
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
$(document).on('click', '.lnk_sensor_1', function (e) {
    e.preventDefault();
    var row = $(this).parents('tr');
    var data = POAttachmentTable.row(row).data();
    var FileAttachmentId = data.FileAttachmentId;
    $.ajax({
        type: "post",
        url: '/Base/IsOnpremiseCredentialValid',
        success: function (data) {
            if (data === true) {
                window.location = '/Purchasing/DownloadAttachment?_fileinfoId=' + FileAttachmentId;
            }
            else {
                ShowErrorAlert(getResourceValue("NotAuthorisedDownloadFileAlert"));
            }
        }

    });

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
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("AddAttachmentAlerts");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("attachmentUpdateSuccessAlert");
        }
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
        "order": [[2, "asc"]],
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
//#region V2-796
$(document).on('click', "#openUpdatePODetailsModal", function (e) {
    e.preventDefault();
    var PurchaseOrderId = $('#PurchaseOrderModel_PurchaseOrderId').val();
    $.ajax({
        url: "/Purchasing/UpdatePODetails", /*"/Purchasing/EditPO",*/
        type: "GET",
        dataType: 'html',
        data: { PurchaseOrderId: PurchaseOrderId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#UpdatePOPopUp').html(data);
            $('#UpdatePOModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            SetPOControls();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});
function POUpdateDetailsOnSuccess(data) {
    $('.modal-backdrop').remove();
    CloseLoader();
    if (data.Result === "success") {
        $(document).find('#UpdatePOModalpopup').hide();
        SuccessAlertSetting.text = getResourceValue("PurchaseOrderUpdatedAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToPODetail(data.PurchaseOrderId, "overview");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', '#btnCancelUpdatePO,.clearstate', function () {
    var areaChargeToId = "";
    $(document).find('#UpdatePOModalpopup select').each(function (i, item) {
        areaChargeToId = $(document).find("#" + item.getAttribute('id')).attr('aria-describedby');
        $('#' + areaChargeToId).hide();
    });

});
//#endregion
//#region V2-810
var TextFieldId_ChargeTo = "";
var HdnfieldId_ChargeTo = "";
$(document).on('click', '#imgChargeToTreeLineItemDynamic,.imgChargeToTreeLineItemDynamic', function (e) {
    TextFieldId_ChargeTo = $(this).data('textfield');
    HdnfieldId_ChargeTo = $(this).data('valuefield');
    //$('#purchaseOrderTreeModal').modal('show');
    $(this).blur();
    //generateLiEquipmentTree(-1);
    generatePOEquipmentTreeDynamic(-1);
});
function generateLiEquipmentTree(paramVal) {
    $.ajax({
        url: '/PlantLocationTree/POLineItemEquipmentHierarchyTree',
        datatype: "json",
        type: "post",
        contenttype: 'application/json; charset=utf-8',
        async: false,
        cache: false,
        beforeSend: function () {
            ShowLoader();
            $(document).find(".cntTree").html("<b>Processing...</b>");
        },
        success: function (data) {
            $(document).find(".cntTree").html(data);
        },
        complete: function () {
            CloseLoader();
            treeTable($(document).find('#tblTree'));
            $(document).find('.radSelect').each(function () {

            });
        },
        error: function (xhr) {
            alert('error');
        }
    });
}

function generatePOEquipmentTreeDynamic(paramVal) {
    $.ajax({
        url: '/PlantLocationTree/POLineItemEquipmentHierarchyTree',
        datatype: "json",
        type: "post",
        contenttype: 'application/json; charset=utf-8',
        async: true,
        cache: false,
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find(".cntTree").html(data);
        },
        complete: function () {
            CloseLoader();
            $('#purchaseOrderTreeModal').modal('show');
            treeTable($(document).find('#tblTree'));
            $(document).find('.radSelect').each(function () {
                if ($(document).find('#' + HdnfieldId_ChargeTo).val() == '0' || $(document).find('#' + HdnfieldId_ChargeTo) == '') {

                    if ($(this).data('equipmentid') === equipmentid) {
                        $(this).attr('checked', true);
                    }

                }
                else {

                    if ($(this).data('equipmentid') == $(document).find('#' + HdnfieldId_ChargeTo).val()) {
                        $(this).attr('checked', true);
                    }

                }

            });
            //-- V2-518 collapse all element
            // looking for the collapse icon and triggered click to collapse
            $.each($(document).find('#tblTree > tbody > tr').find('img[src="data:image/png;base64,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"]'), function (i, elem) {
                var parentId = elem.parentNode.parentNode.getAttribute('data-tt-id');
                $(document).find('#tblTree > tbody > tr[data-tt-id=' + parentId + ']').trigger('click');
            });
            //-- collapse all element
        },
        error: function (xhr) {
            alert('error');
        }
    });
}
$(document).on('change', '.radSelect', function () {
    var equipmentid = $(this).data('equipmentid');
    var clientlookupid = $(this).data('clientlookupid').split(' ')[0];
    $('#purchaseOrderTreeModal').modal('hide');
    $(document).find('#ChargeToClientLookupId').val(clientlookupid);
    $(document).find('#ChargeToId').val(equipmentid);
    if ($('#DirectBuyLineItemModalpopup').hasClass('show')) {
        $(document).find('#' + TextFieldId_ChargeTo).val(clientlookupid).removeClass('input-validation-error');
        $(document).find('#' + HdnfieldId_ChargeTo).val(equipmentid).removeClass('input-validation-error');
        if ($(document).find('#' + TextFieldId_ChargeTo).css('display') == 'none') {
            $(document).find('#' + TextFieldId_ChargeTo).css('display', 'block');
        }
        if ($(document).find('#' + HdnfieldId_ChargeTo).css('display') == 'block') {
            $(document).find('#' + HdnfieldId_ChargeTo).css('display', 'none');
        }
        $(document).find('#' + HdnfieldId_ChargeTo).parent().find('div > button.ClearEquipmentModalPopupGridData').css('display', 'block');

    }
});
//#endregion

//#region UnVoid
$(document).on('click', '#unVoid', function () {
    var PurchaseOrderId = $(document).find('#PurchaseOrderModel_PurchaseOrderId').val();
    var message;
    $.ajax({
        url: '/Purchasing/UpdateUnVoidStatus',
        data: { PurchaseOrderId: PurchaseOrderId },
        type: "GET",
        datatype: "html",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {

            if (data == "success") {
                SuccessAlertSetting.text = getResourceValue("PurchaseOrderUpdatedAlert");
                swal(SuccessAlertSetting, function () {
                    localStorage.setItem("PURCHASEORDERSTATUS", '1')
                    localStorage.setItem("postatustext", getResourceValue("OpenAlert"));
                    RedirectToPODetail(PurchaseOrderId, "overview");
                });
            }
            else { GenericSweetAlertMethod(data.Result); }
        },
        complete: function () {
            CloseLoader();
        }
    });
});
//#endregion

//#region V2-1006
$(document).on('change', '#attachmentModel_FileContent', function () {
    var val = $(this).val();
    var fileName = val.replace(/^.*[\\\/]/, '');
    var fileExt = fileName.substr(fileName.lastIndexOf('.') + 1).toLowerCase();
    if (fileExt == 'PDF' || fileExt == 'pdf') {
        $('#attachmentModel_PrintwithForm').removeAttr('disabled');
    }
    else {
        $('#attachmentModel_PrintwithForm').prop('checked', false);
        $('#attachmentModel_PrintwithForm').attr("disabled", true);
    }
});
//#endregion

//#region V2-1079
$(document).on('click', '#sendEDIPOtoVendor', function () {
    var PurchaseOrderId = $(document).find('#PurchaseOrderModel_PurchaseOrderId').val();
    if ($('#PurchaseOrderModel_SentOrderRequest').val() == 'True') {
        CancelAlertSetting.text = getResourceValue("POAlreadyExportedAlert");
        swal(CancelAlertSetting, function () {
            SendEDIPOtoVendor(PurchaseOrderId);
        });
    }
    else {
        SendEDIPOtoVendor(PurchaseOrderId);
    }
});
function SendEDIPOtoVendor(PurchaseOrderId) {
    $.ajax({
        url: '/Purchasing/SendEDIPOtoVendor',
        data: JSON.stringify({ PurchaseOrderId: PurchaseOrderId }),
        type: "POST",
        datatype: "json",
        contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (result) {
            if (result.success) {
                SuccessAlertSetting.text = result.message;
                swal(SuccessAlertSetting, function () {
                });
            }
        },
        complete: function () {
            CloseLoader();
        }
    });
}
//#endregion
//#region V2-1112 Custom EPM PO form
$(document).on('click', '#btnCustomEPMPOModal,#btnSaveAnotherCustomEPMPOModal', function (e) {
    if (this.id === "btnSaveAnotherCustomEPMPOModal") {
        $("#btnCustomEPMPOSave").val("saveAddCustomEPMPO");
    }
    else {
        $("#btnCustomEPMPOSave").val("saveCustomEPMPO");
    }
})
$(document).on('click', '#btnCustomEPMPOSave,#btnCustomEPMPOCancel', function (e) {
    if($(document).find("#form1").valid()) {
        $("#customPurchaseOrderNumberModal").modal('hide');
    }
    
})
$(document).on('click', '.clearstate', function () {
    var id = ""
    $(document).find('#addCustomPurchaseOrder_Initials').val('').removeClass('input-validation-error');
    $(document).find('#addCustomPurchaseOrder_ShiptoSuffix').val("").trigger('change.select2');
    $(document).find("#addCustomPurchaseOrder_ShiptoSuffix").removeClass("input-validation-error");
    $(document).find('#customPurchaseOrderNumberModal select').each(function (i, item) {
        id = $(document).find("#" + item.getAttribute('id')).attr('aria-describedby');
        $('#' + id).hide();
    });
});


//#endregion