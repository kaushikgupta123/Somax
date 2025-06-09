var equipmentid = -1;
$(function () {
    $(document).find('.select2picker').select2({});
    if ($(document).find('#MultiStoreroom').val() == "True") {
        $(document).find('#openinvpartgrid').hide();
        $(document).find('#btnQrScanner').hide();
    }
    //asset Tree 
    $(document).on("change", "#partsIssue_ChargeType", function () {
        var option = '';
        chargeTypeText = $('option:selected', this).text();
        var type = $(this).val();
        chargeTypeSelected = type;
        if (type == "") {
            $(document).find("#imgChargeToTree").hide();
            option = "--Select--";
            $(document).find("#hdnChargeToId").val("").trigger('change');
            $("#hdnChargeToId").attr('disabled', 'disabled');
            $(document).find("#opengrid").hide();
        }
        else {
            if (type == "Equipment") {
                $(document).find("#imgChargeToTree").show();

            }
            else {
                $(document).find("#imgChargeToTree").hide();
            }
            $("#hdnChargeToId").removeAttr('disabled');
            $(document).find("#opengrid").show();

        }
        $(document).find("#txtChargeToId").val("");
        var tlen = $(document).find("#partsIssue_ChargeType").val();
        if (tlen.length > 0) {
            var areaddescribedby = $(document).find("#partsIssue_ChargeType").attr('aria-describedby');
            $('#' + areaddescribedby).hide();
            $(document).find('form').find("#partsIssue_ChargeType").removeClass("input-validation-error");
        }
        else {
            var areaddescribedby = $(document).find("#partsIssue_ChargeType").attr('aria-describedby');
            $('#' + areaddescribedby).show();
            $(document).find('form').find("#partsIssue_ChargeType").addClass("input-validation-error");
        }
    });
    //multistoreroom 
    $(document).on('change', '.ddlStoreroom', function () {
        $(document).find('#txtPartId').val('');
        $(document).find('#hdnPartId').val('');
        if ($(this).val() != '') {
            $(document).find('#openinvpartgrid').show();
            $(document).find('#btnQrScanner').show();
        } else {
            $(document).find('#openinvpartgrid').hide();
            $(document).find('#btnQrScanner').hide();
        }
        var tlen = $(document).find(".ddlStoreroom").val();
        if (tlen.length > 0) {
            var areaddescribedby = $(document).find(".ddlStoreroom").attr('aria-describedby');
            $('#' + areaddescribedby).hide();
            $(document).find('form').find(".ddlStoreroom").removeClass("input-validation-error");
        }
        else {
            var areaddescribedby = $(document).find(".ddlStoreroom").attr('aria-describedby');
            $('#' + areaddescribedby).show();
            $(document).find('form').find(".ddlStoreroom").addClass("input-validation-error");
        }
    });
});

$(document).on('click', "#opengrid", function () {
   
        var textChargeToId = $("#partsIssue_ChargeType option:selected").val();
        if (textChargeToId == "WorkOrder") { generateWorkOrderDataTable(); }
        else if (textChargeToId == "Equipment") { generateEquipmentDataTable(); }
});


$(document).on('click', '#btncancel', function () {
    swal(CancelAlertSetting, function () {
        window.location.href = "../Dashboard/Dashboard";
    });
});
function funcRefreshPage(PersonnelId) {
    $(document).find("#partsIssue_selectedPersonnelId").val(PersonnelId).trigger('change');
    $(document).find("#partsIssue_ChargeType").val("").trigger('change');
    $('#hdnChargeToId').val("");
    $('#txtChargeToId').val("");
    $('#txtPartId').val("");
    $('#hdnPartId').val("");
    $('#partsIssue_Quantity').val("");
    $('#partsIssue_Comments').val("");
    if ($(document).find('#MultiStoreroom').val() == "True") {
        $(document).find("#partsIssue_StoreroomId").val("").trigger('change');
        var areaddescribedby = $(document).find(".ddlStoreroom").attr('aria-describedby');
        $('#' + areaddescribedby).hide();
        $(document).find('form').find(".ddlStoreroom").removeClass("input-validation-error");
    }
    ResetErrorDiv();
    var areaddescribedby = $(document).find("#partsIssue_ChargeType").attr('aria-describedby');
    $('#' + areaddescribedby).hide();
    $(document).find('form').find("#partsIssue_ChargeType").removeClass("input-validation-error");

    var areaddescribedbyPartid = $(document).find("#txtPartId").attr('aria-describedby');
    $('#' + areaddescribedbyPartid).hide();
    $(document).find('form').find("#txtPartId").removeClass("input-validation-error");
    $(document).find('form').trigger("reset");
}


