//#region Wizard
var AssetModelWizard = function () {
    //== Base elements
    var wizardEl = $('#m_wizard');
    var formEl = $('#formAssetModelWizardDynamic');
    var wizard;

    //== Private functions
    var initWizard = function () {
        //== Initialize form wizard
        wizard = wizardEl.mWizard({
            startStep: 1
        });
        //== Validation before going to next page
        wizard.on('beforeNext', function (wizard) {
            //if (validator.form() !== true) {
            //    return false;  // don't go to the next step
            //}
            if ($('.m-wizard__form-step--current').attr('id') == 'WoCompletionCriteriaStep') {
                var isvalid = formEl.valid();
                if (isvalid) {
                    return true;
                } else {
                    formEl.submit();
                    return false;
                }
            }
        });
        //== Change event
        wizard.on('change', function (wizard) {
            //mApp.scrollTop();
        });
    }

    return {
        // public functions
        init: function () {
            wizardEl = $('#m_wizard');
            formEl = $('#formAssetModelWizardDynamic');
            initWizard();
        }
    };
}();
//#endregion Wizard

//#region Asset Model completion Wizard
$(document).on('click', '#assetModelWizard', function () {
    var equipmentid = LRTrim($(document).find('#EquipData_EquipmentId').val());
    var ClientlookUpId = $(document).find('#EquipData_ClientLookupId').val();
    var Name = $('#EquipData_Name').val();
    var Status = $('#EquipData_Status').val();
    var isRemoveFromService = $('#EquipData_RemoveFromService').val();
    LoadAssetModelCompletionWizardModal($('#AssetModelInformationWizardDiv'), equipmentid, ClientlookUpId, Name, Status, isRemoveFromService, "EquipmentDetails");
});


var globalPageType = "";
function LoadAssetModelCompletionWizardModal(divElement, equipmentid, ClientlookUpId, Name, Status, isRemoveFromService, PageType) {
    $.ajax({
        url: "/Equipment/CompleteAssetModelFromWizard",
        type: "POST",
        data: { EquipmentId: equipmentid, ClientlookUpId: ClientlookUpId, Name: Name, isRemoveFromService: isRemoveFromService, Status: Status },
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            divElement.html(data);
        },
        complete: function () {
            SetEquimentForWizardControls();
            AssetModelWizard.init();

            $('#AssetModelWizardModal').modal({ backdrop: 'static', keyboard: false, show: true });
            globalPageType = PageType;
        },
        error: function () {
            CloseLoader();
        }
    });
}
//#endregion
function SetEquimentForWizardControls() {
    CloseLoader();
    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse(document);
    $('.select2picker, form').change(function () {
        var areaddescribedby = $(this).attr('aria-describedby');
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
    });
    $(document).find('.select2picker').select2({});
    $(document).find('.dtpicker').datepicker({
        changeMonth: true,
        changeYear: true,
        "dateFormat": "mm/dd/yy",
        autoclose: true
    });
    SetFixedHeadStyle();
}
function AssetModelAddDynamicOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("EquipmentSaveAlerts");
        $(document).find('#AssetModelWizardModal').modal('hide');
        if (data.Command == "saveAssetModel") {
            swal(SuccessAlertSetting, function () {
                titleText = getResourceValue("AlertActive");
                RedirectToEquipmentDetail(data.EquipmentId, "equipment");
            });
        }
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', '#closewoEquipTreeModal', function () {
    $(document).find('#AssetModelTreeModal').modal('hide');
});

//#region Equipment Hierarchy ModalTree 
var TextFieldId_ChargeTo = "";
var HdnfieldId_ChargeTo = "";
$(document).on('click', '#imgAssetModelChargeToTreeLineItemDynamic', function (e) {
    TextFieldId_ChargeTo = $(this).data('textfield');
    HdnfieldId_ChargeTo = $(this).data('valuefield');
    $(this).blur();
    generateWoAssetModelTreeDynamic(-1);
});
function generateWoAssetModelTreeDynamic(paramVal) {
    $.ajax({
        url: '/Equipment/WorkOrderAssetModelHierarchyTreeDynamic',
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
            $('#AssetModelTreeModal').modal('show');
            treeTable($(document).find('#AssetModelTreeModal').find('#tblTree'));
            $(document).find('.radSelectWoDynamic').each(function () {
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
$(document).on('change', ".radSelectWoDynamic", function () {
    var s = $(this).data;
    $(document).find('#' + HdnfieldId_ChargeTo).val('0');
    equipmentid = $(this).data('equipmentid');
    var clientlookupid = $(this).data('clientlookupid');
    var chargetoname = $(this).data('itemname');
    chargetoname = chargetoname.substring(0, chargetoname.length - 1);
    $.ajax({
        url: '/PlantLocationTree/MapEquipmentHierarchyTree',
        datatype: "json",
        type: "post",
        contenttype: 'application/json; charset=utf-8',
        async: false,
        cache: false,
        data: { _EquipmentId: equipmentid },
        success: function (data) {
            $('#commonWOTreeModal').modal('hide');
            //$(document).find('#' + TextFieldId_ChargeTo).val(clientlookupid).removeClass('input-validation-error');
            //$(document).find('#' + HdnfieldId_ChargeTo).val(equipmentid).trigger('change');
            $(document).find('#' + TextFieldId_ChargeTo).val(clientlookupid).css("display", "block");
            $(document).find('#' + HdnfieldId_ChargeTo).val(equipmentid).removeClass('input-validation-error').css("display", "none").trigger('change');
            $(document).find('#' + HdnfieldId_ChargeTo).parent().find('div > button.ClearAssetModalPopupGridData').css('display', 'block');
            $('#AssetModelTreeModal').modal('hide');
            //V2-948
            var SourceAssetAccount = $(document).find("#SourceAssetAccount").val();
            if (SourceAssetAccount != undefined && SourceAssetAccount == "True") {
                getlaboraccount(equipmentid);
            }
            //V2-948
        }
    });
});
$(document).on('hidden.bs.modal', '#AssetModelTreeModal', function () {
    TextFieldId_ChargeTo = "";
    HdnfieldId_ChargeTo = "";
});
//#endregion
