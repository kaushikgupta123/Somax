var PurchaseRequestSelectedItemArray = [];
var ConvertPRToPOdt;
var dtinnerGrid;
var PurchaseRequestIds = [];
var PRIdListToStatus = [];
var ConvertPRPOGridTotalGridItem = 0;
var TextFieldId_ChargeTo = "";
var HdnfieldId_ChargeTo = "";
function PurchaseRequestSelectedItem(PurchaseRequestId, ClientLookupId, CountLineItem, Reason, Creator_PersonnelName, Approved_PersonnelName, VendorId, VendorIsExternal, Status) {
    this.PurchaseRequestId = PurchaseRequestId;
    this.ClientLookupId = ClientLookupId;
    this.CountLineItem = CountLineItem;
    this.Reason = Reason;
    this.Creator_PersonnelName = Creator_PersonnelName;
    this.Approved_PersonnelName = Approved_PersonnelName;
    this.VendorId = VendorId;
    this.VendorIsExternal = VendorIsExternal;
    this.Status = Status;
}
$(function () {
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
    var reqfromapproval = $('#linkToSearch').data('reqfromapproval');
    if (reqfromapproval && reqfromapproval != 'False') {
        localStorage.setItem("PURCHASEREQUESTSTATUS", '12');
        $(document).find('#spnlinkToSearch').text(getResourceValue("AwaitingApprovalAlert"));
    }

});
//#region Add-Edit
$(document).find('.select2picker').select2({});
function PurchaseRequestAddOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        var message;
        if (data.Command == "save") {
            if (data.mode == "add") {
                localStorage.setItem("PURCHASEREQUESTSTATUS", '10')
                localStorage.setItem("prstatustext", getResourceValue("MyOpenRequestsAlert"));
                SuccessAlertSetting.text = getResourceValue("PurchaseRequestAddedAlert");

            }
            else {
                SuccessAlertSetting.text = getResourceValue("PurchaseRequestUpdatedAlert");
            }
            swal(SuccessAlertSetting, function () {
                RedirectToPRequestDetail(data.purchaserequestid, "overview");
            });
        }
        else {
            SuccessAlertSetting.text = getResourceValue("PurchaseRequestAddedAlert");
            ResetErrorDiv();
            swal(SuccessAlertSetting, function () {
                $(document).find('form').trigger("reset");
                $(document).find('form').find("input").removeClass("input-validation-error");
            });
        }
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
};
function PurchaseRequestAddDynamicOnSuccess(data) {
    CloseLoader();
    if (data.Result == "Error") {
        ErrorAlertSetting.text = /*"Validation error"*/getResourceValue("PurchaseRequestValidationAlert");
        swal(ErrorAlertSetting, function () {
            $(document).find('form').trigger("reset");
            $(document).find('form').find("select").val("").trigger('change.select2');
            $(document).find('form').find("select").removeClass("input-validation-error");
            $(document).find('form').find("input").removeClass("input-validation-error");
            $(document).find('form').find(".ClearVendorModalPopupGridData").hide();
        });
    }
    else if (data.Result == "ErrorAssetMgt") {
        ErrorAlertSetting.text = getResourceValue("PurchaseRequestAssetMgtValidationAlert");
        swal(ErrorAlertSetting, function () {
            $(document).find('form').trigger("reset");
            $(document).find('form').find("select").val("").trigger('change.select2');
            $(document).find('form').find("select").removeClass("input-validation-error");
            $(document).find('form').find("input").removeClass("input-validation-error");
            $(document).find('form').find(".ClearVendorModalPopupGridData").hide();
        });
    }
    else {
        if (data.Result == "success") {
            if (data.Command == "save") {
                var message;
                localStorage.setItem("PURCHASEREQUESTSTATUS", '10')
                localStorage.setItem("prstatustext", getResourceValue("MyOpenRequestsAlert"));
                SuccessAlertSetting.text = getResourceValue("PurchaseRequestAddedAlert");
                swal(SuccessAlertSetting, function () {
                    RedirectToPRequestDetail(data.purchaserequestid, "overview");
                });
            }
            else {
                SuccessAlertSetting.text = getResourceValue("PurchaseRequestAddedAlert");
                ResetErrorDiv();
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
};
function PurchaseRequestEditDynamicOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        if (data.Command == "save") {
            var message;
            SuccessAlertSetting.text = getResourceValue("PurchaseRequestUpdatedAlert");
            swal(SuccessAlertSetting, function () {
                RedirectToPRequestDetail(data.purchaserequestid, "overview");
            });
        }
        else {
            SuccessAlertSetting.text = getResourceValue("PurchaseRequestAddedAlert");
            ResetErrorDiv();
            swal(SuccessAlertSetting, function () {
                $(document).find('form').trigger("reset");
                $(document).find('form').find("input").removeClass("input-validation-error");
            });
        }
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
};

$(document).on('click', "#punchoutId", function () {
    CreatePunchoutRequest();
});
function CreatePunchoutRequest() {
    var PurchaseRequestId = $(document).find('#purchaseRequestModel_PurchaseRequestId').val();
    var ClientId = $(document).find('#purchaseRequestModel_ClientId').val();
    var SiteId = $(document).find('#purchaseRequestModel_SiteId').val();;
    var CreatedBy_PersonnelId = $(document).find('#purchaseRequestModel_CreatedBy_PersonnelId').val();;
    var VendorId = $(document).find('#purchaseRequestModel_VendorId').val();;;
    $.ajax({
        url: '/PurchaseRequest/CreatePunchOut',
        data: { PurchaseRequestId: PurchaseRequestId, ClientId: ClientId, SiteId: SiteId, CreatedBy_PersonnelId: CreatedBy_PersonnelId, VendorId: VendorId },
        type: "POST",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data) {
                if (data.ResponseCode == '200') {
                    window.open(data.ResponseURL, '_blank');
                }
                else {
                    if (data.ResponseMessage) {
                        ShowErrorAlert(data.ResponseMessage);
                    }
                }
            }
            CloseLoader();
        },
        complete: function () {
            CloseLoader();
        }
    });
}
var IsValidPartId = false;
var IsValidUnitofMeasure = false;
var IsValidRequiredDate = false;
var IsValidAccount = false;
var IsValidCategory = false;
$(document).on('click', "#btnConfirm", function () {
    var purchaseRequestId = $('#hdrprid').val();
    var status = $('#hdrprstatus').val();
    var d = ProcessShoppingCartData();
    var postdata = JSON.stringify({ 'PurchaseRequestId': purchaseRequestId, 'Status': status, "ShoppingCartData": d });
    var IsValid = ValidateStockOrDirectBuy(d);
    if (IsValid === true) {
        if (d.length > 0) {
            $.ajax({
                url: '/PurchaseRequest/AddLineItems',
                data: postdata,
                beforeSend: function () {
                    ShowLoader();
                },
                type: "post",
                datatype: "json",
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    CloseLoader();
                    if (data.Result == "success") {
                        SuccessAlertSetting.text = getResourceValue("LineItemAddedAlert");
                        swal(SuccessAlertSetting, function () {
                            window.location.href = "../PurchaseRequest/DetailFromShoppingCart?PurchaseRequestId=" + purchaseRequestId;
                        });
                    }
                    else if (data.Result === "failed" && data.Mode === 'Validation') {
                        var errorValidMsg = '';
                        if (data.IsValidPartId == true) {
                            var partOrNonPartAlert = getResourceValue("selectPartOrNonPartAlert");
                            if (partOrNonPartAlert) {
                                errorValidMsg += (errorValidMsg ? "," : "") + partOrNonPartAlert;
                            }
                        }
                        if (data.IsValidUnitofMeasure == true) {
                            var validUnitofMeasure = getResourceValue("ValidUnitofMeasure");
                            if (validUnitofMeasure) {
                                errorValidMsg += (errorValidMsg ? "," : "") + validUnitofMeasure;
                            }
                        }
                        if (data.IsValidRequiredDate == true) {
                            var validRequiredDate = getResourceValue("ValidRequiredDate");
                            if (validRequiredDate) {
                                errorValidMsg += (errorValidMsg ? "," : "") + validRequiredDate;
                            }
                        }
                        if (data.IsValidAccount == true) {
                            var validAccount = getResourceValue("ValidAccount");
                            if (validAccount) {
                                errorValidMsg += (errorValidMsg ? "," : "") + validAccount;
                            }
                        }
                        if (data.IsValidCategory == true && IsUsePartMaster == true) {
                            var validCategory = getResourceValue("ValidCategory");
                            if (validCategory) {
                                errorValidMsg += (errorValidMsg ? "," : "") + validCategory;
                            }
                        }
                        if (errorValidMsg != '') {
                            GenericSweetAlertMethod(errorValidMsg.split(','));
                        }
                        else {
                            ShowErrorAlert(data.Result);
                        }

                    }
                },
                complete: function () {
                    CloseLoader();
                }
            });
        }
        else {
            ShowErrorAlert(getResourceValue("noValidDataAvailableAlert"));
        }
    }
    else {
        var errorValidMsg = '';
        if (IsValidPartId == true) {
            var partOrNonPartAlert = getResourceValue("selectPartOrNonPartAlert");
            if (partOrNonPartAlert) {
                errorValidMsg += (errorValidMsg ? "," : "") + partOrNonPartAlert;
            }
        }
        if (IsValidUnitofMeasure == true) {
            var validUnitofMeasure = getResourceValue("ValidUnitofMeasure");
            if (validUnitofMeasure) {
                errorValidMsg += (errorValidMsg ? "," : "") + validUnitofMeasure;
            }
        }
        if (IsValidRequiredDate == true) {
            var validRequiredDate = getResourceValue("ValidRequiredDate");
            if (validRequiredDate) {
                errorValidMsg += (errorValidMsg ? "," : "") + validRequiredDate;
            }
        }
        if (IsValidAccount == true) {
            var validAccount = getResourceValue("ValidAccount");
            if (validAccount) {
                errorValidMsg += (errorValidMsg ? "," : "") + validAccount;
            }
        }
        if (IsValidCategory == true && IsUsePartMaster == true) {
            var validCategory = getResourceValue("ValidCategory");
            if (validCategory) {
                errorValidMsg += (errorValidMsg ? "," : "") + validCategory;
            }
        }
        GenericSweetAlertMethod(errorValidMsg.split(','));
    }
});
$(document).on('click', "#btnShoppingCartCancel", function () {
    var purchaseRequestId = $('#hdrprid').val();
    swal(CancelAlertSetting, function () {
        window.location.href = "../PurchaseRequest/DetailFromShoppingCart?PurchaseRequestId=" + purchaseRequestId;
    });
});
function ValidateStockOrDirectBuy(d) {
    IsValidPartId = false;
    IsValidUnitofMeasure = false;
    IsValidRequiredDate = false;
    IsValidAccount = false;
    IsValidCategory = false;
    var IsOraclePurchaseRequestExportInUse = $(document).find('#IsOraclePurchaseRequestExportInUse').val();
    var IsValid = true;
    if (d.length > 0) {
        $.each(d, function (i, item) {
            if (item.PartId === '' && item.ChargeToID === '') {
                IsValid = false;
                IsValidPartId = true;
            }
            else if (item.PartId !== '' && item.ChargeToID !== '') {
                IsValid = false;
                IsValidPartId = true;
            }
            //V2-1119 Shopping Cart Processing Validation Changes
            if (item.UnitofMeasure === '') {
                IsValid = false;
                IsValidUnitofMeasure = true;
            }
            if (IsOraclePurchaseRequestExportInUse == 'True') {
                if (item.PartId > 0) {
                    IsValid = validateRequiredDate(item.RequiredDate);
                    IsValidRequiredDate = true;
                }
                else {
                    if (item.AccountId === '') {
                        IsValid = false;
                        IsValidAccount = true;
                    }
                    if (item.UNSPSC === '') {
                        IsValid = false;
                        IsValidCategory = true;
                    }
                    if (validateRequiredDate(item.RequiredDate) == false) {
                        IsValid = false;
                        IsValidRequiredDate = true;
                    }
                }
            }
        });
    }
    return IsValid;
}
//V2-1119 Validate the RequiredDate
function validateRequiredDate(requiredDate) {
    if (!requiredDate) {
        // RequiredDate is not selected/entered
        return false;
    }
    var currentDate = new Date();
    var inputDate = new Date(requiredDate);
    if (inputDate <= currentDate) {
        // RequiredDate is not in the future
        return false;
    }
    // RequiredDate is selected/entered and is in the future
    return true;
}
function ProcessShoppingCartData() {
    var ShoppingCartApprovalData = [];
    for (i = 0; i < dtPrShoppingCartTable.data().length; i++) {
        var tabledata = dtPrShoppingCartTable.row(i).data();
        var elem = $(document).find('#tblPurchaseRequestShoppingCart > tbody > tr').eq(i);
        var data = {
            AccountId: elem.find('.accountlookupval').val(),
            ChargeToID: elem.find('.chargetolookupval').val(),
            ChargeType: elem.find('.select-chargetype').val(),
            ClientId: tabledata.ClientId,
            SiteId: tabledata.SiteId,
            Description: tabledata.Description,
            ManufacturerName: tabledata.ManufacturerName,
            ManufacturerPartID: tabledata.ManufacturerPartID,
            OrderQuantity: tabledata.OrderQuantity,
            PartId: $(document).find('#PartId_' + i).val(),
            PartStoreroomId: 0,
            PunchoutLineItemId: tabledata.PunchoutLineItemId,
            SupplierPartAuxiliaryId: tabledata.SupplierPartAuxiliaryId,
            SupplierPartId: tabledata.SupplierPartId,
            UnitCost: tabledata.UnitCost,
            UnitofMeasure: elem.find('.select-OrderUnit').val(),
            Classification: tabledata.Classification,
            RequiredDate: $(document).find('#RequiredDate_' + i).val(),
            UNSPSC: $(document).find('#PartCategoryMasterId_' + i).val()
        };
        ShoppingCartApprovalData.push(data);
    }
    return ShoppingCartApprovalData;
}
$(document).on('click', '.AddPrequest', function () {
    var PRUsePunchOutSecurity = $('#purchaseRequestModel_PRUsePunchOutSecurity').val();
    var IsSitePunchOut = $(document).find('#purchaseRequestModel_IsSitePunchOut').val();
    if (PRUsePunchOutSecurity == 'True' && IsSitePunchOut == 'True') {
        swal({
            title: getResourceValue("makePunchoutPurchaseAlert"),
            type: "warning",
            showCancelButton: true,
            confirmButtonClass: "confirm btn btn-lg btn-sm btn-primary",
            cancelButtonClass: "btn-sm",
            confirmButtonText: getResourceValue("CancelAlertYes"),
            cancelButtonText: getResourceValue("CancelAlertNo")
        },
            function (isConfirm) {
                if (isConfirm == true) {
                    generatePunchOutVendorDataTable();
                }
                else {
                    AjaxPurchaseRequestAdd();
                }
            });
    }
    else {
        AjaxPurchaseRequestAdd();

    }

});
function AjaxPurchaseRequestAdd() {
    $.ajax({
        //url: "/PurchaseRequest/AddPurchaseRequest",
        url: "/PurchaseRequest/ShowAddPurchaseRequestDynamic",
        type: "GET",
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpurchaserequest').html(data);
        },
        complete: function () {
            SetPRControls();
        },
        error: function () {
            CloseLoader();
        }
    });

}
$(document).on('click', "#btnCancelAddPRRequest", function () {
    var purchaseRequestId = $('#EditPurchaseRequest_PurchaseRequestId').val();
    if (typeof purchaseRequestId !== "undefined" && purchaseRequestId != 0) {
        swal(CancelAlertSetting, function () {
            RedirectToPRequestDetail(purchaseRequestId);
        });
    }
    else {
        swal(CancelAlertSetting, function () {
            window.location.href = "../PurchaseRequest/Index?page=Procurement_Requests";
        });
    }
});
$(document).on('click', "#purchaserequestedit", function () {
    EditPurchaseRequest();
});
function EditPurchaseRequest() {
    var PurchaseRequestId = $(document).find('#purchaseRequestModel_PurchaseRequestId').val();
    var ChildCount = $(document).find('#purchaseRequestModel_ChildCount').val();
    $.ajax({
        //url: '/PurchaseRequest/EditPurchaserequest',
        url: '/PurchaseRequest/EditPurchaseRequestDynamic',
        data: { PurchaseRequestId: PurchaseRequestId, ChildCount: ChildCount },
        type: "GET",
        datatype: "html",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpurchaserequest').html(data);
        },
        complete: function () {
            SetPRControls();
        }
    });
}
$(document).on('click', '.brdprrequest', function () {
    var PurchaseRequestid = $(this).attr('data-val');
    RedirectToPRequestDetail(PurchaseRequestid);
});
$(document).on('click', '#approvePR', function () {
    swal({
        title: getResourceValue("CancelAlertSure"),
        text: "Please confirm for approved",
        type: "warning",
        showCancelButton: true,
        closeOnConfirm: false,
        confirmButtonClass: "btn-sm btn-primary",
        cancelButtonClass: "btn-sm",
        confirmButtonText: getResourceValue("CancelAlertYes"),
        cancelButtonText: getResourceValue("CancelAlertNo")
    }, function () {
        var PurchaseRequestId = $(document).find('#purchaseRequestModel_PurchaseRequestId').val();
        var status = "approve";
        var clientLookupId = $(document).find("#purchaseRequestModel_ClientLookupId").val();
        var currStatus = $(document).find('#purchaseRequestModel_Status').val();
        var lineCount = $(document).find('#purchaseRequestModel_CountLineItem').val();
        var errormessage = '';
        if (!((currStatus == "AwaitApproval" || currStatus == "Open" || currStatus == "Resubmit") && (lineCount > 0))) {
            errormessage = clientLookupId + getResourceValue("PurchaseRequestApproveAlert");
        }
        if (errormessage.length > 0) {
            GenericSweetAlertMethod(errormessage);
        }
        else {
            UpdatePurchaseRequestApproveStatus(PurchaseRequestId, status, clientLookupId)
        }
    });
});
$(document).on('click', '#updatedenystatus', function () {
    var PurchaseRequestId = $(document).find('#purchaseRequestModel_PurchaseRequestId').val();
    var Comments = $('#txtdenycomments').val();
    var status = "deny";
    var errormessage = '';
    var clientLookupId = $(document).find("#purchaseRequestModel_ClientLookupId").val();
    var currStatus = $(document).find('#purchaseRequestModel_Status').val();
    var lineCount = $(document).find('#purchaseRequestModel_CountLineItem').val();
    if (!(currStatus == "AwaitApproval" || currStatus == "Open" || currStatus == "Resubmit")) {
        errormessage = clientLookupId + getResourceValue("PurchaseRequestDenyAlert");
    }
    if (errormessage.length > 0) {
        GenericSweetAlertMethod(errormessage);
    }
    else {
        UpdatePurchaseRequestDenyOrcancelOrRetToReqStatus(PurchaseRequestId, status, clientLookupId, Comments)
    }
});
$(document).on('click', '#updatecancelstatus', function () {
    var PurchaseRequestId = $(document).find('#purchaseRequestModel_PurchaseRequestId').val();
    var clientLookupId = $(document).find("#purchaseRequestModel_ClientLookupId").val();
    var Comments = $('#txtcancelcomments').val();
    var status = "Cancel";
    UpdatePurchaseRequestDenyOrcancelOrRetToReqStatus(PurchaseRequestId, status, clientLookupId, Comments)
});
$(document).on('click', '#openPREmailModal', function () {
    var ToEmail = $('#hdnToEmailId').val();
    var CcEmail = $('#hdnCcEmailId').val();
    $(document).find('#prEmailModel_ToEmailId').removeClass('input-validation-error').val(ToEmail);
    $(document).find('#prEmailModel_CcEmailId').removeClass('input-validation-error').val(CcEmail);
    $(document).find('#prEmailModel_MailBodyComments').val('');
    $('#emailModal').modal({ backdrop: 'static', keyboard: false, show: true });
});
function EmailSentSuccess() {
    var PurchaseRequestId = $(document).find('#purchaseRequestModel_PurchaseRequestId').val();
    $('.modal').modal('hide');
    $('.modal-backdrop').hide();
    SuccessAlertSetting.text = getResourceValue("MailSentSuccessAlert");
    swal(SuccessAlertSetting, function () {
    });
    CloseLoader();
    SetPRControls();
}
$(document).on('click', "#btnCancelSendEmail", function () {
    $('.modal').modal('hide');
    $('.modal-backdrop').hide();
});
$(document).on('click', '#btnsendforapproval', function () {
    var PurchaseRequestId = LRTrim($(document).find('#purchaseRequestModel_PurchaseRequestId').val());
    var clientLookupId = $(document).find("#purchaseRequestModel_ClientLookupId").val();
    var Personnel = LRTrim($('#txtsendtopersonnel').val());
    if (!Personnel) {
        var message = getResourceValue("alertSelectPersonnel");
        ShowTextMissingCommonAlert(message);
        return false;
    }
    var Comments = LRTrim($('#txtsendforapprovalcomments').val());
    var status = "SendforApproval";
    UpdateSendForApprovalStatus(PurchaseRequestId, status, clientLookupId, Comments, Personnel);
});
$(document).on('click', '#btnreturntorequester', function () {
    var PurchaseRequestId = LRTrim($(document).find('#purchaseRequestModel_PurchaseRequestId').val());
    var ReturnComments = LRTrim($('#txtPurchaseRequestbackcomments').val());
    if (!ReturnComments) {
        var message = getResourceValue("EnterCommentsAlert");
        ShowTextMissingCommonAlert(message);
        return false;
    }
    var status = "ReturnToRequester";
    var errormessage = '';
    var clientLookupId = $(document).find("#purchaseRequestModel_ClientLookupId").val();
    var currStatus = $(document).find('#purchaseRequestModel_Status').val();
    if (currStatus != "AwaitApproval") {
        errormessage = clientLookupId + getResourceValue("PurchaseRequestReturntoRequestorAlert");
    }
    if (errormessage.length > 0) {
        GenericSweetAlertMethod(errormessage);
    }
    else {
        UpdatePurchaseRequestDenyOrcancelOrRetToReqStatus(PurchaseRequestId, status, clientLookupId, ReturnComments)
    }

});
function GetSuccessAlertMessage(status) {
    var message;
    switch (status) {
        case "approve":
            message = getResourceValue("spnPuchaseRequestApprovedSuccessfully");
            break;
        case "deny":
            message = getResourceValue("alertPRDenied");
            break;
        case "Cancel":
            message = getResourceValue("alertPRCancel");
            break;
        case "EmailToVendor":
            message = getResourceValue("MailSentSuccessAlert");
            break;
        case "SendforApproval":
            message = getResourceValue("alertPRApproval");
            break;
        case "ReturnToRequester":
            message = getResourceValue("alertPRBackRequestor");
            break;
    }
    return message;
}
$(document).on('change', '#lineItem_ChargeType', function () {

    $(document).find('#txtChargeToId').val('');
    var type = $(this).val();
    if (type == "") {
        $("#imgChargeToTreeLineItem").hide();
    }
    else {
        if (type == "Equipment") {
            $("#imgChargeToTreeLineItem").show();
        }
        else {
            $("#imgChargeToTreeLineItem").hide();
        }
    }

});
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