//#region QR Reader
$(document).on('click', '#btnQrScanner', function () {
    $(document).find('#txtPartId').val('');
    $(document).find('#hdnPartId').val('');
    if ($(document).find('#MultiStoreroom').val() == "True") {
        if ($(document).find('.ddlStoreroom').val() == "") {
            return false;
        }
    }
    if (!$(document).find('#QrCodeReaderModal').hasClass('show')) {
        $(document).find('#QrCodeReaderModal').modal("show");
        StartQRReaderPartCheckout();
    }
});
function StartQRReaderPartCheckout() {
    Html5Qrcode.getCameras().then(devices => {
        if (devices && devices.length) {
            scanner = new Html5Qrcode('reader', false);
            scanner.start({ facingMode: "environment" }, {
                fps: 10,
                qrbox: 230,
                //aspectRatio: aspectratio //1.7777778
            }, success => {
                onScanSuccessPartCheckout(success);
            }, error => {
            });
        } else {
            if ($(document).find('#QrCodeReaderModal').hasClass('show')) {
                $(document).find('#QrCodeReaderModal').modal("hide");
            }
        }
    }).catch(e => {
        if ($(document).find('#QrCodeReaderModal').hasClass('show')) {
            $(document).find('#QrCodeReaderModal').modal("hide");
        }
        if (e && e.startsWith('NotReadableError')) {
            ShowErrorAlert("Unable to start camera. It seems that another app is using the camera.");
        }
        else if (e && e.startsWith('NotFoundError')) {
            ShowErrorAlert("Camera device not found.");
        }

    });
}
function onScanSuccessPartCheckout(decodedText) {
    var url = "/Parts/GetPartIdByClientLookUpId?clientLookUpId=" + decodedText;
    if ($(document).find('#MultiStoreroom').val() == "True") {
        var StoreroomId = $(document).find('.ddlStoreroom').val();
        url = "/Base/GetPartIdByStoreroomIdAndClientLookUpforMultiStoreroom?ClientLookupId=" + decodedText + "&StoreroomId=" + StoreroomId;

    }
    $.ajax({
        url: url,
        type: "GET",
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if ($(document).find('#QrCodeReaderModal').hasClass('show')) {
                $(document).find('#QrCodeReaderModal').modal("hide");
            }
            if (data.PartId > 0) {
                $(document).find('#txtPartId').val(decodedText);
                $(document).find('#hdnPartId').val(data.PartId);

            } else if ($(document).find('#MultiStoreroom').val() == "True") {
                var msg = getResourceValue('spnInvalidQrCodeMsgforMultiStoreroom');
                ShowErrorAlert(msg.replace('${decodedText}', decodedText));
            }
            else {
                //Show Error Swal
                ShowErrorAlert(getResourceValue('spnInvalidQrCodeMsg').replace('${decodedText}', decodedText));
            }

        },
        complete: function () {
            StopCamera();
            CloseLoader();
        },
        error: function (xhr) {
            ShowErrorAlert("Something went wrong. Please try again.");
            CloseLoader();
        }
    });
}
function detectMob() {
    var isMobile = false;
    if (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|ipad|iris|kindle|Android|Silk|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino/i.test(navigator.userAgent)
        || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(navigator.userAgent.substr(0, 4))) {
        return true;
    }
    return isMobile;
}

//#endregion

//#region ModalTree
$(document).on('click', '#imgChargeToTree,#imgChargeToTreereturn', function (e) {
    $(this).blur();
    generateEquipmentTree(-1);
});
function generateEquipmentTree(paramVal) {
    $.ajax({
        url: '/PlantLocationTree/EquipmentHierarchyTree',
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
            $('#inventoryCheckoutTreeModal').modal('show');
        },
        complete: function () {
            CloseLoader();
            treeTable($(document).find('#tblTree'));
            $(document).find('.radSelect').each(function () {
                if ($(document).find('#hdnId').val() == '0' || $(document).find('#hdnId').val() == '') {

                    if ($(this).data('equipmentid') === equipmentid) {
                        $(this).attr('checked', true);
                    }

                }
                else {

                    if ($(this).data('equipmentid') == $(document).find('#hdnId').val()) {
                        $(this).attr('checked', true);
                    }

                }

            });
        },
        error: function (xhr) {
            alert('error');
        }
    });
}
$(document).on('change', '.radSelect', function () { 
    equipmentid = $(this).data('equipmentid');
    var clientlookupid = $(this).data('clientlookupid').split(' ')[0];
    $('#inventoryCheckoutTreeModal').modal('hide');
    $(document).find('#txtChargeToId').val(clientlookupid);
    $(document).find('#hdnChargeToId').val(equipmentid);
    $(document).find('#txtChargeToId').removeClass('input-validation-error');
});
//#endregion
function OnSuccessSavePartsIssue(data) {
    CloseLoader();
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("PartIssueAddAlert");
        if (data.btnType == "btnIssueAndAddAnother") {
            swal(SuccessAlertSetting, function () {
                funcRefreshPage(data.PersonnelId);
            });
        } else {
            swal(SuccessAlertSetting, function () {
                window.location.href = "../Dashboard/Dashboard";
            });
        }
    } else if (data.Result == "failed") {
        ErrorAlertSetting.text = data.errorMsg;
        swal(ErrorAlertSetting, function () {
          
        });
    }
}