$(document).on('click', '.ClearEquipmentModalPopupGridData', function () {
    $(document).find('#' + $(this).data('textfield')).val('').css("display", "none");
    $(document).find('#' + $(this).data('valuefield')).val('').css("display", "block");
    $(this).css('display', 'none');
});


$(document).on('click', "#opengrid", function () {
    var textChargeToId = $("#PartNotInInventoryModel_ChargeType option:selected").val();
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

$(document).on('click', ".OpenChargeToModalPopupGrid", function () {
    var textChargeToId = $("#ChargeType option:selected").val();
    if (textChargeToId == "Account") { generateAccountDataTable(); }
    else if (textChargeToId == "WorkOrder") { generateWorkOrderDataTable(); }
    else if (textChargeToId == "Equipment") { generateEquipmentDataTable(); }

});

$(document).on('click', ".OpenEquipmentModalPopupGrid", function () {
    generateEquipmentDataTable();
});

//#endregion
//#region Notes
var dtpNotesTable;
function GeneratePNotesGrid() {
    var PurchaseRequestId = $(document).find('#purchaseRequestModel_PurchaseRequestId').val();
    if ($(document).find('#prnotesTable').hasClass('dataTable')) {
        dtpNotesTable.destroy();
    }
    dtpNotesTable = $("#prnotesTable").DataTable({
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
            "url": "/PurchaseRequest/PopulateNotes?PurchaseRequestId=" + PurchaseRequestId,
            "type": "post",
            "datatype": "json"
        },
        columnDefs: [
            {
                targets: [3], render: function (a, b, data, d) {
                    return '<a class="btn btn-outline-success editprnote gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                        '<a class="btn btn-outline-danger delprnote gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
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
                {
                    "data": "ObjectId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center"
                }
            ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', "#btnPRAddNote", function () {
    var PurchaseRequestId = $(document).find('#purchaseRequestModel_PurchaseRequestId').val();
    var ClientLookupId = $(document).find('#purchaseRequestModel_ClientLookupId').val();
    $.ajax({
        url: "/PurchaseRequest/AddNotes",
        type: "GET",
        dataType: 'html',
        data: { PurchaseRequestId: PurchaseRequestId, ClientLookupId: ClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpurchaserequest').html(data);
        },
        complete: function () {
            SetPRControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', '.editprnote', function () {
    var data = dtpNotesTable.row($(this).parents('tr')).data();
    EditPRNote(data.NotesId);
});
$(document).on('click', '.delprnote', function () {
    var data = dtpNotesTable.row($(this).parents('tr')).data();
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/PurchaseRequest/DeleteNotes',
            data: {
                _notesId: data.NotesId
            },
            beforeSend: function () {
                ShowLoader();
            },
            type: "post",
            datatype: "json",
            success: function (data) {
                if (data.Result == "success") {
                    CloseLoader();
                    ShowDeleteAlert(getResourceValue("noteDeleteSuccessAlert"));
                }
            },
            complete: function () {
                dtpNotesTable.state.clear();
                GeneratePNotesGrid();
            }
        });
    });
});
$(document).on('click', '#brdprnotes', function () {
    var PurchaseRequestId = $(this).attr('data-val');
    RedirectToPRequestDetail(PurchaseRequestId);
});
$(document).on('click', "#btnprnotescancel", function () {
    var PurchaseRequestid = $(document).find('#notesModel_PurchaseRequestId').val();
    RedirectToDetailOncancel(PurchaseRequestid, "notes");
});
function EditPRNote(notesid) {
    var PurchaseRequestId = $(document).find('#purchaseRequestModel_PurchaseRequestId').val();
    var ClientLookupId = $(document).find('#purchaseRequestModel_ClientLookupId').val();
    $.ajax({
        url: "/PurchaseRequest/EditNote",
        type: "GET",
        dataType: 'html',
        data: { PurchaseRequestId: PurchaseRequestId, NotesId: notesid, ClientLookupId: ClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpurchaserequest').html(data);
        },
        complete: function () {
            SetPRControls();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
function PRNotesAddOnSuccess(data) {
    CloseLoader();
    var PurchaseRequestId = data.purchaserequestid;
    if (data.Result == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("AddNoteAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("UpdateNoteAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToPRequestDetail(PurchaseRequestId, "notes");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
//#endregion
//#region Attachments
var PRAttachmentTable;
function GeneratePAttachmentsGrid() {
    var PurchaseRequestId = $(document).find('#purchaseRequestModel_PurchaseRequestId').val();
    var attchCount = 0;
    if ($(document).find('#PRAttachmentTable').hasClass('dataTable')) {
        PRAttachmentTable.destroy();
    }
    PRAttachmentTable = $("#PRAttachmentTable").DataTable({
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
            "url": "/PurchaseRequest/PopulateAttachments?PurchaseRequestId=" + PurchaseRequestId,
            "type": "post",
            "datatype": "json",
            "dataSrc": function (response) {
                attchCount = response.recordsTotal;
                if (attchCount > 0) {
                    $(document).find('#pureqAttachmentCount').show();
                    $(document).find('#pureqAttachmentCount').html(attchCount);
                }
                else {
                    $(document).find('#pureqAttachmentCount').hide();
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
                        return '<a class=lnk_sensor_1 href="javascript:void(0)"  target="_blank">' + row.FullName + '</a>';
                    }
                },
                { "data": "FileSizeWithUnit", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "OwnerName", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "CreateDate", "type": "date" },
                { "data": "ObjectId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center" }
            ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', "#btnPRAddAttachment", function () {
    var PurchaseRequestId = $(document).find('#purchaseRequestModel_PurchaseRequestId').val();
    var ClientLookupId = $(document).find('#purchaseRequestModel_ClientLookupId').val();
    $.ajax({
        url: "/PurchaseRequest/AddAttachments",
        type: "GET",
        dataType: 'html',
        data: { PurchaseRequestId: PurchaseRequestId, ClientLookupId: ClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpurchaserequest').html(data);
        },
        complete: function () {
            SetPRControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', '.delAttchBttn', function () {
    var data = PRAttachmentTable.row($(this).parents('tr')).data();
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/PurchaseRequest/DeleteAttachments',
            data: {
                _fileAttachmentId: data.FileAttachmentId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    PRAttachmentTable.state.clear();
                    ShowDeleteAlert(getResourceValue("attachmentDeleteSuccessAlert"));
                }
                else {
                    ShowErrorAlert(data.Message);
                }
            },
            complete: function () {
                GeneratePAttachmentsGrid();
                CloseLoader();
            }
        });
    });
});
$(document).on('click', '.lnk_sensor_1', function (e) {
    e.preventDefault();
    var row = $(this).parents('tr');
    var data = PRAttachmentTable.row(row).data();
    var FileAttachmentId = data.FileAttachmentId;
    $.ajax({
        type: "post",
        url: '/Base/IsOnpremiseCredentialValid',
        success: function (data) {
            if (data === true) {
                window.location = '/PurchaseRequest/DownloadAttachment?_fileinfoId=' + FileAttachmentId;
            }
            else {
                ShowErrorAlert(getResourceValue("NotAuthorisedDownloadFileAlert"));
            }
        }

    });
});
$(document).on('click', '#brdprattachment', function () {
    var PurchaseRequestId = $(this).attr('data-val');
    RedirectToPRequestDetail(PurchaseRequestId);
});
$(document).on('click', "#btnprattachmentcancel", function () {
    var PurchaseRequestid = $(document).find('#attachmentModel_PurchaseRequestId').val();
    RedirectToDetailOncancel(PurchaseRequestid, "attachment");
});
function AttachmentPOAddOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        var PurchaseRequestId = data.purchaserequestid;
        SuccessAlertSetting.text = getResourceValue("AddAttachmentAlerts");
        swal(SuccessAlertSetting, function () {
            RedirectToPRequestDetail(PurchaseRequestId, "attachment");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
//#endregion
//#region ModalTree
$(document).on('click', '#imgChargeToTreeLineItem', function (e) {
    $('#purchaseRequestTreeModal').modal('show');
    $(this).blur();
    generateLiEquipmentTree(-1);
});
$(document).on('click', '#imgChargeToTreeLineItemDynamic', function (e) {
    TextFieldId_ChargeTo = $(this).data('textfield');
    HdnfieldId_ChargeTo = $(this).data('valuefield');
    $(this).blur();
    generatePREquipmentTreeDynamic(-1);
});
function generateLiEquipmentTree(paramVal) {
    $.ajax({
        url: '/PlantLocationTree/LineItemEquipmentHierarchyTree',
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

function generatePREquipmentTreeDynamic(paramVal) {
    $.ajax({
        url: '/PlantLocationTree/LineItemEquipmentHierarchyTree',
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
            $('#purchaseRequestTreeModal').modal('show');
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
            $.each($(document).find('#tblTree > tbody > tr').find('img[src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAACXBIWXMAAAsTAAALEwEAmpwYAAAKT2lDQ1BQaG90b3Nob3AgSUNDIHByb2ZpbGUAAHjanVNnVFPpFj333vRCS4iAlEtvUhUIIFJCi4AUkSYqIQkQSoghodkVUcERRUUEG8igiAOOjoCMFVEsDIoK2AfkIaKOg6OIisr74Xuja9a89+bN/rXXPues852zzwfACAyWSDNRNYAMqUIeEeCDx8TG4eQuQIEKJHAAEAizZCFz/SMBAPh+PDwrIsAHvgABeNMLCADATZvAMByH/w/qQplcAYCEAcB0kThLCIAUAEB6jkKmAEBGAYCdmCZTAKAEAGDLY2LjAFAtAGAnf+bTAICd+Jl7AQBblCEVAaCRACATZYhEAGg7AKzPVopFAFgwABRmS8Q5ANgtADBJV2ZIALC3AMDOEAuyAAgMADBRiIUpAAR7AGDIIyN4AISZABRG8lc88SuuEOcqAAB4mbI8uSQ5RYFbCC1xB1dXLh4ozkkXKxQ2YQJhmkAuwnmZGTKBNA/g88wAAKCRFRHgg/P9eM4Ors7ONo62Dl8t6r8G/yJiYuP+5c+rcEAAAOF0ftH+LC+zGoA7BoBt/qIl7gRoXgugdfeLZrIPQLUAoOnaV/Nw+H48PEWhkLnZ2eXk5NhKxEJbYcpXff5nwl/AV/1s+X48/Pf14L7iJIEyXYFHBPjgwsz0TKUcz5IJhGLc5o9H/LcL//wd0yLESWK5WCoU41EScY5EmozzMqUiiUKSKcUl0v9k4t8s+wM+3zUAsGo+AXuRLahdYwP2SycQWHTA4vcAAPK7b8HUKAgDgGiD4c93/+8//UegJQCAZkmScQAAXkQkLlTKsz/HCAAARKCBKrBBG/TBGCzABhzBBdzBC/xgNoRCJMTCQhBCCmSAHHJgKayCQiiGzbAdKmAv1EAdNMBRaIaTcA4uwlW4Dj1wD/phCJ7BKLyBCQRByAgTYSHaiAFiilgjjggXmYX4IcFIBBKLJCDJiBRRIkuRNUgxUopUIFVIHfI9cgI5h1xGupE7yAAygvyGvEcxlIGyUT3UDLVDuag3GoRGogvQZHQxmo8WoJvQcrQaPYw2oefQq2gP2o8+Q8cwwOgYBzPEbDAuxsNCsTgsCZNjy7EirAyrxhqwVqwDu4n1Y8+xdwQSgUXACTYEd0IgYR5BSFhMWE7YSKggHCQ0EdoJNwkDhFHCJyKTqEu0JroR+cQYYjIxh1hILCPWEo8TLxB7iEPENyQSiUMyJ7mQAkmxpFTSEtJG0m5SI+ksqZs0SBojk8naZGuyBzmULCAryIXkneTD5DPkG+Qh8lsKnWJAcaT4U+IoUspqShnlEOU05QZlmDJBVaOaUt2ooVQRNY9aQq2htlKvUYeoEzR1mjnNgxZJS6WtopXTGmgXaPdpr+h0uhHdlR5Ol9BX0svpR+iX6AP0dwwNhhWDx4hnKBmbGAcYZxl3GK+YTKYZ04sZx1QwNzHrmOeZD5lvVVgqtip8FZHKCpVKlSaVGyovVKmqpqreqgtV81XLVI+pXlN9rkZVM1PjqQnUlqtVqp1Q61MbU2epO6iHqmeob1Q/pH5Z/YkGWcNMw09DpFGgsV/jvMYgC2MZs3gsIWsNq4Z1gTXEJrHN2Xx2KruY/R27iz2qqaE5QzNKM1ezUvOUZj8H45hx+Jx0TgnnKKeX836K3hTvKeIpG6Y0TLkxZVxrqpaXllirSKtRq0frvTau7aedpr1Fu1n7gQ5Bx0onXCdHZ4/OBZ3nU9lT3acKpxZNPTr1ri6qa6UbobtEd79up+6Ynr5egJ5Mb6feeb3n+hx9L/1U/W36p/VHDFgGswwkBtsMzhg8xTVxbzwdL8fb8VFDXcNAQ6VhlWGX4YSRudE8o9VGjUYPjGnGXOMk423GbcajJgYmISZLTepN7ppSTbmmKaY7TDtMx83MzaLN1pk1mz0x1zLnm+eb15vft2BaeFostqi2uGVJsuRaplnutrxuhVo5WaVYVVpds0atna0l1rutu6cRp7lOk06rntZnw7Dxtsm2qbcZsOXYBtuutm22fWFnYhdnt8Wuw+6TvZN9un2N/T0HDYfZDqsdWh1+c7RyFDpWOt6azpzuP33F9JbpL2dYzxDP2DPjthPLKcRpnVOb00dnF2e5c4PziIuJS4LLLpc+Lpsbxt3IveRKdPVxXeF60vWdm7Obwu2o26/uNu5p7ofcn8w0nymeWTNz0MPIQ+BR5dE/C5+VMGvfrH5PQ0+BZ7XnIy9jL5FXrdewt6V3qvdh7xc+9j5yn+M+4zw33jLeWV/MN8C3yLfLT8Nvnl+F30N/I/9k/3r/0QCngCUBZwOJgUGBWwL7+Hp8Ib+OPzrbZfay2e1BjKC5QRVBj4KtguXBrSFoyOyQrSH355jOkc5pDoVQfujW0Adh5mGLw34MJ4WHhVeGP45wiFga0TGXNXfR3ENz30T6RJZE3ptnMU85ry1KNSo+qi5qPNo3ujS6P8YuZlnM1VidWElsSxw5LiquNm5svt/87fOH4p3iC+N7F5gvyF1weaHOwvSFpxapLhIsOpZATIhOOJTwQRAqqBaMJfITdyWOCnnCHcJnIi/RNtGI2ENcKh5O8kgqTXqS7JG8NXkkxTOlLOW5hCepkLxMDUzdmzqeFpp2IG0yPTq9MYOSkZBxQqohTZO2Z+pn5mZ2y6xlhbL+xW6Lty8elQfJa7OQrAVZLQq2QqboVFoo1yoHsmdlV2a/zYnKOZarnivN7cyzytuQN5zvn//tEsIS4ZK2pYZLVy0dWOa9rGo5sjxxedsK4xUFK4ZWBqw8uIq2Km3VT6vtV5eufr0mek1rgV7ByoLBtQFr6wtVCuWFfevc1+1dT1gvWd+1YfqGnRs+FYmKrhTbF5cVf9go3HjlG4dvyr+Z3JS0qavEuWTPZtJm6ebeLZ5bDpaql+aXDm4N2dq0Dd9WtO319kXbL5fNKNu7g7ZDuaO/PLi8ZafJzs07P1SkVPRU+lQ27tLdtWHX+G7R7ht7vPY07NXbW7z3/T7JvttVAVVN1WbVZftJ+7P3P66Jqun4lvttXa1ObXHtxwPSA/0HIw6217nU1R3SPVRSj9Yr60cOxx++/p3vdy0NNg1VjZzG4iNwRHnk6fcJ3/ceDTradox7rOEH0x92HWcdL2pCmvKaRptTmvtbYlu6T8w+0dbq3nr8R9sfD5w0PFl5SvNUyWna6YLTk2fyz4ydlZ19fi753GDborZ752PO32oPb++6EHTh0kX/i+c7vDvOXPK4dPKy2+UTV7hXmq86X23qdOo8/pPTT8e7nLuarrlca7nuer21e2b36RueN87d9L158Rb/1tWeOT3dvfN6b/fF9/XfFt1+cif9zsu72Xcn7q28T7xf9EDtQdlD3YfVP1v+3Njv3H9qwHeg89HcR/cGhYPP/pH1jw9DBY+Zj8uGDYbrnjg+OTniP3L96fynQ89kzyaeF/6i/suuFxYvfvjV69fO0ZjRoZfyl5O/bXyl/erA6xmv28bCxh6+yXgzMV70VvvtwXfcdx3vo98PT+R8IH8o/2j5sfVT0Kf7kxmTk/8EA5jz/GMzLdsAAAAgY0hSTQAAeiUAAICDAAD5/wAAgOkAAHUwAADqYAAAOpgAABdvkl/FRgAAAHFJREFUeNpi/P//PwMlgImBQsA44C6gvhfa29v3MzAwOODRc6CystIRbxi0t7fjDJjKykpGYrwwi1hxnLHQ3t7+jIGBQRJJ6HllZaUUKYEYRYBPOB0gBShKwKGA////48VtbW3/8clTnBIH3gCKkzJgAGvBX0dDm0sCAAAAAElFTkSuQmCC"]'), function (i, elem) {
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
    $('#purchaseRequestTreeModal').modal('hide');
    $(document).find('#ChargeToClientLookupId').val(clientlookupid);
    $(document).find('#ChargeToId').val(equipmentid);
});
//#endregion
//#region Convert To Po
function GetConvertPRtoPO() {
    if ($(document).find('#ConvertPRToPOTable').hasClass('dataTable')) {
        ConvertPRToPOdt.destroy();
    }
    ConvertPRToPOdt = $("#ConvertPRToPOTable").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        serverSide: true,
        "order": [[2, "asc"]],
        stateSave: true,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/PurchaseRequest/GetConvertToPurchaseOrderMainGrid",
            "type": "post",
            "datatype": "json",
            "dataSrc": function (result) {
                ConvertPRPOGridTotalGridItem = result.recordsTotal;
                return result.data;
            },
            global: true
        },
        "columns":
            [
                {
                    "data": "PurchaseRequestId",
                    "bVisible": true,
                    "bSortable": false,
                    "autoWidth": false,
                    "bSearchable": false,
                    "mRender": function (data, type, row) {
                        return '<img id="' + data + '" src="../../Images/details_open.png" alt="expand/collapse" rel="' + data + '" style="cursor: pointer;"/>';
                    }
                },
                {
                    "data": "PurchaseRequestId",
                    orderable: false,
                    "bSortable": false,
                    className: 'select-checkbox text-center',
                    targets: 0,
                    'render': function (data, type, full, meta) {
                        if ($('#cprpoidselectall').is(':checked')) {
                            return '<input type="checkbox" checked="checked" name="id[]" data-PRid="' + data + '" class="chksearchConvert' + data + '"  value="'
                                + $('<div/>').text(data).html() + '">';
                        } else {
                            var found = PurchaseRequestSelectedItemArray.some(function (el) {
                                return el.PurchaseRequestId === data;
                            });
                            if (found) {
                                return '<input type="checkbox" checked="checked" name="id[]" data-PRid="' + data + '" class="chksearchConvert ' + data + '"  value="'
                                    + $('<div/>').text(data).html() + '">';
                            }
                            else {
                                return '<input type="checkbox" name="id[]" data-PRid="' + data + '" class="chksearchConvert ' + data + '"  value="'
                                    + $('<div/>').text(data).html() + '">';
                            }
                        }
                    }
                },
                { "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "CountLineItem", "autoWidth": true, "bSearchable": true, "bSortable": true, "class": "text-right" },
                {
                    "data": "Reason", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    mRender: function (data, type, full, meta) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                { "data": "Creator_PersonnelName", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Approved_PersonnelName", "autoWidth": true, "bSearchable": true, "bSortable": true }
            ],
        initComplete: function () {
            $(document).find('#ConvertPRToPOTable').off('click', 'tbody td img');
            $(document).find('#ConvertPRToPOTable').on('click', 'tbody td img', function (e) {
                var tr = $(this).closest('tr');
                var row = ConvertPRToPOdt.row(tr);
                if (this.src.match('details_close')) {
                    this.src = "../../Images/details_open.png";
                    row.child.hide();
                    tr.removeClass('shown');
                }
                else {
                    this.src = "../../Images/details_close.png";
                    var purchaseRequestId = $(this).attr("rel");
                    $.ajax({
                        url: "/PurchaseRequest/InnerGrid/?PurchaseRequestId=" + purchaseRequestId,
                        async: true,
                        type: "GET",
                        datatype: "json",
                        beforeSend: function () {
                            ShowLoader();
                        },
                        success: function (LineItemModel) {
                            row.child(LineItemModel).show();
                            dtinnerGrid = row.child().find('.PRToPO').DataTable(
                                {
                                    "order": [[0, "asc"]],
                                    sDom: 'tir',
                                    paging: false,
                                    searching: false,
                                    "bProcessing": true,
                                    responsive: true,
                                    scrollY: 300,
                                    "scrollCollapse": true,
                                    language: {
                                        url: dataTableLocalisationUrl
                                    },
                                    "columnDefs": [
                                        { className: 'text-right', targets: [3, 5, 6] },
                                        {
                                            "render": function (data, type, row) {
                                                return "<div class='text-wrap'>" + data + "</div>";
                                            }
                                            , targets: [2]
                                        }
                                    ],
                                    initComplete: function () { tr.addClass('shown'); row.child().find('.dataTables_scroll').addClass('tblchild-scroll'); CloseLoader(); }
                                });
                        },
                        complete: function () {
                            CloseLoader();
                        }
                    });
                }
            });
        }
    });
}
$(document).on('click', '#ConverttoPurchaseOrder', function () {
    $(document).find('#convertToPoModal').modal('show');
    GetConvertPRtoPO();
    SetPageLengthMenu();
});
$(document).on('click', '#cprpoidselectall', function (e) {
    PurchaseRequestIds = [];
    var checked = this.checked;
    $.ajax({
        url: "/PurchaseRequest/GetConvertPRToPO",
        async: true,
        type: "GET",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data) {
                $.each(data, function (index, item) {
                    var found = PurchaseRequestSelectedItemArray.some(function (el) {
                        return el.PurchaseRequestId === item.PurchaseRequestId;
                    });
                    if (checked) {
                        if (PurchaseRequestIds.indexOf(item.PurchaseRequestId) == -1)
                            PurchaseRequestIds.push(item.PurchaseRequestId);
                        var itemLS = new PurchaseRequestSelectedItem(item.PurchaseRequestId, item.ClientLookupId, item.CountLineItem, item.Reason, item.Creator_PersonnelName, item.Approved_PersonnelName, item.VendorId, item.VendorIsExternal, item.Status);
                        if (!found) { PurchaseRequestSelectedItemArray.push(itemLS); }
                    } else {
                        var i = PurchaseRequestIds.indexOf(item.PurchaseRequestId);
                        PurchaseRequestIds.splice(i, 1);
                        if (found) {
                            PurchaseRequestSelectedItemArray = PurchaseRequestSelectedItemArray.filter(function (el) {
                                return el.PurchaseRequestId !== item.PurchaseRequestId;
                            });
                        }
                    }
                });
            }
        },
        complete: function () {
            ConvertPRToPOdt.column(1).nodes().to$().each(function (index, item) {
                if (checked) {
                    $(this).find('.chksearchConvert').prop('checked', 'checked');
                } else {
                    $(this).find('.chksearchConvert').prop('checked', false);
                }
            });
            CloseLoader();
        }
    });
});
$(document).on('change', '.chksearchConvert', function () {
    var data = ConvertPRToPOdt.row($(this).parents('tr')).data();
    if (!this.checked) {
        var index = PurchaseRequestIds.indexOf(data.PurchaseRequestId);
        PurchaseRequestIds.splice(index, 1);
        var el = $('#cprpoidselectall').get(0);
        if (el && el.checked && ('indeterminate' in el)) {
            el.indeterminate = true;
        }
        PurchaseRequestSelectedItemArray = PurchaseRequestSelectedItemArray.filter(function (el) {
            return el.PurchaseRequestId !== data.PurchaseRequestId;
        });
    }
    else {
        PurchaseRequestIds.push(data.PurchaseRequestId);
        var item = new PurchaseRequestSelectedItem(data.PurchaseRequestId, data.ClientLookupId, data.CountLineItem, data.Reason, data.Creator_PersonnelName, data.Approved_PersonnelName, data.VendorId, data.VendorIsExternal, data.Status);
        var found = PurchaseRequestSelectedItemArray.some(function (el) {
            return el.PurchaseRequestId === data.PurchaseRequestId;
        });
        if (!found) { PurchaseRequestSelectedItemArray.push(item); }
    }
    if (ConvertPRPOGridTotalGridItem == PurchaseRequestSelectedItemArray.length) {
        $('#cprpoidselectall').prop('checked', 'checked');
    }
    else {
        $('#cprpoidselectall').prop('checked', false);
    }
});
var tempPurchaseRequestIds = [];

$(document).on('hide.bs.modal', '#convertToPoModal', function () {
    var PurchaseRequestId = LRTrim($(document).find('#purchaseRequestModel_PurchaseRequestId').val());
    if (tempPurchaseRequestIds.indexOf(parseInt(PurchaseRequestId)) != -1) {
        RedirectToPRequestDetail(PurchaseRequestId, "overview");
    }
    tempPurchaseRequestIds = [];
});
$(document).on('click', '#btnCancel,#btnclose', function () {
    PurchaseRequestSelectedItemArray = [];
    PurchaseRequestIds = [];
    $(document).find('#convertToPoModal').modal('hide');
    ConvertPRPOGridTotalGridItem = 0;
    $('#cprpoidselectall').prop('checked', false);
});
//#endregion

var SuccessPRDetails = [];
var ErrorDetails = [];
$(document).on('click', '#approveListPR', function () {
    swal({
        title: getResourceValue("CancelAlertSure"),
        text: "Please confirm for approved",
        type: "warning",
        showCancelButton: true,
        closeOnConfirm: false,
        confirmButtonClass: "btn-sm btn-primary",
        cancelButtonClass: "btn-sm",
        confirmButtonText: getResourceValue("CancelAlertYes"),
        cancelButtonText: getResourceValue("CancelAlertNo")
    }, function () {
        if (SelectPRDetails.length < 1) {
            ShowGridItemSelectionAlert();
            return false;
        }
        else {
            var i, SelectPRIdIdx = -1;
            for (i = 0; i < SelectPRDetails.length; i++) {
                if (!(SelectPRDetails[i].Status == "AwaitApproval" || SelectPRDetails[i].Status == "Open" || SelectPRDetails[i].Status == "Resubmit")) {
                    SelectPRIdIdx = SelectPRId.indexOf(SelectPRDetails[i].PurchaseRequestId);
                    SelectPRId.splice(SelectPRIdIdx, 1);
                    ErrorDetails.push(SelectPRDetails[i].ClientLookupId + "  " + getResourceValue("PurchaseRequestApproveAlert"));
                }
                else if (SelectPRDetails[i].ChildCount < 1) {
                    SelectPRIdIdx = SelectPRId.indexOf(SelectPRDetails[i].PurchaseRequestId);
                    SelectPRId.splice(SelectPRIdIdx, 1);
                    ErrorDetails.push(SelectPRDetails[i].ClientLookupId + "  (" + getResourceValue("PurchaseRequestLineItemApproveAlert") + ")");
                }
                else {
                    var thisPrRequest = new PurchaseRequestNotInSelectedItem(SelectPRDetails[i].PurchaseRequestId, SelectPRDetails[i].ClientLookupId, SelectPRDetails[i].Status, SelectPRDetails[i].ChildCount, SelectPRDetails[i].VendorId, SelectPRDetails[i].VendorIsExternal);  /*V2-375*/
                    SuccessPRDetails.push(thisPrRequest);
                }
            }
            if (ErrorDetails.length > 0 && SuccessPRDetails.length > 0) {
                $.ajax({
                    url: '/PurchaseRequest/UpdateStatusApproveBatch',
                    type: "POST",
                    datatype: "json",
                    data: JSON.stringify(SelectPRId),
                    contentType: 'application/json; charset=utf-8',
                    beforeSend: function () {
                        ShowLoader();
                    },
                    success: function (data) {
                        if (data.data == "success") {
                            var htmltext = "<ul style='list-style: none;padding: 0;margin: 0;text-align:left;'>";
                            if (typeof ErrorMsg !== "string") {
                                $.each(ErrorDetails, function (index, value) {
                                    htmltext = htmltext + "<li style='color: #d43f3a;'><i class='fa fa-circle bull'></i>" + value + "</li>";
                                });
                            }
                            else {
                                var thiserror = "<li style='color: #d43f3a;'><i class='fa fa-circle bull'></i>" + ErrorMsg + "</li>";
                                htmltext = htmltext + thiserror;
                            }
                            htmltext = htmltext + "</ul>";
                            HtmlAlertSettings.text = htmltext;
                            swal(HtmlAlertSettings, function () {
                                dtPrTable.page('first').draw('page');
                            });
                            return false;
                        }
                        else {
                            GenericSweetAlertMethod(data.data);
                        }
                        $(".updateArea").hide();
                        $(".actionBar").fadeIn();
                    },
                    complete: function () {
                        ActionComplete();
                    },
                    error: function () {
                        CloseLoader();
                        GenericSweetAlertMethod();
                    }
                });

            }
            else if (ErrorDetails.length > 0) {
                GenericSweetAlertMethod(ErrorDetails);
                ActionComplete();
                return false;
            }
            else if (SuccessPRDetails.length > 0) {
                $.ajax({
                    url: '/PurchaseRequest/UpdateStatusApproveBatch',
                    type: "POST",
                    datatype: "json",
                    data: JSON.stringify(SelectPRId),
                    contentType: 'application/json; charset=utf-8',
                    beforeSend: function () {
                        ShowLoader();
                    },
                    success: function (data) {
                        if (data.data == "success") {
                            SuccessAlertSetting.text = getResourceValue("spnPuchaseRequestApprovedSuccessfully");
                            swal(SuccessAlertSetting, function () {
                                dtPrTable.page('first').draw('page');
                            });
                        }
                        else {
                            GenericSweetAlertMethod(data.data);
                        }
                    },
                    complete: function () {
                        ActionComplete();

                    },
                    error: function () {
                        CloseLoader();
                        GenericSweetAlertMethod();
                    }
                });
            }
        }
    });
});
$(document).on('click', '#denyListPR', function () {
    if (SelectPRDetails.length < 1) {
        ShowGridItemSelectionAlert();
        return false;
    }
    else {
        $(document).find('#denySearchModal').modal('show');
    }
});
$(document).on('click', '#updatedenyBatchstatus', function () {
    var comments = $('#txtdenycomments').val();
    var jsonResult = {
        "list": SelectPRDetails,
        "comments": comments
    }
    $.ajax({
        url: '/PurchaseRequest/UpdateStatusDenyBatch',
        type: "POST",
        datatype: "json",
        data: JSON.stringify(jsonResult),
        contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.data == "success") {
                SuccessAlertSetting.text = getResourceValue("alertPRDenied");
                swal(SuccessAlertSetting, function () {
                    dtPrTable.page('first').draw('page');
                });
            }
            else {
                GenericSweetAlertMethod(data.data);
            }
        },
        complete: function () {
            $(document).find('#txtdenycomments').val("").trigger('change');
            $(document).find('#denySearchModal').modal('hide');
            ActionComplete();
        },
        error: function () {
            CloseLoader();
            GenericSweetAlertMethod();
        }
    });
});
$(document).on('click', '#returnToRequestorListPR', function () {
    if (SelectPRDetails.length < 1) {
        ShowGridItemSelectionAlert();
        return false;
    }
    else {
        $(document).find('#returnTorequesterSearchModal').modal('show');
    }
});
$(document).on('click', '#btnreturntorequesterSearch', function () {
    var comments = LRTrim($('#txtPurchaseRequestbackcomments').val());
    if (!comments) {
        var message = getResourceValue("EnterCommentsAlert");
        ShowTextMissingCommonAlert(message);
        return false;
    }
    else {
        for (var i = 0; i < SelectPRDetails.length; i++) {
            PRIdListToStatus.push(SelectPRDetails[i].WorkOrderId);
        }
        var jsonResult = {
            "list": SelectPRDetails,
            "comments": comments
        }
        $.ajax({
            url: '/PurchaseRequest/UpdateStatusReturnToRequestorBatch',
            type: "POST",
            datatype: "json",
            data: JSON.stringify(jsonResult),
            contentType: 'application/json; charset=utf-8',
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.data == "success") {
                    SuccessAlertSetting.text = getResourceValue("PurchaseRequestReturnRequestorAlert");
                    swal(SuccessAlertSetting, function () {
                        dtPrTable.page('first').draw('page');
                    });
                }
                else {
                    GenericSweetAlertMethod(data.data);
                }
            },
            complete: function () {
                $(document).find('#txtPurchaseRequestbackcomments').val("").trigger('change');
                $(document).find('#returnTorequesterSearchModal').modal('hide');
                ActionComplete();
            },
            error: function () {
                CloseLoader();
                GenericSweetAlertMethod();
            }
        });
    }
});
$(document).on('click', '#ConvertToPurchaseOrderListPR', function () {

    swal({
        title: getResourceValue("CancelAlertSure"),
        text: getResourceValue("ConvertPurchaseRequestToPurchaseOrderAlert"),
        type: "warning",
        showCancelButton: true,
        closeOnConfirm: false,
        confirmButtonClass: "btn-sm btn-primary",
        cancelButtonClass: "btn-sm",
        confirmButtonText: getResourceValue("CancelAlertYes"),
        cancelButtonText: getResourceValue("CancelAlertNo"),
        closeOnConfirm: false
    }, ConvertToPurchaseOrderList);
});
function ConvertToPurchaseOrderList() {
    if (SelectPRDetails.length < 1) {
        ShowGridItemSelectionAlert();
        return false;
    }
    else {
        var CannotConvertedToPO = getResourceValue("PurchaseRequestCannotConvertedToPurchaseOrderAlert");
        var CannotConvertedToPOVendor = getResourceValue("PurchaseRequestCannotConvertedToPurchaseOrderVendorAlert");
        var CannotConvertedToPOInterface = getResourceValue("PurchaseRequestCannotConvertToPurchaseOrderInterfaceAlert")

        var ipropinuse = $(document).find('#ipropInUse').val();
        var i, SelectPRIdIdx = -1;
        for (i = 0; i < SelectPRDetails.length; i++) {
            SelectPRIdIdx = SelectPRId.indexOf(SelectPRDetails[i].PurchaseRequestId);
            if (!(SelectPRDetails[i].Status == "Approved")) {
                ErrorDetails.push(SelectPRDetails[i].ClientLookupId + " " + CannotConvertedToPO);
                SelectPRId.splice(SelectPRIdIdx, 1);
            }
            else if (SelectPRDetails[i].VendorId < 1) {
                ErrorDetails.push(SelectPRDetails[i].ClientLookupId + " " + CannotConvertedToPOVendor);
                SelectPRId.splice(SelectPRIdIdx, 1);
            }
            else if (SelectPRDetails[i].VendorIsExternal && ipropinuse) {
                ErrorDetails.push(SelectPRDetails[i].ClientLookupId + " " + CannotConvertedToPOInterface);
                SelectPRId.splice(SelectPRIdIdx, 1);
            }
            else {
                var thisPrRequest = new PurchaseRequestNotInSelectedItem(SelectPRDetails[i].PurchaseRequestId, SelectPRDetails[i].ClientLookupId, SelectPRDetails[i].Status, SelectPRDetails[i].ChildCount, SelectPRDetails[i].VendorId, SelectPRDetails[i].VendorIsExternal);  /*V2-375*/
                SuccessPRDetails.push(thisPrRequest);
            }
        }
        if (ErrorDetails.length > 0 && SuccessPRDetails.length > 0) {
            $.ajax({
                url: '/PurchaseRequest/ConvertPRToPO',
                type: "POST",
                datatype: "json",
                data: JSON.stringify(SelectPRId),
                contentType: 'application/json; charset=utf-8',
                beforeSend: function () {
                    ShowLoader();
                },
                success: function (data) {
                    if (data.data == "Error") {
                        ErrorAlertSetting.text = getResourceValue("PurchaseOrderConvertValidationAlert");
                        swal(ErrorAlertSetting, function () {
                            SelectPRDetails = [];
                            dtPrTable.page('first').draw('page');
                        });
                    }
                    else if (data.data == "ErrorAssetMgt") {
                        ErrorAlertSetting.text = getResourceValue("PurchaseOrderConvertAssetMgtValidationAlert");
                        swal(ErrorAlertSetting, function () {
                            SelectPRDetails = [];
                            dtPrTable.page('first').draw('page');
                        });
                    }
                    else {
                        if (data.data == "success") {
                            var htmltext = "<ul style='list-style: none;padding: 0;margin: 0;text-align:left;'>";
                            if (typeof ErrorMsg !== "string") {
                                $.each(ErrorDetails, function (index, value) {
                                    htmltext = htmltext + "<li style='color: #d43f3a;'><i class='fa fa-circle bull'></i>" + value + "</li>";
                                });
                            }
                            else {
                                var thiserror = "<li style='color: #d43f3a;'><i class='fa fa-circle bull'></i>" + ErrorMsg + "</li>";
                                htmltext = htmltext + thiserror;
                            }
                            htmltext = htmltext + "</ul>";
                            HtmlAlertSettings.text = htmltext;
                            swal(HtmlAlertSettings, function () {
                                dtPrTable.page('first').draw('page');
                            });
                            return false;
                        }
                        else {
                            GenericSweetAlertMethod(ErrorDetails.concat(data.data));
                            dtPrTable.page('first').draw('page');
                        }
                    }
                },
                complete: function () {
                    ActionComplete();
                },
                error: function () {
                    CloseLoader();
                    GenericSweetAlertMethod();
                }
            });

        }
        else if (ErrorDetails.length > 0) {
            GenericSweetAlertMethod(ErrorDetails);
            ActionComplete();
            return false;
        }
        else if (SuccessPRDetails.length > 0) {
            $.ajax({
                url: '/PurchaseRequest/ConvertPRToPO',
                type: "POST",
                datatype: "json",
                data: JSON.stringify(SelectPRId),
                contentType: 'application/json; charset=utf-8',
                beforeSend: function () {
                    ShowLoader();
                },
                success: function (data) {
                    if (data.data == "Error") {
                        ErrorAlertSetting.text = getResourceValue("PurchaseOrderConvertValidationAlert");
                        swal(ErrorAlertSetting, function () {
                            SelectPRDetails = [];
                            dtPrTable.page('first').draw('page');
                        });
                    }
                    else if (data.data == "ErrorAssetMgt") {
                        ErrorAlertSetting.text = getResourceValue("PurchaseOrderConvertAssetMgtValidationAlert");
                        swal(ErrorAlertSetting, function () {
                            SelectPRDetails = [];
                            dtPrTable.page('first').draw('page');
                        });
                    }
                    else {
                        if (data.data == "success") {
                            SuccessAlertSetting.text = getResourceValue("PurchaseRequestConvertPurchaseOrderSuccessfulAlert");
                            swal(SuccessAlertSetting, function () {
                                dtPrTable.page('first').draw('page');
                            });
                        }
                        else {
                            GenericSweetAlertMethod(data.data);
                            dtPrTable.page('first').draw('page');
                        }
                    }
                },
                complete: function () {
                    ActionComplete();
                },
                error: function () {
                    CloseLoader();
                    GenericSweetAlertMethod();
                }
            });
        }
    }
}
$(document).on('click', '#sendToCoupaListPR', function () {

    swal({
        title: getResourceValue("CancelAlertSure"),
        text: getResourceValue("confirmSendToCoupaAlert"),
        type: "warning",
        showCancelButton: true,
        closeOnConfirm: false,
        confirmButtonClass: "btn-sm btn-primary",
        cancelButtonClass: "btn-sm",
        confirmButtonText: getResourceValue("CancelAlertYes"),
        cancelButtonText: getResourceValue("CancelAlertNo")
    }, function () {
        if (SelectPRDetails.length < 1) {
            ShowGridItemSelectionAlert();
            return false;
        }
        else {
            var i;
            for (i = 0; i < SelectPRDetails.length; i++) {
                if (!SelectPRDetails[i].VendorIsExternal) {
                    ErrorDetails.push(SelectPRDetails[i].ClientLookupId + " " + getResourceValue("PurchaseRequestCannotSendToInterfaceVendorAlert"));
                }
                else if (!(SelectPRDetails[i].Status == "Approved")) {
                    ErrorDetails.push(SelectPRDetails[i].ClientLookupId + " " + getResourceValue("PurchaseRequestCannotSendToInterfaceStatusAlert"));
                }
                else {
                    var thisPrRequest = new PurchaseRequestNotInSelectedItem(SelectPRDetails[i].PurchaseRequestId, SelectPRDetails[i].ClientLookupId, SelectPRDetails[i].Status, SelectPRDetails[i].ChildCount, SelectPRDetails[i].VendorId, SelectPRDetails[i].VendorIsExternal);  /*V2-375*/
                    SuccessPRDetails.push(thisPrRequest);
                }
            }
            if (ErrorDetails.length > 0 && SuccessPRDetails.length > 0) {
                var jsonResult = {
                    "list": SuccessPRDetails
                }
                $.ajax({
                    url: '/PurchaseRequest/SendToCoupaListPR',
                    type: "POST",
                    datatype: "json",
                    data: JSON.stringify(jsonResult),
                    contentType: 'application/json; charset=utf-8',
                    beforeSend: function () {
                        ShowLoader();
                    },
                    success: function (data) {
                        if (data.data == "success") {
                            var htmltext = "<ul style='list-style: none;padding: 0;margin: 0;text-align:left;'>";
                            if (typeof ErrorMsg !== "string") {
                                $.each(ErrorDetails, function (index, value) {
                                    htmltext = htmltext + "<li style='color: #d43f3a;'><i class='fa fa-circle bull'></i>" + value + "</li>";
                                });
                            }
                            else {
                                var thiserror = "<li style='color: #d43f3a;'><i class='fa fa-circle bull'></i>" + ErrorMsg + "</li>";
                                htmltext = htmltext + thiserror;
                            }
                            htmltext = htmltext + "</ul>";
                            HtmlAlertSettings.text = htmltext;
                            swal(HtmlAlertSettings, function () {
                                dtPrTable.page('first').draw('page');
                            });
                            return false;
                        }
                        else {
                            GenericSweetAlertMethod(data.data);
                        }
                    },
                    complete: function () {
                        ActionComplete();
                    },
                    error: function () {
                        CloseLoader();
                        GenericSweetAlertMethod();
                    }
                });

            }
            else if (ErrorDetails.length > 0) {
                GenericSweetAlertMethod(ErrorDetails);
                ActionComplete();
                return false;
            }
            else if (SuccessPRDetails.length > 0) {
                var jsonResult = {
                    "list": SuccessPRDetails
                }
                $.ajax({
                    url: '/PurchaseRequest/SendToCoupaListPR',
                    type: "POST",
                    datatype: "json",
                    data: JSON.stringify(jsonResult),
                    contentType: 'application/json; charset=utf-8',
                    beforeSend: function () {
                        ShowLoader();
                    },
                    success: function (data) {
                        if (data.data == "success") {
                            SuccessAlertSetting.text = getResourceValue("PurchaseRequestSentSuccessfulAlert");
                            swal(SuccessAlertSetting, function () {
                                dtPrTable.page('first').draw('page');
                            });
                        }
                        else {
                            GenericSweetAlertMethod(data.data);
                        }
                    },
                    complete: function () {
                        ActionComplete();
                    },
                    error: function () {
                        CloseLoader();
                        GenericSweetAlertMethod();
                    }
                });
            }
        }
    });
});
$(document).on('click', '#btnConvert', function (e) {
    if (PurchaseRequestSelectedItemArray.length <= 0) {
        ShowGridItemSelectionAlert();
        return false;
    }
    else {
        $.ajax({
            url: '/PurchaseRequest/ConvertPRToPO',
            "datatype": "json",
            data: JSON.stringify(PurchaseRequestIds),
            contentType: 'application/json; charset=utf-8',
            type: "POST",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.data == "Error") {
                    ErrorAlertSetting.text = getResourceValue("PurchaseOrderConvertValidationAlert");
                    swal(ErrorAlertSetting, function () {
                        PurchaseRequestSelectedItemArray = [];
                        ConvertPRToPOdt.page('first').draw('page');
                    });
                } else if (data.data == "ErrorAssetMgt") {
                    ErrorAlertSetting.text = getResourceValue("PurchaseOrderConvertAssetMgtValidationAlert");
                    swal(ErrorAlertSetting, function () {
                        PurchaseRequestSelectedItemArray = [];
                        ConvertPRToPOdt.page('first').draw('page');
                    });
                }
                else {
                    if (data.data == "success") {
                        SuccessAlertSetting.text = getResourceValue("PurchaseRequestConvertPurchaseOrderSuccessfulAlert");
                        swal(SuccessAlertSetting, function () {
                            $(document).find('#convertToPoModal').modal('hide');
                            $('.modal-backdrop').hide();
                            var purchaserequestid = $(document).find('#purchaseRequestModel_PurchaseRequestId').val();
                            RedirectToPRequestDetail(purchaserequestid, "overview");
                        });
                    }
                    else {
                        GenericSweetAlertMethod(data.data);
                        PurchaseRequestSelectedItemArray = [];
                        ConvertPRToPOdt.page('first').draw('page');
                    }
                }
            },
            complete: function () {
                ConvertPRToPOdt.state.clear();
                PurchaseRequestSelectedItemArray = [];
                tempPurchaseRequestIds = PurchaseRequestIds;
                PurchaseRequestIds = [];
                ConvertPRPOGridTotalGridItem = 0;
                CloseLoader();
            },
            error: function () {
                CloseLoader();
                GenericSweetAlertMethod();
            }
        });
    }
});
$(document).on('click', '#sendToCoupa', function () {
    swal({
        title: getResourceValue("CancelAlertSure"),
        text: getResourceValue("confirmSendToCoupaAlert"),
        type: "warning",
        showCancelButton: true,
        closeOnConfirm: false,
        confirmButtonClass: "btn-sm btn-primary",
        cancelButtonClass: "btn-sm",
        confirmButtonText: getResourceValue("CancelAlertYes"),
        cancelButtonText: getResourceValue("CancelAlertNo")
    }, function () {
        var purchaserequestid = $(document).find('#purchaseRequestModel_PurchaseRequestId').val();
        var clientlookupid = $(document).find('#purchaseRequestModel_ClientLookupId').val();
        var status = $(document).find('#purchaseRequestModel_Status').val();
        var childcount = $(document).find('#purchaseRequestModel_CountLineItem').val();
        var vendorid = $(document).find('#purchaseRequestModel_VendorId').val();
        var vendorisexternal = $(document).find('#purchaseRequestModel_VendorIsExternal').val();
        var modelobject = new PurchaseRequestNotInSelectedItem(purchaserequestid, clientlookupid, status, childcount, vendorid, vendorisexternal);
        var modelitems = [];
        modelitems.push(modelobject);
        var jsonResult = {
            "list": modelitems
        }
        $.ajax({
            url: '/PurchaseRequest/SendToCoupaListPR',
            type: "POST",
            datatype: "json",
            data: JSON.stringify(jsonResult),
            contentType: 'application/json; charset=utf-8',
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.data == "success") {
                    SuccessAlertSetting.text = getResourceValue("PurchaseRequestSentSuccessfulAlert");
                    swal(SuccessAlertSetting, function () {
                        RedirectToPRequestDetail(purchaserequestid, "overview");
                    });

                }
                else {
                    GenericSweetAlertMethod(data.data);
                }

            },
            complete: function () {
                ActionComplete();
            },
            error: function () {
                CloseLoader();
                GenericSweetAlertMethod();
            }
        });
    });
});

//#region Action Button
function UpdatePurchaseRequestApproveStatus(purchaseRequestId, status, clientLookupId) {
    $.ajax({
        url: '/PurchaseRequest/UpdateStatus',
        data: {
            PurchaseRequestId: purchaseRequestId, Status: status, ClientLookupId: clientLookupId
        },
        type: "POST",
        datatype: "html",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('.modal').modal('hide');
            $('.modal-backdrop').hide();
            if (data.data == "success") {
                SuccessAlertSetting.text = getResourceValue("ApproveSuccessAlert");
                swal(SuccessAlertSetting, function () {
                    RedirectToPRequestDetail(purchaseRequestId, "overview");
                });
            }
            else {
                GenericSweetAlertMethod(data.data);
            }
        },
        complete: function () {
            CloseLoader();
            SetPRControls();
        }
    });
};
function UpdatePurchaseRequestDenyOrcancelOrRetToReqStatus(purchaseRequestId, status, clientLookupId, comments) {
    var lineCount = $(document).find('#purchaseRequestModel_CountLineItem').val();
    $.ajax({
        url: '/PurchaseRequest/UpdateStatus',
        data: {
            PurchaseRequestId: purchaseRequestId, Status: status, ClientLookupId: clientLookupId, Comments: comments, lineCount: lineCount
        },
        type: "POST",
        datatype: "html",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('.modal').modal('hide');
            $('.modal-backdrop').hide();
            if (data.data == "success") {
                if (status == "deny") {
                    SuccessAlertSetting.text = getResourceValue("alertPRDenied");
                }
                else if (status == "Cancel") {
                    SuccessAlertSetting.text = getResourceValue("alertPRCancel");
                }
                else {
                    SuccessAlertSetting.text = getResourceValue("ReturntoRequestorSuccessAlert");
                }
                swal(SuccessAlertSetting, function () {
                    RedirectToPRequestDetail(purchaseRequestId, "overview");
                });
            }
            else {
                GenericSweetAlertMethod(data.data);
            }
        },
        complete: function () {
            CloseLoader();
            SetPRControls();
        }
    });
};
function UpdateSendForApprovalStatus(purchaseRequestId, status, clientLookupId, comments, personnelId) {
    $.ajax({
        url: '/PurchaseRequest/UpdateStatus',
        data: {
            PurchaseRequestId: purchaseRequestId, Status: status, ClientLookupId: clientLookupId, Comments: comments, PersonnelId: personnelId
        },
        type: "POST",
        datatype: "html",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('.modal').modal('hide');
            $('.modal-backdrop').hide();
            if (data.data == "success") {
                SuccessAlertSetting.text = getResourceValue("SendApprovalSuccessAlert");
                swal(SuccessAlertSetting, function () {
                    RedirectToPRequestDetail(purchaseRequestId, "overview");
                });
            }
            else {
                GenericSweetAlertMethod(data.data);
            }
        },
        complete: function () {
            CloseLoader();
            SetPRControls();
        }
    });
};
function ActionComplete() {
    $(".updateArea").hide();
    $(".actionBar").fadeIn();
    SelectPRDetails = [];
    SelectPRId = [];
    PurchaseRequestIds = [];
    PRIdListToStatus = [];
    SuccessPRDetails = [];
    ErrorDetails = [];
    $(document).find('.chkPRsearch').prop('checked', false);
    $(document).find('.dt-body-center').find('#purchaserequest-select-all').prop('checked', false);
    $(document).find('.itemcount').text(0);
    CloseLoader();
}
//#endregion

//#region V2-726
function SendPRForApprovalOnSuccess(data) {
    $(document).find('#SendForApprovalModalDetailsPage').modal('hide');
    var PurchaseRequestId = data.PurchaseRequestId;
    if (data.data === "success") {
        if (data.ApprovalGroupId >= 0) {
            SuccessAlertSetting.text = getResourceValue("SendApprovalSuccessAlert");
            swal(SuccessAlertSetting, function () {
                CloseLoader();
                RedirectToPRequestDetail(PurchaseRequestId, "overview")
            });
        }
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', '#btncancelsendPRForApproval,.clearstate1', function () {
    var areaChargeToId = "";
    $(document).find('#SendForApprovalModalDetailsPage select').each(function (i, item) {
        areaChargeToId = $(document).find("#" + item.getAttribute('id')).attr('aria-describedby');
        $('#' + areaChargeToId).hide();
    });

});
//#endregion

//#region V2-693 Send to sap
$(document).on('click', '#SendToSAP', function () {
    swal({
        title: getResourceValue("CancelAlertSure"),
        text: 'Please confirm for send to sap', //getResourceValue("confirmSendToCoupaAlert"),
        type: "warning",
        showCancelButton: true,
        closeOnConfirm: false,
        confirmButtonClass: "btn-sm btn-primary",
        cancelButtonClass: "btn-sm",
        confirmButtonText: getResourceValue("CancelAlertYes"),
        cancelButtonText: getResourceValue("CancelAlertNo")
    }, function () {
        var purchaserequestid = $(document).find('#purchaseRequestModel_PurchaseRequestId').val();
        var modelitems = [];
        modelitems.push(purchaserequestid);
        $.ajax({
            url: '/PurchaseRequest/SendToSAPListPR',
            type: "POST",
            datatype: "json",
            data: {
                "PurchaseRequestIds": modelitems
            },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.data == "success") {
                    SuccessAlertSetting.text = getResourceValue("PurchaseRequestSentSuccessfulAlert");
                    swal(SuccessAlertSetting, function () {
                        RedirectToPRequestDetail(purchaserequestid, "overview");
                    });
                }
                else {
                    GenericSweetAlertMethod(data.data);
                }

            },
            complete: function () {
                CloseLoader();
            },
            error: function () {
                CloseLoader();
                GenericSweetAlertMethod();
            }
        });
    });
});
$(document).on('click', '#SendToSAPListPR', function () {
    var modelitems = [];
    swal({
        title: getResourceValue("CancelAlertSure"),
        text: 'Please confirm for send to sap', //getResourceValue("confirmSendToCoupaAlert"),
        type: "warning",
        showCancelButton: true,
        closeOnConfirm: false,
        confirmButtonClass: "btn-sm btn-primary",
        cancelButtonClass: "btn-sm",
        confirmButtonText: getResourceValue("CancelAlertYes"),
        cancelButtonText: getResourceValue("CancelAlertNo")
    }, function () {
        if (SelectPRDetails.length < 1) {
            ShowGridItemSelectionAlert();
            return false;
        }
        else {
            var i;
            for (i = 0; i < SelectPRDetails.length; i++) {
                if (!SelectPRDetails[i].VendorIsExternal) {
                    ErrorDetails.push(SelectPRDetails[i].ClientLookupId + " " + "Purchase Request Vendor is not external");
                }
                else if (!(SelectPRDetails[i].Status == "Approved")) {
                    ErrorDetails.push(SelectPRDetails[i].ClientLookupId + " " + "Purchase Request Status is not Approved");
                }
                else {
                    var thisPrRequest = new PurchaseRequestNotInSelectedItem(SelectPRDetails[i].PurchaseRequestId, SelectPRDetails[i].ClientLookupId, SelectPRDetails[i].Status, SelectPRDetails[i].ChildCount, SelectPRDetails[i].VendorId, SelectPRDetails[i].VendorIsExternal);  /*V2-375*/
                    SuccessPRDetails.push(thisPrRequest);
                    modelitems.push(SelectPRDetails[i].PurchaseRequestId);
                }
            }
            if (ErrorDetails.length > 0 && SuccessPRDetails.length > 0) {
                $.ajax({
                    url: '/PurchaseRequest/SendToSAPListPR',
                    type: "POST",
                    datatype: "json",
                    data: {
                        "PurchaseRequestIds": modelitems
                    },
                    beforeSend: function () {
                        ShowLoader();
                    },
                    success: function (data) {
                        if (data.data == "success") {
                            GenericSweetAlertMethod(ErrorDetails);
                            dtPrTable.page('first').draw('page');
                        }
                        else {
                            GenericSweetAlertMethod(data.data);
                        }
                    },
                    complete: function () {
                        ActionComplete();
                    },
                    error: function () {
                        CloseLoader();
                        GenericSweetAlertMethod();
                    }
                });

            }
            else if (ErrorDetails.length > 0) {
                GenericSweetAlertMethod(ErrorDetails);
                ActionComplete();
                return false;
            }
            else if (SuccessPRDetails.length > 0) {
                $.ajax({
                    url: '/PurchaseRequest/SendToSAPListPR',
                    type: "POST",
                    datatype: "json",
                    data: {
                        "PurchaseRequestIds": modelitems
                    },
                    beforeSend: function () {
                        ShowLoader();
                    },
                    success: function (data) {
                        if (data.data == "success") {
                            SuccessAlertSetting.text = getResourceValue("PurchaseRequestSentSuccessfulAlert");
                            swal(SuccessAlertSetting, function () {
                                dtPrTable.page('first').draw('page');
                            });
                        }
                        else {
                            GenericSweetAlertMethod(data.data);
                        }
                    },
                    complete: function () {
                        ActionComplete();
                    },
                    error: function () {
                        CloseLoader();
                        GenericSweetAlertMethod();
                    }
                });
            }
        }
    });
});
//#endregion

//#region V2-730
$(document).on('click', '#MultiLevelApprovePR', function () {
    swal({
        title: getResourceValue("CancelAlertSure"),
        text: getResourceValue("spnPleaseConfirmToApprove"),
        type: "warning",
        showCancelButton: true,
        closeOnConfirm: false,
        confirmButtonClass: "btn-sm btn-primary",
        cancelButtonClass: "btn-sm",
        confirmButtonText: getResourceValue("CancelAlertYes"),
        cancelButtonText: getResourceValue("CancelAlertNo")
    }, function () {
        var PurchaseRequestId = $(document).find('#purchaseRequestModel_PurchaseRequestId').val();
        var ApprovalGroupId = $(document).find('#ApprovalRouteModelByObjectId_ApprovalGroupId').val();
        var clientLookupId = $(document).find("#purchaseRequestModel_ClientLookupId").val();
        var currStatus = $(document).find('#purchaseRequestModel_Status').val();
        var lineCount = $(document).find('#purchaseRequestModel_CountLineItem').val();
        var errormessage = '';
        if (!((currStatus == "AwaitApproval" || currStatus == "Open" || currStatus == "Resubmit") && (lineCount > 0))) {
            errormessage = clientLookupId + getResourceValue("PurchaseRequestApproveAlert");
        }
        if (errormessage.length > 0) {
            GenericSweetAlertMethod(errormessage);
        }
        else {
            MultiLevelApprovePR(PurchaseRequestId, ApprovalGroupId, clientLookupId)
        }
    });
});

function MultiLevelApprovePR(purchaseRequestId, ApprovalGroupId, clientLookupId) {
    $.ajax({
        url: '/PurchaseRequest/MultiLevelApprovePR',
        data: {
            PurchaseRequestId: purchaseRequestId, ApprovalGroupId: ApprovalGroupId, ClientLookupId: clientLookupId
        },
        type: "POST",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('.sweet-overlay').fadeOut();
            $('.showSweetAlert').fadeOut();
            if (data.data == "success") {
                if (data.ApproverList.length > 0) {
                    $.ajax({
                        url: "/PurchaseRequest/SendPRForMultiLevelApproval",
                        type: "POST",
                        dataType: 'html',
                        data: { Approvers: data.ApproverList, PurchaseRequestId: purchaseRequestId, ApprovalGroupId: ApprovalGroupId },
                        beforeSend: function () {
                            ShowLoader();
                        },
                        success: function (data) {
                            $('#MultiLevelApproverListPopUp').html(data);
                            $('#MultiLevelApproverListModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
                        },
                        complete: function () {
                            SetPRControls();
                            CloseLoader();
                        },
                        error: function (jqXHR, exception) {
                            CloseLoader();
                        }
                    });
                }
                else {
                    $.ajax({
                        url: "/PurchaseRequest/MultiLevelFinalApprove",
                        type: "GET",
                        datatype: "json",
                        data: { PurchaseRequestId: purchaseRequestId, ApprovalGroupId: ApprovalGroupId },
                        beforeSend: function () {
                            ShowLoader();
                        },
                        success: function (data) {
                            var PurchaseRequestId = data.PurchaseRequestId;
                            if (data.Result === "success") {
                                if (data.ApprovalGroupId >= 0) {
                                    SuccessAlertSetting.text = getResourceValue("spnPuchaseRequestApprovedSuccessfully");
                                    swal(SuccessAlertSetting, function () {
                                        CloseLoader();
                                        RedirectToPRequestDetail(PurchaseRequestId, "overview")
                                    });
                                }
                            }
                            else {
                                ShowGenericErrorOnAddUpdate(data);
                            }
                        },
                        complete: function () {
                            SetPRControls();
                            CloseLoader();
                        },
                        error: function (jqXHR, exception) {
                            CloseLoader();
                        }
                    });
                }
            }
            else {
                GenericSweetAlertMethod(data.data);
            }
        },
        complete: function () {
            CloseLoader();
            SetPRControls();
        }
    });
};

function SendPRForMultiLevelApprovalOnSuccess(data) {
    $(document).find('#MultiLevelApproverListModalpopup').modal('hide');
    var PurchaseRequestId = data.PurchaseRequestId;
    if (data.data === "success") {
        if (data.ApprovalGroupId >= 0) {
            SuccessAlertSetting.text = getResourceValue("SendApprovalSuccessAlert");
            swal(SuccessAlertSetting, function () {
                CloseLoader();
                RedirectToPRequestDetail(PurchaseRequestId, "overview")
            });
        }
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}


$(document).on('click', '#denyMultiLevelPR', function () {
    swal({
        title: getResourceValue("CancelAlertSure"),
        text: getResourceValue("spnPleaseConfirmToDeny"),
        type: "warning",
        showCancelButton: true,
        closeOnConfirm: false,
        confirmButtonClass: "btn-sm btn-primary",
        cancelButtonClass: "btn-sm",
        confirmButtonText: getResourceValue("CancelAlertYes"),
        cancelButtonText: getResourceValue("CancelAlertNo")
    }, function () {
        var PurchaseRequestId = $(document).find('#purchaseRequestModel_PurchaseRequestId').val();
        var ApprovalGroupId = $(document).find('#ApprovalRouteModelByObjectId_ApprovalGroupId').val();
        var clientLookupId = $(document).find("#purchaseRequestModel_ClientLookupId").val();
        var currStatus = $(document).find('#purchaseRequestModel_Status').val();
        var lineCount = $(document).find('#purchaseRequestModel_CountLineItem').val();
        var errormessage = '';
        if (!((currStatus == "AwaitApproval") && (lineCount > 0))) {
            errormessage = clientLookupId + getResourceValue("PurchaseRequestApproveAlert");
        }
        if (errormessage.length > 0) {
            GenericSweetAlertMethod(errormessage);
        }
        else {
            MultiLevelDenyPR(PurchaseRequestId, ApprovalGroupId, clientLookupId)
        }
    });
});
function MultiLevelDenyPR(purchaseRequestId, ApprovalGroupId, clientLookupId) {
    $.ajax({
        url: '/PurchaseRequest/MultiLevelDenyPR',
        data: {
            PurchaseRequestId: purchaseRequestId, ApprovalGroupId: ApprovalGroupId, ClientLookupId: clientLookupId
        },
        type: "POST",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.data == "success") {
                SuccessAlertSetting.text = getResourceValue("alertPRDenied");
                swal(SuccessAlertSetting, function () {
                    RedirectToPRequestDetail(purchaseRequestId, "overview");
                });
            }
            else {
                ErrorAlertSetting.text = getResourceValue("FailedAlert");
                swal(ErrorAlertSetting, function () {
                    RedirectToPRequestDetail(purchaseRequestId, "overview");
                });
            }
        },
        complete: function () {
            CloseLoader();
            SetPRControls();
        }
    });
}
//#endregion
//#region V2-820
function ReviewAndSendPRApprovalOnSuccess(data) {
    $(document).find('#ReviewSendForApprovalModalDetailsPage').modal('hide');
    var PurchaseRequestId = data.PurchaseRequestId;
    if (data.data === "success") {
        if (data.ApprovalGroupId >= 0) {
            SuccessAlertSetting.text = getResourceValue("ReviewAndSendApprovalAlert");
            swal(SuccessAlertSetting, function () {
                CloseLoader();
                RedirectToPRequestDetail(PurchaseRequestId, "overview")
            });
        }
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', '#btncancelsendPRForApprovalReview,.clearstate1', function () {
    var areaChargeToId = "";
    $(document).find('#ReviewSendForApprovalModalDetailsPage select').each(function (i, item) {
        areaChargeToId = $(document).find("#" + item.getAttribute('id')).attr('aria-describedby');
        $('#' + areaChargeToId).hide();
    });

});
//#endregion

//#region V2-1046
var SlectedPRLineItemArray = [];
var consolidatetotalcount = 0;
$(document).on('click', '#PRConsolidate', function () {
    var PurchaseRequestId = $(document).find('#purchaseRequestModel_PurchaseRequestId').val();
    $.ajax({
        url: "/PurchaseRequest/GetPurchaseRequestConsolidate",
        type: "POST",
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#ConsolidatePopUp').html(data);
            $('#ConsolidateModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            generatePRLineDataTableForConsolidate(PurchaseRequestId);
            SetAdvSearchControl();
            CloseLoader();
            $("#Consolidateadvsearchcontainer .sidebar").mCustomScrollbar({
                theme: "minimal"
            });
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
});
function SetAdvSearchControl() {
    $("#sidebar2").mCustomScrollbar({
        theme: "minimal"
    });
    $('#dismissPRC, .overlay').on('click', function () {
        $('#sidebar2').removeClass('active');
        $('.overlay2').fadeOut();
    });
    $(document).find('#ConsolidateidebarCollapse').on('click', function () {
        $('#sidebar2').addClass('active');
        $('.overlay2').fadeIn();
        $('.collapse.in').toggleClass('in');
        $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    });
}
$(document).on("click", "#ConsolidateidebarCollapse", function (e) {
    e.preventDefault();
    $('#Consolidateadvsearchcontainer .sidebar').addClass('active');
    $('.overlay2').fadeIn();
    $('.collapse.in').toggleClass('in');
    $('a[aria-expanded=true]').attr('aria-expanded', 'false');
});
$(document).on('click', '#dismissPRC', function () {
    $(document).find('#Consolidateadvsearchcontainer .sidebar').removeClass('active');
    $(document).find('.overlay2').fadeOut();
});
//#region Advance Search
$(document).on('click', "#btnConsolidateDataAdvSrch", function (e) {
    var searchitemhtml = "";
    hGridfilteritemcount = 0;
    $('#advsearchsidebarConsolidate').find('.adv-item').each(function (index, item) {
        if ([].concat($(this).val()).filter(function (valueOfFilter) { return valueOfFilter != ''; }).length) {
            hGridfilteritemcount++;
            var s = $(this).attr('id');
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossHistory" aria-hidden="true"></a></span>';
        }

    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $('#Consolidatesearchfilteritems').html(searchitemhtml);
    $('#Consolidateadvsearchcontainer').find('.sidebar').removeClass('active');
    $(document).find('.overlay2').fadeOut();
    PRConsolidateGridAdvanceSearch();
});
function PRConsolidateGridAdvanceSearch() {
    dtPRLConsolidateTable.page('first').draw('page');
    $('.Consolidatefilteritemcount').text(hGridfilteritemcount);
}
$(document).on('click', '#ConsolidateClearAdvSearchFilter', function () {
    clearAdvanceSearchPRConsolidate();
    dtPRLConsolidateTable.page('first').draw('page');
});
function clearAdvanceSearchPRConsolidate() {
    var filteritemcount = 0;
    $('#advsearchsidebarConsolidate').find('input:text').val('');
    $('.Consolidatefilteritemcount').text(filteritemcount);
    $('#Consolidatesearchfilteritems').find('span').html('');
    $('#Consolidatesearchfilteritems').find('span').removeClass('tagTo');
}
$(document).on('click', '.btnCrossHistory', function () {
    var btnCrossedId = $(this).parent().attr('id');
    var searchtxtId = btnCrossedId.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    hGridfilteritemcount--;
    PRConsolidateGridAdvanceSearch();
});
//#endregion
function generatePRLineDataTableForConsolidate(PurchaseRequestId) {
    if ($(document).find('#ConsolidateTable').hasClass('dataTable')) {
        dtPRLConsolidateTable.destroy();
    }
    dtPRLConsolidateTable = $("#ConsolidateTable").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        serverSide: true,
        "order": [[1, "asc"]],
        stateSave: true,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/PurchaseRequest/GetPRLineGridDataForConsolidate",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.PurchaseRequestId = PurchaseRequestId;
                d.Description = LRTrim($("#prcGridadvsearchDescription").val());
                d.VendorClientLookupId = LRTrim($("#prcGridadvsearchVendorId").val());
                d.VendorName = LRTrim($("#prcGridadvsearchVendorName").val());
            },
            "dataSrc": function (result) {
                consolidatetotalcount = result.recordsTotal;
                return result.data;
            },
            global: true
        },
        "columns":
            [
                {
                    "data": "PurchaseRequestLineItemId",
                    orderable: false,
                    "bSortable": false,
                    className: 'select-checkbox dt-body-center',
                    targets: 0,
                    'render': function (data, type, full, meta) {
                        if ($('#prconsolidateselectall').is(':checked')) {
                            return '<input type="checkbox" checked="checked" name="id[]" data-PRLIid="' + data + '" class="chksearchConsolidate ' + data + '"  value="'
                                + $('<div/>').text(data).html() + '">';
                        } else {
                            var found = SlectedPRLineItemArray.some(function (el) {
                                return el === data;
                            });
                            if (found) {
                                return '<input type="checkbox" checked="checked" name="id[]" data-PRLIid="' + data + '" class="chksearchConsolidate ' + data + '"  value="'
                                    + $('<div/>').text(data).html() + '">';
                            }
                            else {
                                return '<input type="checkbox" name="id[]" data-PRLIid="' + data + '" class="chksearchConsolidate ' + data + '"  value="'
                                    + $('<div/>').text(data).html() + '">';
                            }
                        }
                    }
                },
                { "data": "PartClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "OrderQuantity", "autoWidth": true, "bSearchable": true, "bSortable": true, "class": "text-right" },
                { "data": "UnitofMeasure", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "UnitCost", "autoWidth": true, "bSearchable": true, "bSortable": true, "class": "text-right" },
                { "data": "VendorClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "VendorName", "autoWidth": true, "bSearchable": true, "bSortable": true }
            ],
        initComplete: function (settings, json) {
            SetPageLengthMenu();
        }
    });
}
$('#ConsolidateTable').find('th').click(function () {
    if ($(this).data('col')) {
        run = true;
        order = $(this).data('col');
    }
});
$(document).on('change', '.chksearchConsolidate', function () {
    var data = dtPRLConsolidateTable.row($(this).parents('tr')).data();
    if (!this.checked) {
        var index = SlectedPRLineItemArray.indexOf(data.PurchaseRequestLineItemId);
        SlectedPRLineItemArray.splice(index, 1);
        var el = $('#prconsolidateselectall').get(0);
        if (el && el.checked && ('indeterminate' in el)) {
            el.indeterminate = true;
        }
        SlectedPRLineItemArray = SlectedPRLineItemArray.filter(function (el) {
            return el !== data.PurchaseRequestLineItemId;
        });
    }
    else {
        var item = data.PurchaseRequestLineItemId;
        var found = SlectedPRLineItemArray.some(function (el) {
            return el === item;
        });
        if (!found) {
            SlectedPRLineItemArray.push(item);
        }
    }
    if (consolidatetotalcount == SlectedPRLineItemArray.length) {
        $('#prconsolidateselectall').prop('checked', 'checked');
    }
    else {
        $('#prconsolidateselectall').prop('checked', false);
    }
    if (SlectedPRLineItemArray.length == 0) {
        $('#btnPRConsolidateProcess').prop("disabled", "disabled");
    }
    else {
        $('#btnPRConsolidateProcess').removeAttr("disabled");
    }
});
$(document).on('click', '#prconsolidateselectall', function (e) {
    SlectedPRLineItemArray = [];
    var PurchaseRequestId = $(document).find('#purchaseRequestModel_PurchaseRequestId').val();
    dtPRLConsolidateTable = $("#ConsolidateTable").DataTable();
    var colname = order;
    var coldir = orderDir;
    var checked = this.checked;
    $.ajax({
        url: '/PurchaseRequest/PRLineForConsolidateSelectAllData',
        data: {
            PurchaseRequestId: PurchaseRequestId,
            Description: LRTrim($("#prcGridadvsearchDescription").val()),
            VendorClientLookupId: LRTrim($("#prcGridadvsearchVendorId").val()),
            VendorName: LRTrim($("#prcGridadvsearchVendorName").val()),
            order: colname,
            orderDir: coldir
        },
        async: true,
        type: "GET",
        beforeSend: function () {
            ShowLoader();
        },
        datatype: "json",
        success: function (data) {
            if (data) {
                $.each(data, function (index, item) {
                    if (checked) {
                        var exist = $.grep(SlectedPRLineItemArray, function (obj) {
                            return obj.PurchaseRequestLineItemId === item.PurchaseRequestLineItemId;
                        });
                        if (exist.length == 0) {
                            SlectedPRLineItemArray.push(item.PurchaseRequestLineItemId);
                        }
                    } else {
                        SlectedPRLineItemArray = $.grep(SlectedPRLineItemArray, function (obj) {
                            return obj.PurchaseRequestLineItemId !== item.PurchaseRequestLineItemId;
                        });

                        var i = SlectedPRLineItemArray.indexOf(item.PurchaseRequestLineItemId);
                        SlectedPRLineItemArray.splice(i, 1);
                    }

                });
            }
        },
        complete: function () {
            $('.itemcount').text(SlectedPRLineItemArray.length);
            selectedcount = SlectedPRLineItemArray.length;
            if (checked) {
                $(document).find('.dt-body-center').find('.chksearchConsolidate').prop('checked', 'checked');
                $('#btnPRConsolidateProcess').removeAttr("disabled");
            }
            else {
                $(document).find('.dt-body-center').find('.chksearchConsolidate').prop('checked', false);
                $('#btnPRConsolidateProcess').prop("disabled", "disabled");

            }
            CloseLoader();
        }
    });
});
$(document).on('click', '#btnPRConsolidateProcess', function () {
    if (SlectedPRLineItemArray.length == 0) {
        return;
    }
    else {
        var PurchaseRequestId = $(document).find('#purchaseRequestModel_PurchaseRequestId').val();
        $.ajax({
            url: "/PurchaseRequest/PurchaseRequestConsolidate",
            type: "POST",
            dataType: 'json',
            data: { PUrchaseRequestLineItemIds: SlectedPRLineItemArray, PurchaseRequestId: PurchaseRequestId },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == 'success') {
                    $(document).find('#ConsolidateModalpopup').modal('hide');
                    $('.modal-backdrop').hide();
                    SlectedPRLineItemArray = [];
                    SuccessAlertSetting.text = getResourceValue("AlertConsolidateSuccess");
                    swal(SuccessAlertSetting, function () {
                        RedirectToPRequestDetail(PurchaseRequestId, "overview")
                    });
                }
                else {
                    ErrorAlertSetting.text = getResourceValue("FailedAlert");
                    swal(ErrorAlertSetting, function () {
                    });
                }
            },
            complete: function () {
                CloseLoader();
            },
            error: function (jqXHR, exception) {
                CloseLoader();
            }
        });
    }
});
$(document).on('click', '.btnconsolidateclose', function () {
    SlectedPRLineItemArray = [];
});
//#endregion

//#region V2-1063
var SlectedItemArray = [];
var mrtotalcount = 0;
$(document).on('click', '#PRMaterialRequest', function () {
    var PurchaseRequestId = $(document).find('#purchaseRequestModel_PurchaseRequestId').val();
    $.ajax({
        url: "/PurchaseRequest/GetPurchaseRequestmaterialRequest",
        type: "POST",
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#MaterialRequestPopUp').html(data);
            $('#MaterialRequestModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            generateDataTableForMaterialRequest(PurchaseRequestId);
            SetMRAdvSearchControl();
            CloseLoader();
            $("#MaterialRequestadvsearchcontainer .sidebar").mCustomScrollbar({
                theme: "minimal"
            });
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
});
function SetMRAdvSearchControl() {
    $("#sidebar2").mCustomScrollbar({
        theme: "minimal"
    });
    $('#dismissPRMR, .overlay').on('click', function () {
        $('#sidebar2').removeClass('active');
        $('.overlay2').fadeOut();
    });
    $(document).find('#MaterialRequestbarCollapse').on('click', function () {
        $('#sidebar2').addClass('active');
        $('.overlay2').fadeIn();
        $('.collapse.in').toggleClass('in');
        $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    });
}
$(document).on("click", "#MaterialRequestbarCollapse", function (e) {
    e.preventDefault();
    $('#MaterialRequestadvsearchcontainer .sidebar').addClass('active');
    $('.overlay2').fadeIn();
    $('.collapse.in').toggleClass('in');
    $('a[aria-expanded=true]').attr('aria-expanded', 'false');
});
$(document).on('click', '#dismissPRMR', function () {
    $(document).find('#MaterialRequestadvsearchcontainer .sidebar').removeClass('active');
    $(document).find('.overlay2').fadeOut();
});
//#region Material Request Advance Search
$(document).on('click', "#btnMaterialRequestDataAdvSrch", function (e) {
    var searchitemhtml = "";
    hGridfilteritemcount = 0;
    $('#advsearchsidebarMaterialRequest').find('.adv-item').each(function (index, item) {
        if ([].concat($(this).val()).filter(function (valueOfFilter) { return valueOfFilter != ''; }).length) {
            hGridfilteritemcount++;
            var s = $(this).attr('id');
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossMRHistory" aria-hidden="true"></a></span>';
        }

    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $('#MaterialRequestsearchfilteritems').html(searchitemhtml);
    $('#MaterialRequestadvsearchcontainer').find('.sidebar').removeClass('active');
    $(document).find('.overlay2').fadeOut();
    MaterialRequestGridAdvanceSearch();
});
function MaterialRequestGridAdvanceSearch() {
    dtMaterialRequestTable.page('first').draw('page');
    $('.MaterialRequestfilteritemcount').text(hGridfilteritemcount);
}
$(document).on('click', '#MaterialRequestClearAdvSearchFilter', function () {
    clearAdvanceSearchMaterialRequest();
    dtMaterialRequestTable.page('first').draw('page');
});
function clearAdvanceSearchMaterialRequest() {
    var filteritemcount = 0;
    $('#advsearchsidebarMaterialRequest').find('input:text').val('');
    $('.MaterialRequestfilteritemcount').text(filteritemcount);
    $('#MaterialRequestsearchfilteritems').find('span').html('');
    $('#MaterialRequestsearchfilteritems').find('span').removeClass('tagTo');
}
$(document).on('click', '.btnCrossMRHistory', function () {
    var btnCrossedId = $(this).parent().attr('id');
    var searchtxtId = btnCrossedId.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    hGridfilteritemcount--;
    MaterialRequestGridAdvanceSearch();
});
//#endregion
function generateDataTableForMaterialRequest(PurchaseRequestId) {
    if ($(document).find('#MaterialRequestTable').hasClass('dataTable')) {
        dtMaterialRequestTable.destroy();
    }
    dtMaterialRequestTable = $("#MaterialRequestTable").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        serverSide: true,
        "order": [[1, "asc"]],
        stateSave: true,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/PurchaseRequest/GetGridDataForMaterialRequest",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.PurchaseRequestId = PurchaseRequestId;
                d.Description = LRTrim($("#prGridadvsearchDescription").val());
                d.PartClientLookupId = LRTrim($("#prGridadvsearchPartId").val());
                d.WorkOrderClientLookupId = LRTrim($("#prGridadvsearchWorkOrderId").val());
            },
            "dataSrc": function (result) {
                mrtotalcount = result.recordsTotal;
                return result.data;
            },
            global: true
        },
        "columns":
            [
                {
                    "data": "EstimatedCostsId",
                    orderable: false,
                    "bSortable": false,
                    className: 'select-checkbox dt-body-center',
                    targets: 0,
                    'render': function (data, type, full, meta) {
                        if ($('#prmaterialrequestselectall').is(':checked')) {
                            return '<input type="checkbox" checked="checked" name="id[]" data-PRECid="' + data + '" class="chksearchMaterialRequest ' + data + '"  value="'
                                + $('<div/>').text(data).html() + '">';
                        } else {
                            var found = SlectedItemArray.some(function (el) {
                                return el === data;
                            });
                            if (found) {
                                return '<input type="checkbox" checked="checked" name="id[]" data-PRECid="' + data + '" class="chksearchMaterialRequest ' + data + '"  value="'
                                    + $('<div/>').text(data).html() + '">';
                            }
                            else {
                                return '<input type="checkbox" name="id[]" data-PRECid="' + data + '" class="chksearchMaterialRequest ' + data + '"  value="'
                                    + $('<div/>').text(data).html() + '">';
                            }
                        }
                    }
                },
                { "data": "PartClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "OrderQuantity", "autoWidth": true, "bSearchable": true, "bSortable": true, "class": "text-right" },
                { "data": "UnitCost", "autoWidth": true, "bSearchable": true, "bSortable": true, "class": "text-right" },
                { "data": "UnitCostQuantity", "autoWidth": true, "bSearchable": true, "bSortable": true, "class": "text-right" },
                { "data": "WorkOrderClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
            ],
        initComplete: function (settings, json) {
            SetPageLengthMenu();
        }
    });
}
$('#MaterialRequestTable').find('th').click(function () {
    if ($(this).data('col')) {
        run = true;
        order = $(this).data('col');
    }
});
$(document).on('change', '.chksearchMaterialRequest', function () {
    var data = dtMaterialRequestTable.row($(this).parents('tr')).data();
    if (!this.checked) {
        var index = SlectedItemArray.indexOf(data.EstimatedCostsId);
        SlectedItemArray.splice(index, 1);
        var el = $('#prMaterialRequestselectall').get(0);
        if (el && el.checked && ('indeterminate' in el)) {
            el.indeterminate = true;
        }
        SlectedItemArray = SlectedItemArray.filter(function (el) {
            return el !== data.EstimatedCostsId;
        });
    }
    else {
        var item = data.EstimatedCostsId;
        var found = SlectedItemArray.some(function (el) {
            return el === item;
        });
        if (!found) {
            SlectedItemArray.push(item);
        }
    }
    if (mrtotalcount == SlectedItemArray.length) {
        $('#prmaterialrequestselectall').prop('checked', 'checked');
    }
    else {
        $('#prmaterialrequestselectall').prop('checked', false);
    }
    if (SlectedItemArray.length == 0) {
        $('#btnMaterialRequestProcess').prop("disabled", "disabled");
    }
    else {
        $('#btnMaterialRequestProcess').removeAttr("disabled");
    }
});
$(document).on('click', '#prmaterialrequestselectall', function (e) {
    SlectedItemArray = [];
    var PurchaseRequestId = $(document).find('#purchaseRequestModel_PurchaseRequestId').val();
    dtMaterialRequestTable = $("#MaterialRequestTable").DataTable();
    var colname = order;
    var coldir = orderDir;
    var checked = this.checked;
    $.ajax({
        url: '/PurchaseRequest/MaterialRequestSelectAllData',
        data: {
            PurchaseRequestId: PurchaseRequestId,
            Description: LRTrim($("#prGridadvsearchDescription").val()),
            PartClientLookupId: LRTrim($("#prGridadvsearchPartId").val()),
            WorkOrderClientLookupId: LRTrim($("#prGridadvsearchWorkOrderId").val()),
            order: colname,
            orderDir: coldir
        },
        async: true,
        type: "GET",
        beforeSend: function () {
            ShowLoader();
        },
        datatype: "json",
        success: function (data) {
            if (data) {
                $.each(data, function (index, item) {
                    if (checked) {
                        var exist = $.grep(SlectedItemArray, function (obj) {
                            return obj.EstimatedCostsId === item.EstimatedCostsId;
                        });
                        if (exist.length == 0) {
                            SlectedItemArray.push(item.EstimatedCostsId);
                        }
                    } else {
                        SlectedItemArray = $.grep(SlectedItemArray, function (obj) {
                            return obj.EstimatedCostsId !== item.EstimatedCostsId;
                        });

                        var i = SlectedItemArray.indexOf(item.EstimatedCostsId);
                        SlectedItemArray.splice(i, 1);
                    }

                });
            }
        },
        complete: function () {
            $('.itemcount').text(SlectedItemArray.length);
            selectedcount = SlectedItemArray.length;
            if (checked) {
                $(document).find('.dt-body-center').find('.chksearchMaterialRequest').prop('checked', 'checked');
                $('#btnMaterialRequestProcess').removeAttr("disabled");
            }
            else {
                $(document).find('.dt-body-center').find('.chksearchMaterialRequest').prop('checked', false);
                $('#btnMaterialRequestProcess').prop("disabled", "disabled");

            }
            CloseLoader();
        }
    });
});
$(document).on('click', '#btnMaterialRequestProcess', function () {
    if (SlectedItemArray.length == 0) {
        return;
    }
    else {
        var PurchaseRequestId = $(document).find('#purchaseRequestModel_PurchaseRequestId').val();
        $.ajax({
            url: "/PurchaseRequest/PurchaseRequestMaterialRequest",
            type: "POST",
            dataType: 'json',
            data: { EstimatedCostIds: SlectedItemArray, PurchaseRequestId: PurchaseRequestId },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == 'success') {
                    $(document).find('#MaterialRequestModalpopup').modal('hide');
                    $('.modal-backdrop').hide();
                    SlectedPRLineItemArray = [];
                    SuccessAlertSetting.text = getResourceValue("AlertMaterialRequestSuccess");
                    swal(SuccessAlertSetting, function () {
                        RedirectToPRequestDetail(PurchaseRequestId, "overview")
                    });
                }
                else {
                    ErrorAlertSetting.text = getResourceValue("FailedAlert");
                    swal(ErrorAlertSetting, function () {
                    });
                }
            },
            complete: function () {
                CloseLoader();
            },
            error: function (jqXHR, exception) {
                CloseLoader();
            }
        });
    }
});
$(document).on('click', '.btnmaterialrequestclose', function () {
    SlectedItemArray = [];
});
//#endregion

//#region V2-1112 EPMCustomConvertToPO
$(document).on('click', '#customEPMConvertToPurchaseOrder', function () {
    $('#addCustomPurchaseOrder_Initials').val('');
    $('#addCustomPurchaseOrder_ShiptoSuffix').val('').trigger('change.select2');
    $('#addCustomPurchaseOrder_Initials,#addCustomPurchaseOrder_ShiptoSuffix').removeClass('input-validation-error');
    $(document).find('#customPurchaseOrderNumberModal').modal({ backdrop: 'static', keyboard: false }, 'show');
});

function CustomConvertEPMPRToPOOnSuccess(data) {
    if (data.data == "Error") {
        ErrorAlertSetting.text = getResourceValue("PurchaseOrderConvertValidationAlert");
        swal(ErrorAlertSetting, function () {
        });
    } else if (data.data == "ErrorAssetMgt") {
        ErrorAlertSetting.text = getResourceValue("PurchaseOrderConvertAssetMgtValidationAlert");
        swal(ErrorAlertSetting, function () {
        });
    }
    else {
        if (data.data == "success") {
            SuccessAlertSetting.text = getResourceValue("PurchaseRequestConvertPurchaseOrderSuccessfulAlert");
            swal(SuccessAlertSetting, function () {
                $(document).find('#customPurchaseOrderNumberModal').modal('hide');
                $('.modal-backdrop').hide();
                var purchaserequestid = $(document).find('#purchaseRequestModel_PurchaseRequestId').val();
                RedirectToPRequestDetail(purchaserequestid, "overview");
            });
        }
        else {
            GenericSweetAlertMethod(data.data);
        }
    }

}

$(document).on('click', '.clearstate', function () {
    var id = "";
    $(document).find('#customPurchaseOrderNumberModal select').each(function (i, item) {
        id = $(document).find("#" + item.getAttribute('id')).attr('aria-describedby');
        $('#' + id).hide();
    });
});
$(document).on('click', '#btnCustomEPMPOSave', function () {
    if ($(document).find('#form3').valid()) {
        $(document).find('#customPurchaseOrderNumberModal').modal('hide');
    };
});
//#endregion